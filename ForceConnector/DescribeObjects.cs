using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace ForceConnector
{
    static class DescribeObjects
    {
        // Dim namedFields() As String
        // Dim standardFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
        // Dim customFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
        // Dim start As Excel.Range

        public static void DescribeSalesforceObjectsByREST()
        {
            try
            {
                var frm = new processDescribeSObject();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "DescribeSObjects Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Complete Describe SObject";
        }

        public static void setWorkSheet(ref Excel.Application excelApp, ref Excel.Workbook workbook, ref Excel.Worksheet worksheet, string objname, bool clear = true)
        {
            try
            {
                bool find_sheet = false;
                foreach (Excel.Worksheet cs in workbook.Sheets)
                {
                    if ((cs.Name ?? "") == (objname ?? ""))
                    {
                        find_sheet = true;
                        worksheet = cs;
                        worksheet.Activate();
                        int totalSheets = excelApp.ActiveWorkbook.Sheets.Count;
                        ((Excel.Worksheet)excelApp.ActiveSheet).Move(After: excelApp.Worksheets[(object)totalSheets]);
                        if (clear)
                        {
                            var allRange = excelApp.ActiveCell.CurrentRegion;
                            allRange.Select();
                            excelApp.Selection.Clear();
                        }
                    }
                }

                if (!find_sheet)
                {
                    Excel.Worksheet newsheet;
                    newsheet = (Excel.Worksheet)excelApp.Worksheets.Add();
                    newsheet.Name = objname;
                    worksheet = newsheet;
                    worksheet.Activate();
                }

                excelApp.ActiveWindow.DisplayGridlines = false;
            }
            catch (Exception ex)
            {
                throw new Exception("setWorkSheet Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void setLayout(ref Excel.Worksheet worksheet, string objname)
        {
            // columns width adjustment
            worksheet.Range["A1"].ColumnWidth = (object)2;
            worksheet.Range["B1:C1"].ColumnWidth = (object)26; // label, api name
            worksheet.Range["D1"].ColumnWidth = (object)20; // type
            worksheet.Range["E1:M1"].ColumnWidth = (object)12; // custom, autonumber, nillable, excrypted, extrenal id, length, digits, precision

            // headline rendering
            var titleRange = worksheet.Range["B1:M1"];
            titleRange.Merge();
            titleRange.RowHeight = (object)26;
            titleRange.Font.Size = (object)20;
            titleRange.Font.Name = "Consolas";
            titleRange.Font.Bold = (object)true;
            titleRange.Value = objname;
            titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
        }

        public static void renderHeader(ref Excel.Worksheet worksheet, ref Excel.Range start, string objname)
        {
            var headerRow = worksheet.Range["B3:M3"];
            start = worksheet.Range["B4"];
            // label, api name, type, custom, autonumber, nillable, length, digits, precision, encrypted, externalId      referenceto, picklist -> comments of type
            headerRow.Font.Bold = (object)true;
            headerRow.Font.Name = "Vernada";
            headerRow.Font.ColorIndex = (object)2;
            headerRow.HorizontalAlignment = Excel.Constants.xlCenter;
            headerRow.VerticalAlignment = Excel.Constants.xlCenter;
            headerRow.Interior.Color = (object)Information.RGB(0, 176, 240);
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
            worksheet.Range["B3"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["M3"].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["B3"].Value = "Label";
            worksheet.Range["C3"].Value = "API Name";
            worksheet.Range["D3"].Value = "Type";
            worksheet.Range["E3"].Value = "Custom";
            worksheet.Range["F3"].Value = "AutoNumber";
            worksheet.Range["G3"].Value = "Nillable";
            worksheet.Range["H3"].Value = "Encrypted";
            worksheet.Range["I3"].Value = "External Id";
            worksheet.Range["J3"].Value = "Length";
            worksheet.Range["K3"].Value = "Scale";
            worksheet.Range["L3"].Value = "Digits";
            worksheet.Range["M3"].Value = "Precision";
        }

        public static int renderNamedField(ref Excel.Worksheet worksheet, ref Excel.Range start, ref Dictionary<string, RESTful.Field> standardFields, int rowPointer)
        {
            if (standardFields.ContainsKey("Id"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["Id"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("MasterRecordId"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["MasterRecordId"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("RecordTypeId"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["RecordTypeId"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("IsDeleted"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["IsDeleted"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("Name"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["Name"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("Subject"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["Subject"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("CurrencyISOCode"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["CurrencyISOCode"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("CreatedById"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["CreatedById"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("CreatedDate"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["CreatedDate"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("LastModifiedById"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["LastModifiedById"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("LastModifiedDate"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["LastModifiedDate"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("SystemModstamp"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["SystemModstamp"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("LastActivityDate"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["LastActivityDate"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("LastViewedDate"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["LastViewedDate"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("LastReferencedDate"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["LastReferencedDate"]);
                rowPointer = rowPointer + 1;
            }

            if (standardFields.ContainsKey("OwnerId"))
            {
                renderField(ref worksheet, ref start, rowPointer, standardFields["OwnerId"]);
                rowPointer = rowPointer + 1;
            }

            return rowPointer;
        }

        public static int renderStandardField(ref Excel.Worksheet worksheet, ref Excel.Range start, ref string[] namedFields, ref Dictionary<string, RESTful.Field> standardFields, int rowPointer, ref int objectCount, ref int numOfPart, int numOfField, string objname, ref System.ComponentModel.BackgroundWorker bgw)
        {
            var keys = standardFields.Keys.ToArray();
            Array.Sort(keys);
            foreach (string key in keys)
            {
                if (!namedFields.Contains(key))
                {
                    int percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    bgw.ReportProgress(percent, "Describe " + objname + " (fields " + rowPointer.ToString() + " / " + numOfField.ToString() + ")");
                    renderField(ref worksheet, ref start, rowPointer, standardFields[key]);
                    rowPointer = rowPointer + 1;
                }
            }

            return rowPointer;
        }

        public static void renderCustomField(ref Excel.Worksheet worksheet, ref Excel.Range start, ref string[] namedFields, ref Dictionary<string, RESTful.Field> customFields, int rowPointer, ref int objectCount, ref int numOfPart, int numOfField, string objname, ref System.ComponentModel.BackgroundWorker bgw)
        {
            var keys = customFields.Keys.ToArray();
            Array.Sort(keys);
            foreach (string key in keys)
            {
                if (!namedFields.Contains(key))
                {
                    int percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    bgw.ReportProgress(percent, "Describe " + objname + " (fields " + rowPointer.ToString("N0") + " / " + numOfField.ToString("N0") + ")");
                    renderField(ref worksheet, ref start, rowPointer, customFields[key]);
                    rowPointer = rowPointer + 1;
                }
            }
        }

        public static void renderField(ref Excel.Worksheet worksheet, ref Excel.Range start, int rownum, RESTful.Field fld)
        {
            var startCell = start.get_Offset(rownum, 0);
            var dataRow = worksheet.Range[startCell, startCell.Offset[0, 11]];
            dataRow.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRow.Font.Name = "Vernada";
            dataRow.Style.IndentLevel = (object)1;
            dataRow.VerticalAlignment = Excel.Constants.xlCenter;
            startCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            startCell.Offset[0, 11].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            startCell.Value = fld.label;
            startCell.Offset[0, 1].Value = fld.name;
            startCell.Offset[0, 1].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            {
                var withBlock = startCell.Offset[0, 2];
                withBlock.Value = fld.type;
                withBlock.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
                if (fld.type == "picklist" | fld.type == "reference")
                {
                    withBlock.ClearComments();
                    withBlock.AddComment();
                    withBlock.Comment.Shape.TextFrame.AutoSize = true;
                    withBlock.Comment.Shape.TextFrame.Characters().Font.Bold = (object)false;
                    withBlock.Comment.Shape.TextFrame.Characters().Font.Name = "Consolas";
                    string comment = "";
                    int i = 0;
                    if (fld.type == "picklist")
                    {
                        comment = "Pickist Values :" + Constants.vbCrLf;
                        var picklists = fld.picklistValues;
                        var loopTo = picklists.Length - 1;
                        for (i = 0; i <= loopTo; i++)
                        {
                            comment = comment + picklists[i].label + " (" + picklists[i].value + ")";
                            if (i < picklists.Length - 1)
                                comment = comment + Constants.vbCrLf;
                        }

                        withBlock.Comment.Text(comment);
                    }
                    else
                    {
                        comment = "Reference To :" + Constants.vbCrLf;
                        var refs = fld.referenceTo;
                        var loopTo1 = refs.Length - 1;
                        for (i = 0; i <= loopTo1; i++)
                        {
                            comment = comment + refs[i];
                            if (i < refs.Length - 1)
                                comment = comment + Constants.vbCrLf;
                        }

                        withBlock.Comment.Text(comment);
                    }
                }
            }

            startCell.Offset[0, 3].Value = fld.custom ? "Yes" : "No";
            startCell.Offset[0, 3].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 4].Value = fld.autoNumber ? "Yes" : "No";
            startCell.Offset[0, 4].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 5].Value = fld.nillable ? "Yes" : "No";
            startCell.Offset[0, 5].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 6].Value = fld.encrypted ? "Yes" : "No";
            startCell.Offset[0, 6].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 7].Value = fld.externalId ? "Yes" : "No";
            startCell.Offset[0, 7].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 8].Value = fld.length;
            startCell.Offset[0, 8].HorizontalAlignment = Excel.Constants.xlRight;
            startCell.Offset[0, 8].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 9].Value = fld.scale;
            startCell.Offset[0, 9].HorizontalAlignment = Excel.Constants.xlRight;
            startCell.Offset[0, 9].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 10].Value = fld.digits;
            startCell.Offset[0, 10].HorizontalAlignment = Excel.Constants.xlRight;
            startCell.Offset[0, 10].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 11].Value = fld.precision;
            startCell.Offset[0, 11].HorizontalAlignment = Excel.Constants.xlRight;
            startCell.Offset[0, 11].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            startCell.Offset[0, 11].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
        }
    }
}