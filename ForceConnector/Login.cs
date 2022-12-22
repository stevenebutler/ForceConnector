using System;
using System.Threading;
using ForceConnector.MiniMETA;
using ForceConnector.Partner;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class frmLogin
    {
        public frmLogin()
        {
            InitializeComponent();
            _btnLogin.Name = "btnLogin";
            _btnCancel.Name = "btnCancel";
            _usingOAuth2.Name = "usingOAuth2";
            _usingSOAP.Name = "usingSOAP";
            _btnNext.Name = "btnNext";
            _SOAPAPI.Name = "SOAPAPI";
            _RESTFULAPI.Name = "RESTFULAPI";
        }

        public static bool success = false;
        private SoapClient soapClient;
        private MetadataPortTypeClient metaClient;
        private Partner.SessionHeader soapSessionHeader = new Partner.SessionHeader();
        private MiniMETA.SessionHeader metaSessionHeader = new MiniMETA.SessionHeader();
        private LoginResult loginRes;

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ThisAddIn.loginType = "oauth";
            usingOAuth2_CheckedChanged(sender, e);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginType = ThisAddIn.loginType;
            string target = cmbDestination.Text;
            string destination = "login";
            responseBox.Text = "";
            if (target == "Sandbox")
            {
                destination = "test";
            }

            if (loginType == "soap")
            {
                btnLogin.Enabled = false;
                soapLogin(destination);
            }
            else
            {
                btnLogin.Enabled = false;

                try
                {
                    using (tokenSource = new CancellationTokenSource())
                    {
                        if (OAuth2.openOAuthLogin(destination, tokenSource.Token))
                        {
                            loginCallback();
                        }
                    }
                }
                finally
                {
                    tokenSource = null;
                }
            }
        }
        private CancellationTokenSource tokenSource = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (tokenSource is not null)
            {
                tokenSource.Cancel();
            }
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
            ThisAddIn.conInfo = RESTAPI.getConnectionInfo();
            ThisAddIn.api = Conversions.ToDouble(cmbVersion.Text);
            Util.displayUserName(ThisAddIn.conInfo.display_name);
            Close();
        }

        private void soapLogin(string destination)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string security = txtSecurityToken.Text;
            string version = cmbVersion.Text;
            string passcode = password + security;
            string endpointUrl;
            var userInfo = new GetUserInfoResult();
            endpointUrl = "https://" + destination + ".salesforce.com/services/Soap/u/" + version + ".0";
            if (soapClient is null)
                soapClient = new SoapClient("Soap", endpointUrl);
            try
            {
                loginRes = soapClient.login(ThisAddIn.soapLoginScopeHeader, ThisAddIn.soapCallOptions, username, passcode);
            }
            catch (Exception ex)
            {
                // This is something else, probably comminication
                responseBox.Text = ex.Message;
                // MsgBox(ex.Message, Title:="Exception")
                // Close()
            }
            // Change the binding to the new endpoint
            userInfo = loginRes.userInfo;
            soapClient = new SoapClient("Soap", loginRes.serverUrl);
            metaClient = new MetadataPortTypeClient("Metadata", loginRes.metadataServerUrl);
            // Create a new session header object and set the session id to that returned by the login
            soapSessionHeader.sessionId = loginRes.sessionId;
            metaSessionHeader.sessionId = loginRes.sessionId;
            ThisAddIn.api = Conversions.ToDouble(version);
            ThisAddIn.userLang = userInfo.userLanguage;
            ThisAddIn.soapClient = soapClient;
            ThisAddIn.metaClient = metaClient;
            ThisAddIn.soapSessionHeader = soapSessionHeader;
            ThisAddIn.metaSessionHeader = metaSessionHeader;
            ThisAddIn.accessToken = loginRes.sessionId;
            ThisAddIn.instanceUrl = loginRes.serverUrl.Substring(0, loginRes.serverUrl.IndexOf("/services"));
            ThisAddIn.id = "https://" + destination + ".salesforce.com/id/" + userInfo.organizationId + "/" + userInfo.userId;
            responseBox.Text = "Login Success!";
            success = true;
            btnNext.Enabled = true;
        }

        private void usingOAuth2_CheckedChanged(object sender, EventArgs e)
        {
            if (usingOAuth2.Checked)
            {
                // lblVersion.Visible = False
                lblUsername.Visible = false;
                lblPassword.Visible = false;
                lblSecurityKey.Visible = false;
                // cmbVersion.Visible = False
                txtUsername.Visible = false;
                txtPassword.Visible = false;
                txtSecurityToken.Visible = false;

                // cmbDestination.Width = 240
                // responseBox.Top = 215
                // btnLogin.Top = 215

                ThisAddIn.loginType = "oauth";
            }
        }

        private void usingSOAP_CheckedChanged(object sender, EventArgs e)
        {
            if (usingSOAP.Checked)
            {
                // lblVersion.Visible = True
                lblUsername.Visible = true;
                lblPassword.Visible = true;
                lblSecurityKey.Visible = true;
                // cmbVersion.Visible = True
                txtUsername.Visible = true;
                txtPassword.Visible = true;
                txtSecurityToken.Visible = true;

                // cmbDestination.Width = 100
                // responseBox.Top = 215
                // btnLogin.Top = 215

                ThisAddIn.loginType = "soap";
            }
        }

        private void RESTFULAPI_CheckedChanged(object sender, EventArgs e)
        {
            if (RESTFULAPI.Checked)
            {
                ThisAddIn.usingRESTful = true;
            }
        }

        private void SOAPAPI_CheckedChanged(object sender, EventArgs e)
        {
            if (SOAPAPI.Checked)
            {
                ThisAddIn.usingRESTful = false;
            }
        }

        // Code for OAuth Login
        // Login callback function to handle ASync OAUTH login process
        public void loginCallback()
        {
            try
            {
                soapClient = new SoapClient("Soap", ThisAddIn.conInfo.urls.partner);
                metaClient = new MetadataPortTypeClient("Metadata", ThisAddIn.conInfo.urls.metadata);
                soapSessionHeader.sessionId = ThisAddIn.accessToken;
                metaSessionHeader.sessionId = ThisAddIn.accessToken;
                ThisAddIn.soapClient = soapClient;
                ThisAddIn.metaClient = metaClient;
                ThisAddIn.soapSessionHeader = soapSessionHeader;
                ThisAddIn.metaSessionHeader = metaSessionHeader;
                responseBox.Text = "Login Successful!";
                success = true;
            }
            catch (Exception ex)
            {
                responseBox.Text = "Logged in, but can not use SOAP features";
                Interaction.MsgBox("You can not use 'Translation Helper' at this time" + Constants.vbCrLf + ex.Message + Constants.vbCrLf + ex.StackTrace, Title: "SOAP Client Error!");
            }
            Operation.LastCheckedLogin = DateTime.Now;
            btnNext.Enabled = true;
            btnNext.PerformClick();
        }

        public bool getSuccess()
        {
            return success;
        }
    }
}