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
            logined = LoginToSalesforceRet;
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
            CheckLoginAndAct(TableWizard.QueryWizard, "QueryTableWizard Exception");
        }

        public static void UpdateSelectedCells()
        {
            CheckLoginAndAct(Operation.UpdateCells, "UpdateSelectedCells Exception");

        }

        public static void InsertSelectedRows()
        {
            CheckLoginAndAct(Operation.InsertRows, "InsertSelectedRows Exception");

        }

        public static void QuerySelectedRows()
        {
            CheckLoginAndAct(Operation.QueryRows, "QuerySelectedRows Exception");
        }

        public static void DescribeSforceObject()
        {
            CheckLoginAndAct(DescribeCustomObject.DescribeSalesforceObjectsBySOAP, "DescribeSforceObject Exception");

        }

        private static void CheckLoginAndAct(Action act, String failMessage)
        {
            try
            {
                if (!logined)
                {
                    if (!LoginToSalesforce())
                    {
                        throw new Exception("Login failed!");
                    }
                }
                act();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + Constants.vbCrLf + ex.StackTrace, Title: failMessage);
            }
        }

        public static void QueryTableData()
        {
            CheckLoginAndAct(Operation.QueryData, "QueryTableData");
        }

        public static void RefreshTableData()
        {
            CheckLoginAndAct(Operation.RefreshData, "RefreshTableData");
        }


        public static void DeleteSelectedRecords()
        {
            CheckLoginAndAct(Operation.DeleteRecords, "DeleteSelectedRecords Exception");
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
            logined = false;
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