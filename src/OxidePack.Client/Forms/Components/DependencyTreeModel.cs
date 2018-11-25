using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Aga.Controls.Tree;

namespace OxidePack.Client.Components
{
    public class DependencyTreeModel : TreeModel
    {
        private string _directory;
        private string[] _files;
        
        public DependencyTreeModel(string directory)
        {
            this._directory = directory;
            ReloadFiles();
        }

        public void ReloadFiles()
        {
            this._files = Directory.GetFiles(this._directory);
            ReloadNodes();
        }

        public void ReloadNodes()
        {
            this.Nodes.Clear();

            foreach (var file in _files)
            {
                CreateDependencyNode(file);
            }

            var rootFiles = Nodes.Where(p => p.Text.EndsWith(".dll")).ToList();
            var rootDirs = Nodes.Except(rootFiles).ToList();
            foreach (var file in rootFiles)
            {
                var dirName = Path.GetFileNameWithoutExtension(file.Text);
                var dir = rootDirs.FirstOrDefault(p => p.Text == dirName);
                if (dir != null)
                {
                    file.Parent.Nodes.Remove(file);
                    dir.Nodes.Add(file);
                }
            }
            
            SortNodes(this.Nodes);
        }

        public void SortNodes(Collection<Node> nodes)
        {
            var directories = nodes.Where(p => p.Text.EndsWith(".dll") == false).OrderBy(p=>p.Text).ToList();
            var files = nodes.Except(directories).OrderBy(p => p.Text).ToList();
            nodes.Clear();
            directories.ForEach(nodes.Add);
            files.ForEach(nodes.Add);
            
            foreach (var node in nodes)
            {
                if (node.Nodes.Count > 1)
                {
                    SortNodes(node.Nodes);
                }
            }
        }
        
        public Node CreateDependencyNode(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var filename = Path.GetFileName(path);
            
            string[] treePath = name.Split('.');
            if (treePath.Length == 1)
            {
                return AddChild(default, this.Nodes, filename, path);
            }

            Node node = null;
            var nodeCollection = this.Nodes;
            Node nodeParent = default;
            for (var i = 0; i < treePath.Length - 1; i++)
            {
                var nodeText = treePath[i];
                node = nodeCollection.FirstOrDefault(p => p.Text == nodeText);
                if (node == null)
                {
                    node = AddChild(nodeParent, nodeCollection, nodeText);
                }
                nodeParent = node;
                nodeCollection = node.Nodes;
            }

            AddChild(node, node.Nodes, filename, path);

            return node;
        }

        public List<string> GetSelectedFiles()
        {
            List<string> filesList = new List<string>();
            var collection = this.Nodes;
            DeepTree(node =>
            {
                if (node.Nodes == null || node.Nodes.Count == 0)
                    if (node.CheckState == CheckState.Checked)
                        filesList.Add(node.Text);
            });
            return filesList;
        }

        public void Load(List<string> fileList)
        {
            DeepTree(node =>
            {
                if (node.Nodes == null || node.Nodes.Count == 0)
                    node.CheckState = fileList.Contains(node.Text) ? CheckState.Checked : CheckState.Unchecked;
            });
        }

        private void DeepTree(Action<Node> action)
        {
            DeepTreeInternal(this.Nodes, action);
        }

        private void DeepTreeInternal(Collection<Node> nodes, Action<Node> action)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var item = nodes[i];
                action(item);
                if (item.Nodes != null && item.Nodes.Count > 0)
                {
                    DeepTreeInternal(item.Nodes, action);
                }
            }
        }
        
        private Node AddChild(Node parent, Collection<Node> nodes, string text, string icon = null)
        {
            DependencyNode node = new DependencyNode(text);
            node.Parent = parent;
            if (string.IsNullOrEmpty(icon) == false)
            {
                if (DLLImage == null)
                {
                    DLLImage = ResizeImage(Icon.ExtractAssociatedIcon(icon).ToBitmap(),16,16);
                }

                node.Icon = DLLImage;
            }
            nodes.Add(node);
            return node;
        }

        private Image DLLImage = null;
        
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}