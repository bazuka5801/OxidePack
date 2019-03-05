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

            public Results()
            {
                IdentifiersNeedsThis = new List<IdentifierNameSyntax>();
            }

            public void Merge(Results results)
            {
                IdentifiersNeedsThis.AddRange(results.IdentifiersNeedsThis);
                results.IdentifiersNeedsThis.Clear();
            }
        }
        
        
        private ClassDeclarationSyntax _baseClass;
        private SemanticModel _semanticModel;
        private Results _results;
        
        public Results Walk(ClassDeclarationSyntax baseClass, SemanticModel semanticModel)
        {
            _baseClass = baseClass;
            _semanticModel = semanticModel;
            _results = new Results();

            Visit(baseClass);
            
            return _results;
        }
        
        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol != null)
            {
                if (symbol.OriginalDefinition?.Kind != SymbolKind.NamedType)
                {
                    if (symbol?.ContainingSymbol?.Name == _baseClass.Identifier.Text)
                    {
                        _results.IdentifiersNeedsThis.Add(node);
                    }
                }
            }

            base.VisitIdentifierName(node);
        }
        
        public override void VisitClassDeclaration(ClassDeclarationSyntax mClass)
        {
            if (mClass == _baseClass)
            {
                base.VisitClassDeclaration(mClass);
                return;
            }
            
            var visitor = new Method2SequenceThisVisitor();
            _results.Merge(visitor.Walk(mClass, _semanticModel));
        }
    }
}