using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OxidePack.Client.Properties;
using OxidePack.Data;
using SapphireEngine;

namespace OxidePack.Client
{
    public partial class PluginManagerForm : Form
    {
        private PluginsProject _PluginsProject;
        
        public PluginManagerForm(PluginsProject pluginsProject)
        {
            InitializeComponent();
            LoadProject(pluginsProject);
            
            ModuleMgr.OnModulesUpdateEvent += OnModulesUpdateEvent;
            ModuleMgr.Refresh();
        }
        
        private List<ModuleView> moduleViews = new List<ModuleView>();
        private PluginProject _PluginSelected;

        private void OnModulesUpdateEvent()
        {
            ThreadUtils.RunInUI(() =>
            {
                var modulesCount = ModuleMgr.Modules.Count;
                var needCreate = modulesCount - moduleViews.Count;
                for (int j = 0; j < needCreate; j++)
                {
                    moduleViews.Add(GenerateModuleView());
                }
                
                for (var i = 0; i < ModuleMgr.Modules.Count; i++)
                {
                    var mView = moduleViews[i];
                    mView.LoadModuleInfo(ModuleMgr.Modules[i]);
                    mView.SetIndex(i);
                    
                    // Reload selected plugin
                    SelectPlugin(_PluginSelected);
                }
            });
        }

        #region [Type] ModuleListViewItem
        
        class ModuleListViewItem
        {
            public string Name;
            public string Version;

            public ModuleListViewItem(string name, string version)
            {
                this.Name = name;
                this.Version = version;
            }

            public override string ToString()
            {
                return $"{Name} ({Version})";
            }
        }
        #endregion

        #region [Method] LoadProject
        void LoadProject(PluginsProject pluginsProject)
        {
            this._PluginsProject = pluginsProject;
            lbPlugins.Items.Clear();

            bool exist = false;
            pluginsProject.GetPluginList().ForEach(name =>
            {
                exist = true;
                var plugin = pluginsProject.GetPlugin(name);
                lbPlugins.Items.Add(new ModuleListViewItem(plugin.config.Name, plugin.config.Version.ToString()));
            });

            if (exist)
            {
                // Select first plugin
                lbPlugins.SetSelected(0, true);
            }
        }
        #endregion

        #region [Type] ModuleView
        class ModuleView
        {
            private const int HEIGHT = 52;

            private Panel pnlModule;
            private Label lblName;
            private Button btnInfo, btnAddRemove;

            public Boolean Enabled = false;

            /// <summary>
            /// 1 - ModuleView, 2 - Enabled
            /// </summary>
            public Action<ModuleView, Boolean> OnEnabledChanged;

            /// <summary>
            /// For set use LoadModuleInfo
            /// </summary>
            public ModuleInfo ModuleInfo { get; private set; }

            public ModuleView(Panel pnlModule, Label lblName, Button btnInfo, Button btnAddRemove)
            {
                this.pnlModule = pnlModule;
                this.lblName = lblName;
                this.btnInfo = btnInfo;
                this.btnAddRemove = btnAddRemove;
                
                this.btnInfo.Click += BtnInfo_OnClick;
                this.btnAddRemove.Click += BtnAddRemove_OnClick;
            }

            private void BtnAddRemove_OnClick(object sender, EventArgs e)
            {
                this.Enabled = !Enabled;
                OnEnabledChanged?.Invoke(this, this.Enabled);
                SetImageAddRemoveButton(this.Enabled);
            }

            public void LoadModuleInfo(ModuleInfo info)
            {
                ModuleInfo = info;
                SetModuleName($"{info.name} ({info.version})");
            }

