Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processDatabaseUpdateRowsNew

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

    Private Sub processDatabaseUpdateRowsNew_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        Dim totalRow As Long
        Dim totalCol As Long
        Dim updatedRow As Long

        Try
            excelApp = ThisAddIn.excelApp
            workbook = excelApp.ActiveWorkbook
            worksheet = workbook.ActiveSheet

            excelApp.StatusBar = "Update Selected Rows..."
            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(0, "Please wait for initialize...")

            If Not checkSession() Then
                statusText = "Session Failed!"
                GoTo errors
            End If
            If Not setDataRanges(excelApp, worksheet, g_table, g_start, g_header, g_body, g_objectType, g_ids, g_sfd, statusText) Then
                GoTo errors
            End If

            Dim fieldMap As Dictionary(Of String, RESTful.Field) = getFieldMap(g_sfd.fields)
            Dim recordSet As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            Dim records As List(Of Object) = New List(Of Object)

            Dim xlSelection As Excel.Range
            Dim xlRow As Excel.Range
            Dim xlColumn As Excel.Range

            Dim strArryCells() As String
            Dim strAddrStart As String = ""
            Dim strAddrEnd As String = ""
            Dim strStatBarText As String = ""
            Dim strMsg As String


            Dim intBatchPointer As Long
            Dim intJobPointer As Long
            Dim intFailedRows As Long
            Dim i As Integer

            Dim blnSkipHidden As Boolean
            Dim blnGoAhead As Boolean
            Dim blnNoLimits As Boolean

            '// check global options
            blnGoAhead = RegQueryBoolValue(GOAHEAD)
            blnNoLimits = RegQueryBoolValue(NOLIMITS)
            'blnSkipHidden = RegQueryBoolValue(SKIPHIDDEN)   '** new option setting?
            blnSkipHidden = True

            '// check the selected range
            excelApp.Intersect(excelApp.Selection, g_body).Select()
            xlSelection = excelApp.Selection
            If xlSelection Is Nothing Then
                statusText = "Nothing was selected"
                GoTo errors
            End If
            If xlSelection.Areas.Count > 1 Then
                statusText = "You can't process multiple areas."
                GoTo errors
            End If

            calcUpdateRange(xlSelection, totalRow, totalCol, blnSkipHidden, blnNoLimits)

            '// have we set the "Skip warning dialogs" option?
            If Not blnGoAhead Then
                strMsg = "You are about to update " & totalRow.ToString("N0") & " row(s) with " & totalCol.ToString("N0") & " column(s)." &
                vbCrLf & vbCrLf & "Do you want to proceed?"

                Dim result As DialogResult = TopMostMessageBox.Show("Update Information", strMsg, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                If result = DialogResult.Cancel Then
                    statusText = "Cancel Update"
                    GoTo cancel
                End If
            End If

            setControlText(btnAction, "Cancel")
            setControlStatus(btnAction, True)

            '// dimension the id and value arrays
            If totalRow < maxBatchSize Then
                ReDim strArryCells(totalRow - 1)
            Else
                ReDim strArryCells(maxBatchSize - 1)
            End If

            For Each xlRow In xlSelection.Rows
                ' initialize dictionary
                recordSet = New Dictionary(Of String, Object)

                If intBatchPointer = 0 Then
                    '// recreate for each new batch
                    Dim workedRow As Long = IIf(totalRow - intJobPointer > maxBatchSize, intJobPointer + maxBatchSize, totalRow)
                    strStatBarText = intJobPointer + 1 & " -> " & workedRow.ToString("N0") & " of " & totalRow.ToString("N0")
                    Dim percent As Integer = CInt((workedRow / totalRow) * 100)
                    bgw.ReportProgress(percent, "Updating: " & strStatBarText)
                End If

                If Not (xlRow.Hidden And blnSkipHidden) Then
                    '// fill elements of the batch; ignore hidden fields if required
                    recordSet.Add("attributes", New RESTful.Attributes(g_objectType))
                    recordSet.Add("Id", FixID(excelApp.Intersect(g_ids, xlRow.EntireRow).Value))
                    i = 0
                    For Each xlColumn In xlRow.Columns
                        If Not (xlColumn.Hidden And blnSkipHidden) Then
                            Dim fld As String = getAPINameFromCell(excelApp.Intersect(g_header, xlColumn.EntireColumn))
                            Dim field As RESTful.Field = fieldMap.Item(fld)
                            If Not field.updateable Then GoTo nextrow

                            If Len(strAddrStart) = 0 Then strAddrStart = xlColumn.Address
                            strAddrEnd = xlColumn.Address
                            xlColumn.Interior.ColorIndex = 36

                            recordSet.Add(fld, toVBtype(xlColumn, field))
nextrow:
                            i = i + 1
                        End If
                    Next xlColumn

                    ' if recordSet does not contains any updatable columns, cancel update
                    If records.Count = 0 And recordSet.Count = 2 Then
                        statusText = "No updatable columns selected, operation canceled."
                        GoTo errors
                    End If

                    records.Add(recordSet)
                    strArryCells(intBatchPointer) = strAddrStart & ":" & strAddrEnd
                    strAddrStart = ""
                    intBatchPointer = intBatchPointer + 1
                    intJobPointer = intJobPointer + 1
                    updatedRow = updatedRow + 1
                End If

                If intBatchPointer = maxBatchSize Or intJobPointer = totalRow Then
                    updateResultHandlerNew(worksheet, intFailedRows, records, strArryCells)

                    '//reinitialize for next batch
                    If totalRow - intJobPointer < maxBatchSize And totalRow - intJobPointer <> 0 Then '6.13
                        ReDim strArryCells(totalRow - intJobPointer - 1)
                    End If
                    strAddrStart = ""
                    strAddrEnd = ""
                    intBatchPointer = 0
                    records = New List(Of Object)
                End If
            Next xlRow

            '' any cleanup code go here
            '' ensure that you close all open resources before exitting out of this Method.
            '' try to skip off whatever is not desperately necessary if CancellationPending is True
            bgw.ReportProgress(100, "Complete.")
            '' set the e.Cancel to True to indicate to the RunWorkerCompleted that you cancelled out
            If bgw.CancellationPending Then
                e.Cancel = True
                bgw.ReportProgress(100, "Cancelled.")
            End If

            If intFailedRows > 0 Then
                strMsg = intFailedRows.ToString("N0") & " of " & totalRow.ToString("N0") &
                " rows failed to update!" & vbCrLf & "Please check the comments" &
                " in the first cell of each highlighted row for details"
                TopMostMessageBox.Show("Warning", strMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            GoTo done
        Catch ex As Exception
            statusText = ex.Message
            GoTo errors
        End Try

cancel:
        statusText = "Update Canceled!"
        e.Cancel = True
        GoTo done
errors:
        If statusText <> "" Then
            ErrorBox(statusText)
            statusText = "UpdateData Error!"
            e.Cancel = True
        End If
done:
        excelApp.StatusBar = "Update : update complete, " & updatedRow & " total rows returned"
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
                lblMessage.Text = "Update Cancelled!"
            End If

        Else
            '' otherwise it completed normally
            'MessageBox.Show("Download completed!")
            lblMessage.Text = "Update completed!"
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