using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;

namespace ForceConnector
{
    static class ForceConnector
    {

        // Const from excel connector
        // TODO make this batch size configurable via the options dialog box
        public const int maxBatchSize = 50;  // used to size Update,Query Row, Create and Delete batches, it is not work with RESTful API. minimum is 200 for REST query
                                             // // limits for update batch sizes
        public const long maxCols = 20L;
        public const long maxRows = 3500L;
        public const long excelLimit = 1048570L; // Excel 2016's maximum rows id 1,048,576
        public const int excelColLimit = 16384; // Excel 2016's column limit is 16384
        public const string NOT_FOUND = "#N/F";
        public const string USE_REFERENCE = "UseReference";
        public const string GOAHEAD = "NoWarn";
        public const string NOLIMITS = "NoLimit";
        public const string AUTOASSIGNRULE = "AutoAssignRule";
        public const string SKIPHIDDEN = "SkipHiddenCells";  // ** allows us to use a special update routine
        public const string GET_MANAGED = "GetManagedData";
        public static Excel.Application excelApp;
        public static Excel.Workbook workbook;
        public static Excel.Worksheet worksheet;
        public static string op;
        private static bool logined = false;

        public static void setActiveSheet()
        {
            excelApp = ThisAddIn.excelApp;
            workbook = excelApp.ActiveWorkbook;
            worksheet = (Excel.Worksheet)workbook.ActiveSheet;
        }

        public static bool LoginToSalesforce()
        {
            bool LoginToSalesforceRet = default;
            LoginToSalesforceRet = false;
            var loginForm = new frmLogin();
            loginForm.ShowDialog();
            LoginToSalesforceRet = loginForm.getSuccess();
            loginForm.Dispose();
            return LoginToSalesforceRet;
        }

        public static void OpenAbout()
        {
            var aboutBox = new frmAbout();
            aboutBox.ShowDialog();
        }

        public static void QueryTableWizard()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    TableWizard.QueryWizard();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "QueryTableWizard Exception");
            }
        }

        public static void UpdateSelectedCells()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    Operation.UpdateCells();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "UpdateSelectedCells Exception");
            }
        }

        public static void InsertSelectedRows()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    Operation.InsertRows();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "InsertSelectedRows Exception");
            }
        }

        public static void QuerySelectedRows()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    Operation.QueryRows();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "QuerySelectedRows Exception");
            }
        }

        public static void DescribeSforceObject()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                {
                    // If ThisAddIn.usingRESTful Then
                    // Call DescribeSalesforceObjectsByREST()
                    // Else
                    DescribeCustomObject.DescribeSalesforceObjectsBySOAP();
                    // End If
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "DescribeSforceObject Exception");
            }
        }

        public static void QueryTableData()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    Operation.QueryData();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "QueryTableData Exception");
            }
        }

        public static void DeleteSelectedRecords()
        {
            try
            {
                if (!Util.checkSession())
                {
                    if (!LoginToSalesforce())
                        throw new Exception("Login failed!");
                }

                if (Util.checkSession())
                    Operation.DeleteRecords();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "DeleteSelectedRecords Exception");
            }
        }

        public static void OptionsForm()
        {
            try
            {
                var optionBox = new frmOption();
                optionBox.ShowDialog();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "OptionsForm Exception");
            }
        }

        public static void LogoutFrom()
        {
            if (Util.checkSession())
            {
                MessageBox.Show("Session alived, logout from Salesforce!");
                ThisAddIn.soapClient.logout(ThisAddIn.soapSessionHeader, ThisAddIn.soapCallOptions);
                ThisAddIn.soapClient = null;
                ThisAddIn.metaClient = null;
                ThisAddIn.accessToken = "";
                ThisAddIn.instanceUrl = "";
                Util.displayUserName("no logon user");
            }
        }
    }
}