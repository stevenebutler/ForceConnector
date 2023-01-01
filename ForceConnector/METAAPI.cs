using System;
using System.Collections.Generic;
using System.Linq;
using ForceConnector.MiniMETA;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.ServiceModel;

namespace ForceConnector
{
    static class METAAPI
    {
        private static MetadataPortTypeClient metaClient;
        private static SessionHeader metaSessionHeader;
        private static AllOrNoneHeader allOrNoneHeader;

        public static void DownloadCustomLabels()
        {
            try
            {
                var frm = new processCustomLabelDownload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "DownloadCustomLabel Exception" + Constants.vbCrLf + ex.Message);
            }

            ThisAddIn.excelApp.StatusBar = "Download CustomLabel Translations completed";
        }

        public static void DownloadCustomLabelTranslations()
        {
            try
            {
                var frm = new processCustomLabelTranslationDownload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "DownloadCustomLabelTranslations() Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Download CustomLabel Translations completed";
        }

        public static void UploadCustomLabels()
        {
            try
            {
                var frm = new processCustomLabelUpload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "UploadCustomLabels() Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Upload New CustomLabels completed";
        }

        public static void UpdateCustomLabelTranslations()
        {
            try
            {
                var frm = new processCustomLabelTranslationUpload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "UpdateCustomLabelTranslation() Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Update CustomLabel Translations completed";
        }

        public static void DownloadObjectTranslations()
        {
            string statusText = "";
            try
            {
                // Query CustomObjectTranslation Metadata
                FileProperties[] m_files;
                var objectMap = new Dictionary<string, List<string>>();
                string[] selectedObject;
                m_files = listMetadata(new[] { "CustomObjectTranslation" });
                if (m_files is object)
                {
                    foreach (FileProperties fp in m_files)
                    {
                        if (!fp.fullName.Contains("__mdt"))
                        {
                            var tmp = fp.fullName.Split('-'); // split "Account-en_US" to "Account, en_US", finally get "Account, {en_US, ko, ...}"
                            if (objectMap.ContainsKey(tmp[0]))
                            {
                                var lang = objectMap[tmp[0]];
                                lang.Add(tmp[1]);
                                if (lang.Count > 1)
                                    lang.Sort();
                            }
                            // objectMap.Add(tmp(0), lang)
                            else
                            {
                                var lang = new List<string>();
                                lang.Add(tmp[1]);
                                if (lang.Count > 1)
                                    lang.Sort();
                                objectMap.Add(tmp[0], lang);
                            }
                        }
                    }
                }
                else
                {
                    statusText = "No ObjectTranslation Data!";
                    goto errors;
                }

                if (objectMap.Keys.Count > 0)
                {
                    var keys = objectMap.Keys.ToList();
                    keys.Sort();
                    var selectForm = new TranslationObjectSelect(keys.ToArray());
                    selectForm.ShowDialog();
                    selectedObject = selectForm.selectedList.ToArray();
                    if (selectedObject.Length == 0)
                    {
                        statusText = "No object selected";
                        goto errors;
                    }
                }
                else
                {
                    statusText = "No Available ObjectTranslation Data!";
                    goto errors;
                }

                var frm = new processObjectTranslationDownload();
                frm.objectMap = objectMap;
                frm.selectedObject = selectedObject;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "DownloadObjectTranslations() Exception");
            }

        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                Interaction.MsgBox(statusText, Title: "Download Objects Translation");
            }

            ;
            ThisAddIn.excelApp.StatusBar = "Download Object Translations completed";
        }

        public static void UpdateObjectTranslations()
        {
            Interaction.MsgBox("Under Development", Title: "No Action");
        }

        public static void DownloadTranslations()
        {
            try
            {
                var frm = new processTranslationDownload();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "DownloadTranslations() Exception");
            }

            ThisAddIn.excelApp.StatusBar = "Download Translations completed";
        }

        public static void UpdateTranslations()
        {
            Interaction.MsgBox("Under Development", Title: "No Action");
        }

        // ******************************************************************************
        // ******************************************************************************
        // * Operation
        // ******************************************************************************
        // ******************************************************************************

        // ******************************************************************************
        // * Custom Label Part
        // ******************************************************************************
        public static void setCustomLabelLayout(ref Excel.Worksheet worksheet, ref Excel.Range start)
        {
            // headline rendering
            var titleRange = worksheet.Range["A1:F1"];
            titleRange.Merge();
            titleRange.RowHeight = 26;
            titleRange.Font.Size = 20;
            titleRange.Font.Name = "Consolas";
            titleRange.Font.Bold = true;
            titleRange.Value = "Custom Label";
            start = worksheet.Range["A3"];
            renderCustomLabelHeader(ref worksheet);
        }

