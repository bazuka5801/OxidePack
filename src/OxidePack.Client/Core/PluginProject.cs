using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

        public Action<PluginProject> OnModulesChanged;
        public Action<PluginProject> OnCompiled;

        private string _Directory;
        
        private string DataFileName => Path.Combine(_Directory, "plugin.json");
        public string Name => Path.GetFileName(_Directory);

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

        #region [Methods] Modules
        public void AddModule(string mName)
        {
            if (config.Modules.Contains(mName))
            {
                throw new Exception($"Module {mName} already exists in {Name} plugin!");
            }

            config.Modules.Add(mName);
            OnModulesChanged?.Invoke(this);
            RequestGeneratedFile();
            SaveConfig();
        }
        
        public void RemoveModule(string mName)
        {
            if (config.Modules.Contains(mName) == false)
            {
                throw new Exception($"Missing {mName} module in {Name} plugin!");
            }

            config.Modules.Remove(mName);
            OnModulesChanged?.Invoke(this);
            var filename = Path.Combine(_Directory, $"{Name}.generated.cs");
            if (config.Modules.Count == 0 && File.Exists(filename))
            {
                csProject.CompileRemove(filename);
                File.Delete(filename);
            }
            else
            {
                RequestGeneratedFile();
            }

            SaveConfig();
        }

        public void RequestGeneratedFile()
        {
            using (GeneratedFileRequest gfRequest = new GeneratedFileRequest
            {
                modules = config.Modules.ToList(),
                @namespace = $"Oxide.Plugins.{Name}",
                pluginname = Name
            })
            {
                Net.cl.SendRPC(RPCMessageType.GeneratedFileRequest, gfRequest);
            }
        }
        
        private static Boolean Compiling;

        void SetCompilingState(bool compiling)
        {
            Compiling = compiling;
            if (compiling)
            {
                MainForm.UpdateStatus($"[{Name}] Compiling ..");
                MainForm.UpdateProgressBar(style: ProgressBarStyle.Marquee);
            }
            else
            {
                MainForm.UpdateStatus($"[{Name}] Compiled..");
                MainForm.UpdateProgressBar(value: 100, max: 100, ProgressBarStyle.Blocks);
            }
        }

        public void OnGeneratedFileResponse(string content)
        {
            var filename = Path.Combine(_Directory, $"{Name}.generated.cs");
            var exists = File.Exists(filename);
            File.WriteAllText(filename, content);
            if (exists == false)
            {
                csProject.CompileAdd(filename);
            }
        }
        #endregion

        
        public void RequestCompile()
        {
            if (Compiling)
            {
                return;
            }
            SetCompilingState(true);
            config.Version.Build++;
            SaveConfig();
            var sources = Directory.GetFiles(_Directory, "*.cs").Select(filename =>
            {
                var content = File.ReadAllText(filename);
                return new SourceFile {filename = Path.GetFileName(filename), content = content, sha256 = content.ToSHA512()};
            }).ToList();
            BuildRequest bRequest = new BuildRequest
            {
                buildOptions = new BuildOptions
                {
                    name = Name,
                    plugininfo = new PluginInfo
                    {
                        name = config.Name,
                        author = config.Author,
                        version = config.Version.ToString(),
                        description = config.Description
                    }
                },
                sources = sources
            };
            Net.cl.SendRPC(RPCMessageType.BuildRequest, bRequest);
        }
        
        public void OnBuildResponse(string content)
        {
            var outputDir = Path.Combine(Path.GetDirectoryName(csProject.FilePath), ".builded");
            if (Directory.Exists(outputDir) == false)
                Directory.CreateDirectory(outputDir);
            var outputPath = Path.Combine(outputDir, $"{Name}.cs");
            File.WriteAllText(outputPath, content);
            OnCompiled?.Invoke(this);
            SetCompilingState(false);
        }

        #region [Methods] Config
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
        #endregion
    }
}