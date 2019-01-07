namespace OxidePack.Client
{
    partial class EncryptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncryptionForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSecret = new System.Windows.Forms.CheckBox();
            this.cbTrashRemoving = new System.Windows.Forms.CheckBox();
            this.cbSpaceRemoving = new System.Windows.Forms.CheckBox();
            this.cbTypes = new System.Windows.Forms.CheckBox();
            this.cbMethods = new System.Windows.Forms.CheckBox();
            this.cbFields = new System.Windows.Forms.CheckBox();
            this.cbLocalVars = new System.Windows.Forms.CheckBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.gridErrors = new System.Windows.Forms.DataGridView();
            this.co_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textEditor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gridErrors, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 561);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textEditor
            // 
            this.textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditor.IsReadOnly = false;
            this.textEditor.Location = new System.Drawing.Point(5, 4);
            this.textEditor.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textEditor.Name = "textEditor";
            this.textEditor.Size = new System.Drawing.Size(657, 273);
            this.textEditor.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEncrypt, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 425);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(657, 132);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSecret);
            this.groupBox1.Controls.Add(this.cbTrashRemoving);
            this.groupBox1.Controls.Add(this.cbSpaceRemoving);
            this.groupBox1.Controls.Add(this.cbTypes);
            this.groupBox1.Controls.Add(this.cbMethods);
            this.groupBox1.Controls.Add(this.cbFields);
            this.groupBox1.Controls.Add(this.cbLocalVars);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Size = new System.Drawing.Size(463, 124);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // cbSecret
            // 
            this.cbSecret.AutoSize = true;
            this.cbSecret.Checked = true;
            this.cbSecret.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSecret.Location = new System.Drawing.Point(287, 83);
            this.cbSecret.Name = "cbSecret";
            this.cbSecret.Size = new System.Drawing.Size(78, 22);
            this.cbSecret.TabIndex = 6;
            this.cbSecret.Text = "Secret";
            this.cbSecret.UseVisualStyleBackColor = true;
            // 
            // cbTrashRemoving
            // 
            this.cbTrashRemoving.AutoSize = true;
            this.cbTrashRemoving.Checked = true;
            this.cbTrashRemoving.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrashRemoving.Location = new System.Drawing.Point(287, 55);
            this.cbTrashRemoving.Name = "cbTrashRemoving";
            this.cbTrashRemoving.Size = new System.Drawing.Size(156, 22);
            this.cbTrashRemoving.TabIndex = 5;
            this.cbTrashRemoving.Text = "Trash Removing";
            this.cbTrashRemoving.UseVisualStyleBackColor = true;
            // 
            // cbSpaceRemoving
            // 
            this.cbSpaceRemoving.AutoSize = true;
            this.cbSpaceRemoving.Checked = true;
            this.cbSpaceRemoving.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSpaceRemoving.Location = new System.Drawing.Point(287, 27);
            this.cbSpaceRemoving.Name = "cbSpaceRemoving";
            this.cbSpaceRemoving.Size = new System.Drawing.Size(161, 22);
            this.cbSpaceRemoving.TabIndex = 4;
            this.cbSpaceRemoving.Text = "Space Removing";
            this.cbSpaceRemoving.UseVisualStyleBackColor = true;
            // 
            // cbTypes
            // 
            this.cbTypes.AutoSize = true;
            this.cbTypes.Checked = true;
            this.cbTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTypes.Location = new System.Drawing.Point(184, 27);
            this.cbTypes.Name = "cbTypes";
            this.cbTypes.Size = new System.Drawing.Size(73, 22);
            this.cbTypes.TabIndex = 3;
            this.cbTypes.Text = "Types";
            this.cbTypes.UseVisualStyleBackColor = true;
            // 
            // cbMethods
            // 
            this.cbMethods.AutoSize = true;
            this.cbMethods.Checked = true;
            this.cbMethods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMethods.Location = new System.Drawing.Point(8, 83);
            this.cbMethods.Name = "cbMethods";
            this.cbMethods.Size = new System.Drawing.Size(96, 22);
            this.cbMethods.TabIndex = 2;
            this.cbMethods.Text = "Methods";
            this.cbMethods.UseVisualStyleBackColor = true;
            // 
            // cbFields
            // 
            this.cbFields.AutoSize = true;
            this.cbFields.Checked = true;
            this.cbFields.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFields.Location = new System.Drawing.Point(8, 55);
            this.cbFields.Name = "cbFields";
            this.cbFields.Size = new System.Drawing.Size(75, 22);
            this.cbFields.TabIndex = 1;
            this.cbFields.Text = "Fields";
            this.cbFields.UseVisualStyleBackColor = true;
            // 
            // cbLocalVars
            // 
            this.cbLocalVars.AutoSize = true;
            this.cbLocalVars.Checked = true;
            this.cbLocalVars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocalVars.Location = new System.Drawing.Point(8, 27);
            this.cbLocalVars.Name = "cbLocalVars";
            this.cbLocalVars.Size = new System.Drawing.Size(150, 22);
            this.cbLocalVars.TabIndex = 0;
            this.cbLocalVars.Text = "Local Variables";
            this.cbLocalVars.UseVisualStyleBackColor = true;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEncrypt.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnEncrypt.ForeColor = System.Drawing.Color.Green;
            this.btnEncrypt.Location = new System.Drawing.Point(476, 3);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(178, 126);
            this.btnEncrypt.TabIndex = 1;
            this.btnEncrypt.Text = "ENCRYPT";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // gridErrors
            // 
            this.gridErrors.AllowUserToAddRows = false;
            this.gridErrors.AllowUserToDeleteRows = false;
            this.gridErrors.AllowUserToResizeColumns = false;
            this.gridErrors.AllowUserToResizeRows = false;
            this.gridErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.co_index,
            this.co_error,
            this.co_line,
            this.co_column});
            this.gridErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridErrors.Location = new System.Drawing.Point(3, 284);
            this.gridErrors.MultiSelect = false;
            this.gridErrors.Name = "gridErrors";
            this.gridErrors.ReadOnly = true;
            this.gridErrors.RowHeadersVisible = false;
            this.gridErrors.Size = new System.Drawing.Size(661, 134);
            this.gridErrors.TabIndex = 2;
            this.gridErrors.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridErrors_CellMouseDoubleClick);
            // 
            // co_index
            // 
            this.co_index.Frozen = true;
            this.co_index.HeaderText = "№";
            this.co_index.Name = "co_index";
            this.co_index.ReadOnly = true;
            this.co_index.Width = 52;
            // 
            // co_error
            // 
            this.co_error.Frozen = true;
            this.co_error.HeaderText = "Error";
            this.co_error.Name = "co_error";
            this.co_error.ReadOnly = true;
            this.co_error.Width = 71;
            // 
            // co_line
            // 
            this.co_line.Frozen = true;
            this.co_line.HeaderText = "Line";
            this.co_line.Name = "co_line";
            this.co_line.ReadOnly = true;
            this.co_line.Width = 67;
            // 
            // co_column
            // 
            this.co_column.Frozen = true;
            this.co_column.HeaderText = "Column";
            this.co_column.Name = "co_column";
            this.co_column.ReadOnly = true;
            this.co_column.Width = 94;
            // 
            // EncryptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = global::OxidePack.Client.Properties.Resources.oxidepack;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximumSize = new System.Drawing.Size(683, 800);
            this.MinimumSize = new System.Drawing.Size(683, 600);
            this.Name = "EncryptionForm";
            this.Text = "Encryption";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EncryptionForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ICSharpCode.TextEditor.TextEditorControl textEditor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbSecret;
        private System.Windows.Forms.CheckBox cbTrashRemoving;
        private System.Windows.Forms.CheckBox cbSpaceRemoving;
        private System.Windows.Forms.CheckBox cbTypes;
        private System.Windows.Forms.CheckBox cbMethods;
        private System.Windows.Forms.CheckBox cbFields;
        private System.Windows.Forms.CheckBox cbLocalVars;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.DataGridView gridErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_error;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_line;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_column;
    }
}