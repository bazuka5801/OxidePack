using System;
using System.Collections.Generic;
using System.Text;

namespace OxidePack.CoreLib.Utils
{
    public class StringUtils
    {
        private static readonly StringBuilder Sb = new StringBuilder();
        
        private static readonly HashSet<char> ExcludedItems = new HashSet<char>()
        {
            ' ',
            '\r',
            '\n',
            '@'
        };
        public static string ConvertToCodePoints(string item)
        {
            Sb.Clear();
            for(int i = 0 ; i < item.Length ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (ExcludedItems.Contains(item[i]))
                {
                    Sb.Append(item[i]);
                    continue;
                }

                if (item[i] == '\\' && char.ToLower(item[i + 1]) == 'u')
                {
                    Sb.Append(item[i]);
                    Sb.Append(item[i+1]);
                    Sb.Append(item[i+2]);
                    Sb.Append(item[i+3]);
                    Sb.Append(item[i+4]);
                    Sb.Append(item[i+5]);
                    i += 5;
                    continue;
                }
                int x = Char.ConvertToUtf32(item, i);
                Sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (Sb[Sb.Length - 1] != ' ')
                Sb.Append(' ');
            return Sb.ToString();
        }

        public static string ConvertStringLiteralText(string item)
        {
            var firstIndex = item.IndexOf('\"');
            var lastIndex = item.LastIndexOf('\"');

            if (firstIndex > -1)
                Sb.Append(item.Substring(0, firstIndex + 1));
            
            for(int i = firstIndex + 1 ; i < lastIndex ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (item[i] == ' ')
                {
                    Sb.Append(item[i]);
                    continue;
                }
                switch (item[i])
                {
                    case '\'': Sb.Append(@"\'"); continue;
                    case '\"': Sb.Append("\\\""); continue;
                    case '\\': Sb.Append(@"\\"); continue;
                    case '\0': Sb.Append(@"\0"); continue;
                    case '\a': Sb.Append(@"\a"); continue;
                    case '\b': Sb.Append(@"\b"); continue;
                    case '\f': Sb.Append(@"\f"); continue;
                    case '\n': Sb.Append(@"\n"); continue;
                    case '\r': Sb.Append(@"\r"); continue;
                    case '\t': Sb.Append(@"\t"); continue;
                    case '\v': Sb.Append(@"\v"); continue;
                }
                int x = Char.ConvertToUtf32(item, i);
                Sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (lastIndex > -1)
                Sb.Append(item.Substring(lastIndex, item.Length - lastIndex));
            return Sb.ToString();
        }
        public static string ConvertStringLiteral2(string item)
        {
            var firstIndex = item.IndexOf('\"');
            var lastIndex = item.LastIndexOf('\"');
            
            Sb.Append(item.Substring(0, firstIndex + 1));
            for(int i = firstIndex + 1 ; i < lastIndex ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (item[i] == ' ')
                {
                    Sb.Append(item[i]);
                    continue;
                }
                
                switch (item[i])
                {
                    case '\'': Sb.Append(@"\'"); continue;
                    case '\"': Sb.Append("\\\""); continue;
                    case '\\': Sb.Append(@"\\"); continue;
                    case '\0': Sb.Append(@"\0"); continue;
                    case '\a': Sb.Append(@"\a"); continue;
                    case '\b': Sb.Append(@"\b"); continue;
                    case '\f': Sb.Append(@"\f"); continue;
                    case '\n': Sb.Append(@"\n"); continue;
                    case '\r': Sb.Append(@"\r"); continue;
                    case '\t': Sb.Append(@"\t"); continue;
                    case '\v': Sb.Append(@"\v"); continue;
                }
                
                int x = Char.ConvertToUtf32(item, i);
                Sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (lastIndex > -1)
                Sb.Append(item.Substring(lastIndex, item.Length - lastIndex));
            return Sb.ToString();
        }

        private static string ToLiteral(string input) {
            StringBuilder literal = new StringBuilder(input.Length + 2);
            literal.Append("\"");
            for (var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '\'': Sb.Append(@"\'"); continue;
                    case '\"': Sb.Append("\\\""); continue;
                    case '\\': Sb.Append(@"\\"); continue;
                    case '\0': Sb.Append(@"\0"); continue;
                    case '\a': Sb.Append(@"\a"); continue;
                    case '\b': Sb.Append(@"\b"); continue;
                    case '\f': Sb.Append(@"\f"); continue;
                    case '\n': Sb.Append(@"\n"); continue;
                    case '\r': Sb.Append(@"\r"); continue;
                    case '\t': Sb.Append(@"\t"); continue;
                    case '\v': Sb.Append(@"\v"); continue;
                }
            }

            literal.Append("\"");
            return literal.ToString();
        }
    }
}