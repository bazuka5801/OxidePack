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
        private (SyntaxList<MemberDeclarationSyntax> members, SyntaxList<UsingDirectiveSyntax> usings) GeneratedCache; 
        

        public string GetGeneratedFile(List<string> modulesNames)
        {
            var modules = modulesNames.Select(mName => ModuleMgr.GetModule(mName, out var module) ? module : null)
                .Where(p => p != null);

            var (classBody, usings) = GeneratedCache = ModuleMgr.CombineModules(modules);

            var generatedClass = ClassDeclaration(PluginName)
                .WithModifiers(
                    TokenList(
                        Token(PublicKeyword),
                        Token(PartialKeyword)))
                .WithMembers(classBody);

            var @namespace = NamespaceDeclaration(ParseName("Oxide.Plugins"))
                .WithUsings(usings)
                .AddMembers(generatedClass);

            _workspace.Options.WithChangedOption (CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format (@namespace, _workspace);
            return formattedCode.ToFullString();
        }
    }
}