using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processDatabaseUpdateRowsNew
    {
        public processDatabaseUpdateRowsNew()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        private string statusText = "";
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private string g_objectType;                  // current entity table, ie "Account"
        private RESTful.DescribeSObjectResult g_sfd;  // global describe for current table, not really needed since toolkit caches
        private Excel.Range g_table;                  // all current region, with table name and header row
        private Excel.Range g_header;                 // row with column labels
        private Excel.Range g_body;                   // area with data, just below the header row
        private Excel.Range g_ids;                    // column with salesforce ID's
        private Excel.Range g_start;                  // some globals to hold info about the region we are working on

        private void processDatabaseUpdateRowsNew_Load(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Regular);
                lblMessage.ForeColor = System.Drawing.Color.Black;
                statusText = "";

                // ' these properties should be set to True (at design-time or runtime) before calling the RunWorkerAsync
                // ' to ensure that it supports Cancellation and reporting Progress
                bgw.WorkerSupportsCancellation = true;
                bgw.WorkerReportsProgress = true;

                // ' call this method to start your asynchronous Task.
                bgw.RunWorkerAsync();
                goto done;
            }
            catch (Exception ex)
            {
                statusText = ex.Message;
            }

        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                progressDownload.Value = 100;
                lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Bold);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = statusText;
                btnAction.Text = "Done";
                btnAction.Enabled = true;
            }

        done:
            ;
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            // ' to cancel the task, just call the BackgroundWorker.CancelAsync method.
            if (btnAction.Text == "Cancel")
            {
                bgw.CancelAsync();
            }
            else if (btnAction.Text == "Done")
            {
                Close();
            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            var totalRow = default(long);
            var totalCol = default(long);
            var updatedRow = default(long);
            try
            {
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.StatusBar = "Update Selected Rows...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(0, "Please wait for initialize...");
                if (!Util.checkSession())
                {
                    statusText = "Session Failed!";
                    goto errors;
                }

                if (!Operation.setDataRanges(ref excelApp, ref worksheet, ref g_table, ref g_start, ref g_header, ref g_body, ref g_objectType, ref g_ids, ref g_sfd, ref statusText, out var headerFields, out _, out _))
                {
                    goto errors;
                }

                var fieldMap = Util.getFieldMap(g_sfd.fields);
                var recordSet = new Dictionary<string, object>();
                var records = new List<Dictionary<string, object>>();
                Excel.Range xlSelection;
                string[] strArryCells;
                string strAddrStart = "";
                string strAddrEnd = "";
                string strStatBarText = "";
                string strMsg;
                var intBatchPointer = default(long);
                var intJobPointer = default(long);
                var intFailedRows = default(long);
                int i;
                bool blnSkipHidden;
                bool blnGoAhead;
                bool blnNoLimits;

                // // check global options
                blnGoAhead = RegDB.RegQueryBoolValue(ForceConnector.GOAHEAD);
                blnNoLimits = RegDB.RegQueryBoolValue(ForceConnector.NOLIMITS);
                // blnSkipHidden = RegQueryBoolValue(SKIPHIDDEN)   '** new option setting?
                blnSkipHidden = true;

                // // check the selected range
                excelApp.Intersect((Excel.Range)excelApp.Selection, g_body).Select();
                xlSelection = (Excel.Range)excelApp.Selection;
                if (xlSelection is null)
                {
                    statusText = "Nothing was selected";
                    goto errors;
                }

                if (xlSelection.Areas.Count > 1)
                {
                    statusText = "You can't process multiple areas.";
                    goto errors;
                }

                Operation.calcUpdateRange(ref xlSelection, ref totalRow, ref totalCol, blnSkipHidden, blnNoLimits);

                // // have we set the "Skip warning dialogs" option?
                if (!blnGoAhead)
                {
                    strMsg = "You are about to update " + totalRow.ToString("N0") + " row(s) with " + totalCol.ToString("N0") + " column(s)." + Constants.vbCrLf + Constants.vbCrLf + "Do you want to proceed?";
                    var result = TopMostMessageBox.Show("Update Information", strMsg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        statusText = "Cancel Update";
                        goto cancel;
                    }
                }

                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);

                // // dimension the id and value arrays
                if (totalRow < ForceConnector.maxBatchSize)
                {
                    strArryCells = new string[(int)(totalRow - 1L + 1)];
                }
                else
                {
                    strArryCells = new string[50];
                }

                foreach (Excel.Range xlRow in xlSelection.Rows)
                {
                    // initialize dictionary
                    recordSet = new Dictionary<string, object>();
                    if (intBatchPointer == 0L)
                    {
                        // // recreate for each new batch
                        long workedRow = Conversions.ToLong(Interaction.IIf(totalRow - intJobPointer > ForceConnector.maxBatchSize, intJobPointer + ForceConnector.maxBatchSize, totalRow));
                        strStatBarText = intJobPointer + 1L + " -> " + workedRow.ToString("N0") + " of " + totalRow.ToString("N0");
                        int percent = (int)Math.Round(workedRow / (double)totalRow * 100d);
                        bgw.ReportProgress(percent, "Updating: " + strStatBarText);
                    }

                    if (Conversions.ToBoolean(!Operators.AndObject(xlRow.Hidden, blnSkipHidden)))
                    {
                        // // fill elements of the batch; ignore hidden fields if required
                        recordSet.Add("attributes", new RESTful.Attributes(g_objectType));
                        recordSet.Add("Id", Util.FixID(Conversions.ToString(excelApp.Intersect(g_ids, xlRow.EntireRow).get_Value())));
                        i = 0;
                        foreach (Excel.Range xlColumn in xlRow.Columns)
                        {
                            if (Conversions.ToBoolean(!Operators.AndObject(xlColumn.Hidden, blnSkipHidden)))
                            {
                                string fld = Util.getAPINameFromCell(excelApp.Intersect(g_header, xlColumn.EntireColumn));
                                var field = fieldMap[fld];
                                if (!field.updateable)
                                    goto nextrow;
                                if (Strings.Len(strAddrStart) == 0)
                                    strAddrStart = xlColumn.get_Address(1);
                                strAddrEnd = xlColumn.get_Address(1);
                                xlColumn.Interior.ColorIndex = (object)36;
                                recordSet.Add(fld, Util.toVBtype(xlColumn, field));
                            nextrow:
                                ;
                                i = i + 1;
                            }
                        }

                        // if recordSet does not contains any updatable columns, cancel update
                        if (records.Count == 0 & recordSet.Count == 2)
                        {
                            statusText = "No updatable columns selected, operation canceled.";
                            goto errors;
                        }

                        records.Add(recordSet);
                        strArryCells[(int)intBatchPointer] = strAddrStart + ":" + strAddrEnd;
                        strAddrStart = "";
                        intBatchPointer = intBatchPointer + 1L;
                        intJobPointer = intJobPointer + 1L;
                        updatedRow = updatedRow + 1L;
                    }

                    if (intBatchPointer == ForceConnector.maxBatchSize | intJobPointer == totalRow)
                    {
                        Operation.updateResultHandlerNew(ref worksheet, ref intFailedRows, records, strArryCells);

                        // //reinitialize for next batch
                        if (totalRow - intJobPointer < ForceConnector.maxBatchSize & totalRow - intJobPointer != 0L) // 6.13
                        {
                            strArryCells = new string[(int)(totalRow - intJobPointer - 1L + 1)];
                        }

                        strAddrStart = "";
                        strAddrEnd = "";
                        intBatchPointer = 0L;
                        records = new List<Dictionary<string, object>>();
                    }
                }

                // ' any cleanup code go here
                // ' ensure that you close all open resources before exitting out of this Method.
                // ' try to skip off whatever is not desperately necessary if CancellationPending is True
                bgw.ReportProgress(100, "Complete.");
                // ' set the e.Cancel to True to indicate to the RunWorkerCompleted that you cancelled out
                if (bgw.CancellationPending)
                {
                    e.Cancel = true;
                    bgw.ReportProgress(100, "Cancelled.");
                }

                if (intFailedRows > 0L)
                {
                    strMsg = intFailedRows.ToString("N0") + " of " + totalRow.ToString("N0") + " rows failed to update!" + Constants.vbCrLf + "Please check the comments" + " in the first cell of each highlighted row for details";
                    TopMostMessageBox.Show("Warning", strMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                goto done;
            }
            catch (Exception ex)
            {
                statusText = ex.Message;
                goto errors;
            }

        cancel:
            ;
            statusText = "Update Canceled!";
            e.Cancel = true;
            goto done;
        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                Util.ErrorBox(statusText);
                statusText = "UpdateData Error!";
                e.Cancel = true;
            }

        done:
            ;
            excelApp.StatusBar = "Update : update complete, " + updatedRow + " total rows returned";
            excelApp.ScreenUpdating = true;
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // ' This event is fired when you call the ReportProgress method from inside your DoWork.
            // ' Any visual indicators about the progress should go here.
            progressDownload.Value = e.ProgressPercentage;
            lblMessage.Text = Conversions.ToString(e.UserState);
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // ' This event is fired when your BackgroundWorker exits.
            // ' It may have exitted Normally after completing its task, 
            // ' or because of Cancellation, or due to any Error.

            if (e.Error is object)
            {
                // ' if BackgroundWorker terminated due to error
                // MessageBox.Show(e.Error.Message)
                lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Bold);
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Error occurred!" + e.Error.Message;
            }
            else if (e.Cancelled)
            {
                // ' otherwise if it was cancelled
                // MessageBox.Show("Download cancelled!")
                if (!string.IsNullOrEmpty(statusText))
                {
                    lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Bold);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = statusText;
                }
                else
                {
                    lblMessage.Text = "Update Cancelled!";
                }
            }
            else
            {
                // ' otherwise it completed normally
                // MessageBox.Show("Download completed!")
                lblMessage.Text = "Update completed!";
            }

            btnAction.Text = "Done";
            btnAction.Enabled = true;
        }

        // ******************************************************
        // * Change controls label in the bgw.DoWork()
        // * ctl -> label, button ...etc
        // ******************************************************
        private void setControlText(Control ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(new setControlTextInvoker(setControlText), ctl, text);
            }
            else
            {
                ctl.Text = text;
            }
        }

        private delegate void setControlTextInvoker(Control ctl, string text);

        private void setControlStatus(Control ctl, bool enabled)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(new setControlStatusInvoker(setControlStatus), ctl, enabled);
            }
            else
            {
                ctl.Enabled = enabled;
            }
        }

        private delegate void setControlStatusInvoker(Control ctl, bool enabled);
    }
}