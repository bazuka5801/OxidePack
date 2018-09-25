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
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.Settings_pnlSolution = new System.Windows.Forms.Panel();
            this.Settings_lblSolutionFile = new System.Windows.Forms.Label();
            this.Settings_btnSolutionOpen = new System.Windows.Forms.Button();
            this.Settings_lblSolutionFilePath = new System.Windows.Forms.Label();
            this.tpProfile = new System.Windows.Forms.TabPage();
            this.mStatus.SuspendLayout();
            this.pnlAuth.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.Settings_pnlSolution.SuspendLayout();
            this.tpProfile.SuspendLayout();
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
            this.mStatus.Size = new System.Drawing.Size(455, 22);
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
            this.pnlAuth.Location = new System.Drawing.Point(197, 6);
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
            this.menuStrip.Size = new System.Drawing.Size(455, 24);
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
            this.tbMain.Size = new System.Drawing.Size(455, 427);
            this.tbMain.TabIndex = 8;
            // 
            // tpMain
            // 
            this.tpMain.Location = new System.Drawing.Point(4, 27);
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tpMain.Size = new System.Drawing.Size(447, 396);
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Main";
            this.tpMain.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.Settings_pnlSolution);
            this.tpSettings.Location = new System.Drawing.Point(4, 27);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(5);
            this.tpSettings.Size = new System.Drawing.Size(447, 396);
            this.tpSettings.TabIndex = 2;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
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
            this.tpProfile.Controls.Add(this.pnlAuth);
            this.tpProfile.Location = new System.Drawing.Point(4, 27);
            this.tpProfile.Name = "tpProfile";
            this.tpProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tpProfile.Size = new System.Drawing.Size(447, 396);
            this.tpProfile.TabIndex = 1;
            this.tpProfile.Text = "Profile";
            this.tpProfile.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 473);
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
            this.tpSettings.ResumeLayout(false);
            this.Settings_pnlSolution.ResumeLayout(false);
            this.tpProfile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCopyUID;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.StatusStrip mStatus;
        private System.Windows.Forms.ToolStripProgressBar pbStatus;
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
        private System.Windows.Forms.Label Settings_lblSolutionFile;
    }
}