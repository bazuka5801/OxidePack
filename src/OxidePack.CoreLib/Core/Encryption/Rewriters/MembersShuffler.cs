using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib
{
    public class MembersShuffler : CSharpSyntaxVisitor<SyntaxNode>
    {
        private static readonly HashSet<SyntaxKind> ShuffleKinds = new HashSet<SyntaxKind>
        {
            SyntaxKind.CompilationUnit,
            SyntaxKind.NamespaceDeclaration,
            SyntaxKind.ClassDeclaration
        };

        private readonly AdhocWorkspace _workspace;
        private EncryptorOptions _options;

        private SemanticModel _semanticModel;

        public MembersShuffler(AdhocWorkspace workspace, EncryptorOptions options)
        {
            _workspace = workspace;
            _options = options;
        }

        public AdhocWorkspace Shuffle()
        {
            foreach (var project in _workspace.CurrentSolution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    _semanticModel = document.GetSemanticModelAsync().Result;
                    var node = _semanticModel.SyntaxTree.GetRoot();

                    node = RecursiveVisit(node);
                    _workspace.TryApplyChanges(document.Project.Solution.WithDocumentSyntaxRoot(document.Id, node));
                }
            }

            return _workspace;
        }

        private SyntaxNode RecursiveVisit(SyntaxNode node)
        {
            foreach (var child in node.ChildNodes().ToList())
            {
                if (!ShuffleKinds.Contains(child.Kind()))
                {
                    continue;
                }

                node = node.ReplaceNode(child, RecursiveVisit(child));
            }

            node = Visit(node);
            return node;
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            var childs = node.Members.ToList();
            childs.Shuffle();
            var usings = node.Usings.ToList();
            usings.Shuffle();
            return node
                .WithMembers(List(childs))
                .WithUsings(List(usings));
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var childs = node.Members.ToList();
            childs.Shuffle();
            return node.WithMembers(List(childs));
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            var childs = node.Members.ToList();
            childs.Shuffle();
            return node.WithMembers(List(childs));
        }
    }
}