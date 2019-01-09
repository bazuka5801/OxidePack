using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using FubuCore.Configuration;
using FubuCsProjFile;
using FubuCsProjFile.MSBuild;
using Mono.Cecil;
using OxidePack.Client;
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

        public bool IsNewVersion;
        
        public CsProject(Solution solution, CsProjFile project, string name, string relativeFilePath)
        {
            this.Solution = solution;
            this.Name = name;
            this.Project = project;
            this.IsNewVersion = HasNewVersion();
            SetFilePath(relativeFilePath);
        }

        public bool HasNewVersion()
        {
            var firstElem = this.Project.BuildProject.doc["Project"];
            if (firstElem.Name == "Project" && firstElem.Attributes["Sdk"]?.Value == "Microsoft.NET.Sdk")
            {
                return true;
            }

            return false;
        }

        public void SetFilePath(string relativeFilePath)
        {
            this.FilePath = Path.Combine(Solution.Directory, relativeFilePath);
            Load();
        }

        public void Load()
        {
            if (IsNewVersion)
            {
                foreach (XmlElement node in this.Project.BuildProject.doc["Project"])
                {
                    if (node.FirstChild?.Name == "Reference" || node.IsEmpty)
                    {
                        var msBuildItemGroup = (MSBuildItemGroup)typeof(MSBuildProject).GetMethod("GetItemGroup", BindingFlags.NonPublic | BindingFlags.Instance)
                            .Invoke(this.Project.BuildProject, new object[] {node});
                        _references = msBuildItemGroup;
                        break;
                    }
                }

                if (_references == null)
                {
                    var node = this.Project.BuildProject.doc["Project"].AddElement("ItemGroup");
                    var msBuildItemGroup = (MSBuildItemGroup)typeof(MSBuildProject).GetMethod("GetItemGroup", BindingFlags.NonPublic | BindingFlags.Instance)
                        .Invoke(this.Project.BuildProject, new object[] {node});
                    _references = msBuildItemGroup;
                }
            }
            else
            {
            _references = this.Project.BuildProject.ItemGroups
                .FirstOrDefault(i => i.Items.Any(item => item.Name == "Reference"));
                
            }

            if (IsNewVersion == false)
            {
                _compile = this.Project.BuildProject.ItemGroups
                               .FirstOrDefault(i => i.Items.Any(p => p.Name == "Compile")) ??
                           this.Project.BuildProject.AddNewItemGroup();
            }

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
            if (IsNewVersion)
            {
                return;
            }
            
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
            if (IsNewVersion)
            {
                return;
            }
            
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
            if (filepath.Contains("*"))
            {
                var existingMultipleReference = _references.Items.FirstOrDefault(reference => reference.Include == (filepath));
                if (existingMultipleReference == null)
                {
                    var item = _references.AddNewItem("Reference", filepath);
                    item.SetMetadata("Private", "False");
                    Project.Save();
                }
                return;
            }
            
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

                existingReference.SetMetadata("Private", "False");
            }
            else
            {
                var item = _references.AddNewItem("Reference", asmFullName);
                item.SetMetadata("HintPath", asmRelativePath);
                item.SetMetadata("Private", "False");
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