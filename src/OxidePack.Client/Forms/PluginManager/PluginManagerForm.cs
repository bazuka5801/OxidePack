using System.Drawing;
using System.Windows.Forms;
using OxidePack.Client.Properties;
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
        }

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
                // Auto-Select first item
                lbPlugins.SetSelected(0, true);
            }
        }

        class ModuleView
        {
            private const int HEIGHT = 52;

            private Panel pnlModule;
            private Label lblName;
            private Button btnInfo, btnAddRemove;

            public ModuleView(Panel pnlModule, Label lblName, Button btnInfo, Button btnAddRemove)
            {
                this.pnlModule = pnlModule;
                this.lblName = lblName;
                this.btnInfo = btnInfo;
                this.btnAddRemove = btnAddRemove;
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
            
            return new ModuleView(pnlModule, lblModuleName, btnModuleInfo, btnAddRemove);
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

            lblPluginName.Text = plugin.config.Name;
            lblVersion.Text = plugin.config.Version.ToString();
            lblAuthor.Text = plugin.config.Author;
        }
        #endregion

        #endregion
    }
}
