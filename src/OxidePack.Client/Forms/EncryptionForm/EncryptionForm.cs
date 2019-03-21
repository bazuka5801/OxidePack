using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using OxidePack.Data;

namespace OxidePack.Client
{
    public partial class EncryptionForm : Form
    {
        private readonly EncryptionModel model;

        public EncryptionForm(EncryptionModel model)
        {
            this.model = model;
            InitializeComponent();

            this.model.Plugin.OnBuilded += OnBuilded;
        }


        private void EncryptionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.model.Plugin.OnBuilded -= OnBuilded;
        }

        private void OnBuilded(BuildResponse bResponse)
        {
            this.textEditor.Text = bResponse.encrypted;
//            FillErrors(bResponse.encryptErrors);
        }

        private void FillErrors(List<CompilerError> errors)
        {
            gridErrors.Rows.Clear();
            int i = 1;
            foreach (var error in errors)
            {
                gridErrors.Rows.Add(i.ToString(), error.errorText, error.line.ToString(), error.column.ToString());
                i++;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            model.Plugin.RequestCompile(new EncryptOptions()
            {
                enabled = true,
                localvars = cbLocalVars.Checked,
                fields = cbFields.Checked,
                methods = cbMethods.Checked,
                types = cbTypes.Checked,
                spacesremoving = cbSpaceRemoving.Checked,
                trashremoving = cbTrashRemoving.Checked,
                encoding = cbSecret.Checked,
                spaghetti = cbSpaghetti.Checked,
                spaghettiControlFlow = cbSpaghettiCF.Checked,
            });
        }

        private void gridErrors_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (gridErrors.SelectedRows.Count > 0)
            {
                var cells = gridErrors.SelectedRows[0].Cells;
                var tab = Convert.ToInt32(cells[0].Value);

                int line = Convert.ToInt32(cells[2].Value);
                int column = Convert.ToInt32(cells[3].Value);


                line -= 1;
                var endIndex = textEditor.Document.GetText(textEditor.Document.GetLineSegment(line)).Length;

                TextLocation location = new TextLocation(column, line);
                TextLocation startLine = new TextLocation(Math.Max(column - 1, 0), line);
                TextLocation endLine = new TextLocation(Math.Min(column, endIndex), line);

                textEditor.Focus();
                textEditor.ActiveTextAreaControl.SelectionManager.SetSelection(startLine, endLine);
                textEditor.ActiveTextAreaControl.Caret.Position = location;
            }
        }
    }

    public class EncryptionModel
    {
        public PluginProject Plugin;
    }
}
