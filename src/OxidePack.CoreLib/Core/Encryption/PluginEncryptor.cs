using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace OxidePack.CoreLib
{
    public class PluginEncryptor
    {
        public EncryptorOptions Options { get; private set; }

        public List<string> IgnoredIdentifiers { get; private set; }

        public List<string> IgnoredComments { get; private set; }

        private static AdhocWorkspace _workspace = new AdhocWorkspace();
        private static PortableExecutableReference _mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private List<PortableExecutableReference> _references;

        public PluginEncryptor(EncryptorOptions options = null, string[] ignoredIdentifiers = null, string[] ignoredComments = null)
        {
            Options = options ?? new EncryptorOptions();
            IgnoredIdentifiers = ignoredIdentifiers?.ToList() ?? new List<string>();
            IgnoredComments = ignoredComments?.ToList() ?? new List<string>();
            _references = Directory.GetFiles("references")
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();
            
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        }

        public string MinifyFromString(string csharpCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);
            var project = _workspace.AddProject("MinifierProject", LanguageNames.CSharp);
            project = project.AddMetadataReference(_mscorlib);
            project = project.AddMetadataReferences(_references);
            _workspace.TryApplyChanges(project.Solution);
            var document = _workspace.AddDocument(project.Id, "Doc", syntaxTree.GetText());
            _workspace = Encrypt(_workspace);
            
            return _workspace.CurrentSolution.Projects.First()
                .Documents.First().GetTextAsync().Result.ToString();
        }

        private AdhocWorkspace Encrypt(AdhocWorkspace workspace)
        {
            var tokensMinifier = new TokensEncryptor(workspace, Options);
            workspace = tokensMinifier.MinifyIdentifiers();
            var membersShuffler = new MembersShuffler(workspace, Options);
            workspace = membersShuffler.Shuffle();
            return workspace;
        }
    }
}