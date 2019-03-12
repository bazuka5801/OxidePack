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
        public Data.SourceFile Data;

        private SourceFile(Data.SourceFile data)
        {
            Data = data;
            Load(data);
        }

        public List<UsingDirectiveSyntax> Usings { get; private set; }
        public MemberDeclarationSyntax[] Members { get; private set; }

        public static SourceFile Create(Data.SourceFile data) => new SourceFile(data);

        public void Load(Data.SourceFile data)
        {
            var sTree = ParseCompilationUnit(data.content);
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
                throw new Exception($"Plugin Class not found in '{data.filename}' file");
                return;
            }

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