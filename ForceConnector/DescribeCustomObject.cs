using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Text;

namespace ForceConnector
{
    static class DescribeCustomObject
    {
        public static Dictionary<string, string> langSet = new Dictionary<string, string>() { { "zh_CN", "Chinese(Simplified)" }, { "zh_TW", "Chinese (Traditional)" }, { "da", "Danish" }, { "nl_NL", "Dutch" }, { "en_US", "English" }, { "fi", "Finnish" }, { "fr", "French" }, { "de", "German" }, { "it", "Italian" }, { "ja", "Japanese" }, { "ko", "Korean" }, { "no", "Norwegian" }, { "pt_BR", "Portuguese (Brazil)" }, { "ru", "Russian" }, { "es", "Spanish" }, { "es_MX", "Spanish (Mexico)" }, { "sv", "Swedish" }, { "th", "Thai" } };
        private static Dictionary<int, string> fieldType = new Dictionary<int, string>() { { 0, "String" }, { 1, "Picklist" }, { 2, "Multi Picklist" }, { 3, "Combobox" }, { 4, "Reference" }, { 5, "Base64" }, { 6, "Boolean" }, { 7, "Currency" }, { 8, "Textarea" }, { 9, "Integer" }, { 10, "Double" }, { 11, "Percent" }, { 12, "Phone" }, { 13, "Id" }, { 14, "Date" }, { 15, "Datetime" }, { 16, "Time" }, { 17, "Url" }, { 18, "Email" }, { 19, "Encrypted String" }, { 20, "DataCategoryGroupReference" }, { 21, "Location" }, { 22, "Address" }, { 23, "AnyType" }, { 24, "Json" }, { 25, "Complex Value" }, { 26, "Long" } };
        private static string[] HeaderLabels = new string[]
        {
            "Label",
            "API Name",
            "Type",
            "Custom",
            "AutoNumber",
            "Nillable",
            "Encrypted",
            "External Id",
            "Length",
            "Scale",
            "Digits",
            "Precision",
            "Description"
        };

