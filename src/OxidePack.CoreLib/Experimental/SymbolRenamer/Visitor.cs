using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;

namespace OxidePack.CoreLib.Experimental.SymbolRenamer
{
    public class VisitorResults
    {
        public List<RenameSymbol> RenameSymbols;
    }

    public class RenameSymbol
    {
        public int StartPosition;
        public int Length;
            
        public string OriginalName;
        public string Name;

        public override string ToString() => $"{OriginalName} => {Name}";
    }
    
    public class Visitor : CSharpSyntaxWalker
    {
        private Dictionary<string, string> _symbolNames;
        private List<RenameSymbol> _renameSymbols;
        
        private HashSet<int> StartPositions = new HashSet<int>();
        
        private VisitorResults _results;
        private SemanticModel  _semanticModel;

        public Visitor() : base(SyntaxWalkerDepth.StructuredTrivia)
        {
        }
        
        public VisitorResults Walk(SyntaxNode node, SemanticModel semanticModel)
        {
            _results = new VisitorResults();
            _semanticModel = semanticModel;
            _symbolNames = new Dictionary<string, string>();
            _renameSymbols = new List<RenameSymbol>();
            
            Visit(node);

            _results.RenameSymbols = _renameSymbols.OrderByDescending(p => p.StartPosition).ToList();
            return _results;
        }

        public override void VisitToken(SyntaxToken token)
        {
            base.VisitToken(token);
            if (token.IsKind(SyntaxKind.IdentifierToken) == false) return;
            var node = token.Parent;
            var span = token.Span;
            if (StartPositions.Contains(span.Start)) return;
            if (node.Parent is NamespaceDeclarationSyntax) return;
            
            ProcessIdentifierNode(node, token.ToString(), span);
        }



        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            base.VisitIdentifierName(node);
            if (node is QualifiedNameSyntax)
            if (StartPositions.Contains(node.SpanStart)) return;
            ProcessIdentifierNode(node, node.ToString(), node.Span);
        }
        
        public void ProcessIdentifierNode(SyntaxNode node, string identifier, TextSpan span)
        {
            var symbol = _semanticModel.GetSymbolInfo(node).Symbol ?? _semanticModel.GetDeclaredSymbol(node) ?? _semanticModel.GetTypeInfo(node).Type;

            string assembly = symbol?.ContainingAssembly?.MetadataName;
            if (assembly == null)
            {
                return;
            }

            if (symbol.IsOverride)
            {
                var overriddenMethod = (symbol as IMethodSymbol)?.OverriddenMethod;
                if (overriddenMethod != null && overriddenMethod.ContainingAssembly?.MetadataName == "CoreLib")
                    symbol = overriddenMethod;
                else
                    return;
            }

            if (node.ToString() == "NpcData")
            {
                
            }
            if (node is ConstructorDeclarationSyntax)
            {
                symbol = symbol.ContainingType;
            }
            
            if (assembly == "CoreLib")
            {
                var text = symbol.FullPath();
                if (_symbolNames.TryGetValue(text, out var name) == false)
                    _symbolNames[text] = name = IdentifierGenerator.GetSimpleName();
                
                _renameSymbols.Add(new RenameSymbol()
                {
                    StartPosition = span.Start,
                    Length = span.End - span.Start,
                    OriginalName = identifier,
                    Name = name
                });
                
                StartPositions.Add(span.Start);
            }
        }
    }
}