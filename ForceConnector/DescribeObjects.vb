Module DescribeObjects
    'Dim namedFields() As String
    'Dim standardFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
    'Dim customFields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
    'Dim start As Excel.Range

    Public Sub DescribeSalesforceObjectsByREST()
        Try
            Dim frm As processDescribeSObject = New processDescribeSObject()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="DescribeSObjects Exception")
        End Try

        ThisAddIn.excelApp.StatusBar = "Complete Describe SObject"
    End Sub

    Sub setWorkSheet(ByRef excelApp As Excel.Application, ByRef workbook As Excel.Workbook, ByRef worksheet As Excel.Worksheet,
                     ByVal objname As String, Optional clear As Boolean = True)
        Try
            Dim find_sheet As Boolean = False
            For Each cs As Excel.Worksheet In workbook.Sheets
                If cs.Name = objname Then
                    find_sheet = True
                    worksheet = cs
                    worksheet.Activate()

                    Dim totalSheets As Integer = excelApp.ActiveWorkbook.Sheets.Count
                    CType(excelApp.ActiveSheet, Excel.Worksheet).Move(After:=excelApp.Worksheets(totalSheets))

                    If clear Then
                        Dim allRange As Excel.Range = excelApp.ActiveCell.CurrentRegion
                        allRange.Select()
                        excelApp.Selection.Clear()
                    End If
                End If
            Next
            If Not find_sheet Then
                Dim newsheet As Excel.Worksheet
                newsheet = CType(excelApp.Worksheets.Add(), Excel.Worksheet)
                newsheet.Name = objname
                worksheet = newsheet
                worksheet.Activate()
            End If
            excelApp.ActiveWindow.DisplayGridlines = False

        Catch ex As Exception
            Throw New Exception("setWorkSheet Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub setLayout(ByRef worksheet As Excel.Worksheet, ByVal objname As String)
        ' columns width adjustment
        worksheet.Range("A1").ColumnWidth = 2
        worksheet.Range("B1:C1").ColumnWidth = 26 ' label, api name
        worksheet.Range("D1").ColumnWidth = 20 ' type
        worksheet.Range("E1:M1").ColumnWidth = 12 ' custom, autonumber, nillable, excrypted, extrenal id, length, digits, precision

        ' headline rendering
        Dim titleRange As Excel.Range = worksheet.Range("B1:M1")
        titleRange.Merge()
        titleRange.RowHeight = 26
        titleRange.Font.Size = 20
        titleRange.Font.Name = "Consolas"
        titleRange.Font.Bold = True
        titleRange.Value = objname
        titleRange.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
    End Sub

    Sub renderHeader(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByVal objname As String)
        Dim headerRow As Excel.Range = worksheet.Range("B3:M3")
        start = worksheet.Range("B4")
        ' label, api name, type, custom, autonumber, nillable, length, digits, precision, encrypted, externalId      referenceto, picklist -> comments of type
        headerRow.Font.Bold = True
        headerRow.Font.Name = "Vernada"
        headerRow.Font.ColorIndex = 2
        headerRow.HorizontalAlignment = Excel.Constants.xlCenter
        headerRow.VerticalAlignment = Excel.Constants.xlCenter
        headerRow.Interior.Color = RGB(0, 176, 240)
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
        worksheet.Range("B3").Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("M3").Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("B3").Value = "Label"
        worksheet.Range("C3").Value = "API Name"
        worksheet.Range("D3").Value = "Type"
        worksheet.Range("E3").Value = "Custom"
        worksheet.Range("F3").Value = "AutoNumber"
        worksheet.Range("G3").Value = "Nillable"
        worksheet.Range("H3").Value = "Encrypted"
        worksheet.Range("I3").Value = "External Id"
        worksheet.Range("J3").Value = "Length"
        worksheet.Range("K3").Value = "Scale"
        worksheet.Range("L3").Value = "Digits"
        worksheet.Range("M3").Value = "Precision"
    End Sub

    Function renderNamedField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range,
                              ByRef standardFields As Dictionary(Of String, RESTful.Field), ByVal rowPointer As Integer) As Integer

        If standardFields.ContainsKey("Id") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("Id"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("MasterRecordId") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("MasterRecordId"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("RecordTypeId") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("RecordTypeId"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("IsDeleted") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("IsDeleted"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("Name") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("Name"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("Subject") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("Subject"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CurrencyISOCode") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("CurrencyISOCode"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CreatedById") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("CreatedById"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CreatedDate") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("CreatedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastModifiedById") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("LastModifiedById"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastModifiedDate") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("LastModifiedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("SystemModstamp") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("SystemModstamp"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastActivityDate") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("LastActivityDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastViewedDate") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("LastViewedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastReferencedDate") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("LastReferencedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("OwnerId") Then
            renderField(worksheet, start, rowPointer, standardFields.Item("OwnerId"))
            rowPointer = rowPointer + 1
        End If

        Return rowPointer
    End Function

    Function renderStandardField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByRef namedFields() As String,
                                 ByRef standardFields As Dictionary(Of String, RESTful.Field), ByVal rowPointer As Integer,
                                 ByRef objectCount As Integer, ByRef numOfPart As Integer, numOfField As Integer, ByRef objname As String,
                                 ByRef bgw As ComponentModel.BackgroundWorker) As Integer
        Dim keys() As String = standardFields.Keys.ToArray()
        Array.Sort(keys)
        For Each key As String In keys
            If Not namedFields.Contains(key) Then
                Dim percent As Integer = CInt(numOfPart * (rowPointer / numOfField)) + (numOfPart * objectCount)
                bgw.ReportProgress(percent, "Describe " & objname & " (fields " & rowPointer.ToString() & " / " & numOfField.ToString() & ")")
                renderField(worksheet, start, rowPointer, standardFields.Item(key))
                rowPointer = rowPointer + 1
            End If
        Next
        Return rowPointer
    End Function

    Sub renderCustomField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByRef namedFields() As String,
                          ByRef customFields As Dictionary(Of String, RESTful.Field), ByVal rowPointer As Integer,
                          ByRef objectCount As Integer, ByRef numOfPart As Integer, numOfField As Integer, ByRef objname As String,
                          ByRef bgw As ComponentModel.BackgroundWorker)
        Dim keys() As String = customFields.Keys.ToArray()
        Array.Sort(keys)
        For Each key As String In keys
            If Not namedFields.Contains(key) Then
                Dim percent As Integer = CInt(numOfPart * (rowPointer / numOfField)) + (numOfPart * objectCount)
                bgw.ReportProgress(percent, "Describe " & objname & " (fields " & rowPointer.ToString("N0") & " / " & numOfField.ToString("N0") & ")")
                renderField(worksheet, start, rowPointer, customFields.Item(key))
                rowPointer = rowPointer + 1
            End If
        Next
    End Sub

    Sub renderField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByVal rownum As Integer, ByVal fld As RESTful.Field)
        Dim startCell As Excel.Range = start.Offset(rownum, 0)
        Dim dataRow As Excel.Range = worksheet.Range(startCell, startCell.Offset(0, 11))

        dataRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        dataRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
        dataRow.Font.Name = "Vernada"
        dataRow.Style.IndentLevel = 1
        dataRow.VerticalAlignment = Excel.Constants.xlCenter
        startCell.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        startCell.Offset(0, 11).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

        startCell.Value = fld.label
        startCell.Offset(0, 1).Value = fld.name
        startCell.Offset(0, 1).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        With startCell.Offset(0, 2)
            .Value = fld.type
            .Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
            If fld.type = "picklist" Or fld.type = "reference" Then
                .ClearComments()
                .AddComment()
                .Comment.Shape.TextFrame.AutoSize = True
                .Comment.Shape.TextFrame.Characters.Font.Bold = False
                .Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
                Dim comment As String = ""
                Dim i As Integer = 0
                If fld.type = "picklist" Then
                    comment = "Pickist Values :" & vbCrLf
                    Dim picklists() As RESTful.PicklistEntry = fld.picklistValues
                    For i = 0 To picklists.Length - 1
                        comment = comment & picklists(i).label & " (" & picklists(i).value & ")"
                        If i < picklists.Length - 1 Then comment = comment & vbCrLf
                    Next
                    .Comment.Text(comment)
                Else
                    comment = "Reference To :" & vbCrLf
                    Dim refs() As String = fld.referenceTo
                    For i = 0 To refs.Length - 1
                        comment = comment & refs(i)
                        If i < refs.Length - 1 Then comment = comment & vbCrLf
                    Next
                    .Comment.Text(comment)
                End If
            End If
        End With
        startCell.Offset(0, 3).Value = If(fld.custom, "Yes", "No")
        startCell.Offset(0, 3).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 4).Value = If(fld.autoNumber, "Yes", "No")
        startCell.Offset(0, 4).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 5).Value = If(fld.nillable, "Yes", "No")
        startCell.Offset(0, 5).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 6).Value = If(fld.encrypted, "Yes", "No")
        startCell.Offset(0, 6).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 7).Value = If(fld.externalId, "Yes", "No")
        startCell.Offset(0, 7).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 8).Value = fld.length
        startCell.Offset(0, 8).HorizontalAlignment = Excel.Constants.xlRight
        startCell.Offset(0, 8).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 9).Value = fld.scale
        startCell.Offset(0, 9).HorizontalAlignment = Excel.Constants.xlRight
        startCell.Offset(0, 9).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 10).Value = fld.digits
        startCell.Offset(0, 10).HorizontalAlignment = Excel.Constants.xlRight
        startCell.Offset(0, 10).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 11).Value = fld.precision
        startCell.Offset(0, 11).HorizontalAlignment = Excel.Constants.xlRight
        startCell.Offset(0, 11).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 11).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
    End Sub
End Module
