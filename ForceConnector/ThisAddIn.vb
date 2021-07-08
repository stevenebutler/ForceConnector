Imports System.Net
Imports System.Windows.Forms
Imports ForceConnector.Partner
Imports ForceConnector.MiniMETA

Public Class ThisAddIn
    Public Const ribbonBoxName As String = "Force.com Connector Next Generation"
    Public Const Ver As String = "2021/06/01 R0.8"

    Public Const USERNAME As String = "Username"
    Public Const AUTHTARGET As String = "Target"

    Public Shared api As Double = 51.0
    Public Shared userLang As String = ""
    Public Shared soapClient As SoapClient
    Public Shared metaClient As MetadataPortTypeClient
    Public Shared soapLoginScopeHeader As Partner.LoginScopeHeader = New Partner.LoginScopeHeader
    Public Shared soapCallOptions As Partner.CallOptions = New CallOptions
    Public Shared soapSessionHeader As Partner.SessionHeader = New Partner.SessionHeader
    Public Shared metaSessionHeader As MiniMETA.SessionHeader = New MiniMETA.SessionHeader

    Public Shared excelApp As Excel.Application
    Public Shared isBreak As Boolean = False

    Public Shared usingRESTful As Boolean = True

    ' Salesforce session properties for RESTful API
    ' frmLogin form refer the these properties
    ' Do not use for SOAP API, but remain for reference
    Public Shared loginType As String
    Public Shared accessToken As String
    Public Shared refreshToken As String
    Public Shared tokenType As String
    Public Shared issuedAt As String
    Public Shared id As String
    Public Shared instanceUrl As String
    Public Shared conInfo As RESTful.ConnectionInfo

    Public Shared UserNames As Dictionary(Of String, String)
    Public Shared RecordTypes As Dictionary(Of String, String)
    Public Shared Profiles As Dictionary(Of String, String)
    Public Shared Roles As Dictionary(Of String, String)
    Public Shared Groups As Dictionary(Of String, String)

    Private interopApi As SalesForceAddInApi

    Protected Overrides Function RequestComAddInAutomationService() As Object
        If interopApi Is Nothing Then
            interopApi = New SalesForceAddInApi
        End If
        Return interopApi
    End Function

    Private Sub ThisAddIn_Startup() Handles Me.Startup
        Globals.Ribbons.ForceRibbon.ribbonForceConnector.Label = ribbonBoxName & " (no logon user)"
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        excelApp = Globals.ThisAddIn.Application


        UserNames = New Dictionary(Of String, String)
        RecordTypes = New Dictionary(Of String, String)
        Profiles = New Dictionary(Of String, String)
        Roles = New Dictionary(Of String, String)
        Groups = New Dictionary(Of String, String)
    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
        accessToken = ""
        refreshToken = ""
        tokenType = ""
        issuedAt = ""
        id = ""
        instanceUrl = ""
        conInfo = Nothing

        UserNames = Nothing
        RecordTypes = Nothing
        Profiles = Nothing
        Roles = Nothing
        Groups = Nothing

        If checkSession() Then
            MessageBox.Show("Session alived, logout from Salesforce!")
            soapClient.logout(soapSessionHeader, soapCallOptions)
        End If
    End Sub
End Class
