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

        private AdhocWorkspace _workspace = new AdhocWorkspace();
        private List<PortableExecutableReference> _references;

        public PluginEncryptor(EncryptorOptions options = null, string[] ignoredIdentifiers = null, string[] ignoredComments = null)
        {
            Options = options ?? new EncryptorOptions();
            IgnoredIdentifiers = ignoredIdentifiers?.ToList() ?? new List<string>();
            IgnoredComments = ignoredComments?.ToList() ?? new List<string>();
            _references = Directory.GetFiles("references")
                .Select(path => MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(),path)))
                .ToList();
            
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        }

        public string MinifyFromString(string csharpCode)
        {
            var sourceTree = CSharpSyntaxTree.ParseText(csharpCode);
            var project = _workspace.AddProject("MinifierProject", LanguageNames.CSharp);
            project = project.WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            project = project.AddMetadataReferences(_references);
            _workspace.TryApplyChanges(project.Solution);
            var document = _workspace.AddDocument(project.Id, "Doc", sourceTree.GetText());
            _workspace = Encrypt(_workspace);

//            var resultTree = _workspace.CurrentSolution.Projects.First()
//                .Documents.First().GetSyntaxTreeAsync().Result;
//            CompilationUnitSyntax root = resultTree.GetCompilationUnitRoot();
//            root = root.NormalizeWhitespace();
//            var formattedCode = Formatter.Format (root, _workspace);
            return _workspace.CurrentSolution.Projects.First()
                .Documents.First().GetTextAsync().Result.ToString();
        }

        private AdhocWorkspace Encrypt(AdhocWorkspace workspace)
        {
            var tokensMinifier = new TokensEncryptor(workspace, Options);
            workspace = tokensMinifier.MinifyIdentifiers();
            var membersShuffler = new MembersShuffler(workspace, Options);
            workspace = membersShuffler.Shuffle();
            if (Options.Secret)
            {
                var encodingChanger = new EncodingChanger(workspace, Options);
                workspace = encodingChanger.Process();
            }

            return workspace;
        }
    }
}