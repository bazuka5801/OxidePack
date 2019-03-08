using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OxidePack.Client
{
    public class ProjectSelectModel
    {
        public bool Success = false;
        public int Selected = -1;
        public List<string> Projects = new List<string>();
    }

    public partial class ProjectSelectForm : Form
    {
        private readonly ProjectSelectModel model;

        public ProjectSelectForm(ProjectSelectModel model)
        {
            this.model = model;
            InitializeComponent();

            model.Projects.ForEach((project) => this.lbProjects.Items.Add(project));
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lbProjects.SelectedIndex >= 0)
            {
                this.model.Selected = lbProjects.SelectedIndex;
                this.model.Success = true;
                this.Close();
            }
        }
    }
}
