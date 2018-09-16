using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxidePack.Client.App;

namespace OxidePack.Client.Forms
{
    public partial class AuthForm : Form
    {
        public static AuthForm Instance;
        
        public AuthForm()
        {
            Instance = this;
            InitializeComponent();
            UpdateStatus("(1/3) Retrieving MachineID");
            ThreadPool.QueueUserWorkItem(o =>
            {
                MachineIdentificator.Value();
                UpdateStatus("(2/3) Connecting to server...");
                AppCore.Initialize();
            });
        }

        public static void UpdateStatus(string text)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(new Action(() => { UpdateStatus(text); }));
                return;
            }
            Instance.lblStatus.Text = $"Status: {text}";
        }
        
    }
}
