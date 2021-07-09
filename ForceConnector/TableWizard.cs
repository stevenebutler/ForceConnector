using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class TableWizard
    {
        private static Excel.Application excelApp;
        private static List<RESTful.Field> fields = new List<RESTful.Field>();

        public static void QueryWizard()
        {
            excelApp = ThisAddIn.excelApp;
            Excel.Range rng;
        WizardStep1:
            ;

            // Wizard Step 1 of 4, Select start position
            rng = WizardStep1();
            if (rng is null)
                goto done;
            rng.Select();
        WizardStep2:
            ;

            // Wizard Step 2 of 4, Select SObject to Query
            var step2 = new frmWizardStep2(ref rng);
            step2.ShowDialog();
            if (!step2.complete)
                goto done;
            if (step2.gotoStep1)
                goto WizardStep1;
            if (!step2.gotoStep3)
                goto done;
            WizardStep3:
            ;

            // Wizard Step 3 of 4, Select Fields to Include
            var step3 = new frmWizardStep3(ref fields, ref rng);
            step3.ShowDialog();
            if (!step3.complete)
                goto done;
            if (step3.gotoStep2)
                goto WizardStep2;
            if (!step3.gotoStep4)
                goto done;
            // Draw fields onto the sheet
            var argflds = fields.ToArray();
            drawWizard(ref rng, ref excelApp, ref argflds);

            // Wizard Step 4 of 4, Add Query Clauses
            var step4 = new frmWizardStep4(ref step3.mapField, rng);
            step4.ShowDialog();
            if (!step4.complete)
                goto done;
            Operation.QueryData();
        done:
            ;
        }

        public static Excel.Range WizardStep1()
        {
            try
            {
                Excel.Range rnData = excelApp.InputBox("Where do you want to put the Sforce Table Query?", "Table Query Wizard - Step 1 of 4", "$A$1", Type: (object)8);
                if (rnData == null)
                    return null;
                Information.Err().Clear();
                return rnData.Cells[1, 1];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void drawWizard(ref Excel.Range rng, ref Excel.Application excelApp, ref RESTful.Field[] flds)
        {
            try
            {
                Excel.Range table;
                Excel.Range start;
                string objName;
                int pos = 0;
                table = excelApp.ActiveCell.CurrentRegion;
                start = (Excel.Range)table.Cells[1,1];
                objName = Conversions.ToString(start.get_Value());
                foreach (RESTful.Field fld in flds)
                {
                    if (fld.name == "Id")
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }

                foreach (RESTful.Field fld in flds)
                {
                    if (Util.IsRequired(fld))
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }

                foreach (RESTful.Field fld in flds)
                {
                    if (Util.IsNameField(fld))
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }

                foreach (RESTful.Field fld in flds)
                {
                    if (Util.IsStandard(fld))
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }

                foreach (RESTful.Field fld in flds)
                {
                    if (Util.IsCustom(fld))
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }

                foreach (RESTful.Field fld in flds)
                {
                    if (Util.IsReadOnly(fld))
                    {
                        pos = drawField(start.get_Offset(1, pos), fld, pos);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static int drawField(Excel.Range cel, RESTful.Field fld, int pos)
        {
            int drawFieldRet = default;
            cel.Value = fld.label;
            cel.WrapText = (object)true;

            // Clear it out and left over comments
            if (cel.Comment is object)
                cel.Comment.Delete();
            string commentStr;
            int commentHeight = 60;
            commentStr = "API Name: " + fld.name + Constants.vbCrLf;
            if (!fld.updateable)
                commentStr = commentStr + "Read Only Field" + Constants.vbCrLf;
            if (Util.IsRequired(fld))
                commentStr = commentStr + "Required on Insert" + Constants.vbCrLf;
            if (fld.name == "Id")
                commentStr = commentStr + "Primary Object Identifier" + Constants.vbCrLf;
            string fieldType = fld.type;
            switch (fieldType ?? "")
            {
                case "picklist":
                case "multipicklist":
                    {
                        commentStr = commentStr + "Type: " + fieldType + Constants.vbCrLf;
                        foreach (RESTful.PicklistEntry pickval in fld.picklistValues)
                            commentStr = commentStr + pickval.value + Constants.vbCrLf;
                        int h = fld.length * 12 + 1;
                        if (h > 60)
                            commentHeight = h;
                        break;
                    }

                default:
                    {
                        commentStr = commentStr + "Type: " + fieldType + Constants.vbCrLf;
                        break;
                    }
            }

            if (string.IsNullOrEmpty(commentStr))
                return 0;
            cel.AddComment();
            cel.Comment.Text(commentStr);
            // cel.Comment.Shape.Height = commentHeight
            // cel.Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
            cel.Comment.Shape.TextFrame.Characters().Font.Bold = (object)false;
            cel.Comment.Shape.TextFrame.AutoSize = true;
            drawFieldRet = pos + 1;
            return drawFieldRet;
        }
    }
}