        public static void renderCustomLabelHeader(ref Excel.Worksheet worksheet)
        {
            var headerRow = worksheet.Range["A2:F2"];
            // Id, FullName, 
            headerRow.Font.Bold = true;
            headerRow.Font.Name = "Vernada";
            headerRow.Font.ColorIndex = 2;
            headerRow.HorizontalAlignment = Excel.Constants.xlCenter;
            headerRow.VerticalAlignment = Excel.Constants.xlCenter;
            headerRow.Interior.Color = Information.RGB(0, 176, 240);
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
            worksheet.Range["A2"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["F2"].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["A2"].Value = "FullName";
            worksheet.Range["A2"].ColumnWidth = 20;
            worksheet.Range["B2"].Value = "Language";
            worksheet.Range["B2"].ColumnWidth = 10;
            worksheet.Range["C2"].Value = "Protected";
            worksheet.Range["C2"].ColumnWidth = 10;
            worksheet.Range["D2"].Value = "Category";
            worksheet.Range["D2"].ColumnWidth = 20;
            worksheet.Range["E2"].Value = "Short Description";
            worksheet.Range["E2"].ColumnWidth = 20;
            worksheet.Range["F2"].Value = "Value";
            worksheet.Range["F2"].ColumnWidth = 30;
        }

        public static bool queryCustomLabel(ref Excel.Application excelApp, ref Excel.Range todo)
        {
            try
            {
                int i = 0;
                string[] fullNames;
                fullNames = new string[todo.Rows.Count];
                foreach (Excel.Range rw in todo.Rows)
                {
                    fullNames[i] = Conversions.ToString(rw.Cells[1, 1].Value);
                    i = i + 1;
                }

                excelApp.StatusBar = "Query CustomLabel from salesforce>" + (Information.UBound(fullNames) + 1);
                var srs = readMetadata("CustomLabel", fullNames);
                if (srs.Length > 0)
                {
                    var cld = new Dictionary<string, Metadata>();
                    foreach (Metadata sr in srs) // just make restults into dict
                        cld.Add(sr.fullName, sr);
                    excelApp.StatusBar = "back from retrieve data at salesforce";
                    i = 1;
                    foreach (Excel.Range rw in todo.Rows)
                    {
                        if (rw.Cells[1, 1].Value is object)
                        {
                            todo.Rows[i].Interior.ColorIndex = 36;
                            CustomLabel md;
                            md = (CustomLabel)cld[Conversions.ToString(rw.Cells[1, 1].Value)];
                            rw.Cells[1, 1].AddComment();
                            rw.Cells[1, 1].Comment.Text("registered");
                            rw.Cells[1, 2].value = md.language;
                            rw.Cells[1, 3].value = md.@protected;
                            rw.Cells[1, 4].value = md.categories;
                            rw.Cells[1, 5].value = md.shortDescription;
                            rw.Cells[1, 6].value = md.value;
                            todo.Rows[i].Interior.ColorIndex = 0;
                            i = i + 1;
                        }
                    }

                    cld = null;
                    srs = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("queryCustomLabel Exception" + Constants.vbCrLf + ex.Message, ex);
            }

        }

        public static void uploadCustomLabel(ref Excel.Application excelApp, ref Excel.Range todo, ref bool someFailed)
        {
            UpsertResult[] urs;
            var urMap = new Dictionary<string, UpsertResult>();
            var metadatas = new List<CustomLabel>();
            var recarray = new List<string>();
            try
            {
                excelApp.StatusBar = Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("Upload :", Operators.AddObject(Operators.SubtractObject(todo.Row, excelApp.Selection.row), 1)), " -> "), Operators.AddObject(Operators.SubtractObject(todo.Row, excelApp.Selection.row), todo.Rows.Count)), " of "), Conversions.ToString(excelApp.Selection.Rows.Count));
                todo.Interior.ColorIndex = 36; // show where we are working
                foreach (Excel.Range rw in todo.Rows)
                {
                    var meta = new CustomLabel();
                    meta.fullName = Conversions.ToString(rw.Cells[1, 1].Value);
                    meta.language = Conversions.ToString(rw.Cells[1, 2].Value);
                    meta.@protected = Conversions.ToBoolean(rw.Cells[1, 3].Value);
                    meta.categories = Conversions.ToString(rw.Cells[1, 4].Value);
                    meta.shortDescription = Conversions.ToString(rw.Cells[1, 5].Value);
                    meta.value = Conversions.ToString(rw.Cells[1, 6].Value);
                    metadatas.Add(meta);
                }

                if (metadatas.Count < 1)  // no metadatas to upload
                {
                    Util.ErrorBox("No CustomLabels to Upload");
                    todo.Interior.ColorIndex = 0; // clear out color
                    goto done;
                }

                urs = upsertMetadata(metadatas.ToArray());
                todo.Interior.ColorIndex = 0; // clear out color
                UpsertResult ur;
                foreach (var currentUr in urs)
                {
                    ur = currentUr;
                    urMap.Add(ur.fullName, ur);
                }

                foreach (Excel.Range rw in todo.Rows)
                {
                    string fullname = Conversions.ToString(rw.Cells[1, 1].Value);
                    ur = urMap[fullname];
                    if (!ur.success)
                    {
                        rw.Interior.ColorIndex = 6;
                        if (rw.Cells[1,1].Comment is object)
                            rw.Cells[1,1].Comment.Delete();
                        rw.Cells[1,1].AddComment();
                        string errMsg = "Upload CustomLabel Failed:" + '\n';
                        foreach (Error err in ur.errors)
                            errMsg = errMsg + ((int)err.statusCode).ToString() + ", " + err.message + '\n';
                        rw.Cells[1,1].Comment.Text(errMsg);
                        rw.Cells[1,1].Comment.Shape.Height = 60;
                        someFailed = true; // will message this later
                    }
                    else
                    {
                        if (rw.Cells[1,1].Comment is null)
                        {
                            rw.Cells[1,1].AddComment();
                            rw.Cells[1,1].Comment.Text("registered");
                        }
                        else
                        {
                            string comments = Conversions.ToString(rw.Cells[1, 1].Comment.Text);
                            if (comments.Contains("registered"))
                            {
                                rw.Cells[1,1].Comment.Text(comments + Constants.vbCrLf + "updated");
                            }
                        }

                        rw.Interior.ColorIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("uploadCustomLabel Exception" + Constants.vbCrLf + ex.Message);
            }

        done:
            ;
        }

        // ******************************************************************************
        // * sObject Translation Download Part
        // ******************************************************************************
        public static void setObjectTranslationLayout(ref Excel.Worksheet worksheet, string objName, ref Excel.Range start)
        {
            // headline rendering
            var titleRange = worksheet.Range["A1:B1"];
            titleRange.Merge();
            titleRange.RowHeight = 26;
            titleRange.Font.Size = 20;
            titleRange.Font.Name = "Consolas";
            titleRange.Font.Bold = true;
            titleRange.Value = objName + " Translation";
            renderObjectTranslationHeader(ref worksheet, ref start);
        }

