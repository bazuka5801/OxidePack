using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using SapphireEngine;
using SapphireEngine.Functions;

namespace OxidePack
{
    public class FSWatcher
    {
        // The filesystem watcher
        private FileSystemWatcher watcher;

        // Changes are buffered briefly to avoid duplicate events
        private Dictionary<string, ChangeQueued> changeQueue;
        
        private class ChangeQueued
        {
            public DateTime lastReadTime;
            public WatcherChangeTypes type;
            public Timer timer;
        }

        private Action<string> callback;
        
        private object lockObject = new object();

        public void Subscribe(Action<string> action) => callback += action;

        public bool Enabled = true;
        
        /// <summary>
        /// Empty - all extensions
        /// </summary>
        public List<string> AcceptExtensions = new List<string>();
        
        public List<string> ExcludeDirectories = new List<string>();
        
        /// <summary>
        /// Initializes a new instance of the FSWatcher class
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        public FSWatcher(string directory, string filter)
        {
            changeQueue = new Dictionary<string, ChangeQueued>();
            LoadWatcher(directory, filter);
        }

        /// <summary>
        /// Loads the filesystem watcher
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        private void LoadWatcher(string directory, string filter)
        {
            // Create the watcher
            watcher = new FileSystemWatcher(directory, filter)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += watcher_Changed;
            watcher.Created += watcher_Changed;
            watcher.Deleted += watcher_Changed;
            watcher.Error += watcher_Error;
            GC.KeepAlive(watcher);
        }

        /// <summary>
        /// Close watcher
        /// </summary>
        public void Close()
        {
            watcher.Dispose();
        }
        
        /// <summary>
        /// Called when the watcher has registered a filesystem change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (Enabled == false)
            {
                return;
            }

            var extension = Path.GetExtension(e.Name);
            if (this.AcceptExtensions.Count > 0 && this.AcceptExtensions.Contains(extension) == false)
            {
                return;
            }

            var directory = Path.GetFileName(Path.GetDirectoryName(e.FullPath));
            if (this.ExcludeDirectories.Count > 0 && this.ExcludeDirectories.Contains(directory))
            {
                return;
            }

            lock (lockObject)
            {
                var watcher = (FileSystemWatcher) sender;
                var length = e.FullPath.Length - watcher.Path.Length - extension.Length - 1;
                var sub_path = e.FullPath.Substring(watcher.Path.Length + 1, length);
               
                var path = e.FullPath;
                
                DateTime lastWriteTime = File.GetLastWriteTime(path);
                if (!changeQueue.TryGetValue(sub_path, out var change))
                {
                    changeQueue[sub_path] = change = new ChangeQueued()
                    {
                        lastReadTime = File.GetLastWriteTime(path)
                    };
                }
                else if (lastWriteTime == change.lastReadTime)
                {
                    return;
                }
                
                change.lastReadTime = lastWriteTime;
                change.type = e.ChangeType;
                change.timer?.Destroy();
                change.timer = null;
                change.timer = Timer.SetTimeout(() =>
                {
                    switch (change.type)
                    {
                        case WatcherChangeTypes.Changed:
                            FireCallback(path);
                            break;

                        case WatcherChangeTypes.Created:
                            FireCallback(path);
                            break;
                    }
                }, .2f);
            }
        }

        private void FireCallback(string fullpath)
        {
            if (IsFileLocked(fullpath))
            {
                Timer.SetTimeout(() => FireCallback(fullpath), 0.1f);
                return;
            }
            callback?.Invoke(fullpath);
        }
        
        
        protected virtual bool IsFileLocked(string fullpath)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(fullpath, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            //file is not locked
            return false;
        }

        private void watcher_Error(object sender, ErrorEventArgs e)
        {
            ConsoleSystem.LogError("FSWatcher error: {0}", e.GetException());
        }
    }
}