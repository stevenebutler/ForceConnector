using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly static object client_id = "SalesforceDevelopmentExperience";
        private readonly static object client_secret = "1384510088588713504";
        private readonly static object callback_url = "http://localhost:1717/OauthRedirect";
        private static string targetDomain = "";

        /// <summary>
    /// OAuthLogin by Default WebBrowser
    /// </summary>
    /// <param name="target"></param>
        public static void openOAuthLogin(string target)
        {
            var prs = new Process();
            try
            {
                targetDomain = string.Format(service_domain, target);
                string targetAddress = string.Format(request_token_url, targetDomain, client_id, callback_url);
                prs = Process.Start(targetAddress);
                processRequest(new[] { "http://localhost:1717/" });
            }
            // processRequestAsync({"http://localhost:1717/"})
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "openOAuthLogin Exception");
            }

            try
            {
                if (!prs.HasExited)
                {
                    prs.CloseMainWindow();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void processRequest(string[] prefixes)
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
                var context = listener.GetContext();
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

                // Dim fullUrl As String = ThisAddIn.conInfo.urls.custom_domain & "/secur/frontdoor.jsp?sid=" & ThisAddIn.accessToken & "&retURL=/lightning/page/home"
                string fullUrl = ThisAddIn.conInfo.urls.custom_domain;
                response.Redirect(fullUrl);
                response.Close();
            }
            catch (HttpListenerException ex)
            {
                throw new Exception("processRequests Exception" + Constants.vbCrLf + ex.Message);
            }
            finally
            {
                // Stop listening for requests.
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
            var client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.Timeout = new TimeSpan(0, 0, 120);
            var content = new FormUrlEncodedContent(values);
            try
            {
                var response = client.PostAsync(URL, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string jsonString = response.Content.ReadAsStringAsync().Result;
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
                else
                {
                    Interaction.MsgBox(((int)response.StatusCode).ToString() + " (" + response.ReasonPhrase + ") " + response.Content.ReadAsStringAsync().Result, Title: "Request AccessToken Failed!");
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