        public static void DescribeSalesforceObjectsBySOAP()
        {
            try
            {
                var frm = new processDescribeCustomObject();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "DescribeSObjects Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Complete Describe SObject";
        }

        public static Partner.DescribeSObjectResult DescribeSObject(string objname, string baseLang)
        {
            return SOAPAPI.DescribeSObject(objname, baseLang);
        }

        public static Dictionary<string, Dictionary<string, string>> getFieldTranslations(string objname, ref Dictionary<string, string> objLabels, ref Partner.Field[] fields, ref List<string> langSet, ref string baseLang, ref int percent, ref System.ComponentModel.BackgroundWorker bgw)
        {
            try
            {
                var fieldMeta = new Dictionary<string, Dictionary<string, string>>();
                var fieldSet = new Dictionary<string, MiniMETA.CustomField>();
                var fieldTranslation = new Dictionary<string, Dictionary<string, string>>();
                bgw.ReportProgress(percent, "Get metadata information for " + objname + "'s fields...");
                MiniMETA.CustomObject co = (MiniMETA.CustomObject)METAAPI.readMetadata("CustomObject", new[] { objname })[0];
                foreach (MiniMETA.CustomField cf in co.fields)
                    fieldSet.Add(cf.fullName, cf);
                if (langSet.Count > 0)
                {
                    bgw.ReportProgress(percent, "Get translation information for " + objname + "'s fields...");
                    foreach (string lang in langSet.ToArray())
                    {
                        var fieldInfo = new Dictionary<string, string>();
                        if ((lang ?? "") != (baseLang ?? ""))
                        {
                            var dsr = DescribeSObject(objname, lang);
                            string baseLabel = Conversions.ToString(Operators.ConcatenateObject(dsr.label + ", ", Interaction.IIf(!string.IsNullOrEmpty(dsr.labelPlural), dsr.labelPlural, "no_plural_label")));
                            objLabels.Add(lang, baseLabel);
                            foreach (Partner.Field fld in dsr.fields)
                                fieldInfo.Add(fld.name, fld.label);
                            fieldTranslation.Add(lang, fieldInfo);
                        }
                        else
                        {
                            foreach (Partner.Field fld in fields)
                                fieldInfo.Add(fld.name, fld.label);
                            fieldTranslation.Add(baseLang, fieldInfo);
                        }
                    }
                }

                bgw.ReportProgress(percent, "Add field description and(or) translation for " + objname + "'s fields...");
                foreach (Partner.Field fld in fields)
                {
                    var fldinfo = new Dictionary<string, string>();
                    if (fieldSet.TryGetValue(fld.name, out var val) && val.description is not null)
                    {
                        fldinfo.Add("desc", val.description);
                    }

                    foreach (string lang in langSet)
                    {
                        if (fieldTranslation.TryGetValue(lang, out var translation) && translation.TryGetValue(fld.name, out var translationName))
                        {
                            fldinfo.Add(lang, string.IsNullOrEmpty(translationName) ? fld.label : translationName);
                        }
                    }

                    fieldMeta.Add(fld.name, fldinfo);
                }

                return fieldMeta;
            }
            catch (Exception ex)
            {
                throw new Exception("getFieldMetadatas Exception", ex);
            }
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

        public static void setLayout(ref Excel.Worksheet worksheet, string objname, ref Dictionary<string, string> objLabels)
        {
            // columns width adjustment
            worksheet.Range["A1"].ColumnWidth = (object)2;
            worksheet.Range["B1:C1"].ColumnWidth = (object)26; // label, api name
            worksheet.Range["D1"].ColumnWidth = (object)20; // type
            worksheet.Range["E1:M1"].ColumnWidth = (object)12; // custom, autonumber, nillable, excrypted, extrenal id, length, digits, precision
            worksheet.Range["N1"].ColumnWidth = (object)30; // description
            if (objLabels.Count > 2)
            {
                string labels = "";
                foreach (string key in objLabels.Keys)
                {
                    if (key != "base")
                    {
                        string trns = objLabels[key];
                        if (trns.Length > 0)
                            labels = labels + "[" + key + "] " + trns + Constants.vbCrLf;
                    }
                }

                if (labels.Length > 0)
                {
                    worksheet.Range["A1"].ClearComments();
                    worksheet.Range["A1"].AddComment();
                    worksheet.Range["A1"].Comment.Shape.TextFrame.AutoSize = true;
                    worksheet.Range["A1"].Comment.Shape.TextFrame.Characters().Font.Bold = (object)false;
                    worksheet.Range["A1"].Comment.Shape.TextFrame.Characters().Font.Name = "Consolas";
                    worksheet.Range["A1"].Comment.Text(labels);
                }
            }

            // headline rendering
            var titleRange = worksheet.Range["B1:N1"];
            titleRange.Merge();
            titleRange.RowHeight = (object)26;
            titleRange.Font.Size = (object)20;
            titleRange.Font.Name = "Consolas";
            titleRange.Font.Bold = (object)true;
            titleRange.Value = objname + " [" + objLabels["base"] + "]";
            titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
        }

        public static void renderHeader(ref Excel.Worksheet worksheet, ref Excel.Range start, string objname)
        {
            var headerRow = worksheet.Range["B3:N3"];
            start = worksheet.Range["B4"];
            headerRow.Font.Bold = true;
            headerRow.Font.Name = "Vernada";
            headerRow.Font.ColorIndex = 2;
            headerRow.HorizontalAlignment = Excel.Constants.xlCenter;
            headerRow.VerticalAlignment = Excel.Constants.xlCenter;
            headerRow.Interior.Color = Information.RGB(0, 176, 240);
            headerRow.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
            headerRow.Value = HeaderLabels;
        }

        public static int renderNamedField(ref Excel.Worksheet worksheet, ref Excel.Range start, string[] namedFieldsOrder, ref Dictionary<string, Partner.Field> standardFields, ref Dictionary<string, Dictionary<string, string>> fieldMeta, int rowPointer, object[,] data, List<CommentPosition> comments)
        {
            foreach (var fld in namedFieldsOrder)
            {
                if (standardFields.ContainsKey(fld))
                {
                    Dictionary<string, string> fldinfo = fieldMeta.ContainsKey(fld) ? fieldMeta[fld] : null;
                    populateFieldValues(ref worksheet, ref start, ref fldinfo, rowPointer, standardFields[fld], data, comments);
                    rowPointer = rowPointer + 1;
                }
            }
            return rowPointer;
        }

        public static int renderStandardField(ref Excel.Worksheet worksheet, ref Excel.Range start, HashSet<string> namedFields, ref Dictionary<string, Partner.Field> standardFields, ref Dictionary<string, Dictionary<string, string>> fieldMeta, int rowPointer, ref int objectCount, ref int numOfPart, int numOfField, string objname, ref System.ComponentModel.BackgroundWorker bgw, object[,] data, List<CommentPosition> comments)
        {
            var keys = standardFields.Keys.ToArray();
            Array.Sort(keys);
            foreach (string key in keys)
            {
                if (!namedFields.Contains(key))
                {
                    int percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    if (percent > 100)
                    {
                        percent = 100;
                    }
                    bgw.ReportProgress(percent, "Describe " + objname + " (fields " + rowPointer.ToString() + " / " + numOfField.ToString() + ")");
                    Dictionary<string, string> fldinfo = (Dictionary<string, string>)Interaction.IIf(fieldMeta.ContainsKey(key), fieldMeta[key], null);
                    populateFieldValues(ref worksheet, ref start, ref fldinfo, rowPointer, standardFields[key], data, comments);
                    rowPointer = rowPointer + 1;
                }
            }

            return rowPointer;
        }

        public static void renderCustomField(ref Excel.Worksheet worksheet, ref Excel.Range start, HashSet<string> namedFields, ref Dictionary<string, Partner.Field> customFields, ref Dictionary<string, Dictionary<string, string>> fieldMeta, int rowPointer, ref int objectCount, ref int numOfPart, int numOfField, string objname, ref System.ComponentModel.BackgroundWorker bgw, object[,] data, List<CommentPosition> comments)
        {
            var keys = customFields.Keys.ToArray();
            Array.Sort(keys);
            foreach (string key in keys)
            {
                if (!namedFields.Contains(key))
                {
                    int percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    if (percent > 100)
                    {
                        percent = 100;
                    }
                    bgw.ReportProgress(percent, "Describe " + objname + " (fields " + rowPointer.ToString("N0") + " / " + numOfField.ToString("N0") + ")");
                    Dictionary<string, string> fldinfo = (Dictionary<string, string>)Interaction.IIf(fieldMeta.ContainsKey(key), fieldMeta[key], null);
                    populateFieldValues(ref worksheet, ref start, ref fldinfo, rowPointer, customFields[key], data, comments);
                    rowPointer = rowPointer + 1;
                }
            }
        }

        private static string getLabelComment(Dictionary<string, string> fieldinfo)
        {
            if (fieldinfo != null)
            {
                string labels = "";
                foreach (string key in fieldinfo.Keys)
                {
                    if (key != "desc")
                    {
                        string trns = fieldinfo[key];
                        if (trns.Length > 0)
                            labels = labels + "[" + key + "] " + trns + Constants.vbCrLf;
                    }
                }
                return labels.Length > 0 ? labels : null;
            }
            return null;

        }

        private static string getFieldTypeComment(Partner.Field fld)
        {

            if (fld.type == Partner.fieldType.picklist || fld.type == Partner.fieldType.multipicklist || fld.type == Partner.fieldType.reference)
            {

                if (fld.type == Partner.fieldType.picklist || fld.type == Partner.fieldType.multipicklist)
                {
                    return "Picklist Values :"
                        + Constants.vbCrLf
                        + string.Join(Constants.vbCrLf, fld.picklistValues.Select(p => $"{p.label} ({p.value})"));
                }
                else
                {
                    return "Reference To :"
                        + Constants.vbCrLf
                        + string.Join(Constants.vbCrLf, fld.referenceTo);
                }
            }
            else
            {
                return null;
            }
        }


        public static void populateFieldValues(ref Excel.Worksheet worksheet, ref Excel.Range start, ref Dictionary<string, string> fieldinfo, int rownum, Partner.Field fld, object[,] data, List<CommentPosition> comments)
        {
            data[rownum, 0] = fld.label;
            string labelComment = getLabelComment(fieldinfo);
            if (labelComment != null)
            {
                comments.Add(new CommentPosition
                {
                    Comment = labelComment,
                    Column = 0,
                    Row = rownum
                });
            }
            string fieldTypeComment = getFieldTypeComment(fld);
            if (fieldTypeComment != null)
            {
                comments.Add(new CommentPosition
                {
                    Comment = fieldTypeComment,
                    Column = 2,
                    Row = rownum
                });
            }
            data[rownum, 1] = fld.name;
            data[rownum, 2] = fieldType[(int)fld.type];
            data[rownum, 3] = fld.custom ? "Yes" : "No";
            data[rownum, 4] = fld.autoNumber ? "Yes" : "No";
            data[rownum, 5] = fld.nillable ? "Yes" : "No";
            data[rownum, 6] = fld.encrypted ? "Yes" : "No";
            data[rownum, 7] = fld.externalId ? "Yes" : "No";
            data[rownum, 8] = fld.length;
            data[rownum, 9] = fld.scale;
            data[rownum, 10] = fld.digits;
            data[rownum, 11] = fld.precision;
            data[rownum, 12] = fieldinfo.ContainsKey("desc") ? fieldinfo["desc"] : "";
        }

        public static void renderComments(ref Excel.Range body, List<CommentPosition> comments)
        {
            foreach (var cP in comments)
            {
                Excel.Range startCell = body.Cells[cP.Row + 1, cP.Column + 1];
                int cellRow = startCell.Row;
                int cellCol = startCell.Column;
                string coord = $"Cell {cellRow},{cellCol}; {startCell.Rows.Count}x{startCell.Columns.Count}";
                Console.WriteLine(coord);
                //startCell.ClearComments();
                startCell.AddComment(cP.Comment);
                startCell.Comment.Shape.TextFrame.AutoSize = true;
                startCell.Comment.Shape.TextFrame.Characters().Font.Bold = false;
                startCell.Comment.Shape.TextFrame.Characters().Font.Name = "Consolas";
            }

        }
    }
}