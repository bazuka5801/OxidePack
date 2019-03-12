using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace OxidePack.CoreLib.Method2Depth
{
    public class FieldsRewritrer : CSharpSyntaxRewriter
    {
        private SyntaxGenerator _generator;

        public SyntaxNode Rewrite(SyntaxNode node, SyntaxGenerator generator)
        {
            _generator = generator;
            return Visit(node);
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node) =>
            _generator.WithAccessibility(node, Accessibility.Public);

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) =>
            _generator.WithAccessibility(node, Accessibility.Public);

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Parent.Parent is NamespaceDeclarationSyntax)
            {
                return node;
            }

            return _generator.WithAccessibility(node, Accessibility.Public);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
            return _generator.WithAccessibility(node, Accessibility.Public);
        }
    }
}