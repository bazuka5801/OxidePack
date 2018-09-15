using OxidePack.Client.Forms;
using System;
using System.Threading;
using System.Windows.Forms;
using OxidePack.Client.App;
using SapphireEngine;

namespace OxidePack.Client
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            InitializeApp();
            Application.EnableVisualStyles();
            Application.Run(new Auth());
        }
        
        private static void InitializeApp()
        {
            RunFramework();
        }

        private static void RunFramework()
        {
            Thread frameworkThread = new Thread(FrameworkWorker) { Name = "Framework", IsBackground = true };
            frameworkThread.Start();
        }

        private static void FrameworkWorker(object o)
        {
            Framework.Initialization<AppCore>(true);
        }
    }
}