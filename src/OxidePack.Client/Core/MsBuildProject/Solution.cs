using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCsProjFile;
using Mono.Cecil;
using SapphireEngine;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class Solution
    {
        public string Directory;
        public string Filename;
        public List<CsProject> CsProjects = new List<CsProject>();

        private readonly FubuCsProjFile.Solution _solution;

        private Solution(string file)
        {
            this.Filename = file;
            _solution = FubuCsProjFile.Solution.LoadFrom(file);
            Directory = Path.GetDirectoryName(file);
            ReloadProjects();
        }

        public static Solution Create(string file)
        {
            var directory = Path.GetDirectoryName(file);
            var filename = Path.GetFileName(file);
            FubuCsProjFile.Solution sln = FubuCsProjFile.Solution.CreateNew(directory, filename);
            sln.Save();
            return Load(file);
        }

        public static Solution Load(string file)
        {
            return new Solution(file);
        }

        public void ReloadProjects()
        {
            CsProjects.Clear();
            foreach (var csProjFile in _solution.Projects)
            {
                CsProjects.Add(new CsProject(this, csProjFile.Project, csProjFile.ProjectName, csProjFile.RelativePath));
            }

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

        public void TestReference()
        {
            var project = _solution.FindProject("Plugins");
            var referenceGroup = project.Project.BuildProject.ItemGroups
                .FirstOrDefault(i => i.Items.Any(item => item.Name == "Reference"));
            referenceGroup.AddNewItem("Reference", "Aga.Controls, Version=1.7.0.0, Culture=neutral, PublicKeyToken=fcc90fbf924463a3")
                .SetMetadata("HintPath","B:\\Work\\OxidePack\\build\\client\\net462\\Aga.Controls.dll");
            project.Project.Save();
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly("B:\\Work\\OxidePack\\build\\client\\net462\\Aga.Controls.dll");
            ConsoleSystem.Log(asm.FullName);
            asm.Dispose();
        }

        public void AddProject(string directory, string name, string relativePath)
        {
            var project = CsProjFile.CreateAtLocation(directory, name);
            project.Save();
            CsProjects.Add(new CsProject(this, project, project.ProjectName, relativePath));
            _solution.AddProject(this.Directory, project);
            _solution.Save();
        }
    }
}