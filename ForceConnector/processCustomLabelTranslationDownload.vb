Imports ForceConnector.MiniMETA
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processCustomLabelTranslationDownload

    Dim numOfCustomLabel As Long

    Dim statusText As String = ""

    Dim m_files() As FileProperties

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

    Private Sub processCustomLabelTranslationDownload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
            excelApp.StatusBar = "Download CustomLabel Translation metadata..."

            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(0, "Please wait for initialize...")

            getTranslations(m_langSet)
            If m_langSet.Count = 0 Then
                statusText = "No translation setting in ORG."
                GoTo errors
            End If

            Dim find_sheet As Boolean = False
            For Each cs As Excel.Worksheet In workbook.Sheets
                If cs.Name = "CustomLabel" Then
                    find_sheet = True
                    worksheet = cs
                    worksheet.Activate()

                    Dim totalSheets As Integer = excelApp.ActiveWorkbook.Sheets.Count
                    CType(excelApp.ActiveSheet, Excel.Worksheet).Move(After:=excelApp.Worksheets(totalSheets))
                End If
            Next

            If find_sheet Then
                setWorkArea(excelApp, worksheet, m_table, m_head, m_body, m_start, m_rows, m_metaType)

                setControlText(btnAction, "Cancel")
                setControlStatus(btnAction, True)
            Else
                statusText = "No CustomLabels in workbook, download CustomLabels first and try again."
                GoTo errors
            End If

            Dim numOfLang As Integer = m_langSet.Count
            Dim langCount As Integer = 0

            bgw.ReportProgress(10, "Set Language Headers worksheet...")
            setLanguageHeaders(excelApp, worksheet, m_head, m_langSet)

            Dim metas() As MiniMETA.Metadata = readMetadata("Translations", m_langSet.ToArray())

            If metas.Length = 0 Then
                statusText = "No translation for CustomLabel"
                GoTo errors
            End If
            For Each meta In metas
                excelApp.ScreenUpdating = False
                Dim langCode As String = meta.fullName
                Dim trns As MiniMETA.Translations = CType(meta, MiniMETA.Translations)
                Dim m_langCol As Integer = getLanguageColumn(m_head, langCode)

                If trns.customLabels IsNot Nothing Then
                    Dim numOfTranslation As Integer = trns.customLabels.Length
                    Dim labelCount As Integer = 1
                    For Each cl As MiniMETA.CustomLabelTranslation In trns.customLabels
                        renderItem(excelApp, m_body, m_rows, m_langCol, cl.name, If(cl.label = Nothing, "<!-- " & cl.name & " -->", cl.label))

                        Dim percent As Integer = CInt(90 * (labelCount / (numOfTranslation * numOfLang)) + ((90 / numOfLang) * langCount)) + 10
                        If percent > 100 Then percent = 100

                        ' change process bar
                        bgw.ReportProgress(percent, "Query CustomLabel... (" & langCode & ") " & labelCount.ToString & " / " & numOfTranslation.ToString())

                        '' check at regular intervals for CancellationPending
                        If bgw.CancellationPending Then
                            bgw.ReportProgress(percent, "Cancelling...")
                            Exit For
                        End If
                        labelCount = labelCount + 1
                    Next

                    If bgw.CancellationPending Then Exit For
                End If

                langCount = langCount + 1
                excelApp.ScreenUpdating = True
            Next

            '' any cleanup code go here
            '' ensure that you close all open resources before exitting out of this Method.
            '' try to skip off whatever is not desperately necessary if CancellationPending is True

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