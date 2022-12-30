using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using ForceConnector.MiniMETA;
using ForceConnector.Partner;
using Excel = Microsoft.Office.Interop.Excel;

namespace ForceConnector
{
    public partial class ThisAddIn
    {
        public const string ribbonBoxName = "Force.com Connector Next Generation";
        public const string Ver = "2021/06/01 R0.8";
        public const string USERNAME = "Username";
        public const string AUTHTARGET = "Target";
        public static double api = 51.0d;
        public static string userLang = "";
        public static SoapClient soapClient;
        public static MetadataPortTypeClient metaClient;
        public static LoginScopeHeader soapLoginScopeHeader = new LoginScopeHeader();
        public static CallOptions soapCallOptions = new CallOptions();
        public static Partner.SessionHeader soapSessionHeader = new Partner.SessionHeader();
        public static MiniMETA.SessionHeader metaSessionHeader = new MiniMETA.SessionHeader();
        public static Excel.Application excelApp;
        public static bool isBreak = false;
        public static bool usingRESTful = true;

        // Salesforce session properties for RESTful API
        // frmLogin form refer the these properties
        // Do not use for SOAP API, but remain for reference
        public static string loginType;
        public static string accessToken;
        public static string refreshToken;
        public static string tokenType;
        public static string issuedAt;
        public static string id;
        public static string instanceUrl;
        public static RESTful.ConnectionInfo conInfo;
        public static Dictionary<string, string> UserNames;
        public static Dictionary<string, string> RecordTypes;
        public static Dictionary<string, string> Profiles;
        public static Dictionary<string, string> Roles;
        public static Dictionary<string, string> Groups;
        private SalesForceAddInApi interopApi;

        protected override object RequestComAddInAutomationService()
        {
            if (interopApi is null)
            {
                interopApi = new SalesForceAddInApi();
            }

            return interopApi;
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Globals.Ribbons.ForceRibbon.ribbonForceConnector.Label = ribbonBoxName + " (no logon user)";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            excelApp = Globals.ThisAddIn.Application;
            UserNames = new Dictionary<string, string>();
            RecordTypes = new Dictionary<string, string>();
            Profiles = new Dictionary<string, string>();
            Roles = new Dictionary<string, string>();
            Groups = new Dictionary<string, string>();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            accessToken = "";
            refreshToken = "";
            tokenType = "";
            issuedAt = "";
            id = "";
            instanceUrl = "";
            conInfo = null;
            UserNames = null;
            RecordTypes = null;
            Profiles = null;
            Roles = null;
            Groups = null;
            if (Util.checkSession())
            {
                //MessageBox.Show("Session alive, logging out from Salesforce!");
                soapClient.logout(soapSessionHeader, soapCallOptions);
            }
        }
        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}