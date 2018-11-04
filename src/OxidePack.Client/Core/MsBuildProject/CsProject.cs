using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using FubuCsProjFile;
using FubuCsProjFile.MSBuild;
using Mono.Cecil;
using OxidePack.Client.Forms;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class CsProject
    {
        public Solution Solution;
        public string Name, FilePath;
        public CsProjFile Project;
        private MSBuildItemGroup _references;
        
        public CsProject(Solution solution, CsProjFile project, string name, string relativeFilePath)
        {
            this.Solution = solution;
            this.Name = name;
            this.Project = project;
            SetFilePath(relativeFilePath);
        }

        public void SetFilePath(string relativeFilePath)
        {
            this.FilePath = Path.Combine(Solution.Directory, relativeFilePath);
            Load();
        }

        public void Load()
        {
            _references = this.Project.BuildProject.ItemGroups
                .FirstOrDefault(i => i.Items.Any(item => item.Name == "Reference"));
            if (_references == null)
            {
                MainForm.ShowMessage($"[CsProject] I can't find references itemgroup in '{Project.ProjectName}' project","Error");
            }
        }

        public void AddReference(string filepath)
        {
            string asmName, asmFullName;
            using (AssemblyDefinition asmDef = AssemblyDefinition.ReadAssembly(filepath))
            {
                asmName = asmDef.Name.Name;
                asmFullName = asmDef.FullName;
            }

            string asmRelativePath = filepath;
            if (Path.GetPathRoot(this.Solution.Directory) == Path.GetPathRoot(filepath))
                asmRelativePath = FileUtils.GetRelativePath(filepath, Path.GetDirectoryName(this.FilePath));
            
            // Check existing item for containing and fix it if invalid
            var existingReference = _references.Items.FirstOrDefault(reference => reference.Include.StartsWith(asmName));
            if (existingReference != null)
            {
                if (existingReference.Include != asmFullName)
                {
                    existingReference.Include = asmFullName;
                }
                string refPath = "";
                if (existingReference.HasMetadata("HintPath"))
                    refPath = existingReference.GetMetadata("HintPath");
                if (refPath != asmRelativePath)
                {
                    existingReference.SetMetadata("HintPath", asmRelativePath);
                }
            }
            else
            {
                _references.AddNewItem("Reference", asmFullName)
                    .SetMetadata("HintPath", asmRelativePath);
            }
            Project.Save();
        }
    }
}