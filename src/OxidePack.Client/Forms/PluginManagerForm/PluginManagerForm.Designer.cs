namespace OxidePack.Client
{
    partial class PluginManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbPlugins = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddPlugin = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPlugin = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblPluginName = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlModules = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlModulesContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRefreshGeneratedFile = new System.Windows.Forms.Button();
            this.btnDeletePlugin = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlPlugin.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlModules.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPlugins
            // 
            this.lbPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlugins.FormattingEnabled = true;
            this.lbPlugins.ItemHeight = 18;
            this.lbPlugins.Location = new System.Drawing.Point(5, 4);
            this.lbPlugins.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lbPlugins.Name = "lbPlugins";
            this.lbPlugins.Size = new System.Drawing.Size(323, 440);
            this.lbPlugins.TabIndex = 0;
            this.lbPlugins.SelectedValueChanged += new System.EventHandler(this.lbPlugins_SelectedValueChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAddPlugin, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbPlugins, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(333, 503);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAddPlugin
            // 
            this.btnAddPlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddPlugin.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddPlugin.Location = new System.Drawing.Point(5, 452);
            this.btnAddPlugin.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddPlugin.Name = "btnAddPlugin";
            this.btnAddPlugin.Size = new System.Drawing.Size(323, 47);
            this.btnAddPlugin.TabIndex = 0;
            this.btnAddPlugin.Text = "Add";
            this.btnAddPlugin.UseVisualStyleBackColor = true;
            this.btnAddPlugin.Click += new System.EventHandler(this.btnAddPlugin_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.pnlPlugin, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pnlModules, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(333, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(362, 503);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // pnlPlugin
            // 
            this.pnlPlugin.Controls.Add(this.tableLayoutPanel3);
            this.pnlPlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPlugin.Location = new System.Drawing.Point(3, 3);
            this.pnlPlugin.Name = "pnlPlugin";
            this.pnlPlugin.Size = new System.Drawing.Size(356, 94);
            this.pnlPlugin.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.lblPluginName, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(356, 94);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblPluginName
            // 
            this.lblPluginName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPluginName.Font = new System.Drawing.Font("Verdana", 16F);
            this.lblPluginName.Location = new System.Drawing.Point(3, 0);
            this.lblPluginName.Name = "lblPluginName";
            this.lblPluginName.Size = new System.Drawing.Size(350, 40);
            this.lblPluginName.TabIndex = 0;
            this.lblPluginName.Text = "...";
            this.lblPluginName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.lblAuthor, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblVersion, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(350, 48);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAuthor.Location = new System.Drawing.Point(178, 24);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(169, 24);
            this.lblAuthor.TabIndex = 4;
            this.lblAuthor.Text = "...";
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Author";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersion.Location = new System.Drawing.Point(178, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(169, 24);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Version";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlModules
            // 
            this.pnlModules.Controls.Add(this.tableLayoutPanel5);
            this.pnlModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModules.Location = new System.Drawing.Point(3, 103);
            this.pnlModules.Name = "pnlModules";
            this.pnlModules.Size = new System.Drawing.Size(356, 397);
            this.pnlModules.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.pnlModulesContainer, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(356, 397);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Verdana", 14F);
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(350, 40);
            this.label4.TabIndex = 0;
            this.label4.Text = "Modules";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlModulesContainer
            // 
            this.pnlModulesContainer.AutoScroll = true;
            this.pnlModulesContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlModulesContainer.Location = new System.Drawing.Point(3, 43);
            this.pnlModulesContainer.Name = "pnlModulesContainer";
            this.pnlModulesContainer.Size = new System.Drawing.Size(350, 254);
            this.pnlModulesContainer.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.28571F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.71429F));
            this.tableLayoutPanel6.Controls.Add(this.btnDeletePlugin, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.btnCompile, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnRefreshGeneratedFile, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 303);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(350, 91);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // btnRefreshGeneratedFile
            // 
            this.btnRefreshGeneratedFile.BackColor = System.Drawing.Color.Silver;
            this.btnRefreshGeneratedFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshGeneratedFile.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefreshGeneratedFile.ForeColor = System.Drawing.Color.Black;
            this.btnRefreshGeneratedFile.Location = new System.Drawing.Point(150, 3);
            this.btnRefreshGeneratedFile.Name = "btnRefreshGeneratedFile";
            this.btnRefreshGeneratedFile.Size = new System.Drawing.Size(197, 39);
            this.btnRefreshGeneratedFile.TabIndex = 1;
            this.btnRefreshGeneratedFile.Text = "Refresh Generated";
            this.btnRefreshGeneratedFile.UseVisualStyleBackColor = false;
            this.btnRefreshGeneratedFile.Click += new System.EventHandler(this.btnRefreshGeneratedFile_Click);
            // 
            // btnDeletePlugin
            // 
            this.btnDeletePlugin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDeletePlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeletePlugin.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDeletePlugin.ForeColor = System.Drawing.Color.Silver;
            this.btnDeletePlugin.Location = new System.Drawing.Point(150, 48);
            this.btnDeletePlugin.Name = "btnDeletePlugin";
            this.btnDeletePlugin.Size = new System.Drawing.Size(197, 40);
            this.btnDeletePlugin.TabIndex = 0;
            this.btnDeletePlugin.Text = "Delete Plugin";
            this.btnDeletePlugin.UseVisualStyleBackColor = false;
            this.btnDeletePlugin.Click += new System.EventHandler(this.btnDeletePlugin_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnCompile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCompile.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Bold);
            this.btnCompile.ForeColor = System.Drawing.Color.Black;
            this.btnCompile.Location = new System.Drawing.Point(3, 3);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(141, 39);
            this.btnCompile.TabIndex = 2;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = false;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // PluginManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 503);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = global::OxidePack.Client.Properties.Resources.oxidepack;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "PluginManagerForm";
            this.Text = "PluginManagerForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlPlugin.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlModules.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbPlugins;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAddPlugin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel pnlPlugin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblPluginName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlModules;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlModulesContainer;
        private System.Windows.Forms.Button btnDeletePlugin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnRefreshGeneratedFile;
        private System.Windows.Forms.Button btnCompile;
    }
}