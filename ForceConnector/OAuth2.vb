Imports System.Diagnostics
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Script.Serialization

Module OAuth2
    Private ReadOnly service_domain As String = "https://{0}.salesforce.com"
    Private ReadOnly request_token_url As String = "{0}/services/oauth2/authorize?client_id={1}&redirect_uri={2}&response_type=code&display=popup&prompt=login"
    Private ReadOnly access_token_url As String = "{0}/services/oauth2/token"

    Private ReadOnly client_id = "SalesforceDevelopmentExperience"
    Private ReadOnly client_secret = "1384510088588713504"
    Private ReadOnly callback_url = "http://localhost:1717/OauthRedirect"
    Private targetDomain As String = ""

    ''' <summary>
    ''' OAuthLogin by Default WebBrowser
    ''' </summary>
    ''' <param name="target"></param>
    Sub openOAuthLogin(ByVal target As String)
        Dim prs As Process = New Process
        Try
            targetDomain = String.Format(service_domain, target)
            Dim targetAddress As String = String.Format(request_token_url, targetDomain, client_id, callback_url)

            prs = Process.Start(targetAddress)
            processRequest({"http://localhost:1717/"})
            'processRequestAsync({"http://localhost:1717/"})
        Catch ex As Exception
            MsgBox(ex.Message, Title:="openOAuthLogin Exception")
        End Try
        Try
            prs.CloseMainWindow()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Sub processRequest(ByVal prefixes() As String)
        If Not HttpListener.IsSupported Then
            ErrorBox("This version of .Net Framework does not support Callback Listener!")
            Exit Sub
        End If

        ' URI prefixes are required,
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then Throw New ArgumentException("prefixes")

        ' Create a listener and add the prefixes.
        Dim listener As HttpListener = New HttpListener()
        For Each s As String In prefixes
            listener.Prefixes.Add(s)
        Next

        Try
            ' Start the listener to begin listening for requests.
            listener.Start()

            Dim context As HttpListenerContext = listener.GetContext()
            Dim request As HttpListenerRequest = context.Request
            Dim response As HttpListenerResponse = context.Response

            Dim options() As String
            Dim callbackUri As String = request.Url.ToString()

            If callbackUri.StartsWith(callback_url, StringComparison.OrdinalIgnoreCase) Then
                'MessageBox.Show(callbackUri)
                options = callbackUri.Replace(callback_url & "?", "").Split("&")

                requestAccessToken(options)
            End If

            getSystemInfo()

            'Dim fullUrl As String = ThisAddIn.conInfo.urls.custom_domain & "/secur/frontdoor.jsp?sid=" & ThisAddIn.accessToken & "&retURL=/lightning/page/home"
            Dim fullUrl As String = ThisAddIn.conInfo.urls.custom_domain
            response.Redirect(fullUrl)
            response.Close()
        Catch ex As HttpListenerException
            Throw New Exception("processRequests Exception" & vbCrLf & ex.Message)
        Finally
            ' Stop listening for requests.
            listener.Close()
        End Try
    End Sub

    Sub requestAccessToken(ByVal options As String())
        Dim authcode As String = ""
        Dim display As String = ""
        For Each opt As String In options
            Dim seperator() As Char = {"="}
            Dim param() As String = opt.Split(seperator, 2)
            Select Case param(0)
                Case "code"
                    authcode = param(1)
                Case "display"
                    display = param(1)
            End Select
        Next

        Dim URL As String = String.Format(access_token_url, targetDomain)

        Dim values = New Dictionary(Of String, String) From {
            {"grant_type", "authorization_code"},
            {"code", authcode},
            {"client_id", client_id},
            {"client_secret", client_secret},
            {"redirect_uri", callback_url}
        }

        Dim client As HttpClient = New HttpClient()
        client.BaseAddress = New Uri(URL)
        client.DefaultRequestHeaders.Clear()
        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json")
        client.Timeout = New TimeSpan(0, 0, 120)

        Dim content = New FormUrlEncodedContent(values)

        Try
            Dim response = client.PostAsync(URL, content).Result

            If response.IsSuccessStatusCode Then

                Try
                    Dim jsonString As String = response.Content.ReadAsStringAsync().Result()
                    Dim jss As JavaScriptSerializer = New JavaScriptSerializer()
                    Dim dataObjects As Object = jss.DeserializeObject(jsonString)
                    ThisAddIn.accessToken = dataObjects("access_token")
                    ThisAddIn.instanceUrl = dataObjects("instance_url")
                    ThisAddIn.issuedAt = dataObjects("issued_at")
                    ThisAddIn.refreshToken = dataObjects("refresh_token")
                    ThisAddIn.tokenType = dataObjects("token_type")
                    ThisAddIn.accessToken = dataObjects("access_token")
                    ThisAddIn.id = dataObjects("id")

                Catch ex As Exception
                    MsgBox(ex.Message, Title:="Handle Response Exception")
                End Try
            Else
                MsgBox(response.StatusCode & " (" & response.ReasonPhrase & ") " & response.Content.ReadAsStringAsync().Result, Title:="Request AccessToken Failed!")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, Title:="Request AccessToken Exception")
        End Try
    End Sub

    Sub getSystemInfo()
        ThisAddIn.conInfo = RESTAPI.getConnectionInfo()
        ThisAddIn.userLang = ThisAddIn.conInfo.language
    End Sub


    ''' <summary>
    ''' Async OAuth Login Process and Callback
    '''  loginCallback() runtime timing issue!!!!!!!!!!!!!!!
    ''' </summary>
    ''' <param name="prefixes"></param>
    Sub processRequestAsync(ByVal prefixes As String())
        If Not HttpListener.IsSupported Then
            ErrorBox("This version of .Net Framework does not support Callback Listener!")
            Exit Sub
        End If

        ' URI prefixes are required,
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then Throw New ArgumentException("prefixes")

        ' Create a listener and add the prefixes.
        Dim listener As HttpListener = New HttpListener()
        For Each s As String In prefixes
            listener.Prefixes.Add(s)
        Next

        listener.Start()
        Dim result As IAsyncResult = listener.BeginGetContext(New AsyncCallback(AddressOf processRequestAsyncCallback), listener)
        ' Applications can do some work here while waiting for the 
        ' request. If no work can be done until you have processed a request,
        ' use a wait handle to prevent this thread from terminating
        ' while the asynchronous operation completes.
        Console.WriteLine("Waiting for request to be processed asyncronously.")
        result.AsyncWaitHandle.WaitOne()
        Console.WriteLine("Request processed asyncronously.")
        'listener.Close()
    End Sub

    Sub processRequestAsyncCallback(ByVal result As IAsyncResult)
        Dim listener As HttpListener = CType(result.AsyncState, HttpListener)
        Dim context As HttpListenerContext = listener.EndGetContext(result)
        Dim request As HttpListenerRequest = context.Request
        Dim response As HttpListenerResponse = context.Response

        Dim options() As String
        Dim callbackUri As String = request.Url.ToString()

        If callbackUri.StartsWith(callback_url, StringComparison.OrdinalIgnoreCase) Then
            'MessageBox.Show(callbackUri)
            options = callbackUri.Replace(callback_url & "?", "").Split("&")

            requestAccessToken(options)
        End If

        getSystemInfo()

        Dim fullUrl As String = ThisAddIn.conInfo.urls.custom_domain
        response.Redirect(fullUrl)
        response.Close()

        listener.Close()
    End Sub

End Module
