using System;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processDatabaseUpdateRows
    {
        public processDatabaseUpdateRows()
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

        private void processDatabaseUpdateRows_Load(object sender, EventArgs e)
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
            long totals = 0L;
            long row_counter = 0L;
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

                bool localUpdateLimitCheck() { Excel.Range args = (Excel.Range)excelApp.Selection; var ret = Operation.UpdateLimitCheck(ref args, ref statusText); args.Select(); return ret; }

                if (!localUpdateLimitCheck())
                    goto errors;
                if (Conversions.ToBoolean(Operators.ConditionalCompareObjectGreater(excelApp.Selection.Rows.Count, ForceConnector.maxRows, false)))
                {
                    statusText = "Exceed the limit of update, cancel the update.";
                    goto errors;
                }

                totals = Conversions.ToLong(excelApp.Selection.Rows.Count);
                row_counter = 0L;
                string msg = "You try to update " + totals.ToString("N0") + " records. Are you sure?";
                var result = TopMostMessageBox.Show("Update Information", msg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    statusText = "Cancel Update";
                    goto cancel;
                }

                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);

                // batch up the data by walking the thru the selection range
                // breaking it up into chunk size Ranges to be uploaded
                long row_pointer = Conversions.ToLong(excelApp.Selection.row); // row where we start to chunk
                bool someFailed = false;
                Excel.Range chunk;
                bgw.ReportProgress(0, "Start updating... (" + totals.ToString("N0") + " record(s))");
                do // build a chunk Range which covers the cells we can update in a batch
                {
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, (Excel.Range)worksheet.Rows[(object)row_pointer]); // first row
                    if (chunk is null)
                        break;                      // done here
                    chunk = chunk.get_Resize(ForceConnector.maxBatchSize);                    // extend the chunk to cover our batchsize
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, chunk); // trim the last chunk
                    row_pointer = row_pointer + ForceConnector.maxBatchSize;              // up our row counter
                    chunk.Interior.ColorIndex = (object)36;
                    var argbgw = bgw;
                    Operation.updateRange(ref excelApp, ref g_header, ref g_objectType, ref g_start, ref g_sfd, ref g_ids, ref chunk, ref someFailed, ref row_counter, ref totals, ref argbgw);
                    bgw = argbgw;
                    Excel.Range sav;
                    sav = (Excel.Range)excelApp.Selection;
                    sav.Select();             // and restore, allows a control-break in here

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        bgw.ReportProgress((int)Math.Round(row_counter / (double)totals * 100d), "Cancelling...");
                        break;
                    }
                }
                while (!(chunk is null));
                if (someFailed)
                {
                    Util.ErrorBox("One or more of the selected rows could not be updated" + '\n' + "see the comments in the colored cells for details");
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
            excelApp.StatusBar = "Update : update complete, " + (row_counter - 1L) + " total rows returned";
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