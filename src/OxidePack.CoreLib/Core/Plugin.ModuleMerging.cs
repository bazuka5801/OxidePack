using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public partial class Plugin
    {
        private (SyntaxList<MemberDeclarationSyntax> members, SyntaxList<UsingDirectiveSyntax> usings) _generatedCache;


        public string GetGeneratedFile(List<string> modulesNames, string @namespace)
        {
            var modules = modulesNames.Select(mName => ModuleMgr.GetModule(mName, out var module) ? module : null)
                .Where(p => p != null);

            var (classBody, usings) = _generatedCache = ModuleMgr.CombineModules(modules);

            var generatedClass = ClassDeclaration(PluginName)
                .WithModifiers(
                    TokenList(
                        Token(PublicKeyword),
                        Token(PartialKeyword)))
                .WithMembers(classBody);

            var namespaceDeclaration = NamespaceDeclaration(ParseName(@namespace))
                .WithUsings(usings)
                .AddMembers(generatedClass);

            Workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(namespaceDeclaration, Workspace);
            return formattedCode.ToFullString();
        }
    }
}