using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OxidePack.CoreLib
{
    public class IdentifierGenerator
    {
        public Dictionary<VariableDeclaratorSyntax, string> RenamedVariables { get; private set; }

        public Dictionary<MethodDeclarationSyntax, string> RenamedMethods { get; private set; }

        public Dictionary<ClassDeclarationSyntax, string> RenamedTypes { get; private set; }

        public Dictionary<PropertyDeclarationSyntax, string> RenamedProperties { get; set; }
        
        public Dictionary<EnumDeclarationSyntax, string> RenamedEnums { get; set; }
        
        public Dictionary<EnumMemberDeclarationSyntax, string> RenamedEnumMembers { get; set; }
        
        public Dictionary<ParameterSyntax, string> RenamedParameters { get; set; }
        
        private List<string> _existingNames = new List<string>();

        private int KeywordStartCount = 1;
        private int LastKeywordIndex => _IdentifierKeywords.Length - 1;
        private List<int> currentKey = new List<int>();
        private int pointer = 0; 
        
        public string GetNewIdentifier()
        {
            if (currentKey.Count <= 0)
            {
                currentKey = new List<int>();
                for (int i = 0; i < KeywordStartCount; i++)
                    currentKey.Add(0);
            }
            if (currentKey.Last() == LastKeywordIndex)
            {
                for (var i = 0; i < currentKey.Count; i++)
                    currentKey[i] = 0;
                currentKey.Add(0);
                pointer = 0;
            }
            else
            {
                if (currentKey[pointer] == LastKeywordIndex)
                    pointer++;
                currentKey[pointer]++;
            }
            return string.Concat(currentKey.Select(p => _IdentifierKeywords[p]));
        }
        private static readonly string[] _cSharpKeywords = new string[]
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char",
            "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float",
            "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out",
            "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
            "ushort", "using", "virtual", "void", "volatile", "while"
        };

        private static readonly string[] _IdentifierKeywords = new string[]
        {
//            "SerezhaNeDelaet", "GraimSlivaet", "FacePunchNeDruzitSoMnoi",
//            "WulfNeHochetDelatDLLplugini", "WulfDelayDLLplugini", "GraimSolieshHanaTebe", "VsePidorasi",
//            "HouganTutProsto"
"a", "b", "c"
        };

        StringBuilder sb = new StringBuilder(512);

        HashSet<string> ExistingIdentifiers = new HashSet<string>();
        static Random rand = new Random();
        
        
        string GenerateFuckIdentifier(int length = 512)
        {
            string str = null;
            do
            {
                sb.Clear();
                for (int i = 0; i < length; i++)
                {
//                    sb.Append(chars[rand.Next(0, chars.Count)]);
                    sb.Append(rand.Next(0, 2) > 0 ? "I" : "l");
                }

                str = sb.ToString();
            } while (ExistingIdentifiers.Contains(str));

            ExistingIdentifiers.Add(str);
            return str;
        }
        
        
        public IdentifierGenerator()
        {
            RenamedVariables = new Dictionary<VariableDeclaratorSyntax, string>();
            RenamedMethods = new Dictionary<MethodDeclarationSyntax, string>();
            RenamedTypes = new Dictionary<ClassDeclarationSyntax, string>();
            RenamedProperties = new Dictionary<PropertyDeclarationSyntax, string>();
            RenamedEnums = new Dictionary<EnumDeclarationSyntax, string>();
            RenamedEnumMembers = new Dictionary<EnumMemberDeclarationSyntax, string>();
            RenamedParameters = new Dictionary<ParameterSyntax, string>();
//            AddCharRange(12544,12591);
//            AddCharRange(12592,12687);
//            AddCharRange(13056,13311);
        }

        public string GetNextName(SyntaxNode node)
        {
            string nextName = GenerateFuckIdentifier();
            _existingNames.Add(nextName);
            AddToDictionary(node);
            return nextName;
        }

        private void AddToDictionary(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                    RenamedMethods.Add((MethodDeclarationSyntax)node, LastName);
                    break;
                case SyntaxKind.VariableDeclarator:
                    RenamedVariables.Add((VariableDeclaratorSyntax)node, LastName);
                    break;
                case SyntaxKind.ClassDeclaration:
                    RenamedTypes.Add((ClassDeclarationSyntax)node, LastName);
                    break;
                case SyntaxKind.PropertyDeclaration:
                    RenamedProperties.Add((PropertyDeclarationSyntax)node, LastName);
                    break;
                case SyntaxKind.EnumDeclaration:
                    RenamedEnums.Add((EnumDeclarationSyntax)node, LastName);
                    break;
                case SyntaxKind.EnumMemberDeclaration:
                    RenamedEnumMembers.Add((EnumMemberDeclarationSyntax)node, LastName);
                    break;
                case SyntaxKind.Parameter:
                    RenamedParameters.Add((ParameterSyntax)node, LastName);
                    break;
            }
        }

        private string LastName => _existingNames.LastOrDefault();


        private static ulong _simpleNameCounter = 0;
        public static string GetSimpleName()
        {
            return $"generated_{_simpleNameCounter++}";
        } 
    }
}