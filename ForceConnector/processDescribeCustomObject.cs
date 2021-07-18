using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class processDescribeCustomObject
    {
        public processDescribeCustomObject()
        {
            InitializeComponent();
            _btnAction.Name = "btnAction";
        }

        public string[] objectList;
        private List<string> m_langSet = new List<string>();
        private string baseLang = "";
        private string statusText = "";
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private Excel.Range start;

        private readonly static string[] namedFieldsOrder = new string[] { "Id", "Name", "Subject", "CurrencyISOCode", "MasterRecordId", "CreatedById", "CreatedDate", "LastModifiedById", "LastModifiedDate", "IsDeleted", "SystemModstamp", "LastActivityDate", "LastViewedDate", "LastReferencedDate", "RecordTypeId", "OwnerId" };
        private readonly static HashSet<string> namedFields = new HashSet<string>(namedFieldsOrder);

        private void processDescribeCustomObject_Load(object sender, EventArgs e)
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
                bool hasTranslation = false;
                excelApp = ThisAddIn.excelApp;
                workbook = excelApp.ActiveWorkbook;
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                excelApp.StatusBar = "Describe Custom Objects...";
                setControlText(btnAction, "Wait...");
                bgw.ReportProgress(0, "Please wait for initialization to complete...");
                METAAPI.getTranslations(ref m_langSet);
                if (m_langSet.Count > 0)
                    hasTranslation = true;
                var selectObjectsBox = new frmObjectList();
                selectObjectsBox.langs = m_langSet;
                if (!hasTranslation)
                {
                    selectObjectsBox.cmbLang.Visible = false;
                    selectObjectsBox.lblSelectLang.Visible = false;
                }

                selectObjectsBox.ShowDialog();
                if (selectObjectsBox.success && selectObjectsBox.objectList.Count > 0)
                {
                    objectList = selectObjectsBox.objectList.ToArray();
                    baseLang = selectObjectsBox.baseLanguage;
                    setControlText(btnAction, "Cancel");
                    setControlStatus(btnAction, true);
                }
                else
                {
                    statusText = "Describe error or no objects";
                    goto errors;
                }

                int numOfObject = objectList.Length;
                int objectCount = 0;
                int numOfPart = (int)Math.Round(100d / numOfObject);
                int percent = 0;
                foreach (string objname in objectList)
                {
                    var fieldMeta = new Dictionary<string, Dictionary<string, string>>();
                    var objLabels = new Dictionary<string, string>();
                    var standardFields = new Dictionary<string, Partner.Field>();
                    var customFields = new Dictionary<string, Partner.Field>();
                    bgw.ReportProgress(percent, "Describe " + objname + "... ");
                    var co = DescribeCustomObject.DescribeSObject(objname, baseLang);
                    var fields = co.fields;
                    string baseLabel = Conversions.ToString(Operators.ConcatenateObject(co.label + ", ", Interaction.IIf(!string.IsNullOrEmpty(co.labelPlural), co.labelPlural, "no_plural_label")));
                    objLabels.Add("base", baseLabel);
                    objLabels.Add(baseLang, baseLabel);
                    var argbgw = bgw;
                    fieldMeta = DescribeCustomObject.getFieldTranslations(objname, ref objLabels, ref fields, ref m_langSet, ref baseLang, ref percent, ref argbgw);
                    bgw = argbgw;
                    int numOfField = fields.Length;
                    int rowPointer = 0;
                    foreach (Partner.Field fld in fields)
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

                    excelApp.ScreenUpdating = true;
                    DescribeCustomObject.setWorkSheet(ref excelApp, ref workbook, ref worksheet, objname);
                    DescribeCustomObject.setLayout(ref worksheet, objname, ref objLabels);
                    DescribeCustomObject.renderHeader(ref worksheet, ref start, objname);
                    // Renders predefined list of fields first (Salesforce Metadata),
                    // Then the standard fields
                    // Then the custom fields
                    object[,] data = new object[numOfField, 13];
                    List<CommentPosition> comments = new List<CommentPosition>();

                    rowPointer = DescribeCustomObject.renderNamedField(ref worksheet, ref start, namedFieldsOrder, ref standardFields, ref fieldMeta, rowPointer, data, comments);
                    var argbgw1 = bgw;
                    rowPointer = DescribeCustomObject.renderStandardField(ref worksheet, ref start, namedFields, ref standardFields, ref fieldMeta, rowPointer, ref objectCount, ref numOfPart, numOfField, objname, ref argbgw1, data, comments);
                    bgw = argbgw1;
                    var argbgw2 = bgw;
                    DescribeCustomObject.renderCustomField(ref worksheet, ref start, namedFields, ref customFields, ref fieldMeta, rowPointer, ref objectCount, ref numOfPart, numOfField, objname, ref argbgw2, data, comments);

                    Excel.Range rng = worksheet.Range[start, start.Offset[numOfField - 1, 12]];

                    rng.Clear();

                    // Set the range values
                    rng.Value = data;

                    // Formatting for the whole range
                    rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    rng.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlDot;
                    rng.IndentLevel = 1;
                    rng.Font.Name = "Vernada";

                    DescribeCustomObject.renderComments(ref rng, comments);
                    
                    bgw = argbgw2;


                    excelApp.ScreenUpdating = true;

                    // ' check at regular intervals for CancellationPending
                    if (bgw.CancellationPending)
                    {
                        bgw.ReportProgress(percent, "Cancelling...");
                        break;
                    }

                    percent = (int)Math.Round(numOfPart * (rowPointer / (double)numOfField)) + numOfPart * objectCount;
                    if (percent > 100)
                    {
                        percent = 100;
                    }
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