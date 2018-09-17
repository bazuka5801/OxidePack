using OxidePack.Client.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using OxidePack.Client.App;
using SapphireEngine;
using SapphireEngine.Functions;

namespace OxidePack.Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (KillIfStarted()) return;
            Framework.Initialization<AppCore>();
        }

        static bool KillIfStarted()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location))
                    .Count() > 1)
            {
                Process.GetCurrentProcess().Kill();
                return true;
            }

            return false;
        }
    }
}