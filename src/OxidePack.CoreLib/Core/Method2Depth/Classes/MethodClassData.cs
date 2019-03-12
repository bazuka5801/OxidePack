using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib.Method2Depth
{
    public class MethodClassData
    {
        public MethodDeclarationSyntax declaration;
        public string getName;
        public string methodClassInitializeMethodName;
        public string methodClassMethodName;
        public string methodClassName;
        public string parentClass;
        public string pushName;

        public MethodClassData()
        {
            methodClassName = IdentifierGenerator.GetSimpleName();
            methodClassMethodName = IdentifierGenerator.GetSimpleName();
            methodClassInitializeMethodName = IdentifierGenerator.GetSimpleName();
            getName = IdentifierGenerator.GetSimpleName();
            pushName = IdentifierGenerator.GetSimpleName();
        }
    }
}