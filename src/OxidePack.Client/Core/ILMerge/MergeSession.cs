using System.Collections.Generic;
using System.IO;
using System.Linq;
using ILRepacking;

namespace OxidePack.Client.Core.ILMerger
{
    public class MergeSession
    {
        public string Directory;
        public string[] Files;
        public MergeSession(string directory)
        {
            this.Directory = directory;
            LoadFiles();
        }

        public void LoadFiles()
        {
            this.Files = new DirectoryInfo(Directory).GetFiles("*.dll").Select(p=>p.FullName).ToArray();
        }
        
        public void Merge()
        {
            var repOptions = new RepackOptions()
            {
                InputAssemblies = Files,
                SearchDirectories = new []{ Directory },
                OutputFile = "References.dll"
            };
            var r = new ILRepack(repOptions);
            r.Repack();
        }
    }
}