using System;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processCustomLabelUpload
    {
        public processCustomLabelUpload()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        private long numOfCustomLabel;
        private bool someFailed = false;
        private string statusText = "";
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private Excel.Range start;
        private Excel.Range m_table;
        private Excel.Range m_start;
        private Excel.Range m_head;
        private Excel.Range m_body;
        private int m_rows; // row pointer to append new value at bottom of list
        private string m_metaType;

        private void processCustomLabelUpload_Load(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Font = new System.Drawing.Font(lblMessage.Font, System.Drawing.FontStyle.Regular);
                lblMessage.ForeColor = System.Drawing.Color.Black;
                statusText = "";
                if (!METAAPI.setMetaBinding())
                {
                    statusText = "Session Failed";
                    goto errors;
                }

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

            try
            {
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                excelApp.StatusBar = "Upload CustomLabel metadata...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(0, "Please wait for initialize...");

                // ' Call initialize process...
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                if (worksheet.Name == "CustomLabel")
                {
                    METAAPI.setWorkArea(ref excelApp, ref worksheet, ref m_table, ref m_head, ref m_body, ref m_start, ref m_rows, ref m_metaType);
                    if (!RegDB.RegQueryBoolValue(ForceConnector.GOAHEAD))
                    {
                        string msg = "You are about to UPLOAD: " + Conversions.ToString(excelApp.Selection.Rows.Count) + " CustomLabel " + " metadata(s) in to Salesforce.com" + Constants.vbCrLf;
                        if (Interaction.MsgBox(msg, (MsgBoxStyle)((int)Constants.vbApplicationModal + (int)Constants.vbOKCancel + (int)Constants.vbExclamation + (int)Constants.vbDefaultButton1), "-- Ready to Upload CustomLabel --") == Constants.vbCancel)
                        {
                            statusText = "Upload canceled!";
                            goto errors;
                        }
                    }

                    setControlText(btnAction, "Cancel");
                    setControlStatus(btnAction, true);
                }
                else
                {
                    statusText = "Current worksheet is not CustomLabel!";
                    goto errors;
                }

                Excel.Range sav = (Excel.Range)excelApp.Selection;
                long row_pointer = sav.Row; // row where we start to chunk
                long row_counter = 0L;
                Excel.Range chunk;
                numOfCustomLabel = sav.Count;
                do // build a chunk Range which covers the cells we can create in a batch
                {
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, (Excel.Range)worksheet.Rows[(object)row_pointer]); // first row
                    if (chunk is null)
                        break;          // done here
                    chunk = chunk.get_Resize(10); // extend the chunk to cover our batchsize
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, chunk); // trim the last chunk
                    row_pointer = row_pointer + chunk.Count;
                    row_counter = row_counter + chunk.Count;

                    // chunk.Interior.ColorIndex = 36
                    METAAPI.uploadCustomLabel(ref excelApp, ref chunk, ref someFailed); // doit
                                                                                        // chunk.Interior.ColorIndex = 0

                    sav.Select(); // and restore

                    // change process bar
                    int percent = (int)Math.Round(100L * row_counter / (double)numOfCustomLabel);
                    bgw.ReportProgress(percent, "Query CustomLabel... " + row_counter.ToString() + " / " + numOfCustomLabel.ToString());

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        bgw.ReportProgress(percent, "Cancelling...");
                        break;
                    }
                }
                while (!(chunk is null));

                // ' any cleanup code go here
                // ' ensure that you close all open resources before exitting out of this Method.
                // ' try to skip off whatever is not desperately necessary if CancellationPending is True

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

        errors:
            ;
            if (!string.IsNullOrEmpty(statusText))
            {
                // bgw.CancelAsync()
                e.Cancel = true;
            }

        done:
            ;
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
                lblMessage.Text = "Error occurred! " + e.Error.Message;
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
                    lblMessage.Text = "Upload Cancelled!";
                    if (someFailed)
                        lblMessage.Text = lblMessage.Text + ", some labels are failed!";
                }
            }
            else
            {
                // ' otherwise it completed normally
                // MessageBox.Show("Download completed!")
                lblMessage.Text = "Upload completed!";
                if (someFailed)
                    lblMessage.Text = lblMessage.Text + ", But some labels are failed!";
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