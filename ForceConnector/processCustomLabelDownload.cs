using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ForceConnector.MiniMETA;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processCustomLabelDownload
    {
        public processCustomLabelDownload()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        private long numOfCustomLabel;
        private string statusText = "";
        private FileProperties[] m_files;
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

        private void processCustomLabelDownload_Load(object sender, EventArgs e)
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

            try
            {
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                excelApp.StatusBar = "Download CustomLabel metadata...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(10, "Please wait for initialize...");

                // ' Call initialize process...
                m_files = METAAPI.listMetadata(new[] { "CustomLabel" });
                if (m_files is object)
                {
                    numOfCustomLabel = m_files.Length;
                    worksheet = METAAPI.getMetaWorkSheet(ref workbook, "CustomLabel");
                    METAAPI.setCustomLabelLayout(ref worksheet, ref start);
                    METAAPI.setWorkArea(ref excelApp, ref worksheet, ref m_table, ref m_head, ref m_body, ref m_start, ref m_rows, ref m_metaType);
                    setControlText(btnAction, "Cancel");
                    setControlStatus(btnAction, true);
                }
                else
                {
                    statusText = "No CustomLabel Data!";
                    goto errors;
                }

                // ' insert check rutine for zero return.
                long i = 1L;
                numOfCustomLabel = m_files.Length;
                IEnumerable<FileProperties> fullNames = m_files.OrderBy(m_file => m_file.fullName);
                foreach (FileProperties m_file in fullNames)
                {
                    m_body.Cells[(object)i, (object)1].value = m_file.fullName;

                    // change process bar
                    int percent = (int)Math.Round(90L * i / (double)numOfCustomLabel) + 10;
                    bgw.ReportProgress(percent, "Set CustomLabels... " + i.ToString() + " / " + numOfCustomLabel.ToString());
                    i = i + 1L;
                }

                bgw.ReportProgress(0, "Start query for CustomLabel...");
                m_body = m_body.get_Resize(i, m_head.Columns.Count);
                m_body.Select();

                // Query CustomLabel
                long row_pointer = Conversions.ToLong(excelApp.Selection.row);
                long row_counter = 0L;
                Excel.Range chunk;
                do
                {
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, (Excel.Range)worksheet.Rows[(object)row_pointer]);
                    if (chunk is null)
                        break;
                    chunk = chunk.get_Resize(10); // max size is 10 for read metadata
                    chunk = excelApp.Intersect((Excel.Range)excelApp.Selection, chunk); // trim the last chunk !
                    row_pointer = row_pointer + chunk.Rows.Count; // up our pos counter
                    row_counter = row_counter + chunk.Rows.Count;
                    // row_pointer = row_pointer + 10 ' up our pos counter
                    // row_counter = row_counter + 10
                    chunk.Interior.ColorIndex = (object)36; // show off...
                    excelApp.ScreenUpdating = false;
                    if (!METAAPI.queryCustomLabel(ref excelApp, ref chunk))
                        goto done; // do it
                    excelApp.ScreenUpdating = true;
                    chunk.Interior.ColorIndex = (object)0;

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

            m_files = null;
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