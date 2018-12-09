using System.Windows.Forms;

namespace OxidePack.Client
{
    partial class ProjectSelectForm
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
            this.lbProjects = new System.Windows.Forms.ListBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbProjects
            // 
            this.lbProjects.FormattingEnabled = true;
            this.lbProjects.ItemHeight = 18;
            this.lbProjects.Location = new System.Drawing.Point(12, 12);
            this.lbProjects.Name = "lbProjects";
            this.lbProjects.Size = new System.Drawing.Size(340, 112);
            this.lbProjects.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSubmit.Location = new System.Drawing.Point(12, 130);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(340, 43);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "Select";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // ProjectSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 189);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lbProjects);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ProjectSelectForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Project selection...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbProjects;
        private System.Windows.Forms.Button btnSubmit;
    }
}