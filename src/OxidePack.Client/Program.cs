using OxidePack.Client.Forms;
using System;
using System.Threading;
using System.Windows.Forms;
using OxidePack.Client.App;
using SapphireEngine;
using SapphireEngine.Functions;

namespace OxidePack.Client
{
    internal class Program
    {
        public static AuthForm AuthForm; 
        
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(AuthForm = new AuthForm());
        }
    }
}