        public static void renderObjectTranslationHeader(ref Excel.Worksheet worksheet, ref Excel.Range start)
        {
            var headerRow = worksheet.Range["A2:B2"];
            start = worksheet.Range["A3"];
            headerRow.Font.Bold = true;
            headerRow.Font.Name = "Vernada";
            headerRow.Font.ColorIndex = 2;
            headerRow.HorizontalAlignment = Excel.Constants.xlCenter;
            headerRow.VerticalAlignment = Excel.Constants.xlCenter;
            headerRow.Interior.Color = Information.RGB(0, 176, 240);
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
            worksheet.Range["A2"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["A2"].Value = "Key";
            worksheet.Range["A2"].ColumnWidth = 60;
            worksheet.Range["B2"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["B2"].Value = "Value";
            worksheet.Range["B2"].ColumnWidth = 30;
        }

        public static void renderBaseObjectDescribe(ref Dictionary<string, string> m_baseObject, string objName)
        {
            // VB.net SOAP deserializer doesn't support comment tag, so make temporary object for original label information.
            // Also CustomObject metadata did not contains original label information. It can be get from partner's SObjectDescribe.
            // {"The maximum message size quota for incoming messages (65536) has been exceeded. To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element."}
            try
            {
                m_baseObject = new Dictionary<string, string>();
                var metas = readMetadata("CustomObject", new[] { objName });
                CustomObject meta = (CustomObject)metas[0];
                string prefix = meta.fullName;
                m_baseObject.Add(prefix, string.IsNullOrEmpty(meta.label) ? "" : meta.label);
                if (meta.nameField is object)
                {
                    m_baseObject.Add(prefix + ".fields.NameField", string.IsNullOrEmpty(meta.nameField.label) ? "" : meta.nameField.label);
                }

                if (meta.fieldSets is object)
                {
                    foreach (FieldSet fs in meta.fieldSets)
                        m_baseObject.Add(prefix + ".fieldSets." + fs.fullName, fs.label);
                }

                if (meta.recordTypes is object)
                {
                    foreach (RecordType rt in meta.recordTypes)
                        m_baseObject.Add(prefix + ".recordTypes." + rt.fullName, rt.label);
                }

                if (meta.sharingReasons is object)
                {
                    foreach (SharingReason sr in meta.sharingReasons)
                        m_baseObject.Add(prefix + ".sharingReasons." + sr.fullName, sr.label);
                }

                if (meta.validationRules is object)
                {
                    foreach (ValidationRule vr in meta.validationRules)
                        m_baseObject.Add(prefix + ".validationRules." + vr.fullName, vr.errorMessage);
                }

                if (meta.webLinks is object)
                {
                    foreach (WebLink wl in meta.webLinks)
                        m_baseObject.Add(prefix + ".." + wl.fullName, wl.masterLabel);
                }

                var gr = RESTAPI.DescribeSObject(objName);
                var fields = gr.fields;
                foreach (RESTful.Field fld in fields)
                {
                    m_baseObject.Add(prefix + ".fields." + fld.name, fld.label);
                    if (fld.picklistValues is object)
                    {
                        foreach (RESTful.PicklistEntry pv in fld.picklistValues)
                            m_baseObject.Add(prefix + ".fields." + fld.name + ".picklist." + pv.label, pv.label);
                    }

                    if (fld.relationshipName is object)
                    {
                        m_baseObject.Add(prefix + ".fields." + fld.name + ".relationship", fld.relationshipName);
                    }

                    if (fld.inlineHelpText is object)
                    {
                        m_baseObject.Add(prefix + ".fields." + fld.name + ".help", fld.inlineHelpText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("renderObjectDescribe Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void renderObjectTranslation(ref Excel.Application excelApp, ref Excel.Range m_head, ref Excel.Range m_body, ref long m_rows, ref Dictionary<string, string> m_baseObject, string objName, string lang, Metadata meta)
        {
            CustomObjectTranslation ot = (CustomObjectTranslation)meta;
            string prefix = objName;
            int m_langCol = getLanguageColumn(ref m_head, lang);
            if (ot.caseValues is object)
            {
                foreach (ObjectNameCaseValue cv in ot.caseValues)
                {
                    if (!cv.plural)
                    {
                        long argm_langCol = m_langCol;
                        renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol, prefix, cv.value);
                    }
                }
            }

            if (ot.nameFieldLabel is object)
            {
                long argm_langCol1 = m_langCol;
                renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol1, prefix + ".NameField", ot.nameFieldLabel);
            }

            if (ot.fields is object)
            {
                foreach (CustomFieldTranslation fld in ot.fields)
                {
                    string keyword = prefix + ".fields." + fld.name;
                    if (fld.label is object)
                    {
                        long argm_langCol2 = m_langCol;
                        renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol2, keyword, fld.label);
                    }
                    else if (fld.caseValues is object)
                    {
                        foreach (ObjectNameCaseValue cv in fld.caseValues)
                        {
                            if (!cv.plural)
                            {
                                keyword = prefix + ".fields.caseValues." + fld.name;
                                long argm_langCol3 = m_langCol;
                                renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol3, keyword, cv.value);
                            }
                            else
                            {
                                keyword = prefix + ".fields.caseValues.plural." + fld.name;
                                long argm_langCol4 = m_langCol;
                                renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol4, keyword, cv.value);
                            }
                        }
                    }

                    if (fld.picklistValues is object)
                    {
                        foreach (PicklistValueTranslation pv in fld.picklistValues)
                        {
                            string masterLabel = pv.masterLabel.Replace("~", "{{tilde}}");
                            long argm_langCol5 = m_langCol;
                            renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol5, keyword + ".picklist." + masterLabel, string.IsNullOrEmpty(pv.translation) ? "<!-- " + pv.masterLabel + " -->" : pv.translation);
                        }
                    }

                    if (fld.relationshipLabel is object)
                    {
                        long argm_langCol6 = m_langCol;
                        renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol6, keyword + ".relationship", fld.relationshipLabel);
                    }

                    if (fld.help is object)
                    {
                        long argm_langCol7 = m_langCol;
                        renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol7, keyword + ".help", fld.help);
                    }
                }
            }

            if (ot.fieldSets is object)
            {
                foreach (FieldSetTranslation fs in ot.fieldSets)
                {
                    long argm_langCol8 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol8, prefix + ".fieldSets." + fs.name, string.IsNullOrEmpty(fs.label) ? "<!-- " + fs.name + " -->" : fs.label);
                }
            }
            // no layout definition in CustomObject, need to describe from ????
            // If ot.layouts IsNot Nothing Then
            // For Each lo As MiniMETA.LayoutTranslation In ot.layouts
            // If lo.sections IsNot Nothing Then
            // For Each ls As MiniMETA.LayoutSectionTranslation In lo.sections
            // renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".layouts." & lo.layout & ".section." & ls.section, If(ls.label = Nothing, ls.section, ls.label))
            // Next
            // End If
            // Next
            // End If
            // no quickAction definition in CustomObject, need to describe from ????
            // If ot.quickActions IsNot Nothing Then
            // For Each qa As MiniMETA.QuickActionTranslation In ot.quickActions
            // renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".quickActions." & qa.name, If(qa.label = Nothing, qa.name, qa.label))
            // Next
            // End If
            if (ot.recordTypes is object)
            {
                foreach (RecordTypeTranslation rt in ot.recordTypes)
                {
                    long argm_langCol9 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol9, prefix + ".recordTypes." + rt.name, string.IsNullOrEmpty(rt.label) ? "<!-- " + rt.name + " -->" : rt.label);
                }
            }

            if (ot.sharingReasons is object)
            {
                foreach (SharingReasonTranslation sr in ot.sharingReasons)
                {
                    long argm_langCol10 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol10, prefix + ".sharingReasons." + sr.name, string.IsNullOrEmpty(sr.label) ? "<!--" + sr.name + "-->" : sr.label);
                }
            }

            if (ot.standardFields is object)
            {
                foreach (StandardFieldTranslation sf in ot.standardFields)
                {
                    long argm_langCol11 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol11, prefix + ".standardFields." + sf.name, string.IsNullOrEmpty(sf.label) ? "<!-- " + sf.name + " -->" : sf.label);
                }
            }

            if (ot.validationRules is object)
            {
                foreach (ValidationRuleTranslation vr in ot.validationRules)
                {
                    long argm_langCol12 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol12, prefix + ".validationRules." + vr.name, string.IsNullOrEmpty(vr.errorMessage) ? "<!-- " + vr.name + " validation rule's error message -->" : vr.errorMessage);
                }
            }

            if (ot.webLinks is object)
            {
                foreach (WebLinkTranslation wl in ot.webLinks)
                {
                    long argm_langCol13 = m_langCol;
                    renderObjectItem(ref excelApp, ref m_body, ref m_rows, ref m_baseObject, ref argm_langCol13, prefix + ".webLinks." + wl.name, string.IsNullOrEmpty(wl.label) ? "<!-- " + wl.name + " -->" : wl.label);
                }
            }
            // no workflowTasks definition in CustomObject, need to describe from ????
            // If ot.workflowTasks IsNot Nothing Then
            // For Each wt As WorkflowTaskTranslation In ot.workflowTasks
            // renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".workflowTasks." & wt.name, If(wt.subject = Nothing, wt.name, wt.subject))
            // If wt.description IsNot Nothing Then
            // renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".workflowTasks." & wt.name & ".description", wt.description)
            // End If
            // Next
            // End If
        }

        // ******************************************************************************
        // * General Translation Download Part
        // ******************************************************************************
        public static void setTranslationLayout(ref Excel.Worksheet worksheet, ref Excel.Range start)
        {
            // headline rendering
            var titleRange = worksheet.Range["A1"];
            titleRange.RowHeight = 26;
            titleRange.Font.Size = 20;
            titleRange.Font.Name = "Consolas";
            titleRange.Font.Bold = true;
            titleRange.Value = "Translations";
            renderTranslationHeader(ref worksheet, ref start);
        }

        public static void renderTranslationHeader(ref Excel.Worksheet worksheet, ref Excel.Range start)
        {
            var headerRow = worksheet.Range["A2"];
            start = worksheet.Range["A3"];
            headerRow.Font.Bold = true;
            headerRow.Font.Name = "Vernada";
            headerRow.Font.ColorIndex = 2;
            headerRow.HorizontalAlignment = Excel.Constants.xlCenter;
            headerRow.VerticalAlignment = Excel.Constants.xlCenter;
            headerRow.Interior.Color = Information.RGB(0, 176, 240);
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDot;
            headerRow.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
            worksheet.Range["A2"].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            worksheet.Range["A2"].Value = "Key";
            worksheet.Range["A2"].ColumnWidth = 60;
        }

        public static void renderTranslations(ref Excel.Application excelApp, ref Excel.Range m_head, ref Excel.Range m_body, ref long m_rows, string lang, Metadata meta)
        {
            Translations trns = (Translations)meta;
            int m_langCol = getLanguageColumn(ref m_head, lang);
            string key = "";
            string label = "";
            if (trns.customApplications is object)
            {
                foreach (CustomApplicationTranslation ca in trns.customApplications)
                {
                    key = "customApplications." + ca.name;
                    label = string.IsNullOrEmpty(ca.label) ? "<!-- " + ca.name + " -->" : ca.label;
                    long argm_langCol = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol, key, label);
                }
            }

            if (trns.customPageWebLinks is object)
            {
                foreach (CustomPageWebLinkTranslation cpwl in trns.customPageWebLinks)
                {
                    key = "customPageWebLinks." + cpwl.name;
                    label = string.IsNullOrEmpty(cpwl.label) ? "<!-- " + cpwl.name + " -->" : cpwl.label;
                    long argm_langCol1 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol1, key, label);
                }
            }

            if (trns.customTabs is object)
            {
                foreach (CustomTabTranslation ct in trns.customTabs)
                {
                    key = "customTabs." + ct.name;
                    label = string.IsNullOrEmpty(ct.label) ? "<!-- " + ct.name + " -->" : ct.label;
                    long argm_langCol2 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol2, key, label);
                }
            }

            if (trns.flowDefinitions is object)
            {
                foreach (FlowDefinitionTranslation fd in trns.flowDefinitions)
                {
                    key = "flowDefinitions." + fd.fullName;
                    label = string.IsNullOrEmpty(fd.label) ? "<!-- " + fd.fullName + " -->" : fd.label;
                    long argm_langCol3 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol3, key, label);
                    if (fd.flows is object)
                    {
                        foreach (FlowTranslation fl in fd.flows)
                        {
                            string flkey = "flowDefinitions." + fd.fullName;
                            if (fl.choices is object)
                            {
                                foreach (FlowChoiceTranslation choice in fl.choices)
                                {
                                    key = flkey + ".choices." + choice.name;
                                    label = string.IsNullOrEmpty(choice.choiceText) ? "<!-- " + choice.name + " choice text -->" : choice.choiceText;
                                    long argm_langCol4 = m_langCol;
                                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol4, key, label);
                                    if (choice.userInput is object)
                                    {
                                        key = flkey + ".choices." + choice.name + ".userInput";
                                        label = string.IsNullOrEmpty(choice.userInput.promptText) ? "<!-- " + choice.name + " user input prompt text -->" : choice.userInput.promptText;
                                        long argm_langCol5 = m_langCol;
                                        renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol5, key, label);
                                        if (choice.userInput.validationRule is object)
                                        {
                                            key = flkey + ".choices." + choice.name + ".userInput.validationRule";
                                            label = string.IsNullOrEmpty(choice.userInput.validationRule.errorMessage) ? "<!-- " + choice.name + " user input validation rules' error message -->" : choice.userInput.validationRule.errorMessage;
                                            long argm_langCol6 = m_langCol;
                                            renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol6, key, label);
                                        }
                                    }
                                }
                            }

                            if (fl.screens is object)
                            {
                                foreach (FlowScreenTranslation screen in fl.screens)
                                {
                                    key = flkey + ".screens." + screen.name + ".pausedText";
                                    label = string.IsNullOrEmpty(screen.pausedText) ? "<!-- " + screen.name + " paused text -->" : screen.pausedText;
                                    long argm_langCol7 = m_langCol;
                                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol7, key, label);
                                    key = flkey + ".screens." + screen.name + ".helpText";
                                    label = string.IsNullOrEmpty(screen.helpText) ? "<!-- " + screen.name + " help text -->" : screen.helpText;
                                    long argm_langCol8 = m_langCol;
                                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol8, key, label);
                                    if (screen.fields is object)
                                    {
                                        foreach (FlowScreenFieldTranslation field in screen.fields)
                                        {
                                            key = flkey + ".screens." + screen.name + ".fields." + field.name + ".fieldText";
                                            label = string.IsNullOrEmpty(field.fieldText) ? "<!-- " + field.name + " field text -->" : field.fieldText;
                                            long argm_langCol9 = m_langCol;
                                            renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol9, key, label);
                                            key = flkey + ".screens." + screen.name + ".fields." + field.name + ".helpText";
                                            label = string.IsNullOrEmpty(field.helpText) ? "<!-- " + field.name + " help text -->" : field.helpText;
                                            long argm_langCol10 = m_langCol;
                                            renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol10, key, label);
                                            if (field.validationRule is object)
                                            {
                                                key = flkey + ".screens." + screen.name + ".fields." + field.name + ".validationRule";
                                                label = string.IsNullOrEmpty(field.validationRule.errorMessage) ? "<!-- " + field.name + " validation rule's error message -->" : field.validationRule.errorMessage;
                                                long argm_langCol11 = m_langCol;
                                                renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol11, key, label);
                                            }
                                        }
                                    }
                                }
                            }

                            if (fl.stages is object)
                            {
                                foreach (FlowStageTranslation stage in fl.stages)
                                {
                                    key = flkey + ".stages." + stage.name;
                                    label = string.IsNullOrEmpty(stage.label) ? "<!-- " + stage.name + " label -->" : stage.label;
                                    long argm_langCol12 = m_langCol;
                                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol12, key, label);
                                }
                            }

                            if (fl.textTemplates is object)
                            {
                                foreach (FlowTextTemplateTranslation tt in fl.textTemplates)
                                {
                                    key = flkey + ".textTemplates." + tt.name;
                                    label = string.IsNullOrEmpty(tt.text) ? "<!-- " + tt.name + " text -->" : tt.text;
                                    long argm_langCol13 = m_langCol;
                                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol13, key, label);
                                }
                            }
                        }
                    }
                }
            }

            if (trns.prompts is object)
            {
                foreach (PromptTranslation prompt in trns.prompts)
                {
                    key = "prompts." + prompt.name;
                    label = string.IsNullOrEmpty(prompt.label) ? "<!-- " + prompt.name + " -->" : prompt.label;
                    long argm_langCol14 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol14, key, label);
                    key = "prompts." + prompt.name + ".description";
                    label = string.IsNullOrEmpty(prompt.description) ? "<!-- " + prompt.name + " description -->" : prompt.description;
                    long argm_langCol15 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol15, key, label);
                }
            }

            if (trns.quickActions is object)
            {
                foreach (GlobalQuickActionTranslation qa in trns.quickActions)
                {
                    key = "quickActions." + qa.name;
                    label = string.IsNullOrEmpty(qa.label) ? "<!-- " + qa.name + " -->" : qa.label;
                    long argm_langCol16 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol16, key, label);
                }
            }

            if (trns.reportTypes is object)
            {
                foreach (ReportTypeTranslation rt in trns.reportTypes)
                {
                    key = "reportTypes." + rt.name;
                    label = string.IsNullOrEmpty(rt.label) ? "<!-- " + rt.name + " -->" : rt.label;
                    long argm_langCol17 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol17, key, label);
                }
            }

            if (trns.scontrols is object)
            {
                foreach (ScontrolTranslation sc in trns.scontrols)
                {
                    key = "scontrols." + sc.name;
                    label = string.IsNullOrEmpty(sc.label) ? "<!-- " + sc.name + " -->" : sc.label;
                    long argm_langCol18 = m_langCol;
                    renderItem(ref excelApp, ref m_body, ref m_rows, ref argm_langCol18, key, label);
                }
            }
        }

        // ******************************************************************************
        // ********* Parsing General Translation Part (UNDER DEVELOPMENT)
        // ******************************************************************************
        public static void updateGeneralTranslation(ref Excel.Application excelApp, ref Excel.Range m_head, ref Excel.Range m_body, ref List<string> m_langSet)
        {
            try
            {
                // prompt has child as description
                var cas = new List<CustomApplicationTranslation>();
                var cpwls = new List<CustomPageWebLinkTranslation>();
                var cts = new List<CustomTabTranslation>();
                var fds = new List<FlowDefinitionTranslation>();
                var ps = new List<PromptTranslation>();
                var qas = new List<GlobalQuickActionTranslation>();
                var rts = new List<ReportTypeTranslation>();
                var scs = new List<ScontrolTranslation>();
                var fdMap = new Dictionary<string, FlowDefinitionTranslation>();
                Excel.Range todo = (Excel.Range)excelApp.Selection;
                if (todo.Columns.Count > 1)
                {
                    Util.ErrorBox("Only one language can upload!");
                    goto done;
                }

                var langCell = excelApp.Intersect((Excel.Range)todo.Cells[1, 1].EntireColumn, m_head);
                if (langCell is null)
                {
                    Util.ErrorBox("Could not find the translatable language");
                    goto done;
                }

                if (!m_langSet.Contains(Conversions.ToString(langCell.get_Value())))
                {
                    Util.ErrorBox(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Select area's language\"", langCell.get_Value()), "\" does not supported!")));
                    goto done;
                }

                foreach (Excel.Range rw in todo.Rows)
                {
                    var nameCell = excelApp.Intersect((Excel.Range)m_body.Cells[1, 1].EntireColumn, rw.EntireRow);
                    string name = Conversions.ToString(nameCell.get_Value());
                    var childs = name.Split('.');
                    switch (childs[0] ?? "")
                    {
                        case "customApplications":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var ca = new CustomApplicationTranslation();
                                    ca.name = childs[1];
                                    ca.label = val;
                                    cas.Add(ca);
                                }

                                break;
                            }

                        case "customPageWebLinks":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var cpwl = new CustomPageWebLinkTranslation();
                                    cpwl.name = childs[1];
                                    cpwl.label = val;
                                    cpwls.Add(cpwl);
                                }

                                break;
                            }

                        case "customTabs":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var ct = new CustomTabTranslation();
                                    ct.name = childs[1];
                                    ct.label = val;
                                    cts.Add(ct);
                                }

                                break;
                            }

                        case "flowDefinitions":
                            {
                                METAAPI.parsingFlowDefinitionTranslation(ref fdMap, childs, Conversions.ToString(rw.get_Value()));
                                break;
                            }

                        case "prompts":
                            {
                                METAAPI.parsingPromptTranslation(ref ps, childs, Conversions.ToString(rw.get_Value()));
                                break;
                            }

                        case "quickActions":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var qa = new GlobalQuickActionTranslation();
                                    qa.name = childs[1];
                                    qa.label = val;
                                    qas.Add(qa);
                                }

                                break;
                            }

                        case "reportTypes":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var rt = new ReportTypeTranslation();
                                    rt.name = childs[1];
                                    rt.label = val;
                                    rts.Add(rt);
                                }

                                break;
                            }

                        case "scontrols":
                            {
                                string val = Conversions.ToString(rw.get_Value());
                                if (!val.Contains("<!--") && val.Length > 0)
                                {
                                    var sc = new ScontrolTranslation();
                                    sc.name = childs[1];
                                    sc.label = val;
                                    scs.Add(sc);
                                }

                                break;
                            }
                    }
                }

                var meta = new Translations();
                meta.fullName = Conversions.ToString(langCell.get_Value());
                if (cas.Count > 0)
                    meta.customApplications = cas.ToArray();
                if (cpwls.Count > 0)
                    meta.customPageWebLinks = cpwls.ToArray();
                if (cts.Count > 0)
                    meta.customTabs = cts.ToArray();
                // If fds.Count > 0 Then meta.flowDefinitions = fds.ToArray
                if (fdMap.Count > 0)
                    meta.flowDefinitions = fdMap.Values.ToArray();
                if (ps.Count > 0)
                    meta.prompts = ps.ToArray();
                if (qas.Count > 0)
                    meta.quickActions = qas.ToArray();
                if (rts.Count > 0)
                    meta.reportTypes = rts.ToArray();
                if (scs.Count > 0)
                    meta.scontrols = scs.ToArray();
                var srs = updateMetadata(new[] { meta });
                foreach (SaveResult sr in srs)
                {
                    if (!sr.success)
                    {
                        string msg = "";
                        foreach (Error err in sr.errors)
                            msg = msg + Constants.vbCrLf + err.message;
                        Interaction.MsgBox(msg, Title: sr.fullName);
                    }
                }

                goto done;
            }
            catch (Exception ex)
            {
                throw new Exception("updateGeneralTranslation Exception" + Constants.vbCrLf + ex.Message);
            }

        done:
            ;
        }

        public static void parsingFlowDefinitionTranslation(ref Dictionary<string, FlowDefinitionTranslation> fdm, string[] childs, string value)
        {
            if (!value.Contains("<!--") && value.Length > 0)
            {
                if (childs.Length == 2)
                {
                    FlowDefinitionTranslation fd;
                    if (fdm.ContainsKey(childs[1]))
                    {
                        fd = fdm[childs[1]];
                        fd.fullName = childs[1];
                        fd.label = value;
                        fdm[childs[1]] = fd;
                    }
                    else
                    {
                        fd = new FlowDefinitionTranslation();
                        fd.fullName = childs[1];
                        fd.label = value;
                        fdm.Add(fd.fullName, fd);
                    }
                }
                else if (childs.Length > 2)
                {
                    FlowDefinitionTranslation fd;
                    FlowTranslation[] flows;
                    if (fdm.ContainsKey(childs[1]))
                    {
                        fd = fdm[childs[1]];
                        flows = fd.flows is null ? Array.Empty<FlowTranslation>() : fd.flows;
                    }
                    else
                    {
                        fd = new FlowDefinitionTranslation();
                        flows = Array.Empty<FlowTranslation>();
                    }

                    switch (childs[3] ?? "")
                    {
                        case "choices":
                            {
                                if (flows.Length > 0)
                                {
                                }

                                break;
                            }

                        case "screens":
                            {
                                break;
                            }

                        case "stages":
                            {
                                break;
                            }

                        case "textTemplates":
                            {
                                break;
                            }
                    }
                }
            }
        }

        public static void parsingPromptTranslation(ref List<PromptTranslation> ps, string[] childs, string value)
        {
            if (!value.Contains("<!--") && value.Length > 0)
            {
                bool hasPrompt = false;
                if (childs.Length == 3)
                {
                    for (int i = 0, loopTo = ps.Count - 1; i <= loopTo; i++)
                    {
                        var p = ps[i];
                        if ((p.name ?? "") == (childs[1] ?? ""))
                        {
                            hasPrompt = true;
                            p.description = value;
                            ps[i] = p;
                        }
                    }

                    if (!hasPrompt)
                    {
                        var p = new PromptTranslation();
                        p.name = childs[1];
                        p.description = value;
                        ps.Add(p);
                    }
                }
                else if (childs.Length == 2)
                {
                    for (int i = 0, loopTo1 = ps.Count - 1; i <= loopTo1; i++)
                    {
                        var p = ps[i];
                        if ((p.name ?? "") == (childs[1] ?? ""))
                        {
                            hasPrompt = true;
                            p.label = value;
                            ps[i] = p;
                        }
                    }

                    if (!hasPrompt)
                    {
                        var p = new PromptTranslation();
                        p.name = childs[1];
                        p.label = value;
                        ps.Add(p);
                    }
                }
            }
        }



        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ' Common Functions Block
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        public static DescribeMetadataResult describeMetadata()
        {
            double api = ThisAddIn.api;
            DescribeMetadataResult dmr;
            if (setMetaBinding())
            {
                dmr = metaClient.describeMetadata(metaSessionHeader, api);
                return dmr;
            }

            throw new Exception("describeMetadata Exception, no session");
        }

        public static FileProperties[] listMetadata(string[] types)
        {
            double api = ThisAddIn.api;
            FileProperties[] fileObjs;
            if (setMetaBinding())
            {
                var metaTypes = new List<ListMetadataQuery>();
                foreach (string mtype in types)
                {
                    var metaType = new ListMetadataQuery();
                    metaType.type = mtype;
                    metaType.folder = Constants.vbNullString;
                    metaTypes.Add(metaType);
                }

                fileObjs = metaClient.listMetadata(metaSessionHeader, metaTypes.ToArray(), api);
                return fileObjs;
            }

            throw new Exception("listMetadata Exception, no session");
        }

        public static Metadata[] readMetadata(string type, string[] fullNames)
        {
            Metadata[] metas;
            if (setMetaBinding())
            {
                metas = metaClient.readMetadata(metaSessionHeader, type, fullNames);
                return metas;
            }

            throw new Exception("readMetadata Exception, no session");
        }

        public static UpsertResult[] upsertMetadata(Metadata[] metadatas)
        {
            UpsertResult[] urs;
            if (setMetaBinding())
            {
                urs = metaClient.upsertMetadata(metaSessionHeader, allOrNoneHeader, metadatas.ToArray());
                return urs;
            }

            throw new Exception("upsertMetadata Exception, no session");
        }

        public static SaveResult[] updateMetadata(Metadata[] metadatas)
        {
            SaveResult[] srs;
            if (setMetaBinding())
            {
                srs = metaClient.updateMetadata(metaSessionHeader, metadatas.ToArray());
                return srs;
            }

            throw new Exception("updateMetadata Exception, no session");
        }

        public static bool setMetaBinding()
        {
            if (!Util.checkSession())
            {
                if (!ForceConnector.LoginToSalesforce())
                    goto done;
            }

            if (Util.checkSession())
            {
                if (metaClient is null)
                {
                    metaClient = ThisAddIn.metaClient;
                    // metaClient = New MetadataPortTypeClient("Metadata", ThisAddIn.conInfo.urls.metadata)
                }

                if (metaSessionHeader is null)
                {
                    metaSessionHeader = ThisAddIn.metaSessionHeader;
                    // metaSessionHeader = New MiniMETA.SessionHeader
                    // metaSessionHeader.sessionId = ThisAddIn.accessToken
                }

                allOrNoneHeader = new AllOrNoneHeader();
                allOrNoneHeader.allOrNone = false;
                return true;
            }

        done:
            ;
            return false;
        }

        // ' BackgroundWorker implemented routines
        public static Excel.Worksheet getMetaWorkSheet(ref Excel.Workbook workbook, string metaname, bool clear = true)
        {
            Excel.Worksheet currentsheet = (Excel.Worksheet)workbook.ActiveSheet;
            try
            {
                bool find_sheet = false;
                foreach (Excel.Worksheet cs in workbook.Sheets)
                {
                    if ((cs.Name ?? "") == (metaname ?? ""))
                    {
                        find_sheet = true;
                        currentsheet = cs;
                        currentsheet.Activate();
                        int totalSheets = workbook.Sheets.Count;
                        // CType(ThisAddIn.excelApp.ActiveSheet, Excel.Worksheet).Move(After:=ThisAddIn.excelApp.Worksheets(totalSheets))
                        currentsheet.Move(After: workbook.Worksheets[totalSheets]);
                        if (clear)
                        {
                            // Dim allRange As Excel.Range = ThisAddIn.excelApp.ActiveCell.CurrentRegion
                            Excel.Range allRange = (Excel.Range)currentsheet.Application.ActiveCell.CurrentRegion;
                            allRange = allRange.get_Resize(allRange.Rows.Count, allRange.Columns.Count);
                            allRange.Select();
                            currentsheet.Application.Selection.Clear();
                        }
                    }
                }

                if (!find_sheet)
                {
                    Excel.Worksheet newsheet;
                    newsheet = (Excel.Worksheet)workbook.Worksheets.Add();
                    newsheet.Name = metaname;
                    currentsheet = newsheet;
                    currentsheet.Activate();
                }
                // excelApp.ActiveWindow.DisplayGridlines = False

                return currentsheet;
            }
            catch (Exception ex)
            {
                throw new Exception("getMetaWorkSheet Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void setWorkArea(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range m_table, ref Excel.Range m_head, ref Excel.Range m_body, ref Excel.Range m_start, ref int m_rows, ref string m_metaType)
        {
            try
            {
                excelApp.StatusBar = "build data Ranges";
                try
                {
                    if (excelApp.ActiveCell.CurrentRegion.Count == 1 && excelApp.ActiveCell.CurrentRegion.get_Value() is null)
                    {
                        worksheet.Range["A1"].Select();
                        m_table = excelApp.ActiveCell.CurrentRegion;
                    }
                    else
                    {
                        m_table = excelApp.ActiveCell.CurrentRegion;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Oops, Could not find an active Worksheet: " + ex.Message  );
                }

                m_start = (Excel.Range)m_table.Cells[1,1];
                m_rows = m_table.Rows.Count;
                if (m_table.Rows.Count == 2)
                {
                    m_body = worksheet.Range[m_table.Cells[3, 1].AddressLocal]; // 5.20
                }
                else
                {
                    m_body = worksheet.Range[m_table.Cells[3, 1], m_table.Cells[m_table.Rows.Count, m_table.Columns.Count]];
                }

                int k;
                var loopTo = m_table.Columns.Count;
                for (k = 1; k <= loopTo; k++)
                {
                    if (!Information.IsNothing(m_table.Cells[2, k].Value2))
                    {
                        m_head = worksheet.Range[m_table.Cells[2, 1], m_table.Cells[2, k]];
                    }
                }

                m_metaType = Conversions.ToString(m_start.get_Value());
                if (string.IsNullOrEmpty(m_metaType))
                    throw new Exception("could not locate a metadata name in cell " + m_start.get_Address(1) + Constants.vbCrLf);
                excelApp.StatusBar = "Query " + m_metaType + " table description";
            }
            catch (Exception ex)
            {
                throw new Exception("setWorkArea Exception" + Constants.vbCrLf + ex.Message);
            }

     
        }
        public static bool doTranslations = true;

        public static void getTranslations(ref List<string> m_langSet)
        {
            if (doTranslations)
            {
                m_langSet = new List<string>();
                try
                {
                    var fileObjs = listMetadata(new[] { "Translations" });
                    if (fileObjs is object)
                    {
                        foreach (FileProperties obj in fileObjs)
                        {
                            string fullName = obj.fullName;
                            if (!m_langSet.Contains(fullName))
                                m_langSet.Add(fullName);
                        }
                    }
                }
                catch (FaultException ex)
                {
                    if (string.Equals(ex.Code, "sf:INVALID_TYPE"))
                    {
                        doTranslations = false;
                    }
                    // Throw New Exception("getTranslations" & vbCrLf & ex.Message)
                }
                catch (Exception) { }
            }
        }

        public static void setLanguageHeaders(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range m_head, ref List<string> m_langSet)
        {
            m_head.Select();
            int columnCount = Conversions.ToInteger(excelApp.Selection.Columns.Count);
            int addedCol = 0;
            Excel.Range lastCell = (Excel.Range)m_head.Cells[1, columnCount];
            if (m_langSet.Count > 1)
                m_langSet.Sort();
            for (int i = 0, loopTo = m_langSet.Count - 1; i <= loopTo; i++)
            {
                bool notFound = true;
                for (int j = 1, loopTo1 = columnCount; j <= loopTo1; j++)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(m_head.Cells[1, j].Value, m_langSet[i], false)))
                    {
                        notFound = false;
                    }
                }

                if (notFound)
                {
                    addedCol = addedCol + 1;
                    lastCell.Copy(lastCell.Offset[0, 1]);
                    lastCell = lastCell.Offset[0, 1];
                    lastCell.Value = m_langSet[i];
                    lastCell.ColumnWidth = 30;
                }
            }

            m_head = worksheet.get_Range(m_head.Cells[1, 1], m_head.Cells[1, (columnCount + addedCol)]);
            m_head.Select();
        }

        public static int getLanguageColumn(ref Excel.Range m_head, string lang)
        {
            int j;
            var loopTo = m_head.Count;
            for (j = 1; j <= loopTo; j++)
            {
                {
                    var withBlock = m_head.Cells[1, j];
                    string apiname = Conversions.ToString(withBlock.Value);
                    if ((apiname.ToLower() ?? "") == (lang.ToLower() ?? ""))
                    {
                        return j;
                    }
                }
            }

            return default;
        }

        public static bool renderObjectItem(ref Excel.Application excelApp, ref Excel.Range m_body, ref long m_rows, ref Dictionary<string, string> m_baseObject, ref long m_langCol, string keyword, string trsnvalue)
        {
            string value = "";
            try
            {
                if (m_baseObject.ContainsKey(keyword))
                    value = m_baseObject[keyword];
                // Dim myValue As Object = excelApp.WorksheetFunction.VLookup(keyword, m_body, 1, False)
                Excel.Range keyCol = (Excel.Range)m_body.Cells[1,1];
                object findRowIdx = excelApp.WorksheetFunction.Match(keyword, keyCol.EntireColumn, false);
                m_body.Cells[Operators.SubtractObject(findRowIdx, 2), 2] = value;
                m_body.Cells[Operators.SubtractObject(findRowIdx, 2), m_langCol] = trsnvalue;
            }
            catch (Exception)
            {
                m_body.Cells[(m_rows - 1L), 1] = keyword;
                m_body.Cells[(m_rows - 1L), 2] = value;
                m_body.Cells[(m_rows - 1L), m_langCol] = trsnvalue;
                m_rows = m_rows + 1L;
                m_body = m_body.get_Resize(m_body.Rows.Count + 1, m_body.Columns.Count);
            }

            return true;
        }

        public static bool renderItem(ref Excel.Application excelApp, ref Excel.Range m_body, ref long m_rows, ref long m_langCol, string keyword, string value)
        {
            try
            {
                // Dim myValue As Object = excelApp.WorksheetFunction.VLookup(keyword, m_body, 1, False)
                Excel.Range keyCol = (Excel.Range)m_body.Cells[1,1];
                object findRowIdx = excelApp.WorksheetFunction.Match(keyword, keyCol.EntireColumn, false);
                m_body.Cells[Operators.SubtractObject(findRowIdx, 2), m_langCol] = value;
            }
            catch (Exception)
            {
                m_body.Cells[(m_rows - 1L), 1] = keyword;
                m_body.Cells[(m_rows - 1L), m_langCol] = value;
                m_rows = m_rows + 1L;
                m_body = m_body.get_Resize(m_body.Rows.Count + 1, m_body.Columns.Count);
            }

            return true;
        }
    }
}