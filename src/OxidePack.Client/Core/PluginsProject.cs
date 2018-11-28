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
        
        class PluginsProjectData
        {
            public List<string> PluginList = new List<string>();
        }

        public PluginsProject(CsProject csProject)
        {
            this.csProject = csProject;
            ReloadConfig();
            OPClientCore.SetPluginsProject(this);
        }

        public PluginProject GetPlugin(string pluginName)
        {
            if (_Plugins.TryGetValue(pluginName, out var plugin))
                return plugin;
            
            var pluginFolder = Path.Combine(Path.GetDirectoryName(csProject.FilePath), pluginName);
            if (Directory.Exists(pluginFolder) == false)
            {
                Directory.CreateDirectory(pluginFolder);
            }

            return _Plugins[pluginName] = new PluginProject(csProject, pluginFolder);
        }

        public void RemovePlugin(string name)
        {
            var pluginFolder = Path.Combine(Path.GetDirectoryName(csProject.FilePath), name);
            if (Directory.Exists(pluginFolder))
            {
                var csFiles = Directory.GetFiles(pluginFolder, "*.cs", SearchOption.AllDirectories).ToList();
                csFiles.ForEach(csProject.CompileRemove);
            }
        }

        public List<string> GetPluginList() => _Config?.PluginList.ToList() ?? new List<string>();

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
    }
}