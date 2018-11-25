using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using OxidePack.Client.App;
using OxidePack.Client.Core.MsBuildProject;
using OxidePack.Client.Properties;
using OxidePack.Data;
using SapphireEngine;

namespace OxidePack.Client
{
    public class PluginProject
    {
        public class PluginProjectData
        {
            public string Name;
            public string Author;
            public string Description;
            public VersionNumber Version = new VersionNumber(0,0,0);
            
            public List<string> Modules = new List<string>();

            public PluginProjectData(string name)
            {
                Name = Config.PluginTemplateDefault.Name.Replace("$name$", name);
                Author = Config.PluginTemplateDefault.Author.Replace("$name$", name);
                Description = Config.PluginTemplateDefault.Description.Replace("$name$", name);
                Version = Config.PluginTemplateDefault.Version;
            }
        }
        
        public CsProject csProject;
        public PluginProjectData config;

        private string _Directory;
        
        private string DataFileName => Path.Combine(_Directory, "plugin.json");
        private string Name => Path.GetFileName(_Directory);

        public PluginProject(CsProject csProject, string directory)
        {
            this.csProject = csProject;
            this._Directory = directory;
            ReloadConfig();
            AdjustMainFile();
        }

        void AdjustMainFile()
        {
            var pluginFile = Path.Combine(_Directory, $"{Name}.cs");
            if (File.Exists(pluginFile) == false)
            {
                var csFile = new StringBuilder(Resources.RustPlugin_Template);
                csFile.Replace("$name$", Name)
                    .Replace("$info-name$", config.Name)
                    .Replace("$author$", config.Author)
                    .Replace("$version$", config.Version.ToString())
                    .Replace("$description$", config.Description);
                File.WriteAllText(pluginFile, csFile.ToString());
                csProject.CompileAdd(pluginFile);
            }
        }

        void ReloadConfig()
        {
            string pluginNameDefault = Path.GetFileName(_Directory);
            if (File.Exists(this.DataFileName) == false)
            {
                config = new PluginProjectData(pluginNameDefault);
                SaveConfig();
            }

            try
            {
                config = JsonConvert.DeserializeObject<PluginProjectData>(File.ReadAllText(this.DataFileName));
            }
            catch
            {
                ConsoleSystem.LogError("PluginsProject: Invalid Config");
                config = new PluginProjectData(pluginNameDefault);
            }
        }

        void SaveConfig()
        {
            if (config == null)
            {
                throw new NullReferenceException("_Config is null!!!");
            }
            
            File.WriteAllText(this.DataFileName, JsonConvert.SerializeObject(config, Formatting.Indented));
        }
    }
}