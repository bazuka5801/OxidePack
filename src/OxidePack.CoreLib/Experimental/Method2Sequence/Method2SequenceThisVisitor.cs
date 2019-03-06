using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Experimental.Method2Sequence
{
    public class Method2SequenceThisVisitor : CSharpSyntaxWalker
    {
        public class Results
        {
            /// <summary>
            ///   Identifiers where need puts _this 
            /// </summary>
            public List<IdentifierNameSyntax> IdentifiersNeedsThis;
            
            /// <summary>
            ///   This expressions to replace with _this 
            /// </summary>
            public List<ThisExpressionSyntax> ThisExpressions;

            public Results()
            {
                IdentifiersNeedsThis = new List<IdentifierNameSyntax>();
                ThisExpressions = new List<ThisExpressionSyntax>();
            }

            public void Merge(Results results)
            {
                IdentifiersNeedsThis.AddRange(results.IdentifiersNeedsThis);
                results.IdentifiersNeedsThis.Clear();
                ThisExpressions.AddRange(results.ThisExpressions);
                results.ThisExpressions.Clear();
            }
        }
        
        
        private ClassDeclarationSyntax _baseClass;
        private SemanticModel _semanticModel;
        private string _baseContainer;
        private Results _results;
        
        public Results Walk(ClassDeclarationSyntax baseClass, string baseContainer, SemanticModel semanticModel)
        {
            _baseClass = baseClass;
            _semanticModel = semanticModel;
            _baseContainer = baseContainer;
            _results = new Results();

            Visit(baseClass);
            
            return _results;
        }
        
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node.GetParent<ConstructorDeclarationSyntax>() != null)
            {
                base.VisitIdentifierName(node);
                return;
            }

//            if (node.ToString() == "_this" &&
//                node.Parent is MemberAccessExpressionSyntax ma &&
//                ma.Expression.ToString() == "_this")
//            {
//                if (ma.GetParent<MemberAccessExpressionSyntax>() != null)
//                {
//                    base.VisitIdentifierName(node);
//                    return;
//                }
//            }

            if (node.GetParent<MemberAccessExpressionSyntax>()?.Name == node)
            {
                base.VisitIdentifierName(node);
                return;
            }
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol != null)
            {
                if (symbol.OriginalDefinition?.Kind != SymbolKind.NamedType)
                {
                    if (node.GetParent<MethodDeclarationSyntax>()?.Identifier.ToString().StartsWith("Unload_execute") == true)
                    {
                        
                    }
                    if (symbol.ContainingType?.ToString().StartsWith(_baseContainer) == true)
                    {
                        if ((symbol.Kind == SymbolKind.Local || symbol.Kind == SymbolKind.Field ||
                             symbol.Kind == SymbolKind.Method) &&
                            symbol.ContainingSymbol?.Name == symbol.ContainingType?.Name && symbol.IsStatic == false)
                        {
                            _results.IdentifiersNeedsThis.Add(node);
                        }
                    }
                    else
                    {
                        if (node.GetParent<MemberAccessExpressionSyntax>()?.Expression == node ||
                            (node.GetParent<InvocationExpressionSyntax>() != null && (symbol.Kind == SymbolKind.Method || symbol.Kind == SymbolKind.Field)))
                        {
                            _results.IdentifiersNeedsThis.Add(node);
                        }
                    }
                }
            }
            else
            {
                
            }

            base.VisitIdentifierName(node);
        }

        public override void VisitThisExpression(ThisExpressionSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol != null)
            {
                if (symbol.Kind == SymbolKind.Parameter &&
                    node.GetParent<MethodDeclarationSyntax>() != null)
                {
                    _results.ThisExpressions.Add(node);
                }
                    
            }
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax mClass)
        {
            if (mClass == _baseClass)
            {
                base.VisitClassDeclaration(mClass);
                return;
            }
            
            var visitor = new Method2SequenceThisVisitor();
            _results.Merge(visitor.Walk(mClass, _baseContainer, _semanticModel));
        }
    }
}