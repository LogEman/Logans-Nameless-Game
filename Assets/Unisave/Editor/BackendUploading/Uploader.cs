using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LightJson;
using Unisave.Utils;
using UnityEngine;

namespace Unisave.Editor.BackendUploading
{
    /// <summary>
    /// Uploads backend folder to the server
    /// </summary>
    public class Uploader
    {
        private readonly UnisavePreferences preferences;

        private readonly ApiUrl apiUrl;

        /// <summary>
        /// Creates the default instance of the uploader
        /// that uses correct preferences.
        /// </summary>
        public static Uploader GetDefaultInstance()
        {
            // NOTE: There's not an easy way to keep the value.
            // Static field gets reset when the game is started.
            // It would have to use editor preferences which is an overkill.
            
            return new Uploader(
                UnisavePreferences.LoadOrCreate()
            );
        }

        private Uploader(UnisavePreferences preferences)
        {
            this.preferences = preferences;

            apiUrl = new ApiUrl(preferences.ServerUrl);
        }

        /// <summary>
        /// This method gets called after recompilation by the hook.
        /// It triggers the upload if preferences are set up that way.
        /// </summary>
        /// <param name="isEditor">
        /// Are we uploading just an editor recompilation? (true)
        /// Or a new build of the game? (false)
        /// </param>
        public void RunAutomaticUpload(bool isEditor)
        {
            if (!preferences.AutomaticBackendUploading)
                return;

            Run(
                isEditor,
                verbose: false,
                useAnotherThread: false // has to be FALSE! see the param. note
            );
        }

        /// <summary>
        /// Performs all the uploading
        /// </summary>
        /// <param name="isEditor">
        /// Are we uploading just an editor recompilation? (true)
        /// Or a new build of the game? (false)
        /// </param>
        /// <param name="verbose">Print additional info</param>
        /// <param name="useAnotherThread">
        /// If true, the networking will happen in a background thread so
        /// that the UI is responsive. BUT! if I start a background thread
        /// right after assembly compilation, it gets killed unexpectedly
        /// for some reason. So there the execution has to be single-threaded.
        /// </param>
        public void Run(bool isEditor, bool verbose, bool useAnotherThread)
        {
            if (verbose)
                Debug.Log("Starting backend upload...");
            
            // list all backend folders
            var backendFolders = new string[] {
                "Assets/" + preferences.BackendFolder
            };

            // get list of files to be uploaded
            var files = new List<BackendFile>();
            files.AddRange(CSharpFile.FindFiles(backendFolders));
            files.AddRange(SOFile.FindFiles(backendFolders));
            
            // compute file hashes
            files.ForEach(f => f.ComputeHash());
            
            // compute backend hash
            string backendHash = Hash.CompositeMD5(
                files.Select(f => f.Hash)
            );
            
            // store the upload time and backend hash
            preferences.LastBackendUploadAt = DateTime.Now;
            preferences.BackendHash = backendHash;
            preferences.Save();

            // NOTE: Debug Methods are thread-safe
            // https://answers.unity.com/questions/714590/
            // thread-safety-and-debuglog.html
            
            // do the rest of computation involving networking in the background
            if (useAnotherThread)
            {
                var backgroundThread = new Thread(() => {
                    BackgroundJob(files, backendHash, isEditor, verbose);
                });
                backgroundThread.Start();
            }
            else // well, not always in the background
            {
                BackgroundJob(files, backendHash, isEditor, verbose);
            }
        }

        /// <summary>
        /// Part the of the uploading that can happen in the background
        /// on another thread
        /// </summary>
        private void BackgroundJob(
            List<BackendFile> files, string backendHash,
            bool isEditor, bool verbose
        )
        {
            /*
             * WARNING: You run in another thread, be aware of what you touch!
             */
            
            // check server reachability
            if (!Http.UrlReachable(apiUrl.Index()))
            {
                Debug.LogError(
                    $"Unisave server at '{apiUrl.Index()}' is not reachable.\n"
                    + "If you want to work offline, you can go to "
                    + "Window/Unisave/Preferences and disable automatic "
                    + "backend uploading."
                );
                return;
            }

            // send all file paths, hashes and global hash to the server
            // and initiate the upload
            JsonObject startResponse = Http.Post(
                apiUrl.BackendUpload_Start(),
                new JsonObject()
                    .Add("game_token", preferences.GameToken)
                    .Add("editor_key", preferences.EditorKey)
                    
                    .Add("backend_hash", backendHash)
                    .Add("framework_version", FrameworkMeta.Version)
                    .Add("asset_version", AssetMeta.Version)
                    .Add("is_editor", isEditor)
                    .Add(
                        "backend_folder_path",
                        "Assets/" + preferences.BackendFolder
                    )
                    .Add("files", new JsonArray(
                        files.Select(f => (JsonValue) new JsonObject()
                            .Add("path", f.Path)
                            .Add("hash", f.Hash)
                        ).ToArray()
                    ))
            );

            // finish upload if requested
            if (startResponse["upload_has_finished"].AsBoolean)
            {
                if (verbose)
                {
                    Debug.Log(
                        "Backend upload done, this backend " +
                        "has already been uploaded."
                    );
                }

                return;
            }

            var filePathsToUpload = new HashSet<string>(
                startResponse["files_to_upload"]
                    .AsJsonArray
                    .Select(x => x.AsString)
            );
            
            // filter out files that needn't be uploaded
            IEnumerable<BackendFile> filteredFiles = files.Where(
                f => filePathsToUpload.Contains(f.Path)
            );

            // send individual files the server has asked for
            foreach (var file in filteredFiles)
            {
                Http.Post(
                    apiUrl.BackendUpload_File(),
                    new JsonObject()
                        .Add("game_token", preferences.GameToken)
                        .Add("editor_key", preferences.EditorKey)
                    
                        .Add("backend_hash", backendHash)
                        .Add("file", new JsonObject()
                            .Add("path", file.Path)
                            .Add("hash", file.Hash)
                            .Add("file_type", file.FileType)
                            .Add(
                                "content",
                                Convert.ToBase64String(
                                    file.ContentForUpload())
                                )
                            )
                );
                
                if (verbose)
                    Debug.Log($"Uploaded '{file.Path}'");
            }

            // finish the upload
            JsonObject finishResponse = Http.Post(
                apiUrl.BackendUpload_Finish(),
                new JsonObject()
                    .Add("game_token", preferences.GameToken)
                    .Add("editor_key", preferences.EditorKey)
                
                    .Add("backend_hash", backendHash)
            );

            if (verbose)
            {
                Debug.Log(
                    "Backend upload done, starting server compilation..."
                );
            }

            // print result of the compilation
            if (!finishResponse["compiler_success"].AsBoolean)
            {
                Debug.LogError(
                    "Server compile error:\n" +
                    finishResponse["compiler_output"].AsString
                );
            }
            else
            {
                if (verbose)
                    Debug.Log("Server compilation done.");
            }
        }
    }
}
