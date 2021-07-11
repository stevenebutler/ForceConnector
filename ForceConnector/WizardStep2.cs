using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class frmWizardStep2
    {
        public frmWizardStep2(ref Excel.Range r)
        {
            rng = r;
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            var lgr = RESTAPI.getSObjectList();
            {
                var withBlock = lstObject;
                withBlock.Columns.Clear();
                withBlock.View = View.Details;
                withBlock.MultiSelect = false;
                withBlock.Columns.Add("sObject Name", 200, HorizontalAlignment.Left);
                withBlock.Columns.Add("sObject API Name", 265, HorizontalAlignment.Left);
                foreach (RESTful.DescribeGlobalSObjectResult gr in lgr)
                {
                    var obj = new Dictionary<string, string>() { { "name", gr.name }, { "label", gr.label }, { "custom", Conversions.ToString(gr.custom) } };
                    objects.Add(obj);
                    if (standards.Contains(gr.name) | gr.custom)
                    {
                        var line = new ListViewItem();
                        line.Text = gr.label;
                        line.SubItems.Add(gr.name);
                        withBlock.Items.Add(line);
                    }
                }
            }

            _btnCancel.Name = "btnCancel";
            _btnBack.Name = "btnBack";
            _btnNext.Name = "btnNext";
            _lstObject.Name = "lstObject";
            _chkSystem.Name = "chkSystem";
            _chkCustom.Name = "chkCustom";
            _chkStandard.Name = "chkStandard";
            _txtSearch.Name = "txtSearch";
        }

        public bool complete = false;
        public bool gotoStep1 = false;
        public bool gotoStep3 = false;
        private Excel.Range rng;
        private List<Dictionary<string, string>> objects = new List<Dictionary<string, string>>();
        private string[] standards = new string[] { "Account", "Campaign", "Case", "Contact", "Contract", "Event", "Lead", "Opportunity", "Pricebook2", "Product2", "Profile", "Quote", "Task", "User", "UserRole" };

        private void btnBack_Click(object sender, EventArgs e)
        {
            gotoStep1 = true;
            complete = true;
            Close();
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
                var itm = lstObject.SelectedItems[0];
                rng.Value = itm.SubItems[0].Text;
                rng.AddComment(); // error when already has comment
                rng.Comment.Text(itm.SubItems[1].Text);
                rng.Comment.Shape.Height = 20f;
                rng.Comment.Shape.Width = 200f;
                gotoStep3 = true;
                complete = true;
                Close();
            }
        }

        private void chkCustom_CheckedChanged(object sender, EventArgs e)
        {
            resetList("");
        }

        private void chkStandard_CheckedChanged(object sender, EventArgs e)
        {
            resetList("");
        }

        private void chkSystem_CheckedChanged(object sender, EventArgs e)
        {
            resetList("");
        }

        private void resetList(string search)
        {
            var objs = objects.ToArray();
            lstObject.Clear();
            {
                var withBlock = lstObject;
                withBlock.Columns.Clear();
                withBlock.View = View.Details;
                withBlock.MultiSelect = false;
                withBlock.Columns.Add("sObject Name", 200, HorizontalAlignment.Left);
                withBlock.Columns.Add("sObject API Name", 265, HorizontalAlignment.Left);
                foreach (Dictionary<string, string> obj in objs)
                {
                    string label = obj["label"];
                    string value = obj["name"];
                    if (label.IndexOf(search, 0, StringComparison.CurrentCultureIgnoreCase) > -1 | value.IndexOf(search, 0, StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        if (chkStandard.Checked && standards.Contains(value))
                        {
                            var line = new ListViewItem();
                            line.Text = label;
                            line.SubItems.Add(value);
                            withBlock.Items.Add(line);
                        }

                        if (chkCustom.Checked && Conversions.ToBoolean(obj["custom"]) == true)
                        {
                            var line = new ListViewItem();
                            line.Text = label;
                            line.SubItems.Add(value);
                            withBlock.Items.Add(line);
                        }

                        if (chkSystem.Checked)
                        {
                            if (!standards.Contains(value) && Conversions.ToBoolean(obj["custom"]) == false)
                            {
                                var line = new ListViewItem();
                                line.Text = label;
                                line.SubItems.Add(value);
                                withBlock.Items.Add(line);
                            }
                        }
                    }
                }

                lstObject.Sort();
            }
        }

        private void lstObject_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    {
                        if (lstObject.Columns[0].ListView.Sorting != SortOrder.Descending)
                        {
                            lstObject.Columns[0].ListView.Sorting = SortOrder.Descending;
                        }
                        else if (lstObject.Columns[0].ListView.Sorting != SortOrder.Ascending)
                        {
                            lstObject.Columns[0].ListView.Sorting = SortOrder.Ascending;
                        }

                        break;
                    }

                case 1:
                    {
                        if (lstObject.Columns[1].ListView.Sorting != SortOrder.Descending)
                        {
                            lstObject.Columns[1].ListView.Sorting = SortOrder.Descending;
                        }
                        else if (lstObject.Columns[1].ListView.Sorting != SortOrder.Ascending)
                        {
                            lstObject.Columns[1].ListView.Sorting = SortOrder.Ascending;
                        }

                        break;
                    }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            if (searchText.Length > 2)
            {
                resetList(searchText);
            }
        }
    }
}