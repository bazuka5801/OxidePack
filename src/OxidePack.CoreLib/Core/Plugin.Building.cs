using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using OxidePack.Data;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using OxidePack.CoreLib.Utils;
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

            var prepareEncrypt = request.encryptOptions?.enabled ?? false;
            
            var sourceList = request.sources.Select(SourceFile.Create).ToList();
            var (pluginBody, pluginUsings) = MergeSources(sourceList, prepareEncrypt);

            if (generated.members != null && generated.usings != null)
            {
                pluginBody.AddRange(generated.members);
                pluginUsings.AddRange(generated.usings);
            }

            var plugininfo = request.buildOptions.plugininfo;
            var attributes = new List<AttributeSyntax>()
            {
                Attribute(ParseName("Info"),
                    ParseAttributeArgumentList(
                        $"(\"{plugininfo.name}\", \"{plugininfo.author}\", \"{plugininfo.version}\")"))
            };
            if (string.IsNullOrEmpty(plugininfo.description) == false)
            {
                attributes.Add(
                    Attribute(ParseName("Description"), ParseAttributeArgumentList($"(\"{plugininfo.description}\")")));
            }
            var generatedClass = ClassDeclaration(PluginName)
                .WithModifiers(TokenList(Token(PublicKeyword)))
                .WithBaseList(
                    BaseList(SeparatedList<BaseTypeSyntax>(new[] {SimpleBaseType(ParseTypeName(plugininfo.baseclass))})))
                .WithMembers(pluginBody)
                .WithAttributeLists(List<AttributeListSyntax>(new[]
                {
                    AttributeList(SeparatedList(attributes))
                }));
            
            var @namespace = NamespaceDeclaration(ParseName("Oxide.Plugins"))
                .WithUsings(pluginUsings)
                .AddMembers(generatedClass);
            
            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (@namespace, _workspace);
            return formattedCode.ToFullString();
        }

        public (CompilerResults cResults, string output) EncryptWithCompiling(string source, EncryptorOptions encryptorOptions, bool forClient = false)
        {
            var encrypted = Encrypt(source, encryptorOptions, forClient = forClient);
            var cResults = CompileUtils.Compile(encrypted, forClient ? "client" : "server");
            return (cResults, encrypted);
        }

        public string Encrypt(string source, EncryptorOptions encryptorOptions, bool forClient = false)
        {
            PluginEncryptor encryptor =
                new PluginEncryptor(encryptorOptions, referencesFolder: forClient ? "client" : "server");
            var output = encryptor.MinifyFromString(source);
            return output;
        }
    }
}