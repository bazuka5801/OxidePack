namespace OxidePack.Client.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
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
            this.btnCopyUID = new System.Windows.Forms.Button();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.mStatus = new System.Windows.Forms.StatusStrip();
            this.pbStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlAuth = new System.Windows.Forms.Panel();
            this.lblAuth = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.msFile = new System.Windows.Forms.ToolStripMenuItem();
            this.msAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.msBuySubscription = new System.Windows.Forms.ToolStripMenuItem();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tpMain = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOxideUpdate = new System.Windows.Forms.Button();
            this.lblOxideCurrentVersion = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOxidePackCurrentVersion = new System.Windows.Forms.Label();
            this.lblRustCurrentVersion = new System.Windows.Forms.Label();
            this.lblOxidePackAvailableVersion = new System.Windows.Forms.Label();
            this.lblRustAvailableVersion = new System.Windows.Forms.Label();
            this.lblOxideAvailableVersion = new System.Windows.Forms.Label();
            this.btnOxidePackUpdate = new System.Windows.Forms.Button();
            this.btnRustUpdate = new System.Windows.Forms.Button();
            this.btnCheckForUpdates = new System.Windows.Forms.Button();
            this.btnSendPluginRequest = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.btnAddReference = new System.Windows.Forms.Button();
            this.btnAddProject = new System.Windows.Forms.Button();
            this.btnCreateNewSolution = new System.Windows.Forms.Button();
            this.Settings_pnlSolution = new System.Windows.Forms.Panel();
            this.Settings_lblSolutionFile = new System.Windows.Forms.Label();
            this.Settings_btnSolutionOpen = new System.Windows.Forms.Button();
            this.Settings_lblSolutionFilePath = new System.Windows.Forms.Label();
            this.tpProfile = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.mStatus.SuspendLayout();
            this.pnlAuth.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.tpMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.Settings_pnlSolution.SuspendLayout();
            this.tpProfile.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCopyUID
            // 
            this.btnCopyUID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyUID.Enabled = false;
            this.btnCopyUID.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCopyUID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCopyUID.Image = global::OxidePack.Client.Properties.Resources.btnCopy;
            this.btnCopyUID.Location = new System.Drawing.Point(160, 36);
            this.btnCopyUID.Name = "btnCopyUID";
            this.btnCopyUID.Size = new System.Drawing.Size(36, 33);
            this.btnCopyUID.TabIndex = 1;
            this.btnCopyUID.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCopyUID.UseMnemonic = false;
            this.btnCopyUID.UseVisualStyleBackColor = true;
            this.btnCopyUID.Click += new System.EventHandler(this.btnCopyUID_Click);
            // 
            // tbUID
            // 
            this.tbUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUID.BackColor = System.Drawing.Color.Silver;
            this.tbUID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUID.Enabled = false;
            this.tbUID.Font = new System.Drawing.Font("Verdana", 11F);
            this.tbUID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.tbUID.Location = new System.Drawing.Point(8, 43);
            this.tbUID.Name = "tbUID";
            this.tbUID.ReadOnly = true;
            this.tbUID.Size = new System.Drawing.Size(144, 18);
            this.tbUID.TabIndex = 2;
            // 
            // mStatus
            // 
            this.mStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbStatus,
            this.lblStatus});
            this.mStatus.Location = new System.Drawing.Point(0, 451);
            this.mStatus.Name = "mStatus";
            this.mStatus.Size = new System.Drawing.Size(569, 22);
            this.mStatus.TabIndex = 5;
            this.mStatus.Text = "statusStrip1";
            // 
            // pbStatus
            // 
            this.pbStatus.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(100, 16);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 17);
            this.lblStatus.Text = "Status:";
            // 
            // pnlAuth
            // 
            this.pnlAuth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAuth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlAuth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAuth.Controls.Add(this.lblAuth);
            this.pnlAuth.Controls.Add(this.btnCopyUID);
            this.pnlAuth.Controls.Add(this.tbUID);
            this.pnlAuth.Location = new System.Drawing.Point(346, 6);
            this.pnlAuth.Name = "pnlAuth";
            this.pnlAuth.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.pnlAuth.Size = new System.Drawing.Size(209, 79);
            this.pnlAuth.TabIndex = 6;
            // 
            // lblAuth
            // 
            this.lblAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAuth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAuth.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAuth.ForeColor = System.Drawing.Color.Teal;
            this.lblAuth.Location = new System.Drawing.Point(9, 6);
            this.lblAuth.Name = "lblAuth";
            this.lblAuth.Size = new System.Drawing.Size(186, 24);
            this.lblAuth.TabIndex = 3;
            this.lblAuth.Text = "Authentication";
            this.lblAuth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msFile,
            this.msAbout,
            this.msBuySubscription});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(569, 24);
            this.menuStrip.TabIndex = 7;
            this.menuStrip.Text = "menuStrip1";
            // 
            // msFile
            // 
            this.msFile.Name = "msFile";
            this.msFile.Size = new System.Drawing.Size(37, 20);
            this.msFile.Text = "File";
            // 
            // msAbout
            // 
            this.msAbout.Name = "msAbout";
            this.msAbout.Size = new System.Drawing.Size(52, 20);
            this.msAbout.Text = "About";
            // 
            // msBuySubscription
            // 
            this.msBuySubscription.Name = "msBuySubscription";
            this.msBuySubscription.Size = new System.Drawing.Size(108, 20);
            this.msBuySubscription.Text = "Buy Subscription";
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tpMain);
            this.tbMain.Controls.Add(this.tpSettings);
            this.tbMain.Controls.Add(this.tpProfile);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(0, 24);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(569, 427);
            this.tbMain.TabIndex = 8;
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add(this.groupBox1);
            this.tpMain.Controls.Add(this.btnSendPluginRequest);
            this.tpMain.Location = new System.Drawing.Point(4, 27);
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tpMain.Size = new System.Drawing.Size(561, 396);
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Main";
            this.tpMain.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(547, 190);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCheckForUpdates, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(541, 164);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Controls.Add(this.btnOxideUpdate, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblOxideCurrentVersion, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOxidePackCurrentVersion, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRustCurrentVersion, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOxidePackAvailableVersion, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRustAvailableVersion, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblOxideAvailableVersion, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnOxidePackUpdate, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRustUpdate, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(535, 128);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOxideUpdate
            // 
            this.btnOxideUpdate.BackgroundImage = global::OxidePack.Client.Properties.Resources.download_icon;
            this.btnOxideUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOxideUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOxideUpdate.Location = new System.Drawing.Point(486, 96);
            this.btnOxideUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnOxideUpdate.Name = "btnOxideUpdate";
            this.btnOxideUpdate.Size = new System.Drawing.Size(49, 32);
            this.btnOxideUpdate.TabIndex = 14;
            this.btnOxideUpdate.UseVisualStyleBackColor = true;
            this.btnOxideUpdate.Click += new System.EventHandler(this.btnOxideUpdate_Click);
            // 
            // lblOxideCurrentVersion
            // 
            this.lblOxideCurrentVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOxideCurrentVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblOxideCurrentVersion.Location = new System.Drawing.Point(165, 96);
            this.lblOxideCurrentVersion.Name = "lblOxideCurrentVersion";
            this.lblOxideCurrentVersion.Size = new System.Drawing.Size(156, 32);
            this.lblOxideCurrentVersion.TabIndex = 8;
            this.lblOxideCurrentVersion.Text = "---";
            this.lblOxideCurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(327, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 32);
            this.label6.TabIndex = 5;
            this.label6.Text = "Available";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(165, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 32);
            this.label5.TabIndex = 4;
            this.label5.Text = "Curent";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F);
            this.label3.Location = new System.Drawing.Point(3, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 32);
            this.label3.TabIndex = 2;
            this.label3.Text = "Oxide";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F);
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rust";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F);
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oxide Pack";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 32);
            this.label4.TabIndex = 3;
            this.label4.Text = "Package";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOxidePackCurrentVersion
            // 
            this.lblOxidePackCurrentVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOxidePackCurrentVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblOxidePackCurrentVersion.Location = new System.Drawing.Point(165, 32);
            this.lblOxidePackCurrentVersion.Name = "lblOxidePackCurrentVersion";
            this.lblOxidePackCurrentVersion.Size = new System.Drawing.Size(156, 32);
            this.lblOxidePackCurrentVersion.TabIndex = 6;
            this.lblOxidePackCurrentVersion.Text = "---";
            this.lblOxidePackCurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRustCurrentVersion
            // 
            this.lblRustCurrentVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRustCurrentVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblRustCurrentVersion.Location = new System.Drawing.Point(165, 64);
            this.lblRustCurrentVersion.Name = "lblRustCurrentVersion";
            this.lblRustCurrentVersion.Size = new System.Drawing.Size(156, 32);
            this.lblRustCurrentVersion.TabIndex = 7;
            this.lblRustCurrentVersion.Text = "---";
            this.lblRustCurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOxidePackAvailableVersion
            // 
            this.lblOxidePackAvailableVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOxidePackAvailableVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblOxidePackAvailableVersion.Location = new System.Drawing.Point(327, 32);
            this.lblOxidePackAvailableVersion.Name = "lblOxidePackAvailableVersion";
            this.lblOxidePackAvailableVersion.Size = new System.Drawing.Size(156, 32);
            this.lblOxidePackAvailableVersion.TabIndex = 9;
            this.lblOxidePackAvailableVersion.Text = "---";
            this.lblOxidePackAvailableVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRustAvailableVersion
            // 
            this.lblRustAvailableVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRustAvailableVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblRustAvailableVersion.Location = new System.Drawing.Point(327, 64);
            this.lblRustAvailableVersion.Name = "lblRustAvailableVersion";
            this.lblRustAvailableVersion.Size = new System.Drawing.Size(156, 32);
            this.lblRustAvailableVersion.TabIndex = 10;
            this.lblRustAvailableVersion.Text = "---";
            this.lblRustAvailableVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOxideAvailableVersion
            // 
            this.lblOxideAvailableVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOxideAvailableVersion.Font = new System.Drawing.Font("Verdana", 12F);
            this.lblOxideAvailableVersion.Location = new System.Drawing.Point(327, 96);
            this.lblOxideAvailableVersion.Name = "lblOxideAvailableVersion";
            this.lblOxideAvailableVersion.Size = new System.Drawing.Size(156, 32);
            this.lblOxideAvailableVersion.TabIndex = 11;
            this.lblOxideAvailableVersion.Text = "---";
            this.lblOxideAvailableVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOxidePackUpdate
            // 
            this.btnOxidePackUpdate.BackgroundImage = global::OxidePack.Client.Properties.Resources.download_icon;
            this.btnOxidePackUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOxidePackUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOxidePackUpdate.Location = new System.Drawing.Point(486, 32);
            this.btnOxidePackUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnOxidePackUpdate.Name = "btnOxidePackUpdate";
            this.btnOxidePackUpdate.Size = new System.Drawing.Size(49, 32);
            this.btnOxidePackUpdate.TabIndex = 12;
            this.btnOxidePackUpdate.UseVisualStyleBackColor = true;
            // 
            // btnRustUpdate
            // 
            this.btnRustUpdate.BackgroundImage = global::OxidePack.Client.Properties.Resources.download_icon;
            this.btnRustUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRustUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRustUpdate.Location = new System.Drawing.Point(486, 64);
            this.btnRustUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.btnRustUpdate.Name = "btnRustUpdate";
            this.btnRustUpdate.Size = new System.Drawing.Size(49, 32);
            this.btnRustUpdate.TabIndex = 13;
            this.btnRustUpdate.UseVisualStyleBackColor = true;
            this.btnRustUpdate.Click += new System.EventHandler(this.btnRustUpdate_ClickAsync);
            // 
            // btnCheckForUpdates
            // 
            this.btnCheckForUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCheckForUpdates.Location = new System.Drawing.Point(0, 134);
            this.btnCheckForUpdates.Margin = new System.Windows.Forms.Padding(0);
            this.btnCheckForUpdates.Name = "btnCheckForUpdates";
            this.btnCheckForUpdates.Size = new System.Drawing.Size(541, 30);
            this.btnCheckForUpdates.TabIndex = 1;
            this.btnCheckForUpdates.Text = "Check for updates...";
            this.btnCheckForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForUpdates.Click += new System.EventHandler(this.btnCheckForUpdates_Click);
            // 
            // btnSendPluginRequest
            // 
            this.btnSendPluginRequest.Location = new System.Drawing.Point(180, 321);
            this.btnSendPluginRequest.Name = "btnSendPluginRequest";
            this.btnSendPluginRequest.Size = new System.Drawing.Size(219, 57);
            this.btnSendPluginRequest.TabIndex = 0;
            this.btnSendPluginRequest.Text = "Send Plugin Request";
            this.btnSendPluginRequest.UseVisualStyleBackColor = true;
            this.btnSendPluginRequest.Click += new System.EventHandler(this.btnSendPluginRequest_Click);
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.btnAddReference);
            this.tpSettings.Controls.Add(this.btnAddProject);
            this.tpSettings.Controls.Add(this.btnCreateNewSolution);
            this.tpSettings.Controls.Add(this.Settings_pnlSolution);
            this.tpSettings.Location = new System.Drawing.Point(4, 27);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(5);
            this.tpSettings.Size = new System.Drawing.Size(561, 396);
            this.tpSettings.TabIndex = 2;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // btnAddReference
            // 
            this.btnAddReference.Location = new System.Drawing.Point(284, 103);
            this.btnAddReference.Name = "btnAddReference";
            this.btnAddReference.Size = new System.Drawing.Size(152, 43);
            this.btnAddReference.TabIndex = 4;
            this.btnAddReference.Text = "AddReference";
            this.btnAddReference.UseVisualStyleBackColor = true;
            this.btnAddReference.Click += new System.EventHandler(this.btnAddReference_Click);
            // 
            // btnAddProject
            // 
            this.btnAddProject.Location = new System.Drawing.Point(302, 58);
            this.btnAddProject.Name = "btnAddProject";
            this.btnAddProject.Size = new System.Drawing.Size(137, 39);
            this.btnAddProject.TabIndex = 3;
            this.btnAddProject.Text = "Add project";
            this.btnAddProject.UseVisualStyleBackColor = true;
            this.btnAddProject.Click += new System.EventHandler(this.btnAddProject_Click);
            // 
            // btnCreateNewSolution
            // 
            this.btnCreateNewSolution.Location = new System.Drawing.Point(8, 357);
            this.btnCreateNewSolution.Name = "btnCreateNewSolution";
            this.btnCreateNewSolution.Size = new System.Drawing.Size(431, 31);
            this.btnCreateNewSolution.TabIndex = 2;
            this.btnCreateNewSolution.Text = "Create new solution";
            this.btnCreateNewSolution.UseVisualStyleBackColor = true;
            this.btnCreateNewSolution.Click += new System.EventHandler(this.btnCreateNewSolution_Click);
            // 
            // Settings_pnlSolution
            // 
            this.Settings_pnlSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings_pnlSolution.Controls.Add(this.Settings_lblSolutionFile);
            this.Settings_pnlSolution.Controls.Add(this.Settings_btnSolutionOpen);
            this.Settings_pnlSolution.Controls.Add(this.Settings_lblSolutionFilePath);
            this.Settings_pnlSolution.Location = new System.Drawing.Point(8, 8);
            this.Settings_pnlSolution.Name = "Settings_pnlSolution";
            this.Settings_pnlSolution.Size = new System.Drawing.Size(431, 44);
            this.Settings_pnlSolution.TabIndex = 1;
            // 
            // Settings_lblSolutionFile
            // 
            this.Settings_lblSolutionFile.Location = new System.Drawing.Point(3, 10);
            this.Settings_lblSolutionFile.Name = "Settings_lblSolutionFile";
            this.Settings_lblSolutionFile.Size = new System.Drawing.Size(129, 24);
            this.Settings_lblSolutionFile.TabIndex = 2;
            this.Settings_lblSolutionFile.Text = "Solution File:";
            this.Settings_lblSolutionFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Settings_btnSolutionOpen
            // 
            this.Settings_btnSolutionOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings_btnSolutionOpen.Image = global::OxidePack.Client.Properties.Resources.btnOpenFolder;
            this.Settings_btnSolutionOpen.Location = new System.Drawing.Point(388, 2);
            this.Settings_btnSolutionOpen.Name = "Settings_btnSolutionOpen";
            this.Settings_btnSolutionOpen.Size = new System.Drawing.Size(40, 40);
            this.Settings_btnSolutionOpen.TabIndex = 1;
            this.Settings_btnSolutionOpen.UseVisualStyleBackColor = true;
            this.Settings_btnSolutionOpen.Click += new System.EventHandler(this.Settings_btnSolutionOpen_Click);
            // 
            // Settings_lblSolutionFilePath
            // 
            this.Settings_lblSolutionFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings_lblSolutionFilePath.AutoEllipsis = true;
            this.Settings_lblSolutionFilePath.Location = new System.Drawing.Point(128, 10);
            this.Settings_lblSolutionFilePath.Name = "Settings_lblSolutionFilePath";
            this.Settings_lblSolutionFilePath.Size = new System.Drawing.Size(254, 24);
            this.Settings_lblSolutionFilePath.TabIndex = 0;
            this.Settings_lblSolutionFilePath.Text = "...";
            this.Settings_lblSolutionFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpProfile
            // 
            this.tpProfile.Controls.Add(this.groupBox2);
            this.tpProfile.Controls.Add(this.pnlAuth);
            this.tpProfile.Location = new System.Drawing.Point(4, 27);
            this.tpProfile.Name = "tpProfile";
            this.tpProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tpProfile.Size = new System.Drawing.Size(561, 396);
            this.tpProfile.TabIndex = 1;
            this.tpProfile.Text = "Profile";
            this.tpProfile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 254);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label20, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label18, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label16, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label14, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(315, 228);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label20
            // 
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Verdana", 12F);
            this.label20.Location = new System.Drawing.Point(160, 171);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(152, 57);
            this.label20.TabIndex = 7;
            this.label20.Text = "...";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(3, 171);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(151, 57);
            this.label19.TabIndex = 6;
            this.label19.Text = "Expired";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("Verdana", 12F);
            this.label18.Location = new System.Drawing.Point(160, 114);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(152, 57);
            this.label18.TabIndex = 5;
            this.label18.Text = "...";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(3, 114);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(151, 57);
            this.label17.TabIndex = 4;
            this.label17.Text = "Subscription";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Verdana", 12F);
            this.label16.Location = new System.Drawing.Point(160, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(152, 57);
            this.label16.TabIndex = 3;
            this.label16.Text = "...";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(3, 57);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(151, 57);
            this.label15.TabIndex = 2;
            this.label15.Text = "Contacts";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Verdana", 12F);
            this.label14.Location = new System.Drawing.Point(160, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 57);
            this.label14.TabIndex = 1;
            this.label14.Text = "...";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(3, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(151, 57);
            this.label13.TabIndex = 0;
            this.label13.Text = "Name";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 473);
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.mStatus);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = global::OxidePack.Client.Properties.Resources.oxidepack;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oxide Pack";
            this.mStatus.ResumeLayout(false);
            this.mStatus.PerformLayout();
            this.pnlAuth.ResumeLayout(false);
            this.pnlAuth.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tbMain.ResumeLayout(false);
            this.tpMain.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.Settings_pnlSolution.ResumeLayout(false);
            this.tpProfile.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCopyUID;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.StatusStrip mStatus;
        private System.Windows.Forms.ToolStripProgressBar pbStatus;
        private System.Windows.Forms.Label lblOxideCurrentVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOxidePackCurrentVersion;
        private System.Windows.Forms.Label lblRustCurrentVersion;
        private System.Windows.Forms.Label lblOxidePackAvailableVersion;
        private System.Windows.Forms.Label lblRustAvailableVersion;
        private System.Windows.Forms.Label lblOxideAvailableVersion;
        private System.Windows.Forms.Button btnOxidePackUpdate;
        private System.Windows.Forms.Button btnOxideUpdate;
        private System.Windows.Forms.Button btnRustUpdate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel pnlAuth;
        private System.Windows.Forms.Label lblAuth;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem msFile;
        private System.Windows.Forms.ToolStripMenuItem msAbout;
        private System.Windows.Forms.ToolStripMenuItem msBuySubscription;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tpMain;
        private System.Windows.Forms.TabPage tpProfile;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.Panel Settings_pnlSolution;
        private System.Windows.Forms.Button Settings_btnSolutionOpen;
        private System.Windows.Forms.Label Settings_lblSolutionFilePath;
        private System.Windows.Forms.Button btnSendPluginRequest;
        private System.Windows.Forms.Button btnCreateNewSolution;
        private System.Windows.Forms.Button btnAddProject;
        private System.Windows.Forms.Button btnAddReference;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnCheckForUpdates;
        private System.Windows.Forms.Label Settings_lblSolutionFile;
    }
}