using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SapphireEngine;

using MemberList = Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax>;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib
{
    public class ModuleManifest
    {
        public string Name;
        public string Version;
        public string Description;
        
        public string[] Files;
    }
    
    public class Module
    {
        private DirectoryInfo _directory;
        private ModuleManifest _manifest;
        private String FullText;

        
        public List<UsingDirectiveSyntax> Usings = new List<UsingDirectiveSyntax>();
        public MemberDeclarationSyntax[] Members { get; private set; }
        public ModuleManifest Manifest => _manifest;
        
        public Module(string directory)
        {
            _directory = new DirectoryInfo(directory);
            LoadModule();
        }

        public void LoadModule()
        {
            var sw = Stopwatch.StartNew();
            
            var files = _directory.GetFiles();

            var manifestFile = files.FirstOrDefault(p => p.Name == "manifest.json");
            if (manifestFile == null)
            {
                throw new Exception($"Module Manifest not found in <{_directory.FullName}>");
                return;
            }

            _manifest = JsonConvert.DeserializeObject<ModuleManifest>(File.ReadAllText(manifestFile.FullName));
            
            // Вытягиваем все члены класса в один список
            List<MemberDeclarationSyntax> memberList = new List<MemberDeclarationSyntax>();
            
            foreach (var filename in _manifest.Files)
            {
                var file = Path.Combine(_directory.FullName, filename);
                var sourceText = File.ReadAllText(file);

                var sTree = ParseCompilationUnit(sourceText);
                Usings.AddRange(sTree.Usings);
                var mainClass = sTree.DescendantNodes(node => node.IsKind(SyntaxKind.ClassDeclaration) == false)
                    .OfType<ClassDeclarationSyntax>().FirstOrDefault();
                

                if (mainClass == null)
                {
                    throw new Exception($"Class not found in '{_manifest.Name}' module");
                    return;
                }

                var members = mainClass.Members.ToList();


                var lTriviaList = members[0].GetLeadingTrivia();
                lTriviaList = lTriviaList.Insert(0, EndOfLine($"// Module: {_manifest.Name}\n" +
                                                $"// File: {filename}\n" +
                                                $"\n"));
                members[0] = members[0].WithLeadingTrivia(lTriviaList);
                
                memberList.AddRange(members);
            }


            var list = memberList[0].GetLeadingTrivia().ToList();
            list.Insert(0, Trivia(RegionDirectiveTrivia( true)
                .WithHashToken(Token(HashToken))
                .WithRegionKeyword(Token(RegionKeyword)) 
                .WithEndOfDirectiveToken(
                    Token(TriviaList(PreprocessingMessage($" [Module] {_manifest.Name}")),
                        EndOfDirectiveToken,
                        TriviaList()) )));
            list.Insert(1, EndOfLine("\r\n"));
            memberList[0] = memberList[0].WithLeadingTrivia(list);

            var list2 = memberList[memberList.Count - 1].GetTrailingTrivia().ToList();
            list2.Add(Trivia(EndRegionDirectiveTrivia(true)
                .WithHashToken(Token(HashToken))
                .WithEndRegionKeyword(Token(EndRegionKeyword))));
            list2.Add(EndOfLine(""));
            memberList[memberList.Count - 1] = memberList[memberList.Count - 1].WithTrailingTrivia(list2);
            
            Members = ParseCompilationUnit(new SyntaxList<MemberDeclarationSyntax>(memberList).ToFullString()).NormalizeWhitespace().Members.ToArray();
            
            File.WriteAllText(Path.Combine(_directory.FullName, "result_test.cs"), new SyntaxList<MemberDeclarationSyntax>(Members).ToFullString());
            
            sw.Stop();
            ConsoleSystem.Log($"Loaded <{_manifest.Name}> module in {sw.Elapsed:s\\.fff} sec.");
        }

        public void InsertInClass(ref ClassDeclarationSyntax gClass)
        {
            gClass = gClass.AddMembers(Members);
        }
    }
}