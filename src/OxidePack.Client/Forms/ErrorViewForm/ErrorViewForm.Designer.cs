namespace OxidePack.Client
{
    partial class ErrorViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorViewForm));
            this.gridErrors = new System.Windows.Forms.DataGridView();
            this.co_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_error = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.co_column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridErrors
            // 
            this.gridErrors.AllowUserToAddRows = false;
            this.gridErrors.AllowUserToDeleteRows = false;
            this.gridErrors.AllowUserToResizeColumns = false;
            this.gridErrors.AllowUserToResizeRows = false;
            this.gridErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.co_index,
            this.co_error,
            this.co_line,
            this.co_column});
            this.gridErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridErrors.Location = new System.Drawing.Point(3, 347);
            this.gridErrors.MultiSelect = false;
            this.gridErrors.Name = "gridErrors";
            this.gridErrors.ReadOnly = true;
            this.gridErrors.RowHeadersVisible = false;
            this.gridErrors.Size = new System.Drawing.Size(628, 134);
            this.gridErrors.TabIndex = 2;
            this.gridErrors.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridErrors_CellMouseDoubleClick);
            // 
            // co_index
            // 
            this.co_index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.co_index.HeaderText = "№";
            this.co_index.Name = "co_index";
            this.co_index.ReadOnly = true;
            this.co_index.Width = 43;
            // 
            // co_error
            // 
            this.co_error.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.co_error.HeaderText = "Error";
            this.co_error.Name = "co_error";
            this.co_error.ReadOnly = true;
            // 
            // co_line
            // 
            this.co_line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.co_line.HeaderText = "Line";
            this.co_line.Name = "co_line";
            this.co_line.ReadOnly = true;
            this.co_line.Width = 40;
            // 
            // co_column
            // 
            this.co_column.HeaderText = "Column";
            this.co_column.Name = "co_column";
            this.co_column.ReadOnly = true;
            this.co_column.Width = 50;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textEditor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridErrors, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 484);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // textEditor
            // 
            this.textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditor.IsReadOnly = false;
            this.textEditor.Location = new System.Drawing.Point(5, 4);
            this.textEditor.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textEditor.Name = "textEditor";
            this.textEditor.Size = new System.Drawing.Size(624, 336);
            this.textEditor.TabIndex = 0;
            // 
            // ErrorViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 484);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = global::OxidePack.Client.Properties.Resources.oxidepack;
            this.Name = "ErrorViewForm";
            ((System.ComponentModel.ISupportInitialize)(this.gridErrors)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView gridErrors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ICSharpCode.TextEditor.TextEditorControl textEditor;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_index;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_error;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_line;
        private System.Windows.Forms.DataGridViewTextBoxColumn co_column;
    }
}