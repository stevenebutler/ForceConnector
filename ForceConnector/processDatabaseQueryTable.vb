Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processDatabaseQueryTable

    Dim statusText As String = ""

    Dim excelApp As Excel.Application
    Dim workbook As Excel.Workbook
    Dim worksheet As Excel.Worksheet

    Dim g_objectType As String                  ' current entity table, ie "Account"
    Dim g_sfd As RESTful.DescribeSObjectResult  ' global describe for current table, not really needed since toolkit caches
    Dim g_table As Excel.Range                  ' all current region, with table name and header row
    Dim g_header As Excel.Range                 ' row with column labels
    Dim g_body As Excel.Range                   ' area with data, just below the header row
    Dim g_ids As Excel.Range                    ' column with salesforce ID's
    Dim g_start As Excel.Range                  ' some globals to hold info about the region we are working on

    Private Sub processDatabaseQueryTable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            lblMessage.Font = New Drawing.Font(lblMessage.Font, Drawing.FontStyle.Regular)
            lblMessage.ForeColor = Drawing.Color.Black
            statusText = ""

            '' these properties should be set to True (at design-time or runtime) before calling the RunWorkerAsync
            '' to ensure that it supports Cancellation and reporting Progress
            bgw.WorkerSupportsCancellation = True
            bgw.WorkerReportsProgress = True

            '' call this method to start your asynchronous Task.
            bgw.RunWorkerAsync()

            GoTo done
        Catch ex As Exception
            statusText = ex.Message
        End Try

errors:
        If statusText <> "" Then
            progressDownload.Value = 100
            lblMessage.Font = New Drawing.Font(lblMessage.Font, Drawing.FontStyle.Bold)
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = statusText
            btnAction.Text = "Done"
            btnAction.Enabled = True
        End If
