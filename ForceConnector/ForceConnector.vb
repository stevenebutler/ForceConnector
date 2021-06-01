Imports System.Windows.Forms

Module ForceConnector

    ' Const from excel connector
    ' TODO make this batch size configurable via the options dialog box
    Public Const maxBatchSize As Integer = 50  ' used to size Update,Query Row, Create and Delete batches, it is not work with RESTful API. minimum is 200 for REST query
    '// limits for update batch sizes
    Public Const maxCols As Long = 20
    Public Const maxRows As Long = 3500

    Public Const excelLimit As Long = 1048570 ' Excel 2016's maximum rows id 1,048,576

    Public Const NOT_FOUND As String = "#N/F"

    Public Const USE_REFERENCE As String = "UseReference"
    Public Const GOAHEAD As String = "NoWarn"
    Public Const NOLIMITS As String = "NoLimit"
    Public Const AUTOASSIGNRULE As String = "AutoAssignRule"
    Public Const SKIPHIDDEN As String = "SkipHiddenCells"  '** allows us to use a special update routine
    Public Const GET_MANAGED As String = "GetManagedData"

    Public excelApp As Excel.Application
    Public workbook As Excel.Workbook
    Public worksheet As Excel.Worksheet

    Public op As String
    Dim logined As Boolean = False

    Public Sub setActiveSheet()
        excelApp = ThisAddIn.excelApp
        workbook = excelApp.ActiveWorkbook
        worksheet = workbook.ActiveSheet
    End Sub

    Function LoginToSalesforce() As Boolean
        LoginToSalesforce = False
        Dim loginForm As frmLogin = New frmLogin
        loginForm.ShowDialog()

        LoginToSalesforce = loginForm.getSuccess()
        loginForm.Dispose()
    End Function

    Sub OpenAbout()
        Dim aboutBox As frmAbout = New frmAbout()
        aboutBox.ShowDialog()
    End Sub

    Sub QueryTableWizard()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then QueryWizard()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="QueryTableWizard Exception")
        End Try
    End Sub

    Sub UpdateSelectedCells()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then UpdateCells()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="UpdateSelectedCells Exception")
        End Try
    End Sub

    Sub InsertSelectedRows()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then InsertRows()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="InsertSelectedRows Exception")
        End Try
    End Sub

    Sub QuerySelectedRows()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then QueryRows()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="QuerySelectedRows Exception")
        End Try
    End Sub

    Sub DescribeSforceObject()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then
                'If ThisAddIn.usingRESTful Then
                '    Call DescribeSalesforceObjectsByREST()
                'Else
                Call DescribeSalesforceObjectsBySOAP()
                'End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="DescribeSforceObject Exception")
        End Try
    End Sub

    Sub QueryTableData()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then QueryData()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="QueryTableData Exception")
        End Try
    End Sub

    Sub DeleteSelectedRecords()
        Try
            If Not checkSession() Then
                If Not LoginToSalesforce() Then Throw New Exception("Login failed!")
            End If

            If checkSession() Then Operation.DeleteRecords()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="DeleteSelectedRecords Exception")
        End Try
    End Sub

    Sub OptionsForm()
        Try
            Dim optionBox As frmOption = New frmOption()
            optionBox.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="OptionsForm Exception")
        End Try
    End Sub

    Sub LogoutFrom()
        If checkSession() Then
            MessageBox.Show("Session alived, logout from Salesforce!")
            ThisAddIn.soapClient.logout(ThisAddIn.soapSessionHeader, ThisAddIn.soapCallOptions)
            ThisAddIn.soapClient = Nothing
            ThisAddIn.metaClient = Nothing
            ThisAddIn.accessToken = ""
            ThisAddIn.instanceUrl = ""

            Call displayUserName("no logon user")
        End If
    End Sub
End Module
