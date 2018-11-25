using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OxidePack.Client.Components
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

        protected CheckState _state = CheckState.Unchecked;
        public override CheckState CheckState
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                if (_state == CheckState.Checked)
                {
                    if (Parent != default)
                    {
                        var parent = Parent;
                        while (string.IsNullOrEmpty(parent.Text) == false)
                        {
                            if (parent.Nodes.All(p => p.CheckState == CheckState.Unchecked))
                                ((DependencyNode)parent)._state = CheckState.Unchecked;
                            else if (parent.Nodes.All(p => p.CheckState == CheckState.Checked))
                                ((DependencyNode) parent)._state = CheckState.Checked;
                            else
                                ((DependencyNode) parent)._state = CheckState.Indeterminate;
                            parent = parent.Parent;
                        }
                    }
                    foreach (var node in base.Nodes)
                        node.CheckState = CheckState.Checked;
                }
                else if (_state == CheckState.Unchecked)
                {
//                    if (Parent != default)
//                        Parent.CheckState = CheckState.Indeterminate;
                    if (Parent != default)
                    {
                        var parent = Parent;
                        while (string.IsNullOrEmpty(parent.Text) == false)
                        {
                            if (parent.Nodes.All(p => p.CheckState == CheckState.Unchecked))
                                ((DependencyNode)parent)._state = CheckState.Unchecked;
                            else if (parent.Nodes.All(p => p.CheckState == CheckState.Checked))
                                ((DependencyNode) parent)._state = CheckState.Checked;
                            else
                                ((DependencyNode) parent)._state = CheckState.Indeterminate;
                            parent = parent.Parent;
                        }
                    }
                    foreach (var node in base.Nodes)
                        node.CheckState = CheckState.Unchecked;
                }
                else if (_state == CheckState.Indeterminate)
                {
                    if (base.Nodes == null || base.Nodes.Count == 0)
                        CheckState = CheckState.Unchecked;
                    else if (base.Nodes.All(p => p.CheckState == CheckState.Unchecked))
                        _state = CheckState.Unchecked;
                    else if (base.Nodes.All(p => p.CheckState == CheckState.Checked))
                        CheckState = CheckState.Unchecked;
                }
            }
        }
    }
}