done:
    End Sub

    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        '' to cancel the task, just call the BackgroundWorker.CancelAsync method.
        If btnAction.Text = "Cancel" Then
            bgw.CancelAsync()
        ElseIf btnAction.Text = "Done" Then
            MyBase.Close()
        End If
    End Sub

    Private Sub bgw_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgw.DoWork
        '' The asynchronous task you want to perform goes here
        '' the following is an example of how it typically goes.
        Dim outrow As Long = 0L
        Try
            excelApp = ThisAddIn.excelApp
            workbook = excelApp.ActiveWorkbook
            worksheet = workbook.ActiveSheet

            excelApp.StatusBar = "Query Database Table..."
            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(0, "Please wait for initialize...")

            If Not checkSession() Then
                statusText = "Session Failed!"
                GoTo errors
            End If
            If Not setDataRanges(excelApp, worksheet, g_table, g_start, g_header, g_body, g_objectType, g_ids, g_sfd, statusText) Then
                GoTo errors
            End If

            Dim where As String = "", sels As String = ""
            Dim refIds As Excel.Range = Nothing, joinfield As String = ""
            Dim oneeachrow As Boolean
            Dim fieldMap As Dictionary(Of String, RESTful.Field) = getFieldMap(g_sfd.fields)

            bgw.ReportProgress(0, "Build Query String...")

            If Not BuildQueryString(excelApp, g_table, g_start, g_header, refIds, joinfield, oneeachrow, fieldMap, sels, where, statusText) Then
                GoTo errors
            End If

            '  Debug.Print "select " & sels & " from " & g_objectType
            '  Debug.Print " " & where

            ' to support join, if we saw a "reference in range" we need to loop over this,
            ' otherwise call once for a normal query
            outrow = 1 ' the row within g_body where output begins
            If refIds IsNot Nothing Then
                ' TODO this could be optimized to pull multiple ref's at one query call
                ' should speed things up on large joins there is lots of overhead now

                Dim c As Excel.Range, tmp As String
                For Each c In refIds.Cells ' loop over a range to output a join
                    tmp = where
                    If (where <> "" And Strings.Right(where, 4) <> "AND ") Then tmp = where & " AND " ' 5.56
                    where = tmp & joinfield & " = '" & c.Value & "'" ' use the ID from the reference colum in each query
                Next c
            End If

            Dim qr As RESTful.QueryResult
            qr = RESTAPI.Query("SELECT COUNT(Id) ROWCOUNT FROM " & g_objectType & " WHERE " & where)
            Dim total As Object = qr.records(0)
            Dim rowCount As Long = CLng(total.Item("ROWCOUNT"))
            If rowCount > excelLimit Then
                statusText = "Number of records exceed the limit of Excel, cancel the download."
                GoTo errors
            End If

            setControlText(btnAction, "Cancel")
            setControlStatus(btnAction, True)

            Dim msg As String = "You try to download " & rowCount.ToString("N0") & " records. Are you sure?"
            Dim result As DialogResult = TopMostMessageBox.Show("Query Information", msg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
            If result = DialogResult.Cancel Then
                statusText = "Cancel Query"
                GoTo cancel
            End If

            outrow = queryDataDraw(excelApp, worksheet, g_header, g_body, g_ids, g_objectType, g_sfd, sels, " WHERE " & where, outrow, rowCount, bgw)
            If (outrow <= 1) Then setControlText(lblMessage, "No data returned for this Query")
            refIds = Nothing

            '' any cleanup code go here
            '' ensure that you close all open resources before exitting out of this Method.
            '' try to skip off whatever is not desperately necessary if CancellationPending is True
            bgw.ReportProgress(100, "Complete.")
            '' set the e.Cancel to True to indicate to the RunWorkerCompleted that you cancelled out
            If bgw.CancellationPending Then
                e.Cancel = True
                bgw.ReportProgress(100, "Cancelled.")
            End If

            GoTo done
        Catch ex As Exception
            statusText = ex.Message
            GoTo errors
        End Try

cancel:
        statusText = "Query Canceled!"
        e.Cancel = True
        GoTo done
errors:
        If statusText <> "" Then
            ErrorBox(statusText)
            statusText = "QueryData Error!"
            e.Cancel = True
        End If
done:
        excelApp.StatusBar = "Query : drawing complete, " & outrow - 1 & " total rows returned"
        excelApp.ScreenUpdating = True
    End Sub

    Private Sub bgw_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bgw.ProgressChanged
        '' This event is fired when you call the ReportProgress method from inside your DoWork.
        '' Any visual indicators about the progress should go here.
        progressDownload.Value = CInt(e.ProgressPercentage)
        lblMessage.Text = e.UserState
    End Sub

    Private Sub bgw_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bgw.RunWorkerCompleted
        '' This event is fired when your BackgroundWorker exits.
        '' It may have exitted Normally after completing its task, 
        '' or because of Cancellation, or due to any Error.

        If e.Error IsNot Nothing Then
            '' if BackgroundWorker terminated due to error
            'MessageBox.Show(e.Error.Message)
            lblMessage.Font = New Drawing.Font(lblMessage.Font, Drawing.FontStyle.Bold)
            lblMessage.ForeColor = Drawing.Color.Red
            lblMessage.Text = "Error occurred!" & e.Error.Message

        ElseIf e.Cancelled Then
            '' otherwise if it was cancelled
            'MessageBox.Show("Download cancelled!")
            If statusText <> "" Then
                lblMessage.Font = New Drawing.Font(lblMessage.Font, Drawing.FontStyle.Bold)
                lblMessage.ForeColor = Drawing.Color.Red
                lblMessage.Text = statusText
            Else
                lblMessage.Text = "Download Cancelled!"
            End If

        Else
            '' otherwise it completed normally
            'MessageBox.Show("Download completed!")
            lblMessage.Text = "Download completed!"
        End If

        btnAction.Text = "Done"
        btnAction.Enabled = True
    End Sub

    '******************************************************
    '* Change controls label in the bgw.DoWork()
    '* ctl -> label, button ...etc
    '******************************************************
    Private Sub setControlText(ByVal ctl As Control, ByVal text As String)
        If ctl.InvokeRequired Then
            ctl.Invoke(New setControlTextInvoker(AddressOf setControlText), ctl, text)
        Else
            ctl.Text = text
        End If
    End Sub
    Private Delegate Sub setControlTextInvoker(ByVal ctl As Control, ByVal text As String)

    Private Sub setControlStatus(ByVal ctl As Control, ByVal enabled As Boolean)
        If ctl.InvokeRequired Then
            ctl.Invoke(New setControlStatusInvoker(AddressOf setControlStatus), ctl, enabled)
        Else
            ctl.Enabled = enabled
        End If
    End Sub
    Private Delegate Sub setControlStatusInvoker(ByVal ctl As Control, ByVal enabled As Boolean)
End Class