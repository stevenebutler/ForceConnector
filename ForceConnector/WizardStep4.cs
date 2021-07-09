using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class frmWizardStep4
    {
        public frmWizardStep4(ref Dictionary<string, RESTful.Field> mapField, Excel.Range rng)
        {
            var fieldTable = new DataTable();
            {
                var withBlock = fieldTable.Columns;
                withBlock.Add("Label", typeof(string));
                withBlock.Add("Value", typeof(string));
            }

            // This call is required by the designer.
            InitializeComponent();
            this.mapField = mapField;
            this.rng = rng;
            rngOrigin = rng;
            objectLabel = Conversions.ToString(rng.get_Value());
            objectName = rng.Comment.Text();

            // Add any initialization after the InitializeComponent() call.
            var keys = mapField.Keys;
            foreach (string key in keys)
            {
                var fld = mapField[key];
                fieldTable.Rows.Add(fld.label + " (" + fld.name + ")", fld.name);
                // cmbField.Items.Add(key)
            }

            cmbField.DisplayMember = "Label";
            cmbField.ValueMember = "Value";
            cmbField.DataSource = fieldTable;
            lstClause.View = View.Details;
            lstClause.Columns.Add("Field", 300, HorizontalAlignment.Left);
            lstClause.Columns.Add("Operator", 200, HorizontalAlignment.Left);
            lstClause.Columns.Add("Value", 300, HorizontalAlignment.Left);
            lstClause.Columns.Add("Field API", 0, HorizontalAlignment.Left);
            _btnAddClause.Name = "btnAddClause";
            _btnClearClause.Name = "btnClearClause";
            _btnClearAll.Name = "btnClearAll";
            _btnRunQuery.Name = "btnRunQuery";
            _btnClose.Name = "btnClose";

            // With Me.lstClause
            // ' this clause make MALFORMED ERROR
            // Dim line As New ListViewItem()
            // line.Text = "IsDeleted"
            // line.SubItems.Add("not equal")
            // line.SubItems.Add("True")
            // line.SubItems.Add("IsDeleted")
            // .Items.Add(line)
            // End With
        }

        public bool complete = false;
        private Dictionary<string, RESTful.Field> mapField;
        private Excel.Range rng;
        private Excel.Range rngOrigin;
        private string objectLabel;
        private string objectName;

        private void btnAddClause_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)cmbField.DataSource;
            var dr = dt.Rows[cmbField.SelectedIndex];
            if (cmbField.SelectedValue is null | cmbOperator.SelectedItem is null)
            {
                MessageBox.Show("You must select the Field and Operator.");
                return;
            }

            AddClause(dr, cmbOperator.SelectedItem, txtValue.Text);
        }

        private void btnClearClause_Click(object sender, EventArgs e)
        {
            var selItems = lstClause.SelectedItems;
            if (selItems.Count > 0)
            {
                foreach (ListViewItem itm in selItems)
                    lstClause.Items.Remove(itm);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            lstClause.Clear();
            rngOrigin.EntireRow.Clear();
            rngOrigin.EntireRow.ClearComments();
            rngOrigin.Value = objectLabel;
            rngOrigin.AddComment(); // error when already has comment
            rngOrigin.Comment.Text(objectName);
            rngOrigin.Comment.Shape.Height = 20f;
            rngOrigin.Comment.Shape.Width = 200f;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            rngOrigin.EntireRow.Clear();
            rngOrigin.EntireRow.ClearComments();
            rngOrigin.Value= objectLabel;
            rngOrigin.AddComment(); // error when already has comment
            rngOrigin.Comment.Text(objectName);
            rngOrigin.Comment.Shape.Height = 20f;
            rngOrigin.Comment.Shape.Width = 200f;
            var listItems = lstClause.Items;
            if (listItems.Count > 0)
            {
                foreach (ListViewItem itm in listItems)
                {
                    Excel.Range offset = rng.Offset[0, 1];
                    Excel.Range offset2 = rng.Offset[0, 2];
                    Excel.Range offset3 = rng.Offset[0, 3];

                    offset.Value  = itm.SubItems[0].Text;
                    offset.AddComment();
                    offset.Comment.Text(itm.SubItems[3].Text);
                    offset.Comment.Shape.Height = 20f;
                    offset.Comment.Shape.Width = 200f;
                    offset2.Value = itm.SubItems[1].Text;
                    if (itm.SubItems[2].Text is object)
                    {
                        offset3.Value = itm.SubItems[2].Text;
                    }

                    rng = offset3;
                }
            }
            else
            {
                Excel.Range offset = rng.Offset[0, 1];
                Excel.Range offset2 = rng.Offset[0, 2];
                Excel.Range offset3 = rng.Offset[0, 3];
                offset.Value = "RECORD ID";
                offset.AddComment();
                offset.Comment.Text("Id");
                offset.Comment.Shape.Height = 20f;
                offset.Comment.Shape.Width = 200f;
                offset2.Value = "not equals";
                offset3.Value = "";
            }

            complete = true;
            Close();
        }

        public void AddClause(DataRow dr, object op, string val)
        {
            {
                var withBlock = lstClause;
                var line = new ListViewItem();
                line.Text = Conversions.ToString(dr.ItemArray[0]);
                line.SubItems.Add(op as string);
                if (val is object)
                {
                    line.SubItems.Add(val);
                }
                else
                {
                    line.SubItems.Add("");
                }

                line.SubItems.Add(dr.ItemArray[1] as string);
                withBlock.Items.Add(line);
            }
        }
    }
}