using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SapphireEngine;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class Solution
    {
        
        public List<CsProject> CsProjects = new List<CsProject>();
        
        public Solution(string file)
        {
            ReloadProjects(file);
        }

        public void ReloadProjects(string file)
        {
            if (File.Exists(file) == false)
                return;
            
            string pattern = Properties.Resources.MsBuildProjectRegex;
            
            using (var stream = File.OpenText(file))
            {
                while (stream.EndOfStream == false)
                {
                    string line = stream.ReadLine();
                    if (string.IsNullOrEmpty(line)) 
                        continue;
                    
                    var result = Regex.Match(line, pattern);
                    if (result.Success)
                    {
                        var name = result.Groups["name"].Value;
                        var csproj = result.Groups["csproj"].Value;
                        ConsoleSystem.Log($"Found {name} project, path -> {csproj}");
                    }
                }
            }
        }
    }
}