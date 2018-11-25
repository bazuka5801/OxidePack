using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FubuCsProjFile;
using Mono.Cecil;
using OxidePack.Client.App;
using OxidePack.Client.Core.OxideDownloader;
using OxidePack.Client;
using SapphireEngine;
using Solution = OxidePack.Client.Core.MsBuildProject.Solution;

namespace OxidePack.Client
{
    public static class OPClientCore
    {
        public static Solution Solution { get; private set; }

        public static void Init()
        {
            Config.OnConfigLoaded += OnConfigLoaded;
        }

        private static void OnConfigLoaded()
        {
            if (File.Exists(Config.SolutionFile))
            {
                SetSolution(Solution.Load(Config.SolutionFile));
            }
        }

        public static void SetSolution(string path)
        {
            if (File.Exists(path))
                SetSolution(Solution.Load(Config.SolutionFile));
            else
                SetSolution((Solution)null);
        }
        public static void SetSolution(Solution solution)
        {
            Solution = solution;
        }

        public static void AddProject(string filename)
        {
            var projectRelativePath = FileUtils.GetRelativePath(Path.GetDirectoryName(filename), Solution.Directory);
            var projectName = Path.GetFileNameWithoutExtension(filename);
            
            Solution.AddProject(filename, projectName, projectRelativePath);
        }

        #region MyRegion

        

        #endregion

        #region MyRegion

        

        #endregion

        #region [Methods] Downloading Dlls

        public static async Task DownloadRustDLLs(Action<string, int> progress, Action completed)
        {
            Regex _rustFilesRegex = new Regex("RustDedicated_Data\\\\Managed\\\\.*.dll");
            var refPath = ".references-cache";
            var downloadConfig = new global::DepotDownloader.DownloadConfig()
            {
                AppID = 258550,
                DownloadAllPlatforms = false,
                InstallDirectory = refPath,
                UsingFileList = true,
                FilesToDownloadRegex = new List<Regex>() { _rustFilesRegex },
                SavePathProcessor = (path) => Path.Combine(refPath, Path.GetFileName(path))
            };

            void OnMessage(string type, object message)
            {
                ConsoleSystem.Log($"[{type}] "+message.ToString().Replace("{", "").Replace("}", ""));
            }

            void OnProgress(string message)
            {
                ConsoleSystem.Log($"[progress] {message}");
            }

            downloadConfig.OnMessageEvent += OnMessage;
            downloadConfig.OnReportProgressEvent += OnProgress;
            var downloader = new global::DepotDownloader.DepotDownloader(downloadConfig);
            await Task.Run(() => downloader.Download(true));
            downloadConfig.OnMessageEvent -= OnMessage;
            downloadConfig.OnReportProgressEvent -= OnProgress;
            downloader.ClearCache();
            Config.RustVersion = GetRustVersion(".references-cache/Assembly-CSharp.dll");
            MainForm.ReloadStatus();
        }

        public static async Task DownloadOxideDLLs(Action<string,int> progress, Action completed)
        {
//            ConsoleSystem.Log($"Oxide Version => {OxideDownloader.GetVersion()}");
            Config.OxideVersion = await OxideDownloader.Download(".references-cache", progress, completed);
            MainForm.ReloadStatus();
        }

        #endregion

        #region [Methods] Versioning

        public static string GetRustCurrentVersion() => Config.RustVersion;
        public static string GetOxideCurrentVersion() => Config.OxideVersion;

        public static string GetRustAvailableVersion()
        {
            Regex _asmCSharpRegex = new Regex("RustDedicated_Data\\\\Managed\\\\Assembly-CSharp.dll");
            var refPath = ".temp";
            var downloadConfig = new global::DepotDownloader.DownloadConfig()
            {
                AppID = 258550,
                DownloadAllPlatforms = false,
                InstallDirectory = refPath,
                UsingFileList = true,
                FilesToDownloadRegex = new List<Regex>() { _asmCSharpRegex },
                SavePathProcessor = (path) => Path.Combine(refPath, Path.GetFileName(path))
            };
            var downloader = new global::DepotDownloader.DepotDownloader(downloadConfig);
            downloader.Download(true);
            var version = GetRustVersion(".temp/Assembly-CSharp.dll");
            downloader.ClearCache();
            Directory.Delete(".temp", true);

            return version;
        }

        public static string GetOxideAvailableVersion()
        {
            return OxideDownloader.GetVersion();
        }

        private static string GetRustVersion(string assemblyCSharpPath)
        {
            using (AssemblyDefinition asmDef = AssemblyDefinition.ReadAssembly(assemblyCSharpPath))
            {
                var protocolType = asmDef.MainModule.GetType("Rust", "Protocol");
                Dictionary<string, int> versionNums = new Dictionary<string, int>()
                {
                    ["network"] = 0,
                    ["save"] = 0,
                    ["report"] = 0,
                };
                for (var i = 0; i < protocolType.Fields.Count; i++)
                {
                    var field = protocolType.Fields[i];
                    if (versionNums.ContainsKey(field.Name))
                        versionNums[field.Name] = (int) field.Constant;
                }
                return string.Join(".", versionNums.Values.ToArray());
            }
        }

        #endregion
    }
}