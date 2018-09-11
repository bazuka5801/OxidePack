using OxidePack.Client.Forms;
using System;
using System.Windows.Forms;

namespace OxidePack.Client
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Auth());
        }
    }
}