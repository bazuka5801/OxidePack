using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class LocalsVisitorResults
    {
        /// <summary>
        ///     Method - [locName, locType]
        /// </summary>
        public Dictionary<string, List<(string locName, TypeSyntax locType)>> MethodsLocals;


        public LocalsVisitorResults()
        {
            MethodsLocals = new Dictionary<string, List<(string locName, TypeSyntax locType)>>();
        }

        public bool GetLocals(MethodDeclarationSyntax method, string parentClass,
            out List<(string locName, TypeSyntax locType)> locals) =>
            MethodsLocals.TryGetValue($"{parentClass}.{method.Identifier.Text}", out locals);
    }
}