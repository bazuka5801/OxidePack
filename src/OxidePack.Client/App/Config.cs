using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OxidePack.Data;

namespace OxidePack.Client.App
{
    public class Config : BaseConfig
    {
        public const int    BufferSize = 512;
        public const string OxideURL = "https://github.com/theumod/uMod.Rust/releases/download/{tag}/Oxide.Rust.zip";
        public const string OxideVersionURL = "https://api.github.com/repos/theumod/uMod.Rust/releases/latest";

        [JsonProperty("SolutionFile")]
        public static string SolutionFile = "...";
        [JsonProperty("RustVersion")]
        public static string RustVersion = "...";
        [JsonProperty("OxideVersion")]
        public static string OxideVersion = "...";

        [JsonProperty("PluginTemplateDefault")]
        public static PluginTemplate PluginTemplateDefault = new PluginTemplate();
        public class PluginTemplate
        {
            [JsonProperty("Name")]
            public string Name = "$name$";
            [JsonProperty("Author")]
            public string Author = "Username";
            [JsonProperty("Description")]
            public string Description = "";
            [JsonProperty("Version")]
            public VersionNumber Version = new VersionNumber(0, 0, 0);
        }
        
        // TODO: Extract it to class
        [JsonProperty("ProjectsConfig")]
        public static Dictionary<string, DependenciesConfig> ProjectsConfig = new Dictionary<string, DependenciesConfig>();
        
        public class DependenciesConfig
        {
            public List<string> SelectedFiles = new List<string>();
            public Boolean Bundle = false;
        }
    }
}