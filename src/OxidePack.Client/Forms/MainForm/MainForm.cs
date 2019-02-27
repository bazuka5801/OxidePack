using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxidePack.Client.App;
using OxidePack.Client.Core.ILMerger;
using OxidePack.Client.Core.MsBuildProject;
using OxidePack.Data;
using SapphireEngine;
using Timer = SapphireEngine.Functions.Timer;

namespace OxidePack.Client
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        
        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            
            Config.OnConfigLoaded += LoadConfig;
            if (Config.IsLoaded)
            {
                ConsoleSystem.Log("Config.IsLoaded!");
                LoadConfig();
            }
            
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
            ThreadUtils.RunInUI(() =>
            {
                Instance.tbUID.Text = key;
                Instance.btnCopyUID.Enabled = string.IsNullOrEmpty(key) == false;
            });
        }

        static string currentStatus = "";
        public static void UpdateStatus(string text)
        {
            ThreadUtils.RunInUI(() =>
            {
                currentStatus = text;
                Instance.lblStatus.Text = $"Status: {text}";
            });
        }

        public static void UpdateProgressBar(int value = 0, int max = 100, ProgressBarStyle style = ProgressBarStyle.Blocks)
        {
            ThreadUtils.RunInUI(() =>
            {
                Instance.pbStatus.Style = style;
                Instance.pbStatus.Maximum = max;
                Instance.pbStatus.Value = value;
            });
        }

        private void SetSolutionFile(string file)
        {
            Settings_lblSolutionFilePath.Text = Config.SolutionFile = file;
            OPClientCore.SetSolution(file);
        }

        private void LoadConfig()
        {
            ThreadUtils.RunInUI(() =>
            {
                SetSolutionFile(Config.SolutionFile);
//                Solution solution = (Config.SolutionFile);
                ReloadStatus();
            });
        }

        #region [Method] Reload Status
        public static void ReloadStatus()
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke((Action) ReloadStatus);
                return;
            }

            Instance.lblOxidePackCurrentVersion.Text = Protocol.Version;
            Instance.lblRustCurrentVersion.Text      = Config.RustVersion;
            Instance.lblOxideCurrentVersion.Text     = Config.OxideVersion;
        }
        #endregion

        #region [Method] CheckForUpdates
        public async Task CheckForUpdates()
        {
            if (Net.cl.IsAuthed == false) return;
            
            var statusBefore = currentStatus;
            UpdateStatus("Checking rust update...");
            UpdateProgressBar(33);
            var RustAvailableVersion = OPClientCore.GetRustAvailableVersion();
            ThreadUtils.RunInUI(() => lblRustAvailableVersion.Text = RustAvailableVersion);
            if (Config.RustVersion != RustAvailableVersion)
            {
                if (MessageBox.Show($"Rust update available! ({RustAvailableVersion})\nUpdate?", "Rust Update", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    await OPClientCore.DownloadRustDLLs(null, null);
                }
            }
            UpdateStatus("Checking oxide update...");
            UpdateProgressBar(66);
            var OxideAvailableVersion = OPClientCore.GetOxideAvailableVersion();
            ThreadUtils.RunInUI(() => lblOxideAvailableVersion.Text = OxideAvailableVersion);
            if (Config.OxideVersion != OxideAvailableVersion)
            {
                if (MessageBox.Show($"Oxide update available! ({OxideAvailableVersion})\nUpdate?", "Oxide Update", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    await DownloadOxide();
                }
            }
            UpdateStatus("Check for updates completed!");
            UpdateProgressBar(100);
            Timer.SetTimeout(() => {
                UpdateStatus(statusBefore);
                UpdateProgressBar();
            }, 5f);
        }
        #endregion

        #region [Method] DownloadOxide
        public async Task DownloadOxide()
        {
            if (Net.cl.IsAuthed == false) return;
            
            var statusBefore = currentStatus;
            await OPClientCore.DownloadOxideDLLs((text, percent) =>
            {
                UpdateStatus(text);
                UpdateProgressBar(percent);
            }, () =>
            {
                UpdateStatus("Download completed!");
                UpdateProgressBar();
                Timer.SetTimeout(() => UpdateStatus(statusBefore), 5f);
            });
        }
        #endregion

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
            if (Net.cl.IsAuthed == false) return;
            using (var openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                DefaultExt = ".sln",
                Filter = "Solution File|*.sln"
            })
            {
                switch (openFileDialog.ShowDialog())
                {
                    case DialogResult.OK:
                        var solution = Solution.Load(openFileDialog.FileName);
                        SetSolutionFile(openFileDialog.FileName);
                        break;
                    default:
                        return;
                }
            }
        }
        #endregion

        #region btnSendPluginRequest_Click
        private void btnSendPluginRequest_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            
            var pluginContent = File.ReadAllText("plugin.cs");
            var bRequest = new BuildRequest()
            {
                sources = new List<SourceFile>()
                {
                    new SourceFile()
                        {filename = "plugin.cs", content = pluginContent, sha256 = pluginContent.ToSHA512()}
                },
                buildOptions = new BuildOptions()
                {
                    name = "plugin"
                }
            };
            Net.cl.SendRPC(RPCMessageType.BuildRequest, bRequest);
        }
        #endregion

        #region btnCreateNewSolution_Click
        private void btnCreateNewSolution_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            
            using (SaveFileDialog sFileDialog = new SaveFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = ".sln",
                Filter = "Solution File|*.sln"
            })
            {
                switch (sFileDialog.ShowDialog())
                {
                    case DialogResult.OK:
                        var solution = Solution.Create(sFileDialog.FileName);
                        SetSolutionFile(solution.Filename);
                        OPClientCore.SetSolution(solution);
                        break;
                    default:
                        return;
                }
            }
        }
        #endregion

        #region btnAddProject_Click
        private void btnAddProject_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            
            using (var openFileDialog = new SaveFileDialog()
            {
                DefaultExt = ".csproj",
                Filter = "C# Project|*.csproj"
            })
            {
                switch (openFileDialog.ShowDialog())
                {
                    case DialogResult.OK:
                        OPClientCore.AddProject(openFileDialog.FileName);
                        break;
                    default:
                        return;
                }
            }
        }
        #endregion

        #region btnEditReference_Click
        private void btnEditReference_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            
            ProjectSelectModel projectSelectModel = new ProjectSelectModel()
            {
                Projects = OPClientCore.Solution.CsProjects.Select(p=>p.Name).ToList()
            };
            new ProjectSelectForm(projectSelectModel).ShowDialog();
            if (projectSelectModel.Success == false)
            {
                return;
            }
            
            var project = OPClientCore.Solution.CsProjects[projectSelectModel.Selected];
            var refsDir = Path.Combine(Directory.GetCurrentDirectory(), ".references-cache");
            if (Config.ProjectsConfig.TryGetValue(project.Name, out var projectConfig) == false)
            {
                Config.ProjectsConfig[project.Name] = projectConfig = new Config.DependenciesConfig();
            }
            
            var form = new DependenciesModel()
            {
                Directory = refsDir,
                Bundle = projectConfig.Bundle,
                SelectedFiles = projectConfig.SelectedFiles
            };
            new DependenciesForm(form).ShowDialog();
            if (form.Changed)
            {
                projectConfig.Bundle = form.Bundle;
                projectConfig.SelectedFiles = form.SelectedFiles;
//                var addedRefs = form.SelectedFiles.Except(project.ReferenceList)
//                    .Select(filename => Path.Combine(refsDir, filename)).ToList();
//                var removedRefs = project.ReferenceList.Except(form.SelectedFiles).ToList();
                if (form.Bundle == false)
                {
//                    addedRefs.ForEach(project.AddReference);
//                    removedRefs.ForEach(project.RemoveReferenceByFile);

                    var projRefsDir = Path.Combine(Directory.GetCurrentDirectory(), "references", "projects", project.Name);
                    if (Directory.Exists(projRefsDir))
                        Directory.Delete(projRefsDir, true);

                    Directory.CreateDirectory(projRefsDir);
                    foreach (var filename in form.SelectedFiles)
                    {
                        var srcFile = Path.Combine(refsDir, filename);
                        var dstFile = Path.Combine(projRefsDir, filename);
                        File.Copy(srcFile, dstFile);
                    }
                    string referencePath = Path.Combine(projRefsDir, "*.dll");
                    project.AddReference(referencePath);
                }
                else
                {
                    if (Directory.Exists(".references-bundle") == false)
                        Directory.CreateDirectory(".references-bundle");
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), ".references-bundle");
                    string outputFileName = Path.Combine(outputDir, "bundle.dll");
                    string referencePath = Path.Combine(outputDir, "*.dll");
                    List<string> MergeFiles = form.SelectedFiles.Select(filename => Path.Combine(refsDir, filename)).ToList();
                    MergeSession session = new MergeSession(refsDir, MergeFiles);
                    session.Merge(outputFileName);

                    project.ReferenceList.ToList().ForEach(project.RemoveReferenceByFile);
                    project.AddReference(referencePath);
                }
            }
        }
        #endregion

        #region btnRustUpdate_Click
        private async void btnRustUpdate_ClickAsync(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            await OPClientCore.DownloadRustDLLs(null, null);
        }
        #endregion

        #region btnOxideUpdate_Click
        private async void btnOxideUpdate_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            await DownloadOxide();
        }
        #endregion

        #region btnCheckForUpdates_Click
        private async void btnCheckForUpdates_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            await CheckForUpdates();
        }
        #endregion

        #region btnOpenPluginManager_Click
        private void btnOpenPluginManager_Click(object sender, System.EventArgs e)
        {
            if (Net.cl.IsAuthed == false) return;
            
            var projects = OPClientCore.Solution.CsProjects;
            ProjectSelectModel psModel = new ProjectSelectModel()
            {
                Projects = projects.Select(p=>p.Name).ToList()
            };
            new ProjectSelectForm(psModel).ShowDialog();
            if (psModel.Success)
            {
                var selectedProject = OPClientCore.Solution.CsProjects[psModel.Selected];
                new PluginManagerForm(new PluginsProject(selectedProject)).Show(this);
            }
        }
        #endregion

        #endregion

        #region [Helper Methods]

        #region [Method] ShowMessage
        public static void ShowMessage(string message, string caption)
        {
            ThreadUtils.RunInUI(() => MessageBox.Show(message, caption));
        }
        #endregion

        #endregion

        private void btnVKGroup_Click(object sender, EventArgs e)
        {
            Process.Start("https://vk.com/public176204769");
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/rFKw9Dz");
        }
    }
}
