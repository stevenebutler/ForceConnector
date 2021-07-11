﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;

namespace ForceConnector
{
    public partial class processDatabaseDeleteRows
    {
        public processDatabaseDeleteRows()
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

        private void processDatabaseDeleteRows_Load(object sender, EventArgs e)
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
            // ' The asynchronous task you want to perform goes here
            // ' the following is an example of how it typically goes.
            long totals = 0L;
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
                

                if (!Operation.setDataRanges(ref excelApp, ref worksheet, ref g_table, ref g_start, ref g_header, ref g_body, ref g_objectType, ref g_ids, ref g_sfd, ref statusText, out List<RESTful.Field> fieldList, out _, out _))
                {
                    goto errors;
                }

                bgw.ReportProgress(0, "Build Query String...");
                totals = Conversions.ToLong(excelApp.Selection.Rows.Count);
                if (!RegDB.RegQueryBoolValue(ForceConnector.NOLIMITS) && totals > ForceConnector.maxRows)
                {
                    statusText = "too many rows selected " + totals.ToString("N0") + ", max is " + ForceConnector.maxRows.ToString("N0");
                    goto errors;
                }

                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);
                string msg = "You try to download " + totals.ToString("N0") + " records. Are you sure?";
                var result = TopMostMessageBox.Show("Query Information", msg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    statusText = "Cancel Query";
                    goto cancel;
                }

                int row_pointer = Conversions.ToInteger(excelApp.Selection.row); // row where we start to chunk
                Excel.Range chunk;
                do // build a chunk Range which covers the cells we can update in a batch
                {
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, (Excel.Range)excelApp.ActiveSheet.Rows(row_pointer));
                    if (chunk is null)
                        break;
                    chunk = chunk.get_Resize(ForceConnector.maxBatchSize); // extend the chunk to cover our batchsize
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, chunk); // trim the last chunk !
                    row_pointer = row_pointer + ForceConnector.maxBatchSize; // up our pos counter
                    int percent = (int)Math.Round(outrow / (double)totals * 100d);
                    if (percent > 100)
                        percent = 100;
                    bgw.ReportProgress(percent, "Delete " + chunk.Count.ToString() + " records from row " + outrow.ToString("N0"));
                    chunk.Interior.ColorIndex = (object)36; // show off...
                    if (!Conversions.ToBoolean(Operation.deleteSelectedRange(ref excelApp, ref worksheet, ref g_ids, ref g_objectType, ref chunk)))
                        goto done; // do it
                    chunk.Interior.ColorIndex = (object)0;

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        // bgw.ReportProgress(percent, "Cancelling...")
                        bgw.ReportProgress((int)Math.Round(outrow / (double)totals * 100d), "Cancelling...");
                        break;
                    }

                    outrow = outrow + chunk.Count;
                }
                while (!(chunk is null));

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
            statusText = "Delete Canceled!";
            e.Cancel = true;
            goto done;
        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                Util.ErrorBox(statusText);
                statusText = "DeleteData Error!";
                e.Cancel = true;
            }

        done:
            ;
            excelApp.StatusBar = "Delete complete, " + (outrow - 1L) + " total rows deleted";
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
                    lblMessage.Text = "Delete Cancelled!";
                }
            }
            else
            {
                // ' otherwise it completed normally
                // MessageBox.Show("Download completed!")
                lblMessage.Text = "Delete completed!";
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