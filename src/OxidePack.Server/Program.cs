using OxidePack.Server.App;
using SapphireEngine;

namespace OxidePack.Server
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Framework.Initialization<AppCore>();
        }
    }
}