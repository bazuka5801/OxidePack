using System.Linq;
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
                            sb.Insert(0, $"{m.Identifier.Text}({string.Join(", ",m.ParameterList.Parameters.Select(p=>p.Type.ToString()+p.Identifier.Text))}).");
                            break;
                        case ClassDeclarationSyntax c:
                            sb.Insert(0, $"{c.Identifier.Text}.");
                            break;
                        case NamespaceDeclarationSyntax n:
                            sb.Insert(0, $"{n.Name}.");
                            break;
                        case IdentifierNameSyntax n:
                            sb.Insert(0, $"{n.Identifier.ToString()}.");
                            break;
                        case VariableDeclaratorSyntax n:
                            sb.Insert(0, $"{n.Identifier.ToString()}.");
                            break;
                        case PropertyDeclarationSyntax n:
                            sb.Insert(0, $"{n.Identifier.ToString()}.");
                            break;
                        case ParameterSyntax n:
                            sb.Insert(0, $"{n.Identifier.ToString()}.");
                            break;
                    }

                    node = node.Parent;
                }

                return sb.ToString().Trim('.');
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

        public static T FirstInDepth<T>(this SyntaxNode node)
            where T : SyntaxNode
        {
            return (T) node.DescendantNodes().FirstOrDefault(p => p is T);
        }
    }
}