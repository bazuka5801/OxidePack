using System;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OxidePack.Client.App
{
    public class Config : BaseConfig
    {
        public const string Host       = "127.0.0.1";
        public const int    Port       = 10000;
        public const int    BufferSize = 512;
        public const string OxideURL = "http://github.com/theumod/umod.rust/releases/download/latest/Oxide.Rust.zip";


        public static string SolutionFile = "...";
    }
}