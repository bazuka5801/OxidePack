using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace OxidePack.CoreLib
{
    public class PackProject
    {
        public PackOptions Options { get; private set; }
        
        private AdhocWorkspace _workspace;
        
        private static List<PortableExecutableReference> _references;
        private static PortableExecutableReference _mscorlib;
        private static String _referencesPath;
        
        static PackProject()
        {
            _mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        }
        
        public PackProject(PackOptions options)
        {
            this._workspace = new AdhocWorkspace();
            this.Options = options;
            _references = Directory.GetFiles("references")
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();
            
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        }

        public static void SetReferencesPath(string path)
        {
            _referencesPath = path;
            ReloadReferences();
        }

        public static void ReloadReferences()
        {
            _references = Directory.GetFiles("References")
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();
        }
    }
}