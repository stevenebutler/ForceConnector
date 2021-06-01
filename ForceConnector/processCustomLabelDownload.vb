Imports ForceConnector.MiniMETA
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processCustomLabelDownload

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

    Private Sub processCustomLabelDownload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Try
            excelApp = ThisAddIn.excelApp
            workbook = excelApp.ActiveWorkbook
            excelApp.StatusBar = "Download CustomLabel metadata..."
            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(10, "Please wait for initialize...")

            '' Call initialize process...
            m_files = listMetadata({"CustomLabel"})
            If m_files IsNot Nothing Then
                numOfCustomLabel = m_files.Length

                worksheet = getMetaWorkSheet(workbook, "CustomLabel")
                setCustomLabelLayout(worksheet, start)
                setWorkArea(excelApp, worksheet, m_table, m_head, m_body, m_start, m_rows, m_metaType)

                setControlText(btnAction, "Cancel")
                setControlStatus(btnAction, True)
            Else
                statusText = "No CustomLabel Data!"
                GoTo errors
            End If

            '' insert check rutine for zero return.
            Dim i As Long = 1L
            numOfCustomLabel = CLng(m_files.Length)

            Dim fullNames As IEnumerable(Of FileProperties) = m_files.OrderBy(Function(m_file) m_file.fullName)
            For Each m_file As FileProperties In fullNames
                m_body.Cells(i, 1).value = m_file.fullName

                ' change process bar
                Dim percent = CInt(90 * i / numOfCustomLabel) + 10
                bgw.ReportProgress(percent, "Set CustomLabels... " & i.ToString() & " / " & numOfCustomLabel.ToString())
                i = i + 1L
            Next

            bgw.ReportProgress(0, "Start query for CustomLabel...")

            m_body = m_body.Resize(i, m_head.Columns.Count)
            m_body.Select()

            ' Query CustomLabel
            Dim row_pointer As Long = excelApp.Selection.row
            Dim row_counter As Long = 0L
            Dim chunk As Excel.Range
            Do
                chunk = excelApp.Intersect(excelApp.Selection, worksheet.Rows(row_pointer))
                If chunk Is Nothing Then Exit Do
                chunk = chunk.Resize(10) ' max size is 10 for read metadata
                chunk = excelApp.Intersect(excelApp.Selection, chunk) ' trim the last chunk !
                row_pointer = row_pointer + chunk.Rows.Count ' up our pos counter
                row_counter = row_counter + chunk.Rows.Count
                'row_pointer = row_pointer + 10 ' up our pos counter
                'row_counter = row_counter + 10
                chunk.Interior.ColorIndex = 36 ' show off...
                excelApp.ScreenUpdating = False

                If Not queryCustomLabel(excelApp, chunk) Then GoTo done ' do it

                excelApp.ScreenUpdating = True
                chunk.Interior.ColorIndex = 0

                ' change process bar
                Dim percent = CInt(100 * row_counter / numOfCustomLabel)
                bgw.ReportProgress(percent, "Query CustomLabel... " & row_counter.ToString & " / " & numOfCustomLabel.ToString())

                '' check at regular intervals for CancellationPending
                If bgw.CancellationPending Then
                    bgw.ReportProgress(percent, "Cancelling...")
                    Exit Do
                End If

            Loop Until chunk Is Nothing

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

        m_files = Nothing
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