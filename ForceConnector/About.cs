using System;

namespace ForceConnector
{
    public partial class frmAbout
    {
        public frmAbout()
        {
            InitializeComponent();
            _btnClose.Name = "btnClose";
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            string brand = "Force.com Connector NG " + ThisAddIn.Ver;
            lblBrand.Text = brand;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}