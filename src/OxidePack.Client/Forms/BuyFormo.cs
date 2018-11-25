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

namespace OxidePack.Client
{
    public partial class AuthForm : Form
    {
        public static AuthForm Instance;
        
        public AuthForm()
        {
            Instance = this;
            InitializeComponent();
            UpdateStatus("(1/3) Retrieving MachineID");
            UpdateProgressBar(style: ProgressBarStyle.Marquee);
            ThreadPool.QueueUserWorkItem(o =>
            {
                UpdateKey(MachineIdentificator.Value());
                UpdateStatus("(2/3) Connecting to server...");
                UpdateProgressBar(value: 2, max: 3);
                AppCore.ConnectToServer();
            });
        }

        private static void UpdateKey(string key)
        {
            RunInMainThread(() =>
            {
                Instance.tbUID.Text = key;
                Instance.btnCopyUID.Enabled = string.IsNullOrEmpty(key) == false;
            });
        }

        public static void UpdateStatus(string text)
        {
            RunInMainThread(() =>
            {
                Instance.lblStatus.Text = $"Status: {text}";
            });
        }

        public static void UpdateProgressBar(int value = 0, int max = 100, ProgressBarStyle style = ProgressBarStyle.Blocks)
        {
            RunInMainThread(() =>
            {
                Instance.pbStatus.Style = style;
                Instance.pbStatus.Maximum = max;
                Instance.pbStatus.Value = value;
            });
        }

        public static void RunInMainThread(Action action)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(action);
                return;
            }

            action();
        }

        private void btnCopyUID_Click( object sender, EventArgs e )
        {
            Clipboard.SetText(tbUID.Text);
        }
    }
}
