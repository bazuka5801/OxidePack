using System.IO;
using System.Windows.Forms;
using OxidePack.Client.Forms.Components;

namespace OxidePack.Client.Forms
{
    public partial class DependenciesForm : Form
    {
        private DependencyTreeModel _model;
        
        public DependenciesForm()
        {
            InitializeComponent();
            InitTree();
        }

        private string Dir = ".temp";
        
        public void InitTree()
        {
            if (Directory.Exists(Dir) == false)
            {
                return;
            }
            _model = new DependencyTreeModel(Dir);
            tvDependencies.Model = _model;
        }
        
        
        
        
        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes( TreeNode treeNode, bool nodeChecked )
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        // Updates all parent tree nodes recursively.
        private void CheckAllParentNodes( TreeNode treeNode, bool nodeChecked )
        {
            TreeNode pNode = treeNode.Parent;
            while (pNode != null)
            {
                if (nodeChecked)
                {
                    pNode.Checked = true;
                }
                else
                {
                    // If any child checked then node checked
                    bool value = false;
                    foreach (TreeNode node in pNode.Nodes)
                    {
                        if (node.Checked)
                        {
                            value = true;
                            break;
                        }
                    }
                    pNode.Checked = value;
                }
                pNode = pNode.Parent;
            }
        }

        private void tvDependencies_AfterCheck( object sender, TreeViewEventArgs e )
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                tvDependencies.BeginUpdate();
                if (e.Node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
                this.CheckAllParentNodes(e.Node, e.Node.Checked);
                tvDependencies.EndUpdate();
            }
        }

        private void tvDependencies_BeforeCheck( object sender, TreeViewCancelEventArgs e )
        {
        }
    }
}
