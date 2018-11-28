using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using OxidePack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public partial class Plugin
    {
        private static List<PortableExecutableReference> _references;
        private static PortableExecutableReference _mscorlib;

        #region [Static] Constructor
        static Plugin()
        {
            _mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            ReloadReferences();
            
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
        }
        #endregion

        #region [Static] [Method] ReloadReferences
        private static void ReloadReferences()
        {
            if (Directory.Exists("references") == false)
                Directory.CreateDirectory("references");
            _references = Directory.GetFiles("references")
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();
        }
        #endregion
        
        private static AdhocWorkspace _workspace = new AdhocWorkspace();

        public string Build(BuildRequest request)
        {
            var generated = GeneratedCache;

            var sourceList = request.sources.Select(SourceFile.Create).ToList();
            var (pluginBody, pluginUsings) = MergeSources(sourceList);

            if (generated.members != null && generated.usings != null)
            {
                pluginBody.AddRange(generated.members);
                pluginUsings.AddRange(generated.usings);
            }
            
            var generatedClass = ClassDeclaration(PluginName)
                .WithModifiers(
                    TokenList(
                        Token(PublicKeyword),
                        Token(PartialKeyword)))
                .WithMembers(pluginBody);
            
            var @namespace = NamespaceDeclaration(ParseName("Oxide.Plugins"))
                .WithUsings(pluginUsings)
                .AddMembers(generatedClass);
            
            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (@namespace, _workspace);
            return formattedCode.ToFullString();
        }
    }
}