using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace ForceConnector
{
    public partial class frmWizardStep3
    {
        public frmWizardStep3(ref List<RESTful.Field> flds, ref Excel.Range rng)
        {
            fields = flds;
            this.rng = rng;
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            string objectName = rng.Comment.Text();
            var gr = RESTAPI.DescribeSObject(objectName);
            {
                var withBlock = lstField;
                withBlock.View = View.Details;
                withBlock.MultiSelect = true;
                withBlock.Columns.Add("Field Label", 200, HorizontalAlignment.Left);
                withBlock.Columns.Add("Field API Name", 265, HorizontalAlignment.Left);
                foreach (RESTful.Field fld in gr.fields)
                {
                    mapField.Add(fld.name, fld);    // if sObject have duplicated label, issue the error here, use api name instead of label
                    var line = new ListViewItem();
                    line.Text = fld.label;
                    line.SubItems.Add(fld.name);
                    line.Selected = true;
                    withBlock.Items.Add(line);
                }

                withBlock.FullRowSelect = true;
            }

            _btnCancel.Name = "btnCancel";
            _btnBack.Name = "btnBack";
            _btnNext.Name = "btnNext";
        }

        private List<RESTful.Field> fields;
        private Excel.Range rng;
        public Dictionary<string, RESTful.Field> mapField = new Dictionary<string, RESTful.Field>();
        public bool complete = false;
        public bool gotoStep2 = false;
        public bool gotoStep4 = false;

        private void btnBack_Click(object sender, EventArgs e)
        {
            gotoStep2 = true;
            complete = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            fields.Clear();
            bool hasId = false;
            if (lstField.SelectedItems is null)
            {
                MessageBox.Show("You must select the fields!");
                goto done;
            }
            else
            {
                var lfield = lstField.SelectedItems;
                foreach (ListViewItem itm in lfield)
                {
                    string apiname = itm.SubItems[1].Text;
                    fields.Add(mapField[apiname]);
                    if (apiname.ToLower() == "id")
                        hasId = true;
                }

                if (!hasId)
                    goto notHasId;
                gotoStep4 = true;
                complete = true;
                Close();
                goto done;
            }

        notHasId:
            ;
            Interaction.MsgBox("Selected field set must include Id field!");
            goto done;
        done:
            ;
        }
    }
}