            private void BtnInfo_OnClick(object sender, EventArgs e)
            {
                if (ModuleInfo == null)
                {
                    ConsoleSystem.LogError($"ModuleView: _ModuleInfo == null");
                    return;
                }

                MessageBox.Show(ModuleInfo.description, $"{ModuleInfo.name} ({ModuleInfo.version})",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            public void SetIndex(int index)
            {
                this.pnlModule.Location = new Point(0, HEIGHT * index);
            }

            public void SetModuleName(string name)
            {
                this.lblName.Text = name;
            }

            public void SetImageAddRemoveButton(bool enabled)
            {
                this.btnAddRemove.BackgroundImage = enabled ? Resources.btnRemove : Resources.btnAdd;
            }
        }
        #endregion

        #region [Method] GenerateModuleView
        private ModuleView GenerateModuleView()
        {
            var pnlModule = new Panel
            {
                BackgroundImageLayout = ImageLayout.Center,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(0, 0),
                Name = $"pnlModule",
                Size = new Size(pnlModulesContainer.Width, 52),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TabIndex = 0
            };
            
            var tlpModule = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
                Name = "tlpModule",
                RowCount = 1,
                Size = new Size(437, 50),
                TabIndex = 0
            };
            tlpModule.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpModule.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tlpModule.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            
            var lblModuleName = new Label
            {
                Dock = DockStyle.Fill,
                Location = new Point(3, 0),
                Name = "lblModuleName",
                Size = new Size(311, 50),
                TabIndex = 0,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            var tlpModuleButtons = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill,
                Location = new Point(320, 3),
                Name = "tableLayoutPanel6",
                RowCount = 1,
                Size = new Size(114, 44),
                TabIndex = 1
            };
            tlpModuleButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpModuleButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tlpModuleButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            
            var btnModuleInfo = new Button
            {
                BackgroundImage = Resources.info,
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Location = new Point(3, 3),
                Name = "btnModuleInfo",
                Size = new Size(58, 38),
                TabIndex = 0,
                UseVisualStyleBackColor = true
            };
            
            var btnAddRemove = new Button
            {
                BackColor = Color.Transparent,
                BackgroundImage = Resources.btnAdd,
                BackgroundImageLayout = ImageLayout.Zoom,
                Dock = DockStyle.Fill,
                Location = new Point(67, 3),
                Name = "btnAddRemove",
                Size = new Size(44, 38),
                TabIndex = 1,
                UseVisualStyleBackColor = false
            };
            
            pnlModule.Controls.Add(tlpModule);
            tlpModule.Controls.Add(lblModuleName, 0, 0);
            tlpModule.Controls.Add(tlpModuleButtons, 1, 0);
            tlpModuleButtons.Controls.Add(btnModuleInfo, 0, 0);
            tlpModuleButtons.Controls.Add(btnAddRemove, 1, 0);
            pnlModulesContainer.Controls.Add(pnlModule);
            var mView = new ModuleView(pnlModule, lblModuleName, btnModuleInfo, btnAddRemove);
            mView.OnEnabledChanged += OnModuleEnabledChanged;
            return mView;
        }
        #endregion

        #region [Method] SelectPlugin

        private void SelectPlugin(PluginProject plugin)
        {
            this._PluginSelected = plugin;
            
            
            this.lblPluginName.Text = plugin.config.Name;
            this.lblVersion.Text = plugin.config.Version.ToString();
            this.lblAuthor.Text = plugin.config.Author;

            this.moduleViews.ForEach(mView =>
            {
                var enabled = plugin.config.Modules.Contains(mView.ModuleInfo.name);
                mView.Enabled = enabled;
                mView.SetImageAddRemoveButton(enabled);
            });

            this.moduleViews = this.moduleViews
                .OrderBy(mView => mView.ModuleInfo.name)
                .ThenByDescending(p => p.Enabled)
                .ToList();
        }
        #endregion

        #region [Method] UI Handlers

        #region btnDeletePlugin_Click
        private void btnDeletePlugin_Click(object sender, System.EventArgs e)
        {
            var selected = (ModuleListViewItem)lbPlugins.SelectedItem;
            var name = selected.Name;
            if (MessageBox.Show(this, "Are you sure?", $"Deleting {name}...", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.Yes)
            {
                _PluginsProject.RemovePlugin(name);
                lbPlugins.Items.Remove(selected);
            }

        }
        #endregion

        #region lbPlugins_SelectedValueChanged
        private void lbPlugins_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (!(lbPlugins.SelectedItem is ModuleListViewItem selected))
            {
                return;
            }
            
            var plugin = _PluginsProject.GetPlugin(selected.Name);
            SelectPlugin(plugin);
        }
        #endregion


        #region [Method] OnModuleEnabledChanged
        private void OnModuleEnabledChanged(ModuleView mView, bool enabled)
        {
            if (_PluginSelected == null)
            {
                throw new NullReferenceException("Plugin Selected is NULL!");
                return;
            }

            if (enabled)
            {
                _PluginSelected.AddModule(mView.ModuleInfo.name);
            }
            else
            {
                _PluginSelected.RemoveModule(mView.ModuleInfo.name);
            }
        }
        #endregion

        #region [Method] btnRefreshGeneratedFile_Click
        private void btnRefreshGeneratedFile_Click(object sender, EventArgs e)
        {
            if (_PluginSelected == null)
            {
                throw new Exception("PluginSelected is null");                
            }

            _PluginSelected.RequestGeneratedFile();
        }
        #endregion

        #endregion
    }
}
