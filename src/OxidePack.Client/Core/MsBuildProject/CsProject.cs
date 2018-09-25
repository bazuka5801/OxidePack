using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace OxidePack.Client.Core.MsBuildProject
{
    public class CsProject
    {
        public Solution Solution;
        public string Name, FilePath;
        
        public CsProject(Solution solution, string name, string relativeFilePath)
        {
            this.Solution = solution;
            this.Name = name;
            SetFilePath(relativeFilePath);
        }

        public void SetFilePath(string relativeFilePath)
        {
            this.FilePath = Path.Combine(Solution.Directory, relativeFilePath);
            Load();
        }

        public void Load()
        {
            
        }
    }
}