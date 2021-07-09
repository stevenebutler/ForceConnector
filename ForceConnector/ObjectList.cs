using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class frmObjectList
    {
        public frmObjectList()
        {
            InitializeComponent();
            _btnNext.Name = "btnNext";
            _btnCancel.Name = "btnCancel";
            _chkStandard.Name = "chkStandard";
            _chkCustom.Name = "chkCustom";
            _chkSystem.Name = "chkSystem";
            _cmbLang.Name = "cmbLang";
        }

        public List<string> objectList = new List<string>();
        public List<string> langs;
        public string baseLanguage = ThisAddIn.userLang;
        public bool success = false;
        private List<Dictionary<string, string>> objects = new List<Dictionary<string, string>>();
        private string[] standards = new string[] { "Account", "Campaign", "Case", "Contact", "Contract", "Event", "Lead", "Opportunity", "Pricebook2", "Product2", "Profile", "Quote", "Task", "User", "UserRole" };

        private void frmObjectList_Load(object sender, EventArgs e)
        {
            setLanguage();
            if (ThisAddIn.usingRESTful)
            {
                var lgr = RESTAPI.getSObjectList();
                {
                    var withBlock = lstObject;
                    withBlock.View = View.Details;
                    withBlock.MultiSelect = true;
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

                    withBlock.Sort();
                }
            }
            else
            {
                var lgr = SOAPAPI.getSObjectList();
                {
                    var withBlock1 = lstObject;
                    withBlock1.View = View.Details;
                    withBlock1.MultiSelect = true;
                    withBlock1.Columns.Add("sObject Name", 200, HorizontalAlignment.Left);
                    withBlock1.Columns.Add("sObject API Name", 265, HorizontalAlignment.Left);
                    foreach (Partner.DescribeGlobalSObjectResult gr in lgr)
                    {
                        var obj = new Dictionary<string, string>() { { "name", gr.name }, { "label", gr.label }, { "custom", Conversions.ToString(gr.custom) } };
                        objects.Add(obj);
                        if (standards.Contains(gr.name) | gr.custom)
                        {
                            var line = new ListViewItem();
                            line.Text = gr.label;
                            line.SubItems.Add(gr.name);
                            withBlock1.Items.Add(line);
                        }
                    }

                    withBlock1.Sort();
                }
            }
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
                    objectList.Add(itm.SubItems[1].Text);
                success = true;
                Close();
            }
        }

        private void chkCustom_CheckedChanged(object sender, EventArgs e)
        {
            resetList();
        }

        private void chkStandard_CheckedChanged(object sender, EventArgs e)
        {
            resetList();
        }

        private void chkSystem_CheckedChanged(object sender, EventArgs e)
        {
            resetList();
        }

        private void resetList()
        {
            var objs = objects.ToArray();
            lstObject.Clear();
            {
                var withBlock = lstObject;
                withBlock.View = View.Details;
                withBlock.MultiSelect = true;
                withBlock.Columns.Add("sObject Name", 200, HorizontalAlignment.Left);
                withBlock.Columns.Add("sObject API Name", 265, HorizontalAlignment.Left);
                foreach (Dictionary<string, string> obj in objs)
                {
                    if (chkStandard.Checked & standards.Contains(obj["name"]))
                    {
                        var line = new ListViewItem();
                        line.Text = obj["label"];
                        line.SubItems.Add(obj["name"]);
                        withBlock.Items.Add(line);
                    }

                    if (chkCustom.Checked & Conversions.ToBoolean(obj["custom"]) == true)
                    {
                        var line = new ListViewItem();
                        line.Text = obj["label"];
                        line.SubItems.Add(obj["name"]);
                        withBlock.Items.Add(line);
                    }

                    if (chkSystem.Checked)
                    {
                        if (!standards.Contains(obj["name"]) & Conversions.ToBoolean(obj["custom"]) == false)
                        {
                            var line = new ListViewItem();
                            line.Text = obj["label"];
                            line.SubItems.Add(obj["name"]);
                            withBlock.Items.Add(line);
                        }
                    }
                }

                lstObject.Sort();
            }
        }

        private void setLanguage()
        {
            cmbLang.DisplayMember = "Label";
            cmbLang.ValueMember = "Value";
            var langTable = new DataTable();
            langTable.Columns.Add("Label", typeof(string));
            langTable.Columns.Add("Value", typeof(string));
            foreach (string key in langs.ToArray())
                langTable.Rows.Add(DescribeCustomObject.langSet[key], key);
            cmbLang.DataSource = langTable;
            cmbLang.SelectedItem = ThisAddIn.userLang;
        }

        private void cmbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            baseLanguage = Conversions.ToString(cmbLang.SelectedValue);
        }
    }
}