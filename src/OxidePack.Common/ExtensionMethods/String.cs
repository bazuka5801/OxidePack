using System;
using System.Security.Cryptography;
using System.Text;
using SapphireEngine;

namespace OxidePack
{
    public static class StringEx
    {
        public static string ToHexString(this byte[] arr)
        {
            StringBuilder hex = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static byte[] ToByteArray(this String s)
        {
            return Encoding.Unicode.GetBytes(s);
        }

        public static string ToSHA512(this string data) => ToSHA512(data.ToByteArray());
        public static string ToSHA512(this byte[] data)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(data).ToHexString();
            }
        }

        public static string NormalizePath(this string path) => path.Replace('/', '\\');
    }
}