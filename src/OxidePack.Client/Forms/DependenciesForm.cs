using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OxidePack.Client.Components;

namespace OxidePack.Client
{
    public class DependenciesModel
    {
        public string Directory;
        public List<string> SelectedFiles;
        public Boolean Bundle;
        public Boolean Changed = false;
    }

    public partial class DependenciesForm : Form
    {
        private string Dir = "";
        
        private DependencyTreeModel _model;
        private DependenciesModel _formModel;
        
        public DependenciesForm(DependenciesModel formModel)
        {
            InitializeComponent();
            ApplyModel(formModel);
            InitTree();
        }

        public void ApplyModel(DependenciesModel formModel)
        {
            Dir = formModel.Directory;
            _formModel = formModel;
            cbBundle.Checked = formModel.Bundle;
        }
        
        public void InitTree()
        {
            if (Directory.Exists(Dir) == false)
            {
                return;
            }
            _model = new DependencyTreeModel(Dir);
            _model.Load(_formModel.SelectedFiles);
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
