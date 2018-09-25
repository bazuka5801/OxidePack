using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCsProjFile;
using SapphireEngine;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class Solution
    {
        public string Directory;
        public List<CsProject> CsProjects = new List<CsProject>();
        
        public Solution(string file)
        {
            Directory = Path.GetDirectoryName(file);
            ReloadProjects(file);
        }

        public void ReloadProjects(string file)
        {
            if (File.Exists(file) == false)
                return;
            
            FubuCsProjFile.Solution sln = FubuCsProjFile.Solution.LoadFrom(file);
            foreach (var csProjFile in sln.Projects)
            {
                ConsoleSystem.Log($"{csProjFile.ProjectName}");
                foreach (var itemGroup in csProjFile.Project.BuildProject.ItemGroups)
                {
                    ConsoleSystem.Log($"---ItemGroup");
                    foreach (var item in itemGroup.Items)
                    {
                        ConsoleSystem.Log($"------ {item.Name}");
                    }
                }
            }
            ConsoleSystem.Log(PathHelper.GetRelativePath("B:\\Work\\OxidePack\\build\\client", sln.Filename));
            
//            string pattern = Properties.Resources.MsBuildProjectRegex;
//            
//            using (var stream = File.OpenText(file))
//            {
//                while (stream.EndOfStream == false)
//                {
//                    string line = stream.ReadLine();
//                    if (string.IsNullOrEmpty(line)) 
//                        continue;
//                    
//                    var result = Regex.Match(line, pattern);
//                    if (result.Success)
//                    {
//                        var name = result.Groups["name"].Value;
//                        var csproj = result.Groups["csproj"].Value;
//                        ConsoleSystem.Log($"Found {name} project, path -> {csproj}");
//                    }
//                }
//            }
        }
    }
}