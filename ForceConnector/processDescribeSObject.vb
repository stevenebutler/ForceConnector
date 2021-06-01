Imports System.ComponentModel
Imports System.Windows.Forms

Public Class processDescribeSObject

    Public objectList() As String

    Dim statusText As String = ""

    Dim excelApp As Excel.Application
    Dim workbook As Excel.Workbook
    Dim worksheet As Excel.Worksheet

    Dim start As Excel.Range

    Dim namedFields() As String
    Dim standardFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
    Dim customFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)

    Private Sub processDescribeSObject_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Try
            excelApp = ThisAddIn.excelApp
            workbook = excelApp.ActiveWorkbook
            worksheet = workbook.ActiveSheet

            excelApp.StatusBar = "Describe SObjects..."
            setControlText(btnAction, "Wait...")
            bgw.ReportProgress(0, "Please wait for initialize...")

            Dim selectObjectsBox As frmObjectList = New frmObjectList()
            selectObjectsBox.ShowDialog()

            If selectObjectsBox.success And selectObjectsBox.objectList.Count > 0 Then
                objectList = selectObjectsBox.objectList.ToArray()

                setControlText(btnAction, "Cancel")
                setControlStatus(btnAction, True)
            Else
                statusText = "Describe error or no objects"
                GoTo errors
            End If

            namedFields = {"Id", "Name", "Subject", "CurrencyISOCode", "MasterRecordId", "CreatedById", "CreatedDate", "LastModifiedById",
                "LastModifiedDate", "IsDeleted", "SystemModstamp", "LastActivityDate", "LastViewedDate", "LastReferencedDate", "RecordTypeId", "OwnerId"}

            Dim numOfObject As Integer = objectList.Length
            Dim objectCount As Integer = 0
            Dim numOfPart As Integer = CInt(100 / numOfObject)
            Dim percent As Integer = 0

            For Each objname As String In objectList
                standardFields.Clear()
                customFields.Clear()

                bgw.ReportProgress(percent, "Describe " & objname & "... ")
                Dim gr As RESTful.DescribeSObjectResult = RESTAPI.DescribeSObject(objname)
                Dim fields() As RESTful.Field = gr.fields
                Dim numOfField As Integer = fields.Length
                Dim rowPointer As Integer = 0
                For Each fld As RESTful.Field In fields
                    If fld.custom Then
                        customFields.Add(fld.name, fld)
                    Else
                        standardFields.Add(fld.name, fld)
                    End If

                    '' check at regular intervals for CancellationPending
                    If bgw.CancellationPending Then
                        bgw.ReportProgress(percent, "Cancelling...")
                        Exit For
                    End If
                Next

                excelApp.ScreenUpdating = False
                DescribeObjects.setWorkSheet(excelApp, workbook, worksheet, objname)
                DescribeObjects.setLayout(worksheet, objname)
                DescribeObjects.renderHeader(worksheet, start, objname)
                rowPointer = DescribeObjects.renderNamedField(worksheet, start, standardFields, rowPointer)
                rowPointer = DescribeObjects.renderStandardField(worksheet, start, namedFields, standardFields, rowPointer,
                                                                 objectCount, numOfPart, numOfField, objname, bgw)
                DescribeObjects.renderCustomField(worksheet, start, namedFields, customFields, rowPointer,
                                                  objectCount, numOfPart, numOfField, objname, bgw)
                excelApp.ScreenUpdating = True

                '' check at regular intervals for CancellationPending
                If bgw.CancellationPending Then
                    bgw.ReportProgress(percent, "Cancelling...")
                    Exit For
                End If

                percent = CInt(numOfPart * (rowPointer / numOfField)) + (numOfPart * objectCount)
                objectCount = objectCount + 1
            Next

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