using ICSharpCode.TextEditor;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OxidePack.Client
{
    public partial class ErrorViewForm : Form
    {
        private ErrorViewFormModel _Model;
        public ErrorViewForm(ErrorViewFormModel model)
        {
            this.InitializeComponent();
            this.LoadModel(model);
        }

        public void LoadModel(ErrorViewFormModel model)
        {
            this._Model = model;
            this.textEditor.Text = this._Model.SourceText;
            this.FillErrors(this._Model.Errors);
        }

        private void FillErrors(List<ErrorModel> errors)
        {
            gridErrors.Rows.Clear();
            int i = 1;
            foreach (ErrorModel error in errors)
            {
                var index = gridErrors.Rows.Add(i.ToString(), error.ErrorText, error.Line.ToString(), error.Column.ToString());
                gridErrors.Rows[index].Tag = error;
                i++;
            }
        }

        private void gridErrors_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (gridErrors.SelectedCells.Count > 0)
            {
                var row = gridErrors.SelectedCells[0].OwningRow;
                var error = (ErrorModel) row.Tag;

                var line = error.Line;
                var column = error.Column;
                var lineEnd = error.LineEnd;
                var columnEnd = error.ColumnEnd;

                line -= 1;
                lineEnd -= 1;
                var endIndexLine = textEditor.Document.GetText(textEditor.Document.GetLineSegment(line)).Length;
                var endIndexLineEnd = textEditor.Document.GetText(textEditor.Document.GetLineSegment(lineEnd)).Length;

                TextLocation location = new TextLocation(column, line);
                TextLocation startLine = new TextLocation(Math.Max(column - 1, 0), line);
                TextLocation endLine = new TextLocation(Math.Min(columnEnd - 1, endIndexLineEnd), lineEnd);

                textEditor.Focus();
                textEditor.ActiveTextAreaControl.SelectionManager.SetSelection(startLine, endLine);
                textEditor.ActiveTextAreaControl.Caret.Position = location;
            }
        }
    }

    public class ErrorViewFormModel
    {
        public           string SourceText;
        public List<ErrorModel> Errors;
    }

    public class ErrorModel
    {
        public    int Line;
        public    int Column;
        public    int LineEnd;
        public    int ColumnEnd;
        public string ErrorText;
    }
}
 