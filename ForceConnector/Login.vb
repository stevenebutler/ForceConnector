Imports ForceConnector.Partner
Imports ForceConnector.MiniMETA
Imports System.Diagnostics
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Description

Public Class frmLogin
    Public Shared success As Boolean = False

    Dim soapClient As SoapClient
    Dim metaClient As MetadataPortTypeClient
    Dim soapSessionHeader As Partner.SessionHeader = New Partner.SessionHeader
    Dim metaSessionHeader As MiniMETA.SessionHeader = New MiniMETA.SessionHeader
    Dim loginRes As LoginResult

    Private Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ThisAddIn.loginType = "oauth"

    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim loginType As String = ThisAddIn.loginType
        Dim target As String = cmbDestination.Text
        Dim destination As String = "login"

        responseBox.Text = ""

        If target = "Sandbox" Then
            destination = "test"
        End If

        If loginType = "soap" Then
            btnLogin.Enabled = False
            Me.soapLogin(destination)
        Else
            btnLogin.Enabled = False
            openOAuthLogin(destination)

            loginCallback()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        ThisAddIn.conInfo = RESTAPI.getConnectionInfo()
        ThisAddIn.api = CDbl(cmbVersion.Text)
        Call displayUserName(ThisAddIn.conInfo.display_name)

        Close()
    End Sub

    Private Sub soapLogin(ByVal destination As String)
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text
        Dim security As String = txtSecurityToken.Text
        Dim version As String = cmbVersion.Text
        Dim passcode As String = password & security
        Dim endpointUrl As String
        Dim userInfo As GetUserInfoResult = New GetUserInfoResult

        endpointUrl = "https://" & destination & ".salesforce.com/services/Soap/u/" & version & ".0"

        If soapClient Is Nothing Then soapClient = New SoapClient("Soap", endpointUrl)

        Try
            loginRes = soapClient.login(ThisAddIn.soapLoginScopeHeader, ThisAddIn.soapCallOptions, username, passcode)
        Catch ex As Exception
            ' This is something else, probably comminication
            responseBox.Text = ex.Message
            'MsgBox(ex.Message, Title:="Exception")
            'Close()
        End Try
        'Change the binding to the new endpoint
        userInfo = loginRes.userInfo
        soapClient = New SoapClient("Soap", loginRes.serverUrl)
        metaClient = New MetadataPortTypeClient("Metadata", loginRes.metadataServerUrl)
        'Create a new session header object and set the session id to that returned by the login
        soapSessionHeader.sessionId = loginRes.sessionId
        metaSessionHeader.sessionId = loginRes.sessionId

        ThisAddIn.api = CDbl(version)
        ThisAddIn.userLang = userInfo.userLanguage
        ThisAddIn.soapClient = soapClient
        ThisAddIn.metaClient = metaClient
        ThisAddIn.soapSessionHeader = soapSessionHeader
        ThisAddIn.metaSessionHeader = metaSessionHeader
        ThisAddIn.accessToken = loginRes.sessionId
        ThisAddIn.instanceUrl = loginRes.serverUrl.Substring(0, loginRes.serverUrl.IndexOf("/services"))
        ThisAddIn.id = "https://" & destination & ".salesforce.com/id/" & userInfo.organizationId & "/" & userInfo.userId

        responseBox.Text = "Login Success!"
        success = True
        btnNext.Enabled = True
    End Sub

    Private Sub usingOAuth2_CheckedChanged(sender As Object, e As EventArgs) Handles usingOAuth2.CheckedChanged
        If usingOAuth2.Checked Then
            'lblVersion.Visible = False
            lblUsername.Visible = False
            lblPassword.Visible = False
            lblSecurityKey.Visible = False
            'cmbVersion.Visible = False
            txtUsername.Visible = False
            txtPassword.Visible = False
            txtSecurityToken.Visible = False

            'cmbDestination.Width = 240
            'responseBox.Top = 215
            'btnLogin.Top = 215

            ThisAddIn.loginType = "oauth"
        End If
    End Sub

    Private Sub usingSOAP_CheckedChanged(sender As Object, e As EventArgs) Handles usingSOAP.CheckedChanged
        If usingSOAP.Checked Then
            'lblVersion.Visible = True
            lblUsername.Visible = True
            lblPassword.Visible = True
            lblSecurityKey.Visible = True
            'cmbVersion.Visible = True
            txtUsername.Visible = True
            txtPassword.Visible = True
            txtSecurityToken.Visible = True

            'cmbDestination.Width = 100
            'responseBox.Top = 215
            'btnLogin.Top = 215

            ThisAddIn.loginType = "soap"
        End If

    End Sub

    Private Sub RESTFULAPI_CheckedChanged(sender As Object, e As EventArgs) Handles RESTFULAPI.CheckedChanged
        If RESTFULAPI.Checked Then
            ThisAddIn.usingRESTful = vbTrue
        End If
    End Sub

    Private Sub SOAPAPI_CheckedChanged(sender As Object, e As EventArgs) Handles SOAPAPI.CheckedChanged
        If SOAPAPI.Checked Then
            ThisAddIn.usingRESTful = vbFalse
        End If
    End Sub

    ' Code for OAuth Login
    ' Login callback function to handle ASync OAUTH login process
    Public Sub loginCallback()
        Try
            soapClient = New SoapClient("Soap", ThisAddIn.conInfo.urls.partner)
            metaClient = New MetadataPortTypeClient("Metadata", ThisAddIn.conInfo.urls.metadata)

            soapSessionHeader.sessionId = ThisAddIn.accessToken
            metaSessionHeader.sessionId = ThisAddIn.accessToken

            ThisAddIn.soapClient = soapClient
            ThisAddIn.metaClient = metaClient
            ThisAddIn.soapSessionHeader = soapSessionHeader
            ThisAddIn.metaSessionHeader = metaSessionHeader

            responseBox.Text = "Login Successful!"
            success = True

        Catch ex As Exception
            responseBox.Text = "Logined, but can not use SOAP features"
            MsgBox("You can not use 'Translation Helper' at this time" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, Title:="SOAP Client Error!")
        End Try

        btnNext.Enabled = True
    End Sub

    Public Function getSuccess() As Boolean
        Return success
    End Function
End Class