using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OxidePack.Server.App
{
    public class Config : BaseConfig
    {
        public static string Host = "127.0.0.1";
        public static int    Port = 10000;

        public static string Title = "Oxide Pack [online: {online}]";
    }
}