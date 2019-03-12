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
        private static readonly string[] CSharpKeywords =
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

        private static readonly string[] IdentifierKeywords =
        {
//            "SerezhaNeDelaet", "GraimSlivaet", "FacePunchNeDruzitSoMnoi",
//            "WulfNeHochetDelatDLLplugini", "WulfDelayDLLplugini", "GraimSolieshHanaTebe", "VsePidorasi",
//            "HouganTutProsto"
            "a", "b", "c"
        };

        private static readonly Random Rand = new Random();


        private static ulong _simpleNameCounter;

        private readonly HashSet<string> _existingIdentifiers = new HashSet<string>();

        private readonly List<string> _existingNames = new List<string>();

        private readonly int _keywordStartCount = 1;

        private readonly StringBuilder _sb = new StringBuilder(512);
        private List<int> _currentKey = new List<int>();
        private int _pointer;


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

        public Dictionary<VariableDeclaratorSyntax, string> RenamedVariables { get; }

        public Dictionary<MethodDeclarationSyntax, string> RenamedMethods { get; }

        public Dictionary<ClassDeclarationSyntax, string> RenamedTypes { get; }

        public Dictionary<PropertyDeclarationSyntax, string> RenamedProperties { get; set; }

        public Dictionary<EnumDeclarationSyntax, string> RenamedEnums { get; set; }

        public Dictionary<EnumMemberDeclarationSyntax, string> RenamedEnumMembers { get; set; }

        public Dictionary<ParameterSyntax, string> RenamedParameters { get; set; }
        private int LastKeywordIndex => IdentifierKeywords.Length - 1;

        private string LastName => _existingNames.LastOrDefault();

        public string GetNewIdentifier()
        {
            if (_currentKey.Count <= 0)
            {
                _currentKey = new List<int>();
                for (var i = 0; i < _keywordStartCount; i++)
                {
                    _currentKey.Add(0);
                }
            }

            if (_currentKey.Last() == LastKeywordIndex)
            {
                for (var i = 0; i < _currentKey.Count; i++)
                {
                    _currentKey[i] = 0;
                }

                _currentKey.Add(0);
                _pointer = 0;
            }
            else
            {
                if (_currentKey[_pointer] == LastKeywordIndex)
                {
                    _pointer++;
                }

                _currentKey[_pointer]++;
            }

            return string.Concat(_currentKey.Select(p => IdentifierKeywords[p]));
        }


        private string FuckIdentifier(int length = 512)
        {
            string str = null;
            do
            {
                _sb.Clear();
                for (var i = 0; i < length; i++)
                {
//                    sb.Append(chars[rand.Next(0, chars.Count)]);
                    _sb.Append(Rand.Next(0, 2) > 0 ? "I" : "l");
                }

                str = _sb.ToString();
            } while (_existingIdentifiers.Contains(str));

            _existingIdentifiers.Add(str);
            return str;
        }

        public string GetNextName(SyntaxNode node)
        {
            var nextName = FuckIdentifier();
            _existingNames.Add(nextName);
            AddToDictionary(node);
            return nextName;
        }

        private void AddToDictionary(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                    RenamedMethods.Add((MethodDeclarationSyntax) node, LastName);
                    break;
                case SyntaxKind.VariableDeclarator:
                    RenamedVariables.Add((VariableDeclaratorSyntax) node, LastName);
                    break;
                case SyntaxKind.ClassDeclaration:
                    RenamedTypes.Add((ClassDeclarationSyntax) node, LastName);
                    break;
                case SyntaxKind.PropertyDeclaration:
                    RenamedProperties.Add((PropertyDeclarationSyntax) node, LastName);
                    break;
                case SyntaxKind.EnumDeclaration:
                    RenamedEnums.Add((EnumDeclarationSyntax) node, LastName);
                    break;
                case SyntaxKind.EnumMemberDeclaration:
                    RenamedEnumMembers.Add((EnumMemberDeclarationSyntax) node, LastName);
                    break;
                case SyntaxKind.Parameter:
                    RenamedParameters.Add((ParameterSyntax) node, LastName);
                    break;
            }
        }

        public static string GetSimpleName() => $"generated_{_simpleNameCounter++}";
    }
}