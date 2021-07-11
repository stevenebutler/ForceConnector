using System;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections;
using System.Collections.Generic;

namespace ForceConnector
{
    public partial class processDatabaseQueryTable
    {
        public processDatabaseQueryTable()
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

        private void processDatabaseQueryTable_Load(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                statusText = ex.Message;
                if (!string.IsNullOrEmpty(statusText))
                {
                    progressDownload.Value = 100;
                    lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Bold);
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = statusText;
                    btnAction.Text = "Done";
                    btnAction.Enabled = true;
                }
            }
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
            // ' The asynchronous task you want to perform goes here
            // ' the following is an example of how it typically goes.
            long outrow = 0L;
            try
            {
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.StatusBar = "Query Database Table...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(0, "Please wait for initialize...");
                if (!Util.checkSession())
                {
                    statusText = "Session Failed!";
                    goto errors;
                }

                if (!Operation.setDataRanges(ref excelApp, ref worksheet, ref g_table, ref g_start, ref g_header, ref g_body, ref g_objectType, ref g_ids, ref g_sfd, ref statusText, out var headerFields, out var fieldLabelMap, out var fieldMap))
                {
                    goto errors;
                }

                string where = "";
                List<string> sels = null;
                Excel.Range refIds = null;
                string joinfield = "";
                var oneeachrow = default(bool);

                bgw.ReportProgress(0, "Build Query String...");
                if (!Operation.BuildQueryString(ref excelApp, ref g_table, ref g_start, ref g_header, ref refIds, ref joinfield, ref oneeachrow, fieldLabelMap, fieldMap, headerFields, out sels, ref where, ref statusText))
                {
                    goto errors;
                }
                
                // Debug.Print "select " && sels && " from " && g_objectType
                // Debug.Print " " && where

                // to support join, if we saw a "reference in range" we need to loop over this,
                // otherwise call once for a normal query
                outrow = 1L; // the row within g_body where output begins
                if (refIds is object)
                {
                    // TODO this could be optimized to pull multiple ref's at one query call
                    // should speed things up on large joins there is lots of overhead now

                    string tmp;
                    foreach (Excel.Range c in refIds.Cells) // loop over a range to output a join
                    {
                        tmp = where;
                        if (!string.IsNullOrEmpty(where) && Strings.Right(where, 4) != "AND ")
                            tmp = where + " AND "; // 5.56
                        where = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(tmp + joinfield + " = '", c.get_Value()), "'")); // use the ID from the reference colum in each query
                    }
                }
                if (string.IsNullOrWhiteSpace(where))
                {
                    where = string.Empty;
                }
                else
                {
                    where = $" WHERE {where}";
                }


                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);

                if (Operation.RequireConfirmation)
                {

                    RESTful.QueryResult qr;
                    qr = RESTAPI.Query($"SELECT COUNT(Id) ROWCOUNT FROM {g_objectType}{where}");
                    var total = qr.records[0] as IDictionary;
                    long rowCount = Conversions.ToLong(total["ROWCOUNT"]);
                    if (rowCount > ForceConnector.excelLimit)
                    {
                        statusText = "Number of records exceed the limit of Excel, cancel the download.";
                        goto errors;
                    }

                    string msg = "You try to download " + rowCount.ToString("N0") + " records. Are you sure?";
                    var result = TopMostMessageBox.Show("Query Information", msg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        statusText = "Cancel Query";
                        goto cancel;
                    }
                }
                bgw.ReportProgress(0, "Preparing to query...");
                var argbgw = bgw;
                outrow = Operation.queryDataDraw(ref excelApp, ref worksheet, ref g_header, ref g_body, ref g_ids, ref g_objectType, ref g_sfd, sels, where, outrow, ref argbgw);
                bgw = argbgw;
                if (outrow <= 1L)
                    setControlText(lblMessage, "No data returned for this Query");
                refIds = null;

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

                goto done;
            }
            catch (Exception ex)
            {
                statusText = ex.Message;
                goto errors;
            }

        cancel:
            ;
            statusText = "Query Canceled!";
            e.Cancel = true;
            goto done;
        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                Util.ErrorBox(statusText);
                statusText = "QueryData Error!";
                e.Cancel = true;
            }

        done:
            excelApp.StatusBar = $"Query : drawing complete, {outrow} total rows returned";
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
                    lblMessage.Text = "Download Cancelled!";
                }
            }
            else
            {
                // ' otherwise it completed normally
                // MessageBox.Show("Download completed!")
                lblMessage.Text = "Download completed!";
            }

            btnAction.Text = "Done";
            btnAction.Enabled = true;
            if (!Operation.RequireConfirmation)
            {
                btnAction.PerformClick();
            }
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