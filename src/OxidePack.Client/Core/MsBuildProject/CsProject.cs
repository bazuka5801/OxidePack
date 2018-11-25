using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using FubuCsProjFile;
using FubuCsProjFile.MSBuild;
using Mono.Cecil;
using OxidePack.Client.Forms;
using SapphireEngine;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class CsProject
    {
        public Solution Solution;
        public string Name, FilePath;
        public CsProjFile Project;
        private MSBuildItemGroup _references;
        private MSBuildItemGroup _compile;
        public List<String> ReferenceList { get; private set; }
        
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
            _compile = this.Project.BuildProject.ItemGroups
                           .FirstOrDefault(i => i.Items.Any(p => p.Name == "Compile")) ?? this.Project.BuildProject.AddNewItemGroup();
            if (_references == null)
            {
                MainForm.ShowMessage($"[CsProject] I can't find references itemgroup in '{Project.ProjectName}' project","Error");
            }
            else
            {
                (ReferenceList ?? (ReferenceList = new List<string>())).Clear();
                foreach (var refItem in _references.Items)
                {
                    if (refItem.HasMetadata("HintPath"))
                    {
                        var filePath = refItem.GetMetadata("HintPath");
                        var asmFileName = Path.GetFileName(filePath);
                        ReferenceList.Add(asmFileName);
                    }
                }
            }
        }

        public void CompileAdd(string filepath)
        {
            string relativePath = filepath;
            if (Path.GetPathRoot(this.Solution.Directory) == Path.GetPathRoot(filepath))
                relativePath = FileUtils.GetRelativePath(filepath, Path.GetDirectoryName(this.FilePath));

            var existingCompile = _compile.Items.FirstOrDefault(i => i.Include == relativePath);
            if (existingCompile == null)
            {
                _compile.AddNewItem("Compile", relativePath);
                Project.Save();
            }
        }
        
        public void CompileRemove(string filepath)
        {
            string relativePath = filepath;
            if (Path.GetPathRoot(this.Solution.Directory) == Path.GetPathRoot(filepath))
                relativePath = FileUtils.GetRelativePath(filepath, Path.GetDirectoryName(this.FilePath));

            var existingCompile = _compile.Items.FirstOrDefault(i => i.Include == relativePath);
            if (existingCompile != null)
            {
                existingCompile.Remove();
                Project.Save();
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

            var asmFileName = Path.GetFileName(filepath);
            if (ReferenceList.Contains(asmFileName) == false)
                ReferenceList.Add(asmFileName);
            Project.Save();
        }

        public void RemoveReferenceByFile(string filename)
        {
            if (_references == null)
            {
                throw new NullReferenceException("_references is null!");
            }

            foreach (var refItem in _references.Items)
            {
                if (refItem.HasMetadata("HintPath"))
                {
                    var filePath = refItem.GetMetadata("HintPath");
                    var asmFileName = Path.GetFileName(filePath);
                    if (asmFileName == filename)
                    {
                        ReferenceList.Remove(filename);
                        refItem.Remove();
                        Project.Save();
                        return;
                    }
                }
            }
            
            ConsoleSystem.LogError($"[CsProject] [{Name}] Failed to delete <{filename}> reference");
        }
    }
}