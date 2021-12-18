using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using OxidePack.CoreLib;
using RustyCode.Core.Data;

namespace RustyCode.Core.Parsers
{
    public class PluginParser
    {
        public PluginMeta Meta;
        private PluginParser()
        {
        }

        public static PluginParser Create(byte[] content)
        {
            var parser = new PluginParser();
            parser.Parse(Encoding.UTF8.GetString(content));
            return parser;
        }

        private void Parse(string sourceCode)
        {
            var sFile = SourceFile.Create(sourceCode);
            var attribute = sFile.MainClass.AttributeLists.First().Attributes.First();
            var attrArgs = attribute.ArgumentList.Arguments;

            var name = sFile.MainClass.Identifier.ToString();
            var displayName = ((LiteralExpressionSyntax)attrArgs[0].Expression).Token.ToString().Trim('\"');
            var author = ((LiteralExpressionSyntax)attrArgs[1].Expression).Token.ToString().Trim('\"');
            var version = ((LiteralExpressionSyntax)attrArgs[2].Expression).Token.ToString().Trim('\"');

            Meta = new PluginMeta()
            {
                Name = name, Author = author, DisplayName = displayName, Version = version
            };
        }
    }
}