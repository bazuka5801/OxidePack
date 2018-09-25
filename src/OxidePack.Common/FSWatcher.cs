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
        private class QueuedChange
        {
            internal WatcherChangeTypes type;
            internal Timer timer;
        }

        // The filesystem watcher
        private FileSystemWatcher watcher;

        // Changes are buffered briefly to avoid duplicate events
        private Dictionary<string, QueuedChange> changeQueue;

        private Action<string> callback;

        public void Subscribe(Action<string> action) => callback += action;

        public bool Enabled = true;
        
        /// <summary>
        /// Initializes a new instance of the FSWatcher class
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        public FSWatcher(string directory, string filter)
        {
            changeQueue = new Dictionary<string, QueuedChange>();
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
            
            var watcher = (FileSystemWatcher)sender;
            var length = e.FullPath.Length - watcher.Path.Length - Path.GetExtension(e.Name).Length - 1;
            var sub_path = e.FullPath.Substring(watcher.Path.Length + 1, length);
            QueuedChange change;
            if (!changeQueue.TryGetValue(sub_path, out change))
            {
                change = new QueuedChange();
                changeQueue[sub_path] = change;
            }
            change.timer?.Dispose();
            change.timer = null;
            var path = e.FullPath;
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    if (change.type != WatcherChangeTypes.Created)
                        change.type = WatcherChangeTypes.Changed;
                    break;

                case WatcherChangeTypes.Created:
                    if (change.type == WatcherChangeTypes.Deleted)
                        change.type = WatcherChangeTypes.Changed;
                    else
                        change.type = WatcherChangeTypes.Created;
                    break;

                case WatcherChangeTypes.Deleted:
                    if (change.type == WatcherChangeTypes.Created)
                    {
                        changeQueue.Remove(sub_path);
                        return;
                    }
                    change.type = WatcherChangeTypes.Deleted;
                    break;
            }
            Timer.SetTimeout(() =>
            {
                change.timer?.Dispose();
                change.timer = Timer.SetTimeout(() =>
                {
                    change.timer = null;
                    changeQueue.Remove(sub_path);
                    switch (change.type)
                    {
                        case WatcherChangeTypes.Changed:
                            FireCallback(path);
                            break;

                        case WatcherChangeTypes.Created:
                            FireCallback(path);
                            break;
                    }
                },.2f);
            }, 0.01f);
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