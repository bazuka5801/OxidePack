using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public class SourceFile
    {
        public Data.SourceFile Data;
        public List<UsingDirectiveSyntax> Usings { get; private set; }
        public MemberDeclarationSyntax[] Members { get; private set; }

        public static SourceFile Create(Data.SourceFile data) => new SourceFile(data);
        
        private SourceFile(Data.SourceFile data)
        {
            this.Data = data;
            Load(data);
        }

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

            if (mainClass.CloseBraceToken.HasLeadingTrivia)
            {
                var lastElement = members[members.Count - 1];
                members[members.Count - 1] = lastElement.WithTrailingTrivia(lastElement.GetLeadingTrivia()
                    .AddRange(mainClass.CloseBraceToken.LeadingTrivia));
            }

            this.Members = members.ToArray();
        }
    }
}