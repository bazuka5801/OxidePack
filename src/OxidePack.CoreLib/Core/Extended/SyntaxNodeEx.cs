using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib
{
    public static class SyntaxNodeEx
    {
        public static bool SequenceToRoot(this SyntaxNode one, SyntaxNode two)
        {
            while (true)
            {
                if (one == null && two == null)
                    return true;
                if (one == null || two == null)
                    return false;
                if (one.Kind() != two.Kind())
                    return false;
                one = one.Parent;
                two = two.Parent;
            }
        }
        
        public static string FullPath(this SyntaxNode node)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                while (node.Parent != null)
                {
                    switch (node)
                    {
                        case MethodDeclarationSyntax m:
                            sb.Insert(0, $"{m.Identifier.Text}.");
                            break;
                        case ClassDeclarationSyntax c:
                            sb.Insert(0, $"{c.Identifier.Text}.");
                            break;
                        case NamespaceDeclarationSyntax n:
                            sb.Insert(0, $"{n.Name}.");
                            break;
                    }

                    node = node.Parent;
                }

                return sb.ToString();
            }
            finally
            {
                sb.Clear();
            }
        }
        
        public static T GetParent<T>(this SyntaxNode node)
            where T : SyntaxNode
        {
            while (node.Parent != null)
            {
                node = node.Parent;
                if (node is T t) return t;
            }

            return default;
        }
        
        public static string GetFullParameter(this ParameterSyntax param)
        {
            string name = "";
            foreach (var modifier in param.Modifiers)
            {
                if (modifier.IsKind(SyntaxKind.RefKeyword))
                    name += "ref ";
                else if (modifier.IsKind(SyntaxKind.OutKeyword))
                    name += "out ";
            }

            return name + param.Identifier;
        }
    }
}