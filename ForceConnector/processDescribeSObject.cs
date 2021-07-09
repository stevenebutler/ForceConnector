using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processDescribeSObject
    {
        public processDescribeSObject()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        public string[] objectList;
        private string statusText = "";
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private Excel.Range start;
        private string[] namedFields;
        private Dictionary<string, RESTful.Field> standardFields = new Dictionary<string, RESTful.Field>();
        private Dictionary<string, RESTful.Field> customFields = new Dictionary<string, RESTful.Field>();

        private void processDescribeSObject_Load(object sender, EventArgs e)
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

            try
            {
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.StatusBar = "Describe SObjects...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(0, "Please wait for initialize...");
                var selectObjectsBox = new frmObjectList();
                selectObjectsBox.ShowDialog();
                if (selectObjectsBox.success & selectObjectsBox.objectList.Count > 0)
                {
                    objectList = selectObjectsBox.objectList.ToArray();
                    setControlText(btnAction, "Cancel");
                    setControlStatus(btnAction, true);
                }
                else
                {
                    statusText = "Describe error or no objects";
                    goto errors;
                }

                namedFields = new[] { "Id", "Name", "Subject", "CurrencyISOCode", "MasterRecordId", "CreatedById", "CreatedDate", "LastModifiedById", "LastModifiedDate", "IsDeleted", "SystemModstamp", "LastActivityDate", "LastViewedDate", "LastReferencedDate", "RecordTypeId", "OwnerId" };
                int numOfObject = objectList.Length;
                int objectCount = 0;
                int numOfPart = (int)Math.Round(100d / numOfObject);
                int percent = 0;
                foreach (string objname in objectList)
                {
                    standardFields.Clear();
                    customFields.Clear();
                    bgw.ReportProgress(percent, "Describe " + objname + "... ");
                    var gr = RESTAPI.DescribeSObject(objname);
                    var fields = gr.fields;
                    int numOfField = fields.Length;
                    int rowPointer = 0;
                    foreach (RESTful.Field fld in fields)
                    {
                        if (fld.custom)
                        {
                            customFields.Add(fld.name, fld);
                        }
                        else
                        {
                            standardFields.Add(fld.name, fld);
                        }

                        // ' check at regular intervals for CancellationPending
                        if (bgw.CancellationPending)
                        {
                            bgw.ReportProgress(percent, "Cancelling...");
                            break;
                        }
                    }

                    excelApp.ScreenUpdating = false;
                    DescribeObjects.setWorkSheet(ref excelApp, ref workbook, ref worksheet, objname);
                    DescribeObjects.setLayout(ref worksheet, objname);
                    DescribeObjects.renderHeader(ref worksheet, ref start, objname);
                    rowPointer = DescribeObjects.renderNamedField(ref worksheet, ref start, ref standardFields, rowPointer);
                    var argbgw = bgw;
                    rowPointer = DescribeObjects.renderStandardField(ref worksheet, ref start, ref namedFields, ref standardFields, rowPointer, ref objectCount, ref numOfPart, numOfField, objname, ref argbgw);
                    bgw = argbgw;
                    var argbgw1 = bgw;
                    DescribeObjects.renderCustomField(ref worksheet, ref start, ref namedFields, ref customFields, rowPointer, ref objectCount, ref numOfPart, numOfField, objname, ref argbgw1);
                    bgw = argbgw1;
                    excelApp.ScreenUpdating = true;

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        bgw.ReportProgress(percent, "Cancelling...");
                        break;
                    }

                    percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    objectCount = objectCount + 1;
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