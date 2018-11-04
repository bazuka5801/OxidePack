using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OxidePack.Client.Forms.Components;

namespace OxidePack.Client.Forms
{
    public class DependenciesModel
    {
        public List<string> SelectedFiles;
        public Boolean Bundle;
        public Boolean Changed = false;
    }

    public partial class DependenciesForm : Form
    {
        private DependencyTreeModel _model;
        private DependenciesModel _formModel;
        
        public DependenciesForm(DependenciesModel formModel)
        {
            InitializeComponent();
            InitTree();
            ApplyModel(formModel);
        }

        public void ApplyModel(DependenciesModel formModel)
        {
            _formModel = formModel;
            _model.Load(formModel.SelectedFiles);
            cbBundle.Checked = formModel.Bundle;
        }

        private string Dir = ".references-cache";
        
        public void InitTree()
        {
            if (Directory.Exists(Dir) == false)
            {
                return;
            }
            _model = new DependencyTreeModel(Dir);
            tvDependencies.Model = _model;
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            this._formModel.Bundle = cbBundle.Checked;
            this._formModel.SelectedFiles = _model.GetSelectedFiles();
            this._formModel.Changed = true;
            this.Close();
        }



        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
