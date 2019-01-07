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
        
        public MergeSession(string directory, List<string> files)
            : this(directory)
        {
            this.Files = files.ToArray();
        }
        
        public MergeSession(string directory)
        {
            this.Directory = directory;
        }

        public void LoadAllFiles()
        {
            this.Files = new DirectoryInfo(Directory).GetFiles("*.dll").Select(p=>p.FullName).ToArray();
        }
        
        public void Merge(string outputFile)
        {
            var repOptions = new RepackOptions()
            {
                InputAssemblies = Files,
                SearchDirectories = new []{ Directory },
                TargetPlatformVersion = "v4",
                TargetKind = ILRepack.Kind.Dll,
                OutputFile = outputFile
            };
            var r = new ILRepack(repOptions);
            r.Repack();
        }
    }
}