Imports ForceConnector.MiniMETA
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processCustomLabelTranslationUpload

    Dim numOfCustomLabel As Long

    Dim statusText As String = ""

    Dim excelApp As Excel.Application
    Dim workbook As Excel.Workbook
    Dim worksheet As Excel.Worksheet

    Dim start As Excel.Range

    Dim m_table As Excel.Range
    Dim m_start As Excel.Range
    Dim m_head As Excel.Range
    Dim m_body As Excel.Range

    Dim m_rows As Integer ' row pointer to append new value at bottom of list
    Dim m_metaType As String

    Dim m_langSet As List(Of String) = New List(Of String)

    Private Sub processCustomLabelTranslationUpload_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            lblMessage.Font = New Drawing.Font(lblMessage.Font, Drawing.FontStyle.Regular)
            lblMessage.ForeColor = Drawing.Color.Black
            statusText = ""

            If Not setMetaBinding() Then
                statusText = "Session Failed"
                GoTo errors
            End If

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

        Try
            excelApp = ThisAddIn.excelApp
            workbook = excelApp.ActiveWorkbook
            excelApp.StatusBar = "Upload CustomLabel Translation metadata..."
            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(0, "Please wait for initialize...")

            getTranslations(m_langSet)
            If m_langSet.Count = 0 Then
                statusText = "No translation setting in ORG."
                GoTo errors
            End If

            '' Call initialize process...
            worksheet = workbook.ActiveSheet
            If worksheet.Name = "CustomLabel" Then
                setWorkArea(excelApp, worksheet, m_table, m_head, m_body, m_start, m_rows, m_metaType)

                If Not RegQueryBoolValue(GOAHEAD) Then
                    Dim msg As String = "You are about to UPLOAD: " & CStr(excelApp.Selection.Rows.Count) & " CustomLabel Translation " &
                    " metadata(s) in to Salesforce.com" & vbCrLf
                    If (MsgBox(msg, vbApplicationModal + vbOKCancel + vbExclamation + vbDefaultButton1,
                        "-- Ready to Upload CustomLabel Translation --") = vbCancel) Then
                        statusText = "Upload canceled!"
                        GoTo errors
                    End If
                End If

                setControlText(btnAction, "Cancel")
                setControlStatus(btnAction, True)
            Else
                statusText = "Current worksheet is not CustomLabel!"
                GoTo errors
            End If

            Dim clts As List(Of MiniMETA.CustomLabelTranslation) = New List(Of MiniMETA.CustomLabelTranslation)
            Dim todo As Excel.Range = excelApp.Selection
            If todo.Columns.Count > 1 Then
                statusText = "Only one language can upload!"
                GoTo errors
            End If
            Dim langCell As Excel.Range = excelApp.Intersect(todo.Cells(1, 1).EntireColumn, m_head)
            If langCell Is Nothing Then
                statusText = "Could not find the translatable language"
                GoTo errors
            End If
            If Not m_langSet.Contains(langCell.Value) Then
                statusText = "Select area's language does not supported!"
                GoTo errors
            End If

            Dim rw As Excel.Range
            Dim labelCount As Integer = 1
            Dim numOfLabel As Integer = todo.Rows.Count
            For Each rw In todo.Rows
                Dim nameCell As Excel.Range = excelApp.Intersect(m_body.Cells(1, 1).EntireColumn, rw.EntireRow)
                Dim clt As MiniMETA.CustomLabelTranslation = New MiniMETA.CustomLabelTranslation

                clt.name = nameCell.Value
                clt.label = rw.Value
                clts.Add(clt)

                Dim percent As Integer = CInt(80 * (labelCount / numOfLabel))
                labelCount = labelCount + 1
                bgw.ReportProgress(percent, "Build translations ... " & labelCount.ToString & " / " & numOfLabel.ToString())

                '' check at regular intervals for CancellationPending
                If bgw.CancellationPending Then
                    bgw.ReportProgress(percent, "Cancelling...")
                    Exit For
                End If
            Next

            Dim metas As List(Of MiniMETA.Translations) = New List(Of MiniMETA.Translations)
            Dim meta As MiniMETA.Translations = New MiniMETA.Translations
            meta.fullName = langCell.Value
            meta.customLabels = clts.ToArray
            metas.Add(meta)

            bgw.ReportProgress(90, "Update translation...")

            Dim srs() As MiniMETA.SaveResult = updateMetadata(metas.ToArray)
            excelApp.ScreenUpdating = False

            For Each sr As MiniMETA.SaveResult In srs
                If Not sr.success Then
                    Dim msg As String = ""
                    For Each err As MiniMETA.Error In sr.errors
                        msg = msg & vbCrLf & err.message
                    Next

                    MsgBox(msg, Title:=sr.fullName & " Error")
                End If
            Next sr

            '' any cleanup code go here
            '' ensure that you close all open resources before exitting out of this Method.
            '' try to skip off whatever is not desperately necessary if CancellationPending is True
            srs = Nothing

            excelApp.ScreenUpdating = True
            bgw.ReportProgress(100, "Done.")

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

errors:
        If statusText <> "" Then
            'bgw.CancelAsync()
            e.Cancel = True
        End If
done:
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
                lblMessage.Text = "Upload Cancelled!"
            End If
        Else
            '' otherwise it completed normally
            'MessageBox.Show("Download completed!")
            lblMessage.Text = "Upload completed!"
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