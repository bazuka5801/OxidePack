using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using OxidePack.Client.Core.MsBuildProject;
using OxidePack.Client.Properties;
using SapphireEngine;

namespace OxidePack.Client
{
    public class PluginsProject
    {
        public CsProject csProject;
        public PluginsProjectData Config;

        private string DataFileName => Path.Combine(Path.GetDirectoryName(csProject.FilePath), "plugins-project.json");

        private Dictionary<string, PluginProject> _Plugins = new Dictionary<string, PluginProject>();

        private FSWatcher _Watcher;

        private string _Directory => Path.GetDirectoryName(csProject.FilePath);

        public class PluginsProjectData
        {
            public string BuildedCopyPath = "";
            public bool ForClient = false;
            public List<string> PluginList = new List<string>();
        }

        public PluginsProject(CsProject csProject)
        {
            this.csProject = csProject;
            ReloadConfig();
            OPClientCore.SetPluginsProject(this);
            StartWatching();
        }

        public PluginProject GetPlugin(string pluginName)
        {
            if (_Plugins.TryGetValue(pluginName, out var plugin))
                return plugin;

            var pluginFolder = Path.Combine(_Directory, pluginName);
            if (Directory.Exists(pluginFolder) == false)
            {
                Directory.CreateDirectory(pluginFolder);
            }

            return _Plugins[pluginName] = new PluginProject(this, pluginFolder);
        }

        public void RemovePlugin(string name)
        {
            var pluginFolder = Path.Combine(_Directory, name);
            if (Directory.Exists(pluginFolder))
            {
                var csFiles = Directory.GetFiles(pluginFolder, "*.cs", SearchOption.AllDirectories).ToList();
                csFiles.ForEach(csProject.CompileRemove);
            }
        }

        public List<string> GetPluginList() => Config?.PluginList.ToList() ?? new List<string>();

        private void StartWatching()
        {
            _Watcher?.Close();
            _Watcher = new FSWatcher(_Directory, "*.*");
            _Watcher.AcceptExtensions.AddRange(new[] {".cs", ".json"});
            _Watcher.ExcludeDirectories.AddRange(new[] {".encrypted", ".builded"});
            _Watcher.Subscribe(OnSourceFileChanged);
        }

        bool GetPluginName(string filename, out string pluginname)
        {
            var relPath = FileUtils.GetRelativePath(filename, _Directory);
            pluginname = relPath.Substring(0, relPath.IndexOf('\\'));

            // Equals root directory
            if (pluginname.Equals(Path.GetFileName(_Directory)))
            {
                return false;
            }
                
            // .builded, etc...
            if (pluginname.StartsWith("."))
            {
                return false;
            }

            return true;
        }
        
        private void OnSourceFileChanged(string file)
        {
            var extension = Path.GetExtension(file);
            if (extension.Equals(".cs"))
            {
                if (GetPluginName(file, out var pluginname) == false)
                {
                    return;
                }
                
                var plugin = GetPlugin(pluginname);
                if (plugin != null)
                {
                    plugin.RequestCompile();
                }
            }
            else if (extension.Equals(".json"))
            {
                var filename = Path.GetFileName(file);
                if (filename.Equals("plugin.json"))
                {
                    if (GetPluginName(file, out var pluginname) == false)
                    {
                        return;
                    }

                    var plugin = GetPlugin(pluginname);
                    if (plugin != null)
                    {
                        plugin.OnConfigChanged();
                    }
                }
            }
        }
        
        public bool AddPlugin(string pName, out PluginProject plugin)
        {
            if (Config.PluginList.Contains(pName))
            {
                plugin = null;
                return false;
            }

            plugin = GetPlugin(pName);
            Config.PluginList.Add(pName);
            SaveConfig();
            return true;
        }

        #region [Methods] Config
        void ReloadConfig()
        {
            if (File.Exists(this.DataFileName) == false)
            {
                Config = new PluginsProjectData();
                SaveConfig();
            }

            try
            {
                Config = JsonConvert.DeserializeObject<PluginsProjectData>(File.ReadAllText(this.DataFileName));
            }
            catch
            {
                ConsoleSystem.LogError("PluginsProject: Invalid Config");
                Config = new PluginsProjectData();
            }
        }

        void SaveConfig()
        {
            if (Config == null)
            {
                throw new NullReferenceException("_Config is null!!!");
            }
            
            File.WriteAllText(this.DataFileName, JsonConvert.SerializeObject(Config, Formatting.Indented));
        }
        #endregion
    }
}