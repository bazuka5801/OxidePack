namespace OxidePack.Client.Forms
{
    partial class DependenciesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DependenciesForm));
            this.tvDependencies = new Aga.Controls.Tree.TreeViewAdv();
            this.treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            this.treeColumn2 = new Aga.Controls.Tree.TreeColumn();
            this.treeColumn3 = new Aga.Controls.Tree.TreeColumn();
            this._nodeCheckBox = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this._nodeIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this._nodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvDependencies
            // 
            this.tvDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvDependencies.BackColor = System.Drawing.SystemColors.Window;
            this.tvDependencies.Columns.Add(this.treeColumn1);
            this.tvDependencies.Columns.Add(this.treeColumn2);
            this.tvDependencies.Columns.Add(this.treeColumn3);
            this.tvDependencies.DefaultToolTipProvider = null;
            this.tvDependencies.DragDropMarkColor = System.Drawing.Color.Black;
            this.tvDependencies.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tvDependencies.Location = new System.Drawing.Point(10, 10);
            this.tvDependencies.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tvDependencies.Model = null;
            this.tvDependencies.Name = "tvDependencies";
            this.tvDependencies.NodeControls.Add(this._nodeCheckBox);
            this.tvDependencies.NodeControls.Add(this._nodeIcon);
            this.tvDependencies.NodeControls.Add(this._nodeTextBox);
            this.tvDependencies.SelectedNode = null;
            this.tvDependencies.Size = new System.Drawing.Size(264, 241);
            this.tvDependencies.TabIndex = 1;
            this.tvDependencies.Text = "treeViewAdv1";
            // 
            // treeColumn1
            // 
            this.treeColumn1.Header = "";
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeColumn1.TooltipText = null;
            // 
            // treeColumn2
            // 
            this.treeColumn2.Header = "";
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeColumn2.TooltipText = null;
            // 
            // treeColumn3
            // 
            this.treeColumn3.Header = "";
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            this.treeColumn3.TooltipText = null;
            // 
            // _nodeCheckBox
            // 
            this._nodeCheckBox.DataPropertyName = "CheckState";
            this._nodeCheckBox.EditEnabled = true;
            this._nodeCheckBox.LeftMargin = 0;
            this._nodeCheckBox.ParentColumn = null;
            this._nodeCheckBox.ThreeState = true;
            // 
            // _nodeIcon
            // 
            this._nodeIcon.DataPropertyName = "Icon";
            this._nodeIcon.LeftMargin = 1;
            this._nodeIcon.ParentColumn = null;
            this._nodeIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // _nodeTextBox
            // 
            this._nodeTextBox.DataPropertyName = "Text";
            this._nodeTextBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._nodeTextBox.IncrementalSearchEnabled = true;
            this._nodeTextBox.LeftMargin = 3;
            this._nodeTextBox.ParentColumn = null;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.ForeColor = System.Drawing.Color.Green;
            this.button1.Location = new System.Drawing.Point(5, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 258);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 48);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.ForeColor = System.Drawing.Color.Maroon;
            this.button2.Location = new System.Drawing.Point(142, 4);
            this.button2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 40);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // DependenciesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tvDependencies);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DependenciesForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dependencies Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Aga.Controls.Tree.TreeViewAdv tvDependencies;
        private Aga.Controls.Tree.TreeColumn treeColumn1;
        private Aga.Controls.Tree.TreeColumn treeColumn2;
        private Aga.Controls.Tree.TreeColumn treeColumn3;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox _nodeCheckBox;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon _nodeIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _nodeTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button2;
    }
}