using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class ThisVisitor : CSharpSyntaxWalker
    {
        private ClassDeclarationSyntax _baseClass;
        private string _baseContainer;
        private MethodsVisitorResults _methodsVisitorResults;
        private SemanticModel _semanticModel;
        private ThisVisitorResults _thisVisitorResults;

        public ThisVisitorResults Walk(ClassDeclarationSyntax baseClass, string baseContainer,
            MethodsVisitorResults methodsVisitorResults, SemanticModel semanticModel)
        {
            _baseClass = baseClass;
            _semanticModel = semanticModel;
            _baseContainer = baseContainer;
            _methodsVisitorResults = methodsVisitorResults;
            _thisVisitorResults = new ThisVisitorResults();

            Visit(baseClass);

            return _thisVisitorResults;
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.GetParent<ConstructorDeclarationSyntax>() != null)
            {
                base.VisitIdentifierName(node);
                return;
            }

            if (node.GetParent<MemberAccessExpressionSyntax>()?.Name == node ||
                node.GetParent<ConditionalAccessExpressionSyntax>()?.WhenNotNull.FirstInDepth<IdentifierNameSyntax>() ==
                node)
            {
                base.VisitIdentifierName(node);
                return;
            }

            if (node.GetParent<InitializerExpressionSyntax>() != null &&
                node.GetParent<AssignmentExpressionSyntax>()?.Left == node)
            {
                base.VisitIdentifierName(node);
                return;
            }

            var info = _semanticModel.GetSymbolInfo(node);

            var symbol = info.Symbol ?? info.CandidateSymbols.FirstOrDefault();
            if (symbol != null)
            {
                if (symbol.OriginalDefinition?.Kind != SymbolKind.NamedType && symbol.IsStatic == false)
                {
                    if (symbol.ContainingType?.ToString().StartsWith(_baseContainer) == true)
                    {
                        if (symbol.HasAnyKind(SymbolKind.Local, SymbolKind.Field, SymbolKind.Method,
                                SymbolKind.Property)
                            && symbol.ContainingSymbol?.Name == symbol.ContainingType?.Name)
                        {
                            _thisVisitorResults.IdentifiersNeedsThis.Add(node.Identifier);
                        }
                    }
                    else
                    {
                        var method = node.GetParent<InvocationExpressionSyntax>();

                        if (node.GetParent<MemberAccessExpressionSyntax>()?.Expression == node ||
                            method != null &&
                            symbol.HasAnyKind(SymbolKind.Method, SymbolKind.Field, SymbolKind.Property))
                        {
                            _thisVisitorResults.IdentifiersNeedsThis.Add(node.Identifier);
                        }
                    }
                }
            }

            base.VisitIdentifierName(node);
        }

        public override void VisitBaseExpression(BaseExpressionSyntax node)
        {
            _thisVisitorResults.IdentifiersNeedsThis.Add(node.Token);
            base.VisitBaseExpression(node);
        }

        public override void VisitGenericName(GenericNameSyntax node)
        {
            if (node.GetParent<MemberAccessExpressionSyntax>()?.Name == node)
            {
                base.VisitGenericName(node);
                return;
            }

            if (node.GetParent<InitializerExpressionSyntax>() != null &&
                node.GetParent<AssignmentExpressionSyntax>()?.Left == node)
            {
                base.VisitGenericName(node);
                return;
            }

            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol != null && symbol.IsKind(SymbolKind.Method) && symbol.IsStatic == false)
            {
                _thisVisitorResults.IdentifiersNeedsThis.Add(node.Identifier);
            }

            base.VisitGenericName(node);
        }

        public override void VisitThisExpression(ThisExpressionSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol != null)
            {
                if (symbol.Kind == SymbolKind.Parameter &&
                    node.GetParent<MethodDeclarationSyntax>() != null)
                {
                    _thisVisitorResults.ThisExpressions.Add(node);
                }
            }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node == _baseClass)
            {
                base.VisitClassDeclaration(node);
                return;
            }

            var visitor = new ThisVisitor();
            _thisVisitorResults.Merge(visitor.Walk(node, _baseContainer, _methodsVisitorResults, _semanticModel));
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var method = node.FullPath();
            if (_methodsVisitorResults.Methods.ContainsKey(method) == false)
            {
                return;
            }

            var identifier = IdentifierGenerator.GetSimpleName();
            _thisVisitorResults.ThisNames.Add(method, identifier);
            base.VisitMethodDeclaration(node);
        }
    }
}