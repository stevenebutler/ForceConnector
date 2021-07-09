using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ForceConnector.MiniMETA;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processObjectTranslationDownload
    {
        public processObjectTranslationDownload()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        public Dictionary<string, List<string>> objectMap = new Dictionary<string, List<string>>();
        public string[] selectedObject;
        private long numOfCustomLabel;
        private string statusText = "";
        private FileProperties[] m_files;
        private Metadata[] metas;
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
        private Dictionary<string, string> m_baseObject = null;
        private List<string> m_langSet = new List<string>();

        private void processObjectTranslationDownload_Load(object sender, EventArgs e)
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
                excelApp.StatusBar = "Download CustomLabel Translation metadata...";
                bgw.ReportProgress(0, "Please wait for initialize...");
                setControlText(btnAction, "Cancel");
                setControlStatus(btnAction, true);
                int numOfObj = selectedObject.Length;
                int objCount = 0;
                int numOfPart = 0;
                foreach (string objName in selectedObject)
                {
                    m_rows = 0;
                    m_langSet = objectMap[objName];
                    int numOfLang = m_langSet.Count;
                    int langCount = 1;
                    if (numOfPart == 0)
                        numOfPart = numOfObj * numOfLang;
                    string tabName = objName;
                    if (tabName.Length > 27)
                    {
                        VBMath.Randomize();
                        int randomValue = (int)Math.Round(Math.Floor((32767 - 1 + 1) * VBMath.Rnd())) + 1;
                        tabName = tabName.Substring(0, 20) + "_" + randomValue.ToString();
                    }

                    worksheet = METAAPI.getMetaWorkSheet(ref workbook, "T(" + tabName + ")", false);
                    if (m_rows == 0)
                    {
                        METAAPI.setObjectTranslationLayout(ref worksheet, objName, ref start);
                        METAAPI.setWorkArea(ref excelApp, ref worksheet, ref m_table, ref m_head, ref m_body, ref m_start, ref m_rows, ref m_metaType);
                        METAAPI.setLanguageHeaders(ref excelApp, ref worksheet, ref m_head, ref m_langSet);
                    }

                    foreach (string lang in m_langSet.ToArray())
                    {
                        System.Threading.Thread.Sleep(100);
                        excelApp.ScreenUpdating = false;
                        string trnsKey = objName + "-" + lang;
                        int cnt = objCount * numOfLang + langCount;
                        int percent = (int)Math.Round(cnt / (double)numOfPart * 90d);
                        string indicator = "Downloads... " + cnt.ToString() + " / " + numOfPart.ToString() + " (" + trnsKey + ") ";
                        // change process bar
                        bgw.ReportProgress(percent, indicator);
                        if (m_baseObject is null)
                            METAAPI.renderBaseObjectDescribe(ref m_baseObject, objName);
                        var metas = METAAPI.readMetadata("CustomObjectTranslation", new[] { trnsKey });
                        if (metas.Length == 0)
                        {
                            statusText = "No translation for " + trnsKey;
                            goto errors;
                        }

                        long argm_rows = m_rows;
                        METAAPI.renderObjectTranslation(ref excelApp, ref m_head, ref m_body, ref argm_rows, ref m_baseObject, objName, lang, metas[0]);

                        // ' check at regular intervals for CancellationPending
                        if (bgw.CancellationPending)
                        {
                            bgw.ReportProgress(percent, "Cancelling...");
                            break;
                        }

                        langCount = langCount + 1;
                        excelApp.ScreenUpdating = true;
                    }

                    if (bgw.CancellationPending)
                        break;
                    objCount = objCount + 1;
                }

                bgw.ReportProgress(100, "Download complete!");

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