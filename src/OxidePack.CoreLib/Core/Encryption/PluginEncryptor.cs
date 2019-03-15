using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;

namespace OxidePack.CoreLib
{
    public class PluginEncryptor
    {
        private readonly List<PortableExecutableReference> _references;
        private readonly string _referencesPath;
        private AdhocWorkspace _workspace = new AdhocWorkspace();

        public PluginEncryptor(EncryptorOptions options = null, string referencesFolder = "server",
            string[] ignoredIdentifiers = null, string[] ignoredComments = null)
        {
            Options = options ?? new EncryptorOptions();
            IgnoredIdentifiers = ignoredIdentifiers?.ToList() ?? new List<string>();
            IgnoredComments = ignoredComments?.ToList() ?? new List<string>();
            _referencesPath = $"references/{referencesFolder}";
            _references = Directory.GetFiles(_referencesPath)
                .Select(path => MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                .ToList();

            var _ = typeof(CSharpFormattingOptions);
        }

        public EncryptorOptions Options { get; }

        public List<string> IgnoredIdentifiers { get; }

        public List<string> IgnoredComments { get; }

        public string MinifyFromString(string csharpCode)
        {
            var sourceTree = CSharpSyntaxTree.ParseText(csharpCode);
            var project = _workspace.AddProject("MinifierProject", LanguageNames.CSharp);
            project = project.WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            project = project.AddMetadataReferences(_references);
            _workspace.TryApplyChanges(project.Solution);
            var document = _workspace.AddDocument(project.Id, "Doc", sourceTree.GetText());
            _workspace = Encrypt(_workspace);
            return _workspace.CurrentSolution.Projects.First()
                .Documents.First().GetTextAsync().Result.ToString();
        }

        private AdhocWorkspace Encrypt(AdhocWorkspace workspace)
        {
            var tokensMinifier = new TokensEncryptor(workspace, Options);
            workspace = tokensMinifier.MinifyIdentifiers();
            var spaghettiGenerator = new Method2Depth.Method2Depth();
            workspace = spaghettiGenerator.ProcessWorkspace(workspace, _referencesPath, Options);
            var membersShuffler = new MembersShuffler(workspace, Options);
            workspace = membersShuffler.Shuffle();
            if (Options.Encoding)
            {
                var encodingChanger = new EncodingChanger(workspace, Options);
                workspace = encodingChanger.Process();
            }

            return workspace;
        }
    }
}