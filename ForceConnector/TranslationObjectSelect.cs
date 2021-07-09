using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ForceConnector
{
    public partial class TranslationObjectSelect
    {
        public TranslationObjectSelect(string[] objectList)
        {
            // This call is required by the designer.
            InitializeComponent();
            // Add any initialization after the InitializeComponent() call.'=
            {
                var withBlock = lstObject;
                withBlock.View = View.Details;
                withBlock.MultiSelect = true;
                withBlock.Columns.Add("sObject Name", 310, HorizontalAlignment.Left);
                foreach (string obj in objectList)
                {
                    var line = new ListViewItem();
                    line.Text = obj;
                    withBlock.Items.Add(line);
                }
            }

            _btnNext.Name = "btnNext";
            _btnCancel.Name = "btnCancel";
        }
        // Public objectList() As String
        public List<string> selectedList = new List<string>();

        private void TranslationObjectSelect_Load(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lstObject.SelectedItems is null)
            {
                MessageBox.Show("You must select a object!");
            }
            else
            {
                foreach (ListViewItem itm in lstObject.SelectedItems)
                    selectedList.Add(itm.Text);
                Close();
            }
        }
    }
}