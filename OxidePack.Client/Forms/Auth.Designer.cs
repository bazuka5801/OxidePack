namespace OxidePack.Client.Forms
{
    partial class Auth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Auth));
            this.lblUID = new System.Windows.Forms.Label();
            this.btnCopyUID = new System.Windows.Forms.Button();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblUID
            // 
            this.lblUID.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUID.Location = new System.Drawing.Point(14, 9);
            this.lblUID.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUID.Name = "lblUID";
            this.lblUID.Size = new System.Drawing.Size(66, 37);
            this.lblUID.TabIndex = 0;
            this.lblUID.Text = "UID";
            this.lblUID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCopyUID
            // 
            this.btnCopyUID.Enabled = false;
            this.btnCopyUID.Location = new System.Drawing.Point(265, 9);
            this.btnCopyUID.Name = "btnCopyUID";
            this.btnCopyUID.Size = new System.Drawing.Size(91, 37);
            this.btnCopyUID.TabIndex = 1;
            this.btnCopyUID.Text = "Copy";
            this.btnCopyUID.UseVisualStyleBackColor = true;
            // 
            // tbUID
            // 
            this.tbUID.Location = new System.Drawing.Point(88, 15);
            this.tbUID.Name = "tbUID";
            this.tbUID.ReadOnly = true;
            this.tbUID.Size = new System.Drawing.Size(171, 27);
            this.tbUID.TabIndex = 2;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(12, 83);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(344, 23);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbStatus.TabIndex = 3;
            this.pbStatus.Value = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 54);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(344, 26);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Auth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 118);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.tbUID);
            this.Controls.Add(this.btnCopyUID);
            this.Controls.Add(this.lblUID);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(OxidePack.Client.Properties.Resources.oxidepack));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Auth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auth";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUID;
        private System.Windows.Forms.Button btnCopyUID;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.Label lblStatus;
    }
}