using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ForceConnector.MiniMETA;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processTranslationDownload
    {
        public processTranslationDownload()
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
        private List<string> m_langSet = new List<string>();

        private void processTranslationDownload_Load(object sender, EventArgs e)
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
                excelApp.StatusBar = "Download Translation metadata...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(10, "Please wait for initialize...");
                METAAPI.getTranslations(ref m_langSet);
                if (m_langSet.Count == 0)
                {
                    statusText = "No translation setting in ORG.";
                    goto errors;
                }

                worksheet = METAAPI.getMetaWorkSheet(ref workbook, "Translation");
                METAAPI.setTranslationLayout(ref worksheet, ref start);
                METAAPI.setWorkArea(ref excelApp, ref worksheet, ref m_table, ref m_head, ref m_body, ref m_start, ref m_rows, ref m_metaType);
                METAAPI.setLanguageHeaders(ref excelApp, ref worksheet, ref m_head, ref m_langSet);
                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);
                int numOfLang = m_langSet.Count;
                int langCount = 0;
                var metas = METAAPI.readMetadata("Translations", m_langSet.ToArray());
                if (metas.Length == 0)
                {
                    statusText = "No translation for CustomLabel";
                    goto errors;
                }

                bgw.ReportProgress(50, "Download Translation Data...");
                foreach (var meta in metas)
                {
                    excelApp.ScreenUpdating = false;
                    string trnsKey = meta.fullName;
                    long argm_rows = m_rows;
                    METAAPI.renderTranslations(ref excelApp, ref m_head, ref m_body, ref argm_rows, trnsKey, meta);
                    int percent = (int)Math.Round(50d * (langCount / (double)(numOfLang * numOfLang)) + 50d / numOfLang * langCount) + 50;
                    if (percent > 100)
                        percent = 100;

                    // change process bar
                    bgw.ReportProgress(percent, "Download... (" + trnsKey + ") " + langCount.ToString() + " / " + numOfLang.ToString());

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        bgw.ReportProgress(percent, "Cancelling...");
                        break;
                    }

                    langCount = langCount + 1;
                    excelApp.ScreenUpdating = true;
                }

                // ' any cleanup code go here
                // ' ensure that you close all open resources before exitting out of this Method.
                // ' try to skip off whatever is not desperately necessary if CancellationPending is True
                bgw.ReportProgress(100, "Download Completed!");

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