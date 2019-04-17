using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public class SourceFile
    {
        public string Content;

        private SourceFile(string content)
        {
            Content = content;
            Load(content);
        }

        public List<UsingDirectiveSyntax> Usings { get; private set; }
        public MemberDeclarationSyntax[] Members { get; private set; }

        public static SourceFile Create(string content) => new SourceFile(content);
        
        public ClassDeclarationSyntax MainClass { get; private set; }

        public void Load(string content)
        {
            var sTree = ParseCompilationUnit(content);
            Usings?.Clear();
            Usings = new List<UsingDirectiveSyntax>(sTree.Usings);

            var @namespace = sTree.Members
                .OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (@namespace != null)
            {
                Usings.AddRange(@namespace.Usings);
            }

            var mainClass = sTree.DescendantNodes(node => node.IsKind(SyntaxKind.ClassDeclaration) == false)
                .OfType<ClassDeclarationSyntax>().FirstOrDefault();

            if (mainClass == null)
            {
                throw new Exception($"Plugin Class not found");
                return;
            }

            MainClass = mainClass;

            var members = mainClass.Members.ToList();

            if (members.Count > 0 && mainClass.CloseBraceToken.HasLeadingTrivia)
            {
                var lastElement = members[members.Count - 1];
                members[members.Count - 1] = lastElement.WithTrailingTrivia(lastElement.GetLeadingTrivia()
                    .AddRange(mainClass.CloseBraceToken.LeadingTrivia));
            }

            Members = members.ToArray();
        }
    }
}