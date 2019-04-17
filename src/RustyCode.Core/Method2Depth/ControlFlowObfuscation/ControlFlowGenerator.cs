using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OxidePack.CoreLib.Method2Depth.ControlFlowObfuscation
{
    public static class ControlFlowGenerator
    {
        private static readonly AdhocWorkspace Workspace = new AdhocWorkspace();
        public static Random rand = new Random();


        public static string Process(string source)
        {
            ParseCompilationUnit(source);
            var tree = CSharpSyntaxTree.ParseText(source);
            var root = tree?.GetCompilationUnitRoot();
            var compilation = CSharpCompilation.Create("CoreLib")
                .AddReferences(Directory.GetFiles("references/")
                    .Select(path =>
                        MetadataReference.CreateFromFile(Path.Combine(Directory.GetCurrentDirectory(), path)))
                    .ToList())
                .AddSyntaxTrees(tree);
            var mainClass = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            var method = mainClass.DescendantNodes().OfType<MethodDeclarationSyntax>().First();

            var statements = method.Body.Statements.ToList();
            var members = new List<MemberDeclarationSyntax>();
            Generate(statements, members, null);
            root = root.ReplaceNode(mainClass,
                mainClass.ReplaceNode(method, method.WithBody(Block(statements))).AddMembers(members.ToArray()));

            root = root.NormalizeWhitespace();
            Workspace.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            var formattedCode = Formatter.Format(root, Workspace);
            return formattedCode.ToFullString();
        }

        public static void Generate(List<StatementSyntax> statements, List<MemberDeclarationSyntax> members,
            StatementSyntax endStatement)
        {
            var sections = new List<SwitchSectionSyntax>();

            var varName = IdentifierGenerator.GetSimpleName();
            var caseNext = GetNextCase();
            var forStatements = statements.ToList();
            statements.Clear();
            statements.Insert(0, ParseStatement($"{varName} = {Encrypt(caseNext.result)};"));
            statements.Add(ParseStatement("START:"));
            for (var i = 0; i < forStatements.Count; i++)
            {
                var statementSyntax = forStatements[i];
                var caseCurrent = caseNext;
                caseNext = GetNextCase();

                var caseStatements = new List<StatementSyntax>
                {
                    statementSyntax,
                    ParseStatement($"{varName} = {caseNext.result};"),
                    ParseStatement("goto START;")
                };
                sections.Add(SwitchSection(
                    List(new SwitchLabelSyntax[] {CaseSwitchLabel(ParseExpression(caseCurrent.caseText))}),
                    List(caseStatements)));
            }

            sections.Add(SwitchSection(
                List(new SwitchLabelSyntax[] {CaseSwitchLabel(ParseExpression(caseNext.caseText))}),
                List(new StatementSyntax[] {BreakStatement()})));

            sections.Shuffle();
            members.Add(FieldDeclaration(VariableDeclaration(ParseTypeName("int"),
                    SeparatedList(new[] {VariableDeclarator(Identifier(varName))})))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword))));
            var swtch = SwitchStatement(ParseExpression($"{varName}"), List(sections));
            statements.Add(swtch);
            if (endStatement != null)
            {
                statements.Add(endStatement);
            }
        }

        public static (int result, string caseText) GetNextCase()
        {
            var num = rand.Next();
            var left = rand.Next(0, 2) == 1;
            var num2 = rand.Next(1, 5);
            num = num >> 5;
            num = num << 10;
            num = num >> 5;
            var asd = left ? num >> num2 : num << num2;
            return (num, left ? $"{asd} << {num2}" : $"{asd} >> {num2}");
        }

        public static string Encrypt(int res) => $"~{~res}";
    }
}