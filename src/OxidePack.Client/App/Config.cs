using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Threading;
using OxidePack.Data;

namespace OxidePack.Client.App
{
    public class Config : BaseConfig
    {
        public const string Version = "1.0.0";
        public const string Host       = "127.0.0.1";
        public const int    Port       = 10000;
        public const int    BufferSize = 512;
        public const string OxideURL = "https://github.com/theumod/uMod.Rust/releases/download/{tag}/Oxide.Rust.zip";
        public const string OxideVersionURL = "https://api.github.com/repos/theumod/uMod.Rust/releases/latest";

        public static string SolutionFile = "...";
        public static string RustVersion = "...";
        public static string OxideVersion = "...";

        public static PluginTemplate PluginTemplateDefault = new PluginTemplate();
        public class PluginTemplate
        {
            public string Name = "$name$";
            public string Author = "Username";
            public string Description = "";
            public VersionNumber Version = new VersionNumber(0, 0, 0);
        }
        
        public static DependenciesConfig Dependencies = new DependenciesConfig();
        public class DependenciesConfig
        {
            public List<string> SelectedFiles = new List<string>();
            public Boolean Bundle = false;
        }
    }
}