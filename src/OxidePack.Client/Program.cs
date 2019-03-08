using OxidePack.Client;
using System;
using System.IO;
using OxidePack.Client.App;
using SapphireEngine;

internal class Bootstrap
{
    private static int Initialization(string arg)
    {
        try
        {
        Program.Main(new string[0]);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            File.AppendAllText("error.log", e.Message+"\n"+e.StackTrace+"\n");
            throw;
        }

        return -1;
    }
}
namespace OxidePack.Client
{
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (KillIfStarted()) return;
            Framework.Initialization<AppCore>();
        }

        private static bool KillIfStarted()
        {
//            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location))
//                    .Count() > 1)
//            {
//                Process.GetCurrentProcess().Kill();
//                return true;
//            }

            return false;
        }
    }
}