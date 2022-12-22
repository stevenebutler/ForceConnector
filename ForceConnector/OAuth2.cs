using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class OAuth2
    {
        private readonly static string service_domain = "https://{0}.salesforce.com";
        private readonly static string request_token_url = "{0}/services/oauth2/authorize?client_id={1}&redirect_uri={2}&response_type=code&display=popup&prompt=login";
        private readonly static string access_token_url = "{0}/services/oauth2/token";
        private readonly static string client_id = "SalesforceDevelopmentExperience";
        private readonly static string client_secret = "1384510088588713504";
        private readonly static string callback_url = "http://localhost:1717/OauthRedirect";
        private static string targetDomain = "";

        /// <summary>
        /// OAuthLogin by Default WebBrowser
        /// </summary>
        /// <param name="target"></param>
        /// <param name="token"></param>
        public static bool openOAuthLogin(string target, CancellationToken token)
        {
            Process prs = null;
            try
            {
                targetDomain = string.Format(service_domain, target);
                string targetAddress = string.Format(request_token_url, targetDomain, client_id, callback_url);
                prs = Process.Start(targetAddress);
                processRequest(new[] { "http://localhost:1717/" }, token);
                return true;
            }
            // processRequestAsync({"http://localhost:1717/"})
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "openOAuthLogin Exception");
                return false;
            }
            finally
            {

                try
                {
                    if (prs is not null && !prs.HasExited)
                    {
                        prs.CloseMainWindow();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void processRequest(string[] prefixes, CancellationToken token)
        {
            if (!HttpListener.IsSupported)
            {
                Util.ErrorBox("This version of .Net Framework does not support Callback Listener!");
                return;
            }

            // URI prefixes are required,
            if (prefixes is null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener and add the prefixes.
            var listener = new HttpListener();
            foreach (string s in prefixes)
                listener.Prefixes.Add(s);
            try
            {
                // Start the listener to begin listening for requests.
                listener.Start();
                var contextTask = listener.GetContextAsync();
                while (!(contextTask.IsCompleted || contextTask.IsFaulted))
                {
                    Thread.Sleep(100);
                    System.Windows.Forms.Application.DoEvents();
                    token.ThrowIfCancellationRequested();
                }
                var context = contextTask.GetAwaiter().GetResult();
                var request = context.Request;
                var response = context.Response;
                string[] options;
                string callbackUri = request.Url.ToString();
                byte[] bytes = null;
                bool good = false;
                if (callbackUri.StartsWith(callback_url, StringComparison.OrdinalIgnoreCase))
                {
                    good = true;
                    bytes = System.Text.Encoding.UTF8.GetBytes("<html><body style='font-family: arial'><h3>Login Complete</h3><p>You may close this window/tab as login has been completed.</p></body></html>");

                }
                else
                {
                    bytes = System.Text.Encoding.UTF8.GetBytes("<html><body style='font-family: arial'><h3>Login Error - invalid callback URI</h3><p>Try logging out before doing another operation.</p></body></html>");
                }
                response.ContentType = "text/html; charset=UTF8";
                response.SendChunked = true;
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
                response.Close();
                if (good)
                {
                    var uri = new Uri(callbackUri);

                    options = uri.Query.Split('&');
                    requestAccessToken(options);
                    getSystemInfo();
                }

            }
            catch (HttpListenerException ex)
            {
                throw new Exception("processRequests Exception" + Constants.vbCrLf + ex.Message);
            }
            finally
            {
                // Stop listening for requests.
                // A sleep is required to avoid browser errors
                Thread.Sleep(10);
                listener.Stop();
                listener.Close();
            }
        }

        public static void requestAccessToken(string[] options)
        {
            string authcode = "";
            string display = "";
            foreach (string opt in options)
            {
                var seperator = new char[] { '=' };
                var param = opt.Split(seperator, 2);
                switch (param[0] ?? "")
                {
                    case "code":
                        {
                            authcode = param[1];
                            break;
                        }

                    case "display":
                        {
                            display = param[1];
                            break;
                        }
                }
            }

            string URL = string.Format(access_token_url, targetDomain);
            var values = new Dictionary<string, string>() { { "grant_type", "authorization_code" }, { "code", authcode }, { "client_id", Conversions.ToString(client_id) }, { "client_secret", Conversions.ToString(client_secret) }, { "redirect_uri", Conversions.ToString(callback_url) } };
            var content = new FormUrlEncodedContent(values);

            try
            {
                try
                {
                    string jsonString = RESTAPI.CallREST("POST", URL, content);
                    var jss = new JavaScriptSerializer();
                    var dataObjects = jss.DeserializeObject(jsonString) as IDictionary;
                    ThisAddIn.accessToken = Conversions.ToString(dataObjects["access_token"]);
                    ThisAddIn.instanceUrl = Conversions.ToString(dataObjects["instance_url"]);
                    ThisAddIn.issuedAt = Conversions.ToString(dataObjects["issued_at"]);
                    ThisAddIn.refreshToken = Conversions.ToString(dataObjects["refresh_token"]);
                    ThisAddIn.tokenType = Conversions.ToString(dataObjects["token_type"]);
                    ThisAddIn.accessToken = Conversions.ToString(dataObjects["access_token"]);
                    ThisAddIn.id = Conversions.ToString(dataObjects["id"]);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.Message, Title: "Handle Response Exception");
                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "Request AccessToken Exception");
            }
        }

        public static void getSystemInfo()
        {
            ThisAddIn.conInfo = RESTAPI.getConnectionInfo();
            ThisAddIn.userLang = ThisAddIn.conInfo.language;
        }


        /// <summary>
        /// Async OAuth Login Process and Callback
        /// loginCallback() runtime timing issue!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="prefixes"></param>
        public static void processRequestAsync(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Util.ErrorBox("This version of .Net Framework does not support Callback Listener!");
                return;
            }

            // URI prefixes are required,
            if (prefixes is null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener and add the prefixes.
            var listener = new HttpListener();
            foreach (string s in prefixes)
                listener.Prefixes.Add(s);
            listener.Start();
            var result = listener.BeginGetContext(new AsyncCallback(processRequestAsyncCallback), listener);
            // Applications can do some work here while waiting for the 
            // request. If no work can be done until you have processed a request,
            // use a wait handle to prevent this thread from terminating
            // while the asynchronous operation completes.
            Console.WriteLine("Waiting for request to be processed asyncronously.");
            result.AsyncWaitHandle.WaitOne();
            Console.WriteLine("Request processed asyncronously.");
            // listener.Close()
        }

        public static void processRequestAsyncCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            var context = listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;
            string[] options;
            string callbackUri = request.Url.ToString();
            if (callbackUri.StartsWith(Conversions.ToString(callback_url), StringComparison.OrdinalIgnoreCase))
            {
                // MessageBox.Show(callbackUri)
                options = callbackUri.Replace(Conversions.ToString(Operators.ConcatenateObject(callback_url, "?")), "").Split('&');
                requestAccessToken(options);
            }

            getSystemInfo();
            string fullUrl = ThisAddIn.conInfo.urls.custom_domain;
            response.Redirect(fullUrl);
            response.Close();
            listener.Close();
        }
    }
}