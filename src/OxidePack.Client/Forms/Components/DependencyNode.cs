using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OxidePack.Client.Forms.Components
{
    /// <summary>
    /// Inherits the node class to show how the class can be extended.
    /// </summary>
    public class DependencyNode : Node
    {
        /// <exception cref="ArgumentNullException">Argument is null.</exception>
        public override string Text
        {
            get => base.Text;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                base.Text = value;
            }
        }

        private bool _checked;
        /// <summary>
        /// Whether the box is checked or not.
        /// </summary>
        public bool Checked
        {
            get => _checked;
            set => _checked = value;
        }
        
        private Image _icon;
        public Image Icon
        {
            get => _icon;
            set => _icon = value;
        }

        /// <summary>
        /// Initializes a new MyNode class with a given Text property.
        /// </summary>
        /// <param name="text">String to set the text property with.</param>
        public DependencyNode(string text)
            : base(text)
        {
        }

        private CheckState _state = CheckState.Unchecked;
        public override CheckState CheckState
        {
            get
            {
                return _state;
                bool any = false, all = true;
                foreach (var p in Nodes)
                {
                    if (p.CheckState != CheckState.Unchecked)
                    {
                        all = false;
                    }
                    else
                    {
                        any = true;
                    }
                }

                if (all)
                    return CheckState.Checked;
                if (any)
                    return CheckState.Indeterminate;
                return CheckState.Unchecked;
            }
            set => _state = value;
        }
    }
}