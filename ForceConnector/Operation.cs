using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class Operation
    {
        public static bool RequireConfirmation = false;

        public static DateTime LastCheckedLogin { get; internal set; }

        public static void QueryData()
        {
            try
            {
                var frm = new processDatabaseQueryTable();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "QueryData Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void RefreshData()
        {
            try
            {
                var frm = new processDatabaseQuerySelectedRows();
                frm.RefreshAll = true;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "QueryData Exception" + Constants.vbCrLf + ex.Message);
            }
        }


        public static void UpdateCells()
        {
            if (RegDB.RegQueryBoolValue(ForceConnector.SKIPHIDDEN))
            {
                UpdateCells_New();
                return;
            }

            try
            {
                var frm = new processDatabaseUpdateRows();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "UpdateCells Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void UpdateCells_New()
        {
            try
            {
                var frm = new processDatabaseUpdateRowsNew();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "UpdateCells Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void InsertRows()
        {
            try
            {
                var frm = new processDatabaseInsertRows();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "InsertRows Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void QueryRows()
        {
            try
            {
                var frm = new processDatabaseQuerySelectedRows();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "InsertRows Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        public static void DeleteRecords()
        {
            try
            {
                var frm = new processDatabaseDeleteRows();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "InsertRows Exception" + Constants.vbCrLf + ex.Message);
            }
        }

        // ******************************************************************************
        // ******************************************************************************
        // * Operation
        // ******************************************************************************
        // ******************************************************************************

        // ******************************************************************************
        // * Query Data Part
        // ******************************************************************************
        public static bool BuildQueryString(
            ref Excel.Application excelApp,
            ref Excel.Range g_table,
            ref Excel.Range g_start,
            ref Excel.Range g_header,
            ref Excel.Range refIds,
            ref string joinfield,
            ref bool oneeachrow,
            Dictionary<string, RESTful.Field> fieldLabelMap,
            Dictionary<string, RESTful.Field> fieldMap,
            List<RESTful.Field> selectFieldList,
            out List<string> sels,
            ref string where,
            ref string statusText
            )
        {
            // remove old contents with value, formatting, comments
            if (g_table.Rows.Count > 2)
            {
                g_table.Offset[2, 0].get_Resize(g_table.Rows.Count - 2, g_table.Columns.Count).Select();
                excelApp.Selection.Clear();
            }

            g_start.Select();

            // build-up field list to query
            sels = selectFieldList.Select(x => x.name).ToList();
            string api = null, opr, vlu;
            int jw;
            RESTful.Field field_obj;
            oneeachrow = false;  // for "on" joins
            jw = 2;

            // build-up where statements
            while (!string.IsNullOrWhiteSpace(g_table.Cells[1, jw].value)) // if it's not empty, assume its more query
            {
                var apiCell = g_table.Cells[1, jw];
                string apiVal = Convert.ToString(apiCell.Value);
                if (fieldLabelMap.ContainsKey(apiVal))
                {
                    api = fieldLabelMap[apiVal].name;
                }
                else
                {
                    var comment = apiCell.Comment;
                    if (comment != null)
                    {
                        api = comment.Text();
                    }
                    if (string.IsNullOrEmpty(api))
                    {
                        api = apiVal;
                    }
                }

                opr = Conversions.ToString(g_table.Cells[1, jw + 1].value); // the operator
                object obVal = g_table.Cells[1, jw + 2].value;
                vlu = Conversions.ToString(g_table.Cells[1, jw + 2].value); // the criteria value(s)
                field_obj = fieldMap[api]; // 5.46 get the field as an object

                // operator
                // add other aliases here if you like
                opr = Strings.LCase(opr);
                switch (opr ?? "")
                {
                    case "equals":
                        {
                            opr = "=";
                            break;
                        }

                    case "contains":
                        {
                            opr = "like";
                            break;
                        }

                    case "not equals":
                        {
                            opr = "!=";
                            break;
                        }

                    case "less than":
                        {
                            opr = "<";
                            break;
                        }

                    case "greater than":
                        {
                            opr = ">";
                            break;
                        }
                }

                // 5.23 basic error check, 5.0 checks for this anyway but we know where the offending cell is
                if (opr == "like" && field_obj.type == "picklist")
                {
                    statusText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("like (or contains) operator in cell ", g_table.Cells[1, (jw + 1)].AddressLocal), " is not valid on picklist fields, "), Constants.vbCrLf), " use --> equals, not equals"));
                    goto errors;
                }

                // special case 'in' and a ref field
                if ((opr == "in" | opr == "on") && field_obj.type == "reference")
                {
                    if (opr == "on")
                        oneeachrow = true;
                    refIds = Util.build_ref_range(vlu); // list of IDs to use in join
                    joinfield = field_obj.name; // save for later, should be only one..
                    if (refIds is null)
                    {
                        statusText = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("Range error, could not build a valid range from the string" + Constants.vbCrLf + "--> " + vlu + " <--" + Constants.vbCrLf + " in the cell ", g_table.Cells[1, (jw + 2)].AddressLocal), "expected valid range (ex: 'A:A') or range name"));
                        goto errors;
                    }
                }
                else
                {
                    // general case
                    // Value ~ assemble the where clause using field, opr and values list
                    // this loop has been re-written (ver 5.04) to properly
                    // deal with comma seperated values i.e. -> field | operator |this,that|
                    // should become (field operator 'this' OR field operator 'that')
                    // unless it's multipicklist type then produce slightly different string for SOQL:
                    // (field inclqudes ('this') or field includes ('that'))
                    // (field excludes ('this') or field excludes ('that'))
                    // 
                    // if values is empty and vlu is the nul string, still need to assemble the clause
                    string clause = "";
                    if (string.IsNullOrEmpty(vlu)) // case of one empty value
                    {
                        if (field_obj.type == "date") // special case, compare value is an empty date 5.49
                        {
                            clause = $"{field_obj.name} {opr} null";
                        }
                        else
                        {
                            clause = $"{field_obj.name} {opr} ''";
                        }
                    }
                    else
                    {
                        string[] values = Strings.Split(vlu, ",");

                        foreach (string vu2 in values) // works for one or many non nul values
                        {
                            string str;
                            string referTo = "";
                            str = vu2;
                            str = Util.escapeQueryString(str); // escape some chars
                            if (field_obj.referenceTo is object && field_obj.referenceTo.Length > 0)
                            {
                                referTo = field_obj.referenceTo[0];
                            }

                            string vu = Util.NameToId(str, referTo); // map strs to refid's 5.46
                            if (Strings.Len(clause) > 0)
                                clause = clause + " or "; // prepend an or
                            if (LikeOperator.LikeString(opr, "like", CompareMethod.Binary))
                                vu = "%" + vu + "%";  // wrap like string with wildcard
                            if (LikeOperator.LikeString(opr, "begins with", CompareMethod.Binary) | LikeOperator.LikeString(opr, "starts with", CompareMethod.Binary))
                                vu = vu + "%"; // wildcar at front
                            if (LikeOperator.LikeString(opr, "ends with", CompareMethod.Binary))
                                vu = "%" + vu; // wlidcard at end
                            if (LikeOperator.LikeString(opr, "regexp", CompareMethod.Binary))
                                opr = "like";  // pass the user provided wildcard
                            string fmtVal;
                            fmtVal = Util.QueryValueFormat(field_obj.type, vu, obVal); // format value for SOQL
                            if (field_obj.type == "multipicklist")
                                fmtVal = "(" + fmtVal + ")"; // special case
                            if (LikeOperator.LikeString(opr, "starts with", CompareMethod.Binary) | LikeOperator.LikeString(opr, "begins with", CompareMethod.Binary) | LikeOperator.LikeString(opr, "ends with", CompareMethod.Binary))  // remap these to 'like'
                            {
                                clause = clause + field_obj.name + " " + "Like" + " " + fmtVal; // **** thanks to tim_bouscal!
                            }
                            else
                            {
                                clause = clause + field_obj.name + " " + opr + " " + fmtVal;
                            } // assemble the clause
                        }
                        if (values.Length > 1)
                        {
                            clause = $"({clause})"; // cant hurt
                        }
                    }

                    where = where + clause; // : Debug.Print where
                    if (!string.IsNullOrWhiteSpace(g_table.Cells[1, (jw + 3)].value))
                    {
                        where = where + " and ";
                    }
                } // to be ready for more

                jw = jw + 3; // slide over to grab the next three cells
            } // end loop while we have more WHERE clauses to add
              // g_table

            goto done;
        errors:
            ;
            return false;
        done:
            ;
            return true;
        }

        // do the query and draw the rows we got back
        public static long queryDataDraw(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range g_header, ref Excel.Range g_body, ref Excel.Range g_ids, ref string g_objectType, ref RESTful.DescribeSObjectResult g_sfd, List<string> sels, string where, long outrow, ref System.ComponentModel.BackgroundWorker bgw)
        {
            RESTful.QueryResult queryData;
            string statusBarText = "Select Data From " + g_objectType;
            excelApp.StatusBar = Strings.Left(statusBarText, 128);

            int currentRow = 0;
            int totals = 0;
            int startRow = g_body.Row;
            try
            {
                queryData = RESTAPI.Query($"SELECT {string.Join(", ", sels)} FROM {g_objectType}{where}");
                totals = queryData.totalSize;
                if (totals == 0)
                {
                    // output something, like... "#N/F"
                    g_body.Cells[outrow, (g_ids.Column - g_body.Column + 1)].value = "#N/F";
                    goto done;
                }
                int column = g_body.Column;
                while (true)
                {
                    ApplyDataToRange(worksheet, startRow + currentRow, column, sels, queryData.records, g_sfd);
                    currentRow += queryData.records.Length;
                    int percent = (int)Math.Round(currentRow / (double)totals * 100d);
                    if (percent > 100)
                        percent = 100;
                    bgw.ReportProgress(percent, "Download records (" + currentRow.ToString("N0") + " / " + totals.ToString("N0") + ")");
                    if (queryData.done) break;
                    if (bgw.CancellationPending)
                    {
                        return currentRow;
                    }
                    queryData = RESTAPI.QueryMore(queryData.nextRecordsUrl);
                }

            }
            catch (Exception ex)
            {
                g_body.Cells[startRow + currentRow, g_ids.Column - g_body.Column + 1].Value = "#Err";
                excelApp.ScreenUpdating = true;
                throw new Exception("queryDataDraw Exception" + Constants.vbCrLf + ex.Message);
            }

        done:
            ;
            return currentRow;
        }

        // ******************************************************************************
        // * Update Selected Rows Part
        // ******************************************************************************
        public static bool UpdateLimitCheck(ref Excel.Range s, ref string statusText)
        {
            if (s.Areas.Count > 1)
            {
                statusText = "Cannot run on multiple selections";
                return false;
            }

            if (RegDB.RegQueryBoolValue(ForceConnector.NOLIMITS))
                return true;

            // adjust these limits to meet your requirements, or flip NOLIMITS in the options dialog
            if (s.Rows.Count > ForceConnector.maxRows | s.Columns.Count > ForceConnector.maxCols)
            {
                statusText = "Selection too large, cannot run on > " + ForceConnector.maxRows.ToString() + "rows and > " + ForceConnector.maxCols.ToString() + " cols";
                return false;
            }

            return true;
        }

        public static void updateRange(
            ref Excel.Application excelApp,
            ref Excel.Worksheet worksheet,
            ref Excel.Range g_header,
            ref string g_objectType,
            ref Excel.Range g_start,
            ref RESTful.DescribeSObjectResult g_sfd,
            ref Excel.Range g_ids,
            ref Excel.Range todo,
            ref bool someFailed,
            ref long row_counter,
            ref long totals,
            List<RESTful.Field> headerFields,
            Dictionary<string, RESTful.Field> fieldLabelMap,
            Dictionary<string, RESTful.Field> fieldMap,
            ref System.ComponentModel.BackgroundWorker bgw)
        {
            var srMap = new Dictionary<string, RESTful.SaveResult>();
            var recordSet = new Dictionary<string, object>();
            //var idlist = new List<string>();
            var rec = new List<Dictionary<string, object>>();
            RESTful.SaveResult[] srs;
            try
            {
                var idlist = objectids(ref excelApp, ref worksheet, ref g_ids, ref todo).Select(x => Util.FixID(x)).ToArray();
                if (idlist.Length == 0)
                    goto done; // how ?
                int percent = (int)Math.Round(row_counter / (double)totals * 100d);
                bgw.ReportProgress(percent, "Building record block from row " + row_counter.ToString("N0"));

                // cell 5 selected
                // start col for range is 3
                // headers goes from 0 through count -1
                // startHeader would be 2
                // 2 = 5 - 3
                int tableStart = g_start.Column;
                int startCol = todo.Column;
                int endCol = startCol + todo.Columns.Count - 1;

                var todoData = todo.Value;
                if (!(todoData is object[,]))
                {
                    // Because Excel based arrays are 1 based we put our data at position 1, 1
                    todoData = new object[2, 2] { { null, null }, { null, todoData } };
                }

                for (var i = 0; i < idlist.Length; i++)
                {

                    recordSet = new Dictionary<string, object>();
                    recordSet.Add("attributes", new RESTful.Attributes(g_objectType));
                    recordSet.Add("Id", idlist[i]);
                    for (int j = startCol; j <= endCol; j++)
                    {

                        var field = headerFields[j - tableStart];
                        // field name
                        string fld = field.name;

                        // only updatable columns add to recordSet
                        if (!field.updateable)
                            goto nextcol;
                        var targetVal = todoData[i + 1, j - startCol + 1];
                        // Excel.Range target = todo.Cells[i + 1, j - startCol + 1];
                        recordSet.Add(fld, Util.toSalesforceType(targetVal, field));
                    nextcol:
                        ;
                    }
                    // if recordSet does not contains any updatable columns, cancel update
                    if (rec.Count == 0 && recordSet.Count == 2)
                    {
                        throw new Exception("No updatable columns selected, operation canceled.");
                    }

                    rec.Add(recordSet);
                }

                srs = RESTAPI.UpdateRecords(rec.ToArray());
                if (srs.Length != idlist.Length)
                {
                    throw new Exception("Update response size doesn't match sent size");
                }
                for (int i = 0; i < srs.Length; i++)
                {
                    var sr = srs[i];
                    // When an item isn't found in Salesforce, the response contains a null Id
                    // In this case we repopulate with the ID we used on the way out.
                    if (sr.id == null)
                    {
                        sr.id = idlist[i];
                    }
                    srMap.Add(sr.id, sr);
                }

                percent = (int)Math.Round((row_counter + srs.Length) / (double)totals * 100d);
                bgw.ReportProgress(percent, "Updating (" + srs.Length + ") records from row " + row_counter.ToString("N0"));
                row_counter = row_counter + srs.Length;
                updateResultHandler(ref excelApp, ref todo, ref someFailed, rec, srMap);
            }
            catch (Exception ex)
            {
                fieldMap = null;
                srMap = null;
                srs = null;
                throw new Exception("updateRange Exception" + Constants.vbCrLf + ex.Message);
            }

        done:
            ;
            fieldMap = null;
            srMap = null;
            srs = null;
        }

        private static void updateResultHandler(ref Excel.Application excelApp, ref Excel.Range todo, ref bool someFailed, List<Dictionary<string, object>> rec, Dictionary<string, RESTful.SaveResult> srMap)
        {
            RESTful.SaveResult s;
            int i = 0;
            todo.ClearComments();
            todo.Interior.ColorIndex = 0;
            foreach (var r in rec.ToArray())
            {
                s = srMap[Conversions.ToString(r["Id"])];

                // find out what is wrong with this record
                if (!s.success)
                {
                    Excel.Range thisrow;
                    Excel.Range firstcel;
                    thisrow = excelApp.Intersect(todo.get_Offset(i, 1).EntireRow, todo);
                    firstcel = (Excel.Range)thisrow.Offset[0, 0].Cells[1, 1];
                    // Debug.Print r.ErrorMessage
                    // turns out that if one field fails, the entire row fails
                    thisrow.Interior.ColorIndex = 6;

                    firstcel.AddComment();
                    string errMsg = "Update Row Failed:" + '\n';
                    foreach (RESTful.SalesforceError err in s.errors)
                        errMsg = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(errMsg, err.statusCode), ", "), err.message), '\n'));
                    firstcel.Comment.Text(errMsg);
                    // firstcel.Comment.Shape.Height = 100f; // is this enough
                    someFailed = true; // will message this later
                }
                else
                {
                    // Do nothing, since we pre-clear on the assumption the update will work
                    // and we only need to do something with a range if something failed.
                    // clear out the color on this row only
                    // also remove any comments which may now be incorrect
                    // for this entire row, need to clear on each col of the selection

                    //thisrow.Interior.ColorIndex = 0;
                    //foreach (Excel.Range c in thisrow.Cells)
                    //{
                    //    if (c.Comment is object)
                    //        c.Comment.Delete();
                    //}
                }

                i = i + 1;
            }
        }

        public static void calcUpdateRange(ref Excel.Range xlSelection, ref long totalRow, ref long totalCol, bool blnSkipHidden, bool blnNoLimits)
        {
            long lngRows;
            long lngCols;
            var lngHiddenRows = default(long);
            var lngHiddenCols = default(long);
            lngRows = xlSelection.Rows.Count;
            lngCols = xlSelection.Columns.Count;

            // // have we set the "Skip hidden fields" option?
            if (blnSkipHidden == true)
            {
                // // count hidden rows
                foreach (Excel.Range xlRow in xlSelection.Rows)
                {
                    if (Conversions.ToBoolean(xlRow.Hidden))
                        lngHiddenRows = lngHiddenRows + 1L;
                }
                // // count hidden columns
                foreach (Excel.Range xlColumn in xlSelection.Columns)
                {
                    if (Conversions.ToBoolean(xlColumn.Hidden))
                        lngHiddenCols = lngHiddenCols + 1L;
                }
            }

            totalRow = lngRows - lngHiddenRows;
            totalCol = lngCols - lngHiddenCols;

            // // have we set the "Disregard reasonable limits" option?
            if (!blnNoLimits)
            {
                // // let's see if the selection is within the confines
                if (totalRow > ForceConnector.maxRows)
                {
                    throw new Exception("You can't process more than " + ForceConnector.maxRows + " rows.");
                }

                if (totalCol > ForceConnector.maxCols)
                {
                    throw new Exception("You can't process more than " + ForceConnector.maxCols + " columns.");
                }
            }
        }

        public static void updateResultHandlerNew(ref Excel.Worksheet worksheet, ref long intFailedRows, List<Dictionary<string, object>> records, string[] strArryCells)
        {
            var srMap = new Dictionary<string, RESTful.SaveResult>();
            RESTful.SaveResult[] srs;
            Excel.Range xlTempRow;
            Excel.Range xlCell;

            // // now, let's do the update
            var rec = records.ToArray();
            srs = RESTAPI.UpdateRecords(rec);
            foreach (RESTful.SaveResult sr in srs)
            {
                srMap.Add(sr.id, sr);
            }
            int i = 0;
            foreach (var r in records)
            {
                // // check if the update was okay
                var sr = srMap[Conversions.ToString(r["Id"])];
                xlTempRow = worksheet.get_Range(strArryCells[i]);
                if (!sr.success)
                {
                    xlTempRow.Interior.ColorIndex = 6;
                    foreach (Excel.Range currentXlCell in xlTempRow.Cells)
                    {
                        xlCell = currentXlCell;
                        if (xlCell.Comment is object)
                            xlCell.Comment.Delete();
                    }

                    {
                        var withBlock = xlTempRow.Cells[1, 1];
                        string errMsg = "Update Row Failed:" + Constants.vbLf;
                        foreach (RESTful.SalesforceError err in sr.errors)
                            errMsg = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(errMsg, err.statusCode), ", "), err.message), Constants.vbLf));
                        withBlock.AddComment();
                        withBlock.Comment.Text(errMsg);
                        withBlock.Comment.Shape.Height = 60;
                    }

                    intFailedRows = intFailedRows + 1L;
                }
                else
                {
                    xlTempRow.Interior.ColorIndex = 0;
                    foreach (Excel.Range currentXlCell1 in xlTempRow.Cells)
                    {
                        xlCell = currentXlCell1;
                        if (xlCell.Comment is object)
                            xlCell.Comment.Delete();
                    }
                }

                i = i + 1;
            }
        }

        // ******************************************************************************
        // * Insert Selected Rows Part
        // ******************************************************************************
        public static void insertSelectedRange(ref Excel.Application excelApp,
            ref Excel.Worksheet worksheet,
            ref Excel.Range g_table,
            ref Excel.Range g_header,
            ref RESTful.DescribeSObjectResult g_sfd,
            ref string g_objectType,
            ref Excel.Range g_ids,
            ref Excel.Range todo,
            ref bool someFailed,
            ref long row_counter,
            ref long totals,
            ref System.ComponentModel.BackgroundWorker bgw,
            List<RESTful.Field> headerFields,
            Dictionary<string, RESTful.Field> fieldLabelMap,
            Dictionary<string, RESTful.Field> fieldMap
            )
        {
            Excel.Range xlSelection;
            var records = new List<object>();
            var recarray = new List<string>();
            try
            {
                xlSelection = (Excel.Range)excelApp.Selection;
                long row_pointer = todo.Row - xlSelection.Row;
                int percent = (int)Math.Round(row_counter / (double)totals * 100d);
                string msg = (row_pointer + 1L).ToString("N0") + " -> " + (row_pointer + todo.Rows.Count).ToString("N0") + " of " + xlSelection.Rows.Count.ToString("N0");
                bgw.ReportProgress(percent, "Create the record block :" + msg);
                int IDcol;
                todo.Interior.ColorIndex = 36; // show where we are working
                foreach (Excel.Range rw in todo.Rows)
                {
                    // don't insert if there is no "new" label in the id column
                    if (!LikeOperator.LikeString(objectid(ref excelApp, ref worksheet, ref g_ids, rw.Row, true), "[nN][eE][wW]*", CompareMethod.Binary))
                        goto nextrow;
                    var attributes = new RESTful.Attributes(g_objectType);
                    var record = new Dictionary<string, object>();
                    record.Add("attributes", attributes);
                    int j;
                    var loopTo = g_header.Count;
                    for (j = 1; j <= loopTo; j++)
                    {
                        var fld = headerFields[j - 1];
                        string name = fld.name;

                        if (name != "Id")
                        {
                            // don't overwrite the id on this row, needs to be empty when passed to create
                            // find the field, TODO i have a routine to find the field, could refactor this loop.


                            // excelApp.StatusBar = "loading value for " && name
                            string celVal = Conversions.ToString(g_table.Cells[(rw.Row + 1 - g_table.Row), j].value);
                            if (!string.IsNullOrEmpty(celVal))
                            {
                                // here we have a value check it and load it into the fld value
                                // 5.10 dont load field values unless the field is createable
                                if (fld.createable)
                                {
                                    var fieldValue = Util.toVBtype((Excel.Range)g_table.Cells[(rw.Row + 1 - g_table.Row), j], fld);
                                    if (fld.type == "reference")
                                    {
                                        record.Add(name, Util.NameToId(Conversions.ToString(fieldValue), fld.referenceTo[0]));
                                    }
                                    else
                                    {
                                        record.Add(name, fieldValue);
                                    }
                                }
                            }
                        }
                        else
                        {
                            IDcol = j; // save this location for later
                            recarray.Add(Conversions.ToString(g_table.Cells[(rw.Row + 1 - g_table.Row), IDcol].Address));
                        }
                    }

                    // if recordSet does not contains any updatable columns, cancel update
                    if (records.Count == 0 && record.Count == 1)
                    {
                        throw new Exception("No insertable columns selected, operation canceled.");
                    }

                    records.Add(record);
                    row_counter = row_counter + 1L;
                nextrow:
                    ;
                }

                if (records.Count < 1)  // no records to insert
                {
                    Util.ErrorBox("No records to Insert in this block, enter the string 'New' on one or more rows");
                    todo.Interior.ColorIndex = 0; // clear out color
                    goto done;
                }

                bgw.ReportProgress(percent, "Insert the record block :" + msg);
                insertResultHandler(ref excelApp, ref worksheet, ref records, ref recarray, ref todo, ref someFailed);
            }
            catch (Exception ex)
            {
                throw new Exception("insertSelectedRange Exception" + Constants.vbCrLf + ex.Message, ex);
            }

        done:
            ;
        }

        private static void insertResultHandler(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref List<object> records, ref List<string> recarray, ref Excel.Range todo, ref bool someFailed)
        {
            RESTful.SaveResult[] srs;
            srs = RESTAPI.CreateRecords(records.ToArray());
            todo.Interior.ColorIndex = 0; // clear out color
            RESTful.SaveResult sr;
            int i = 0;
            var loopTo = Information.UBound(recarray.ToArray());
            for (i = 0; i <= loopTo; i++)
            {
                sr = srs[i];
                var firstcel = worksheet.get_Range(recarray[i]);
                Excel.Range thisrow;
                thisrow = excelApp.Intersect(firstcel.EntireRow, todo);

                // find out what is wrong with this record
                if (!sr.success)
                {
                    // Debug.Print r.ErrorMessage
                    // turns out that if one field fails, the entire row fails
                    thisrow.Interior.ColorIndex = 6;
                    foreach (Excel.Range c in thisrow.Cells)
                    {
                        if (c.Comment is object)
                            c.Comment.Delete();
                    }

                    try
                    {
                        firstcel.AddComment();
                    }
                    catch (Exception ex)
                    {
                        firstcel.ClearComments();
                        firstcel.AddComment();
                    }

                    string errMsg = "Insert Row Failed:" + '\n';
                    foreach (RESTful.SalesforceError err in sr.errors)
                        errMsg = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(errMsg, err.statusCode), ", "), err.message), '\n'));
                    firstcel.Comment.Text(errMsg);
                    firstcel.Comment.Shape.Height = 60f; // is this enough
                    someFailed = true; // will message this later
                }
                else
                {
                    firstcel.Value = sr.id;
                    // clear out the color on this row only
                    // also remove any comments which may now be incorrect
                    // for this entire row, need to clear on each col of the selection
                    thisrow.Interior.ColorIndex = 0;
                    foreach (Excel.Range c in thisrow.Cells)
                    {
                        if (c.Comment is object)
                            c.Comment.Delete();
                    }
                }
            } // 5.43 end
              // no return value from this func
        }

        // ******************************************************************************
        // * Query Selected Rows Part
        // ******************************************************************************
        public static bool querySelectedRow(
            ref Excel.Application excelApp,
            ref Excel.Worksheet worksheet,
            ref Excel.Range g_header,
            ref Excel.Range g_body,
            ref Excel.Range g_ids,
            ref string g_objectType,
            ref RESTful.DescribeSObjectResult g_sfd,
            List<string> sels,
            ref Excel.Range todo,
            ref long outrow,
            ref long totals,
            ref System.ComponentModel.BackgroundWorker bgw,
            List<RESTful.Field> headerFields,
            Dictionary<string, RESTful.Field> fieldLabelMap,
            Dictionary<string, RESTful.Field> fieldMap
        )
        {
            try
            {
                int percent = 0;

                percent = (int)Math.Round(outrow / (double)totals * 100d);
                if (percent > 100)
                    percent = 100;
                bgw.ReportProgress(percent, $"Download {todo.Rows.Count} records from row {outrow:N0}");
                var idlist = objectids(ref excelApp, ref worksheet, ref g_ids, ref todo);

                var qrs = RESTAPI.RetrieveRecords(g_objectType, idlist, sels.ToArray());
                //var sd = new Dictionary<string, object>();
                //foreach (IDictionary x in qrs)
                //{
                //    if (x != null)
                //    {
                //        if (!sd.ContainsKey(Conversions.ToString(x["Id"])))
                //        {
                //            sd.Add(Conversions.ToString(x["Id"]), x);
                //        }
                //    }
                //}

                int skipColumn = g_ids.Column;
                ApplyDataToRange(worksheet, todo.Row, g_body.Column, headerFields.Select(x => x.name).ToList(), qrs, g_sfd, skipColumn);
                outrow += qrs.Length;
                percent = (int)Math.Round(outrow / (double)totals * 100d);
                if (percent > 100)
                    percent = 100;
                bgw.ReportProgress(percent, "Write record (" + outrow.ToString("N0") + " / " + totals.ToString("N0") + ")");
                return true;
                //foreach (Excel.Range rw in todo.Rows)
                //{
                //    var key = Operation.objectid(ref excelApp, ref worksheet, ref g_ids, rw.Row, true);
                //    if (sd.ContainsKey(key))
                //    {
                //        var so = sd[key];
                //        formatWriteRow(ref worksheet, ref g_header, ref g_body, ref g_sfd, so as IDictionary, Conversions.ToInteger(Operators.SubtractObject(rw.Row, 2)), headerFields, fieldLabelMap, fieldMap, false);
                //        outrow = outrow + 1L;
                //        percent = (int)Math.Round(outrow / (double)totals * 100d);
                //        if (percent > 100)
                //            percent = 100;
                //        bgw.ReportProgress(percent, "Write record (" + outrow.ToString("N0") + " / " + totals.ToString("N0") + ")");
                //    }
                //    else
                //    {
                //        Excel.Range badRange = worksheet.Cells[rw.Row, g_ids.Column];
                //        badRange.Font.ColorIndex = 7;
                //    }
                //}

                //sd = null;
                //qrs = null;
                //return true;
            }
            catch (Exception ex)
            {
                throw new Exception("querySelectedRow Exception" + Constants.vbCrLf + ex.Message);
            }

            return false;
        }

        // ******************************************************************************
        // * Delete Selected Rows Part
        // ******************************************************************************
        public static object deleteSelectedRange(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range g_ids, ref string g_objectType, ref Excel.Range todo)
        {
            try
            {
                string[] idlist;
                var drMap = new Dictionary<string, RESTful.DeleteResult>();
                idlist = new string[todo.Rows.Count];
                int i = 0;
                Excel.Range rw;
                foreach (Excel.Range currentRw in todo.Rows)
                {
                    rw = currentRw;
                    idlist[i] = objectid(ref excelApp, ref worksheet, ref g_ids, rw.Row, true);
                    i = i + 1;
                }

                var drs = RESTAPI.DeleteRecords(g_objectType, idlist);
                foreach (var dr in drs)
                {
                    if (dr.id is object)
                    {
                        drMap.Add(dr.id, dr);
                    }
                }

                // TODO when one fails we don't need to skip all of the rest...
                // do it like the create calls?

                foreach (Excel.Range currentRw1 in todo.Rows)
                {
                    rw = currentRw1; // draw results
                    if (drMap.ContainsKey(Conversions.ToString(rw.Cells[1, 1].value)))
                    {
                        excelApp.Intersect(g_ids, (Excel.Range)worksheet.Rows[rw.Row]).Value = "deleted";
                    }
                    else
                    {
                        rw.Cells[1, 1].AddComment();
                        string errMsg = "Delete Row Failed";
                        rw.Cells[1, 1].Comment.Text(errMsg);
                        rw.Cells[1, 1].Comment.Shape.Height = 60;
                    } // is this enough
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("deleteSelectedRange Exception" + Constants.vbCrLf + ex.Message);
            }

            return false;
        }

        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ' Common Functions Block
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        /// <summary>
        /// set global ranges, labels for the current active region
        /// used by most other calls which operate on a Range of data
        /// except sfDescribe which creates a default table layout
        /// </summary>
        /// <returns>Boolean</returns>
        public static bool setDataRanges(
            ref Excel.Application excelApp,
            ref Excel.Worksheet worksheet,
            ref Excel.Range g_table,
            ref Excel.Range g_start,
            ref Excel.Range g_header,
            ref Excel.Range g_body,
            ref string g_objectType,
            ref Excel.Range g_ids,
            ref RESTful.DescribeSObjectResult g_sfd,
            ref string statusText,
            out List<RESTful.Field> headerFields,
            out Dictionary<string, RESTful.Field> fieldLabelMap,
            out Dictionary<string, RESTful.Field> fieldMap
            )
        {
            try
            {
                excelApp.StatusBar = "build data ranges...";
                try
                {
                    g_table = excelApp.ActiveCell.CurrentRegion;
                }
                catch (Exception ex)
                {
                    statusText = "Oops, Could not find an active Worksheet: " + ex.Message;
                    headerFields = null;
                    fieldLabelMap = null;
                    fieldMap = null;
                    return false;
                }

                g_start = g_table.Cells[1, 1];
                // see how many rows we have before setting body... 5.20
                if (g_table.Rows.Count == 2)
                {

                    // body is going to be outside the table... so place it where we need
                    g_body = g_table.Cells[3, 1]; // 5.20
                }
                else
                {
                    g_body = worksheet.Range[g_table.Cells[3, 1], g_table.Cells[g_table.Rows.Count, g_table.Columns.Count]];
                    //g_body = worksheet.get_Range(g_table.Cells[3, 1], g_table.Cells[g_table.Rows.Count, g_table.Columns.Count]);
                }



                if (g_start.Comment != null)
                {
                    g_objectType = g_start.Comment.Text();

                }
                if ((string.IsNullOrEmpty(g_objectType) || g_objectType.Contains(" ")) && g_start.Value != null)
                {
                    g_objectType = g_start.Value;
                }
                if (string.IsNullOrEmpty(g_objectType) || g_objectType.Contains(" "))
                {
                    statusText = "could not locate a object name in cell " + g_start.get_Address(1) + Constants.vbCrLf + "use Describe Sforce Object menu item to select a valid object";
                    headerFields = null;
                    fieldLabelMap = null;
                    fieldMap = null;

                    return false;

                }
                g_sfd = RESTAPI.DescribeSObject(g_objectType);

                var fieldByLabels = Util.getFieldLabelMap(g_sfd.fields);
                var fields = Util.getFieldMap(g_sfd.fields);
                fieldMap = fields;
                fieldLabelMap = fieldByLabels;


                object[,] headerValues = g_table.Rows[2].Value;

                // Try to map header names to a field without reading the notes
                int maxHeader = 1;
                int idField = default;
                headerFields = new List<RESTful.Field>();

                for (int kk = 1; kk <= headerValues.Length; kk++)
                {
                    int idx = kk - 1;
                    if (headerValues[1, kk] != null)
                    {
                        var headerValue = Convert.ToString(headerValues[1, kk]);
                        if (fieldByLabels.ContainsKey(headerValue))
                        {
                            var fld = fieldByLabels[headerValue];
                            if (string.Compare(fld.name, "id", true) == 0)
                            {
                                idField = kk;
                            }
                            headerFields.Add(fld);
                            maxHeader = kk;
                        }
                        else
                        {
                            // Can't find it, read the note
                            Excel.Range cell = g_table[2, kk];
                            if (cell.Comment != null)
                            {

                                string commentText = cell.Comment.Text();
                                if (commentText != null && commentText.StartsWith("API Name"))
                                {
                                    var fld = Util.getAPINameFromCell(cell);
                                    if (fields.ContainsKey(fld))
                                    {
                                        headerFields.Add(fields[fld]);
                                        if (string.Compare(fld, "id", true) == 0)
                                        {
                                            idField = kk;
                                        }

                                        maxHeader = kk;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                // trim the g_header Range down if g_table.columns.count is greater than
                // the number of non blank cells in row 2 !!
                g_header = worksheet.Range[g_table.Cells[2, 1], g_table.Cells[2, maxHeader]];

                excelApp.StatusBar = "Query " + g_objectType + " table description";

                int gcol = idField;
                if (gcol == default)
                {
                    return false;
                }
                g_ids = excelApp.Intersect(g_body, (Excel.Range)g_body.Columns[gcol]);
                return true;

            }
            catch (Exception ex)
            {
                statusText = "set_Range Exception" + Constants.vbCrLf + ex.Message;
                headerFields = null;
                fieldLabelMap = null;
                fieldMap = null;
                return false;
            }
        }

        public static int getObjectIdColumn(ref Excel.Range g_header, ref string statusText) // have a map of labels and one is the id
        {
            int j;
            var loopTo = g_header.Count;
            for (j = 1; j <= loopTo; j++)
            {
                {
                    var withBlock = g_header.Cells[1, j];
                    string apiname = Util.getAPIName(Conversions.ToString(withBlock.Comment.Text));
                    if (apiname.ToLower() == "id")
                    {
                        return j;
                    }
                }
            }

            statusText = "no Object Id found in the column header row";
            return default;
        }

        public static string objectid(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range g_ids, object row, bool quiet = true)
        {
            try
            {
                var t = excelApp.Intersect(g_ids, (Excel.Range)worksheet.Rows[row]);
                var tempId = Conversions.ToString(t.get_Value());
                if (Strings.Len(tempId) == 15)
                {
                    tempId = Util.FixID(tempId);
                }
                else if (Strings.LCase(tempId) == "new")
                {

                }
                else if (Strings.Len(tempId) < 15)
                {
                    // Debug.Print tempId
                    if (quiet)
                        Interaction.MsgBox("unrecognized object id >" + tempId + "<");
                }
                // Debug.Print "object id is " && tempId
                return tempId;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string[] objectids(ref Excel.Application excelApp, ref Excel.Worksheet worksheet, ref Excel.Range g_ids, ref Excel.Range todo)
        {
            try
            {
                var t = excelApp.Intersect(g_ids, todo.EntireRow);
                if (t == null)
                {
                    return Array.Empty<string>();
                }
                var ids = t.Value;
                if (ids is string)
                {
                    return new string[] { ids };
                }
                if (ids is object[,] idarr)
                {
                    List<string> allIds = new List<string>();
                    for (int i = 1; i <= idarr.Length; i++)
                    {
                        allIds.Add(Conversions.ToString(idarr[i, 1]));
                    }
                    return allIds.ToArray();
                }
                else
                {
                    return Array.Empty<string>();
                }
            }
            catch (Exception)
            {
                return Array.Empty<string>();
            }
        }
        public static void ApplyDataToRange(Excel.Worksheet worksheet, long startRow, int startColumn, List<string> fieldOrder, Dictionary<string, object>[] objects, RESTful.DescribeSObjectResult g_sfd, int skipColumn = -1)
        {
            if (objects.Length <= 0)
            {
                return;
            }
            if (skipColumn != -1)
            {
                if (skipColumn != startColumn)
                {
                    throw new ArgumentException("Id column must be first column in range to update rows");
                }
                startColumn++;
                fieldOrder = fieldOrder.Skip(1).ToList();

            }
            var fields = Util.getFieldMap(g_sfd.fields);
            long maxRow = objects.Length + startRow - 1;
            if (maxRow > ForceConnector.excelLimit)
            {
                throw new ArgumentException($"Too many rows: {maxRow}");
            }
            int columns = fieldOrder.Count;
            int maxCol = startColumn + columns - 1;
            if (maxCol > ForceConnector.excelColLimit)
            {
                throw new ArgumentException($"Too many columns: {maxCol}");
            }

            // Make a big 2D array to hold the data and splat into Excel in one hit

            object[,] data = new object[objects.Length, fieldOrder.Count];


            for (int i = 0; i < objects.Length; i++)
            {
                var sob = objects[i];

                for (int j = 0; j < fieldOrder.Count; j++)
                {

                    if (sob.TryGetValue(fieldOrder[j], out var obVal))
                    {
                        if (obVal is Dictionary<string, object> complex)
                        {
                            data[i, j] = string.Join(Environment.NewLine, complex.Select(x => $"{x.Key}: {x.Value}"));
                        }
                        else
                        {
                            data[i, j] = Convert.ToString(obVal);
                        }
                    }
                }
            }
            Excel.Range rng = worksheet.Range[worksheet.Cells[startRow, startColumn], worksheet.Cells[maxRow, maxCol]];
            rng.Value = data;

        }

        public static void formatWriteRow(
            ref Excel.Worksheet worksheet,
            ref Excel.Range g_header,
            ref Excel.Range g_body,
            ref RESTful.DescribeSObjectResult g_sfd,
            IDictionary so,
            int row,
            List<RESTful.Field> headerFields,
            Dictionary<string, RESTful.Field> fieldLabelMap,
            Dictionary<string, RESTful.Field> fieldMap,
            bool isInsert = true
        )
        {
            var fields = Util.getFieldMap(g_sfd.fields);
            object maxRowHght;
            maxRowHght = worksheet.StandardHeight * 3d;
            for (int j = 1, loopTo = g_header.Count; j <= loopTo; j++)
            {
                var field = headerFields[j - 1];
                string name = field.name;
                string fmt;
                int rheight;


                // for query selected row, skip writing the Id column
                if (!isInsert && field.type == "id")
                    goto nextcol;
                fmt = Util.typeToFormat(field.type);
                rheight = Conversions.ToInteger(g_body.Cells[row, j].RowHeight);  // before height

                // map owner id to names (5.29)
                // only do this if the option flag is set... or should it be default
                // if querybool(SPELL_USERNAME) then ...
                // 
                if (field.type == "reference")
                {
                    g_body.Cells[row, j].value = Util.IdToName(Conversions.ToString(so[name]));
                }
                else
                {
                    // need to preserve text fields as text in excel or we may
                    // lose any leading zeros... !!!
                    // therefore we need to respect the field type here
                    // gotcha: the format must be set both before and after as the value
                    // assignment appears to trump some formats
                    g_body.Cells[row, j].NumberFormat = fmt;
                    // 6.02 by MO'L
                    // Check type. Do not trim if it's a date or datetime as this will convert the date to text and lose international formatting: MO'L
                    switch (field.type ?? "")
                    {
                        case "date":
                        case "datetime":
                            {
                                g_body.Cells[row, j].value = so[name];
                                break;
                            }

                        case "address":
                            {
                                if (so[name] is Dictionary<string, object> addr)
                                {
                                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(addr.Count, 0, false)))
                                        break;
                                    string full_address = "";
                                    full_address = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(addr["street"], ", "), addr["city"]), ", "), addr["state"]), " "), addr["postalCode"]), ", "), addr["country"]));
                                    g_body.Cells[row, j].value = full_address;
                                }

                                break;
                            }

                        case "location":
                            {
                                if (so[name] is Dictionary<string, object> loc)
                                {
                                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(loc.Count, 0, false)))
                                        break;
                                    string location = "";
                                    location = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(loc["latitude"], ", "), loc["longitude"]));
                                    g_body.Cells[row, j].value = location;
                                }

                                break;
                            }

                        default:
                            {
                                g_body.Cells[row, j].value = Strings.Left(Conversions.ToString(so[name]), 32767);
                                break;
                            }
                    }

                    g_body.Cells[row, j].NumberFormat = fmt;  // some formats like to be applied after
                    if (so[name] is string)
                    {
                        if (Util.IsHyperlink(field, Conversions.ToString(so[name])))
                            Util.AddHyperlink(g_body.Cells[row, j], so[name]); // 6.09
                    }

                    // do something about the auto resizing, just to try to avoid blowup in long text
                    // fields as they are loaded into the cells, but dont mess with it if the user
                    // has set a height first
                    if (Conversions.ToBoolean(Operators.AndObject(Operators.ConditionalCompareObjectGreater(g_body.Cells[row, j].RowHeight, maxRowHght, false), Operators.ConditionalCompareObjectGreater(g_body.Cells[row, j].RowHeight, rheight + 1, false))))
                    {
                        g_body.Cells[row, j].RowHeight = maxRowHght; // set some default max
                    }
                }

            nextcol:
                ;
            }
        }
    }
}