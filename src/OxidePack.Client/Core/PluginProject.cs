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
        private readonly string _Directory;
        public PluginProjectData config;
        public CsProject csProject;
        public Action<BuildResponse> OnBuilded;
        public Action<PluginProject> OnChanged;

        public Action<PluginProject> OnModulesChanged;

        public PluginsProject Project;

        public PluginProject(PluginsProject project, string directory)
        {
            Project = project;
            csProject = project.csProject;
            _Directory = directory;
            ReloadConfig();
            AdjustMainFile();
        }

        private string DataFileName => Path.Combine(_Directory, "plugin.json");
        public string Name => Path.GetFileName(_Directory);

        public bool ForClient => Project.Config.ForClient;

        private void AdjustMainFile()
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


        public void AddSourceFile(string filename)
        {
            var sourceFilename = Path.Combine(_Directory, $"{Name}.{filename}.cs");
            if (File.Exists(sourceFilename) == false)
            {
                var csFile = new StringBuilder(Resources.RustPlugin_SourceFile);
                csFile.Replace("$name$", Name)
                    .Replace("$info-name$", config.Name)
                    .Replace("$author$", config.Author)
                    .Replace("$version$", config.Version.ToString())
                    .Replace("$description$", config.Description);
                File.WriteAllText(sourceFilename, csFile.ToString());
                csProject.CompileAdd(sourceFilename);
            }
        }

        public void RequestCompile(EncryptOptions options = null)
        {
            if (Compiling) return;
            SetCompilingState(true);
            config.Version.Build++;
            SaveConfig();
            var sources = Directory.GetFiles(_Directory, "*.cs", SearchOption.AllDirectories).Select(filename =>
            {
                var content = File.ReadAllText(filename);
                return new SourceFile
                    {filename = Path.GetFileName(filename), content = content, sha256 = content.ToSHA512()};
            }).ToList();
            var bRequest = new BuildRequest
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
                encryptOptions = options ?? new EncryptOptions {enabled = false},
                sources = sources
            };

            if (ForClient)
            {
                bRequest.buildOptions.forClient = true;
                bRequest.buildOptions.compileDll = true;
            }

            Net.cl.SendRPC(RPCMessageType.BuildRequest, bRequest);
        }

        public void OnBuildResponse(BuildResponse bResponse)
        {
            var outputDir = Path.Combine(Path.GetDirectoryName(csProject.FilePath), ".builded");
            if (Directory.Exists(outputDir) == false)
                Directory.CreateDirectory(outputDir);


            if (bResponse.compiledAssembly != null)
            {
                var outputDllPath = Path.Combine(outputDir, $"{Name}.dll");
                File.WriteAllBytes(outputDllPath, bResponse.compiledAssembly);
            }
            else
            {
                var outputPath = Path.Combine(outputDir, $"{Name}.cs");
                File.WriteAllText(outputPath, bResponse.content);
            }

            if (bResponse.buildErrors.Count > 0)
                ThreadUtils.RunInUI(() =>
                {
                    var errorModel = new ErrorViewFormModel
                    {
                        SourceText = bResponse.content,
                        Errors = bResponse.buildErrors.Select(p => new ErrorModel
                        {
                            Column = p.column, Line = p.line,
                            ColumnEnd = p.columnEnd, LineEnd = p.lineEnd,
                            ErrorText = p.errorText
                        }).ToList()
                    };
                    new ErrorViewForm(errorModel).Show();
                });

            var copyPath = Project.Config.BuildedCopyPath;

            if (string.IsNullOrEmpty(copyPath) == false
                && Directory.Exists(copyPath))
            {
                if (bResponse.compiledAssembly != null)
                {
                    var copyOutputDllPath = Path.Combine(copyPath, $"{Name}.dll");
                    File.WriteAllBytes(copyOutputDllPath, bResponse.compiledAssembly);
                }
                else
                {
                    var copyOutputPath = Path.Combine(copyPath, $"{Name}.cs");
                    File.WriteAllText(copyOutputPath, bResponse.content);
                }
            }

            if (string.IsNullOrEmpty(bResponse.encrypted) == false)
            {
                var encryptedDir = Path.Combine(Path.GetDirectoryName(csProject.FilePath), ".encrypted");
                if (Directory.Exists(encryptedDir) == false)
                    Directory.CreateDirectory(encryptedDir);
                var encryptedPath = Path.Combine(encryptedDir, $"{Name}.cs");
                File.WriteAllText(encryptedPath, bResponse.encrypted);

                if (string.IsNullOrEmpty(copyPath) == false
                    && Directory.Exists(copyPath))
                {
                    var copyOutputPath = Path.Combine(copyPath, $"{Name}.cs");
                    File.WriteAllText(copyOutputPath, bResponse.encrypted);
                }
            }

            OnBuilded?.Invoke(bResponse);
            OnChanged?.Invoke(this);
            SetCompilingState(false);
        }

        public void OnConfigChanged()
        {
            ReloadConfig();
            OnChanged?.Invoke(this);
        }

        public class PluginProjectData
        {
            public string Author;
            public string Description;

            public List<string> Modules = new List<string>();
            public string Name;
            public VersionNumber Version = new VersionNumber(0, 0, 0);

            public PluginProjectData(string name)
            {
                Name = Config.PluginTemplateDefault.Name.Replace("$name$", name);
                Author = Config.PluginTemplateDefault.Author.Replace("$name$", name);
                Description = Config.PluginTemplateDefault.Description.Replace("$name$", name);
                Version = Config.PluginTemplateDefault.Version;
            }
        }

        #region [Methods] Modules

        public void AddModule(string mName)
        {
            if (config.Modules.Contains(mName)) throw new Exception($"Module {mName} already exists in {Name} plugin!");

            config.Modules.Add(mName);
            OnModulesChanged?.Invoke(this);
            RequestGeneratedFile();
            SaveConfig();
        }

        public void RemoveModule(string mName)
        {
            if (config.Modules.Contains(mName) == false)
                throw new Exception($"Missing {mName} module in {Name} plugin!");

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
            using (var gfRequest = new GeneratedFileRequest
            {
                modules = config.Modules.ToList(),
                @namespace = $"Oxide.Plugins.{Name}",
                pluginname = Name
            })
            {
                Net.cl.SendRPC(RPCMessageType.GeneratedFileRequest, gfRequest);
            }
        }

        private static bool Compiling;

        private void SetCompilingState(bool compiling)
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
                MainForm.UpdateProgressBar(100, 100, ProgressBarStyle.Blocks);
            }
        }

        public void OnGeneratedFileResponse(string content)
        {
            var filename = Path.Combine(_Directory, $"{Name}.generated.cs");
            var exists = File.Exists(filename);
            File.WriteAllText(filename, content);
            if (exists == false) csProject.CompileAdd(filename);
        }

        #endregion

        #region [Methods] Config

        private void ReloadConfig()
        {
            var pluginNameDefault = Path.GetFileName(_Directory);
            if (File.Exists(DataFileName) == false)
            {
                config = new PluginProjectData(pluginNameDefault);
                SaveConfig();
            }

            try
            {
                config = JsonConvert.DeserializeObject<PluginProjectData>(File.ReadAllText(DataFileName));
            }
            catch
            {
                ConsoleSystem.LogError("PluginsProject: Invalid Config");
                config = new PluginProjectData(pluginNameDefault);
            }
        }

        private void SaveConfig()
        {
            if (config == null) throw new NullReferenceException("_Config is null!!!");

            File.WriteAllText(DataFileName, JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        #endregion
    }
}