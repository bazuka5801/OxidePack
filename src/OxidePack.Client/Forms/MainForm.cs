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
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        
        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            Config.OnConfigLoaded += LoadConfig;
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

        private void SetSolutionFile(string file)
        {
            Settings_lblSolutionFilePath.Text = Config.SolutionFile = file;
        }

        private void LoadConfig()
        {
            RunInMainThread(() =>
            {
                SetSolutionFile(Config.SolutionFile); 
            });
        }

        #region [Methods] UI Handlers
        
        #region [Method] btnCopyUID_Click
        private void btnCopyUID_Click( object sender, EventArgs e )
        {
            Clipboard.SetText(tbUID.Text);
        }
        #endregion

        #region [Method] Settings_btnSolutionOpen_Click
        private void Settings_btnSolutionOpen_Click( object sender, EventArgs e )
        {
            using (var openFileDialog = new OpenFileDialog() {CheckFileExists = true, DefaultExt = ".sln", Filter = "Solution File|*.sln"})
            {
                switch (openFileDialog.ShowDialog())
                {
                    case DialogResult.OK:
                        SetSolutionFile(openFileDialog.FileName);
                        break;
                    default:
                        return;
                }
            }
        }
        #endregion
        
        #endregion

        #region [Helper Methods]

        #region [Method] RunInMainThread
        private static void RunInMainThread(Action action)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke(action);
                return;
            }

            action();
        }
        #endregion

        #endregion
    }
}
