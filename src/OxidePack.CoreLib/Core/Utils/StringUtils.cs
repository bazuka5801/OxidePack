using System;
using System.Collections.Generic;
using System.Text;

namespace OxidePack.CoreLib.Utils
{
    public class StringUtils
    {
        private static StringBuilder sb = new StringBuilder();
        
        private static HashSet<char> ExcludedItems = new HashSet<char>()
        {
            ' ',
            '\r',
            '\n',
            '@'
        };
        public static string ConvertToCodePoints(string item)
        {
            sb.Clear();
            for(int i = 0 ; i < item.Length ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (ExcludedItems.Contains(item[i]))
                {
                    sb.Append(item[i]);
                    continue;
                }

                if (item[i] == '\\' && char.ToLower(item[i + 1]) == 'u')
                {
                    sb.Append(item[i]);
                    sb.Append(item[i+1]);
                    sb.Append(item[i+2]);
                    sb.Append(item[i+3]);
                    sb.Append(item[i+4]);
                    sb.Append(item[i+5]);
                    i += 5;
                    continue;
                }
                int x = Char.ConvertToUtf32(item, i);
                sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (sb[sb.Length - 1] != ' ')
                sb.Append(' ');
            return sb.ToString();
        }

        public static string ConvertStringLiteralText(string item)
        {
            var firstIndex = item.IndexOf('\"');
            var lastIndex = item.LastIndexOf('\"');

            if (firstIndex > -1)
                sb.Append(item.Substring(0, firstIndex + 1));
            
            for(int i = firstIndex + 1 ; i < lastIndex ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (item[i] == ' ')
                {
                    sb.Append(item[i]);
                    continue;
                }
                switch (item[i])
                {
                    case '\'': sb.Append(@"\'"); continue;
                    case '\"': sb.Append("\\\""); continue;
                    case '\\': sb.Append(@"\\"); continue;
                    case '\0': sb.Append(@"\0"); continue;
                    case '\a': sb.Append(@"\a"); continue;
                    case '\b': sb.Append(@"\b"); continue;
                    case '\f': sb.Append(@"\f"); continue;
                    case '\n': sb.Append(@"\n"); continue;
                    case '\r': sb.Append(@"\r"); continue;
                    case '\t': sb.Append(@"\t"); continue;
                    case '\v': sb.Append(@"\v"); continue;
                }
                int x = Char.ConvertToUtf32(item, i);
                sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (lastIndex > -1)
                sb.Append(item.Substring(lastIndex, item.Length - lastIndex));
            return sb.ToString();
        }
        public static string ConvertStringLiteral2(string item)
        {
            var firstIndex = item.IndexOf('\"');
            var lastIndex = item.LastIndexOf('\"');
            
            sb.Append(item.Substring(0, firstIndex + 1));
            for(int i = firstIndex + 1 ; i < lastIndex ; i += Char.IsSurrogatePair(item,i) ? 2 : 1)
            {
                if (item[i] == ' ')
                {
                    sb.Append(item[i]);
                    continue;
                }
                
                switch (item[i])
                {
                    case '\'': sb.Append(@"\'"); continue;
                    case '\"': sb.Append("\\\""); continue;
                    case '\\': sb.Append(@"\\"); continue;
                    case '\0': sb.Append(@"\0"); continue;
                    case '\a': sb.Append(@"\a"); continue;
                    case '\b': sb.Append(@"\b"); continue;
                    case '\f': sb.Append(@"\f"); continue;
                    case '\n': sb.Append(@"\n"); continue;
                    case '\r': sb.Append(@"\r"); continue;
                    case '\t': sb.Append(@"\t"); continue;
                    case '\v': sb.Append(@"\v"); continue;
                }
                
                int x = Char.ConvertToUtf32(item, i);
                sb.Append(string.Format("\\u{0:X4}", x));
            }

            if (lastIndex > -1)
                sb.Append(item.Substring(lastIndex, item.Length - lastIndex));
            return sb.ToString();
        }
        
        static string ToLiteral(string input) {
            StringBuilder literal = new StringBuilder(input.Length + 2);
            literal.Append("\"");
            for (var i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '\'': sb.Append(@"\'"); continue;
                    case '\"': sb.Append("\\\""); continue;
                    case '\\': sb.Append(@"\\"); continue;
                    case '\0': sb.Append(@"\0"); continue;
                    case '\a': sb.Append(@"\a"); continue;
                    case '\b': sb.Append(@"\b"); continue;
                    case '\f': sb.Append(@"\f"); continue;
                    case '\n': sb.Append(@"\n"); continue;
                    case '\r': sb.Append(@"\r"); continue;
                    case '\t': sb.Append(@"\t"); continue;
                    case '\v': sb.Append(@"\v"); continue;
                }
            }

            literal.Append("\"");
            return literal.ToString();
        }
    }
}