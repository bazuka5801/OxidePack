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
        private PluginsProjectData _Config;

        private string DataFileName => Path.Combine(Path.GetDirectoryName(csProject.FilePath), "plugins-project.json");

        private Dictionary<string, PluginProject> _Plugins = new Dictionary<string, PluginProject>();

        private FSWatcher _Watcher;

        private string _Directory => Path.GetDirectoryName(csProject.FilePath);

        class PluginsProjectData
        {
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

            return _Plugins[pluginName] = new PluginProject(csProject, pluginFolder);
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

        public List<string> GetPluginList() => _Config?.PluginList.ToList() ?? new List<string>();

        private void StartWatching()
        {
            _Watcher?.Close();
            _Watcher = new FSWatcher(_Directory, "*.cs");
            _Watcher.Subscribe(OnSourceFileChanged);
        }

        private void OnSourceFileChanged(string file)
        {
            var relPath = FileUtils.GetRelativePath(file, _Directory);
            var pluginname = relPath.Substring(0, relPath.IndexOf('\\'));
            // .builded, etc...
            if (pluginname.StartsWith("."))
            {
                return;
            }
            var plugin = GetPlugin(pluginname);
            if (plugin != null)
            {
                plugin.RequestCompile();
            }
        }
        
        public bool AddPlugin(string pName, out PluginProject plugin)
        {
            if (_Config.PluginList.Contains(pName))
            {
                plugin = null;
                return false;
            }

            plugin = GetPlugin(pName);
            _Config.PluginList.Add(pName);
            return true;
        }

        #region [Methods] Config
        void ReloadConfig()
        {
            if (File.Exists(this.DataFileName) == false)
            {
                _Config = new PluginsProjectData();
                SaveConfig();
            }

            try
            {
                _Config = JsonConvert.DeserializeObject<PluginsProjectData>(File.ReadAllText(this.DataFileName));
            }
            catch
            {
                ConsoleSystem.LogError("PluginsProject: Invalid Config");
                _Config = new PluginsProjectData();
            }
        }

        void SaveConfig()
        {
            if (_Config == null)
            {
                throw new NullReferenceException("_Config is null!!!");
            }
            
            File.WriteAllText(this.DataFileName, JsonConvert.SerializeObject(_Config, Formatting.Indented));
        }
        #endregion
    }
}