using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class ThisVisitorResults
    {
        /// <summary>
        ///     Identifiers where need puts _this
        /// </summary>
        public List<SyntaxToken> IdentifiersNeedsThis;

        /// <summary>
        ///     This expressions to replace with _this
        /// </summary>
        public List<ThisExpressionSyntax> ThisExpressions;

        /// <summary>
        ///     The Dictionary of method path and 'this'
        /// </summary>
        public Dictionary<string, string> ThisNames;

        public ThisVisitorResults()
        {
            IdentifiersNeedsThis = new List<SyntaxToken>();
            ThisExpressions = new List<ThisExpressionSyntax>();
            ThisNames = new Dictionary<string, string>();
        }

        public void Merge(ThisVisitorResults results)
        {
            IdentifiersNeedsThis.AddRange(results.IdentifiersNeedsThis);
            results.IdentifiersNeedsThis.Clear();

            ThisExpressions.AddRange(results.ThisExpressions);
            results.ThisExpressions.Clear();

            ThisNames.AddRange(results.ThisNames);
            results.ThisNames.Clear();
        }
    }
}