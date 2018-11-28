
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace OxidePack.CoreLib.Utils
{
    public static class EditUtils
    {
        public static List<MemberDeclarationSyntax> InRegion(List<MemberDeclarationSyntax> memberList, string region)
        {
            if (memberList.Count > 0)
            {
                var list = memberList[0].GetLeadingTrivia().ToList();
                list.Insert(0, Trivia(RegionDirectiveTrivia(true)
                    .WithHashToken(Token(HashToken))
                    .WithRegionKeyword(Token(RegionKeyword))
                    .WithEndOfDirectiveToken(
                        Token(TriviaList(PreprocessingMessage($" {region}")),
                            EndOfDirectiveToken,
                            TriviaList()))));
                list.Insert(1, EndOfLine("\r\n"));
                memberList[0] = memberList[0].WithLeadingTrivia(list);

                var list2 = memberList[memberList.Count - 1].GetTrailingTrivia().ToList();
                list2.Add(Trivia(EndRegionDirectiveTrivia(true)
                    .WithHashToken(Token(HashToken))
                    .WithEndRegionKeyword(Token(EndRegionKeyword))));
                list2.Add(EndOfLine(""));
                memberList[memberList.Count - 1] = memberList[memberList.Count - 1].WithTrailingTrivia(list2);
            }

            return memberList;
        }

        public static MemberDeclarationSyntax AddTrailingNewLine(MemberDeclarationSyntax member)
        {
            var list = member.GetTrailingTrivia().ToList();
            list.Add(EndOfLine("\r\n"));
            return member.WithTrailingTrivia(list);
        }
    }
}