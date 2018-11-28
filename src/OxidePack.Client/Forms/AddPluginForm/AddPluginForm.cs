using System;
using System.Windows.Forms;

namespace OxidePack.Client
{
    public class AddPluginModel
    {
        public String Name { get; set; }
        public Boolean Success { get; set; } = false;
    }

    public partial class AddPluginForm : Form
    {
        AddPluginModel _Model;

        public AddPluginForm(AddPluginModel model)
        {
            this._Model = model;
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this._Model.Name = tbPluginName.Text;
            this._Model.Success = true;
            this.Close();
        }
    }
}
