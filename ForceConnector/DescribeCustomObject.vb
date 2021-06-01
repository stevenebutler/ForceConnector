Module DescribeCustomObject

    Public langSet As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
        {"zh_CN", "Chinese(Simplified)"},
        {"zh_TW", "Chinese (Traditional)"},
        {"da", "Danish"},
        {"nl_NL", "Dutch"},
        {"en_US", "English"},
        {"fi", "Finnish"},
        {"fr", "French"},
        {"de", "German"},
        {"it", "Italian"},
        {"ja", "Japanese"},
        {"ko", "Korean"},
        {"no", "Norwegian"},
        {"pt_BR", "Portuguese (Brazil)"},
        {"ru", "Russian"},
        {"es", "Spanish"},
        {"es_MX", "Spanish (Mexico)"},
        {"sv", "Swedish"},
        {"th", "Thai"}
    }

    Dim fieldType As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String) From {
        {0, "String"},
        {1, "Picklist"},
        {2, "Multi Picklist"},
        {3, "Combobox"},
        {4, "Reference"},
        {5, "Base64"},
        {6, "Boolean"},
        {7, "Currency"},
        {8, "Textarea"},
        {9, "Integer"},
        {10, "Double"},
        {11, "Percent"},
        {12, "Phone"},
        {13, "Id"},
        {14, "Date"},
        {15, "Datetime"},
        {16, "Time"},
        {17, "Url"},
        {18, "Email"},
        {19, "Encrypted String"},
        {20, "DataCategoryGroupReference"},
        {21, "Location"},
        {22, "Address"},
        {23, "AnyType"},
        {24, "Json"},
        {25, "Complex Value"},
        {26, "Long"}
    }

    Public Sub DescribeSalesforceObjectsBySOAP()
        Try
            Dim frm As processDescribeCustomObject = New processDescribeCustomObject()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, Title:="DescribeSObjects Exception")
        End Try

        ThisAddIn.excelApp.StatusBar = "Complete Describe SObject"
    End Sub

    Public Function DescribeSObject(ByVal objname As String, ByVal baseLang As String) As Partner.DescribeSObjectResult
        Return SOAPAPI.DescribeSObject(objname, baseLang)
    End Function

    Public Function getFieldTranslations(ByVal objname As String, ByRef objLabels As Dictionary(Of String, String),
                                      ByRef fields() As Partner.Field, ByRef langSet As List(Of String), ByRef baseLang As String,
                                      ByRef percent As Integer, ByRef bgw As ComponentModel.BackgroundWorker) As Dictionary(Of String, Dictionary(Of String, String))
        Try
            Dim fieldMeta As Dictionary(Of String, Dictionary(Of String, String)) = New Dictionary(Of String, Dictionary(Of String, String))
            Dim fieldSet As Dictionary(Of String, MiniMETA.CustomField) = New Dictionary(Of String, MiniMETA.CustomField)
            Dim fieldTranslation As Dictionary(Of String, Dictionary(Of String, String)) = New Dictionary(Of String, Dictionary(Of String, String))

            bgw.ReportProgress(percent, "Get metadata information for " & objname & "'s fields...")
            Dim co As MiniMETA.CustomObject = CType(readMetadata("CustomObject", {objname})(0), MiniMETA.CustomObject)

            For Each cf As MiniMETA.CustomField In co.fields
                fieldSet.Add(cf.fullName, cf)
            Next

            If langSet.Count > 0 Then
                bgw.ReportProgress(percent, "Get translation information for " & objname & "'s fields...")
                For Each lang As String In langSet.ToArray()
                    Dim fieldInfo As Dictionary(Of String, String) = New Dictionary(Of String, String)
                    If lang <> baseLang Then
                        Dim dsr As Partner.DescribeSObjectResult = DescribeSObject(objname, lang)
                        Dim baseLabel As String = dsr.label & ", " & IIf(dsr.labelPlural <> Nothing, dsr.labelPlural, "no_plural_label")
                        objLabels.Add(lang, baseLabel)
                        For Each fld As Partner.Field In dsr.fields
                            fieldInfo.Add(fld.name, fld.label)
                        Next
                        fieldTranslation.Add(lang, fieldInfo)
                    Else
                        For Each fld As Partner.Field In fields
                            fieldInfo.Add(fld.name, fld.label)
                        Next
                        fieldTranslation.Add(baseLang, fieldInfo)
                    End If
                Next
            End If

            bgw.ReportProgress(percent, "Add field description and(or) tarnslation for " & objname & "'s fields...")

            For Each fld As Partner.Field In fields
                Dim fldinfo As Dictionary(Of String, String) = New Dictionary(Of String, String)
                If fieldSet.ContainsKey(fld.name) Then
                    Dim desc As String = fieldSet.Item(fld.name).description
                    If desc IsNot Nothing Then fldinfo.Add("desc", desc)
                End If
                For Each lang As String In langSet
                    If fieldTranslation.ContainsKey(lang) Then
                        Dim trans As Dictionary(Of String, String) = fieldTranslation.Item(lang)
                        If trans.ContainsKey(fld.name) Then
                            fldinfo.Add(lang, IIf(trans.Item(fld.name) = "", fld.label, trans.Item(fld.name)))
                        End If
                    End If
                Next
                fieldMeta.Add(fld.name, fldinfo)
            Next

            Return fieldMeta
        Catch ex As Exception
            Throw New Exception("getFieldMetadatas Exception")
        End Try
    End Function

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

    Sub setLayout(ByRef worksheet As Excel.Worksheet, ByVal objname As String, ByRef objLabels As Dictionary(Of String, String))
        ' columns width adjustment
        worksheet.Range("A1").ColumnWidth = 2
        worksheet.Range("B1:C1").ColumnWidth = 26 ' label, api name
        worksheet.Range("D1").ColumnWidth = 20 ' type
        worksheet.Range("E1:M1").ColumnWidth = 12 ' custom, autonumber, nillable, excrypted, extrenal id, length, digits, precision
        worksheet.Range("N1").ColumnWidth = 30 ' description

        If objLabels.Count > 2 Then
            Dim labels As String = ""
            For Each key As String In objLabels.Keys()
                If key <> "base" Then
                    Dim trns As String = objLabels.Item(key)
                    If trns.Length > 0 Then labels = labels & "[" & key & "] " & trns & vbCrLf
                End If
            Next
            If labels.Length > 0 Then
                worksheet.Range("A1").ClearComments()
                worksheet.Range("A1").AddComment()
                worksheet.Range("A1").Comment.Shape.TextFrame.AutoSize = True
                worksheet.Range("A1").Comment.Shape.TextFrame.Characters.Font.Bold = False
                worksheet.Range("A1").Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
                worksheet.Range("A1").Comment.Text(labels)
            End If
        End If

        ' headline rendering
        Dim titleRange As Excel.Range = worksheet.Range("B1:N1")
        titleRange.Merge()
        titleRange.RowHeight = 26
        titleRange.Font.Size = 20
        titleRange.Font.Name = "Consolas"
        titleRange.Font.Bold = True
        titleRange.Value = objname & " [" & objLabels.Item("base") & "]"
        titleRange.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
    End Sub

    Sub renderHeader(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByVal objname As String)
        Dim headerRow As Excel.Range = worksheet.Range("B3:N3")
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
        worksheet.Range("N3").Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
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
        worksheet.Range("N3").Value = "Description"

    End Sub

    Function renderNamedField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range,
                              ByRef standardFields As Dictionary(Of String, Partner.Field),
                              ByRef fieldMeta As Dictionary(Of String, Dictionary(Of String, String)),
                              ByVal rowPointer As Integer) As Integer

        Dim fldinfo As Dictionary(Of String, String)
        If standardFields.ContainsKey("Id") Then
            fldinfo = IIf(fieldMeta.ContainsKey("Id"), fieldMeta.Item("Id"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("Id"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("MasterRecordId") Then
            fldinfo = IIf(fieldMeta.ContainsKey("MasterRecordId"), fieldMeta.Item("MasterRecordId"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("MasterRecordId"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("RecordTypeId") Then
            fldinfo = IIf(fieldMeta.ContainsKey("RecordTypeId"), fieldMeta.Item("RecordTypeId"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("RecordTypeId"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("IsDeleted") Then
            fldinfo = IIf(fieldMeta.ContainsKey("IsDeleted"), fieldMeta.Item("IsDeleted"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("IsDeleted"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("Name") Then
            fldinfo = IIf(fieldMeta.ContainsKey("Name"), fieldMeta.Item("Name"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("Name"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("Subject") Then
            fldinfo = IIf(fieldMeta.ContainsKey("Subject"), fieldMeta.Item("Subject"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("Subject"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CurrencyISOCode") Then
            fldinfo = IIf(fieldMeta.ContainsKey("CurrencyISOCode"), fieldMeta.Item("CurrencyISOCode"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("CurrencyISOCode"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CreatedById") Then
            fldinfo = IIf(fieldMeta.ContainsKey("CreatedById"), fieldMeta.Item("CreatedById"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("CreatedById"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("CreatedDate") Then
            fldinfo = IIf(fieldMeta.ContainsKey("CreatedDate"), fieldMeta.Item("CreatedDate"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("CreatedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastModifiedById") Then
            fldinfo = IIf(fieldMeta.ContainsKey("LastModifiedById"), fieldMeta.Item("LastModifiedById"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("LastModifiedById"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastModifiedDate") Then
            fldinfo = IIf(fieldMeta.ContainsKey("LastModifiedDate"), fieldMeta.Item("LastModifiedDate"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("LastModifiedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("SystemModstamp") Then
            fldinfo = IIf(fieldMeta.ContainsKey("SystemModstamp"), fieldMeta.Item("SystemModstamp"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("SystemModstamp"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastActivityDate") Then
            fldinfo = IIf(fieldMeta.ContainsKey("LastActivityDate"), fieldMeta.Item("LastActivityDate"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("LastActivityDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastViewedDate") Then
            fldinfo = IIf(fieldMeta.ContainsKey("LastViewedDate"), fieldMeta.Item("LastViewedDate"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("LastViewedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("LastReferencedDate") Then
            fldinfo = IIf(fieldMeta.ContainsKey("LastReferencedDate"), fieldMeta.Item("LastReferencedDate"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("LastReferencedDate"))
            rowPointer = rowPointer + 1
        End If
        If standardFields.ContainsKey("OwnerId") Then
            fldinfo = IIf(fieldMeta.ContainsKey("OwnerId"), fieldMeta.Item("OwnerId"), Nothing)
            renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item("OwnerId"))
            rowPointer = rowPointer + 1
        End If

        Return rowPointer
    End Function

    Function renderStandardField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByRef namedFields() As String,
                                 ByRef standardFields As Dictionary(Of String, Partner.Field),
                                 ByRef fieldMeta As Dictionary(Of String, Dictionary(Of String, String)),
                                 ByVal rowPointer As Integer, ByRef objectCount As Integer, ByRef numOfPart As Integer,
                                 numOfField As Integer, ByRef objname As String, ByRef bgw As ComponentModel.BackgroundWorker) As Integer
        Dim keys() As String = standardFields.Keys.ToArray()
        Array.Sort(keys)
        For Each key As String In keys
            If Not namedFields.Contains(key) Then
                Dim percent As Integer = CInt(numOfPart * (rowPointer / numOfField)) + (numOfPart * objectCount)
                bgw.ReportProgress(percent, "Describe " & objname & " (fields " & rowPointer.ToString() & " / " & numOfField.ToString() & ")")
                Dim fldinfo As Dictionary(Of String, String) = IIf(fieldMeta.ContainsKey(key), fieldMeta.Item(key), Nothing)
                renderField(worksheet, start, fldinfo, rowPointer, standardFields.Item(key))
                rowPointer = rowPointer + 1
            End If
        Next
        Return rowPointer
    End Function

    Sub renderCustomField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByRef namedFields() As String,
                          ByRef customFields As Dictionary(Of String, Partner.Field),
                          ByRef fieldMeta As Dictionary(Of String, Dictionary(Of String, String)),
                          ByVal rowPointer As Integer, ByRef objectCount As Integer, ByRef numOfPart As Integer,
                          numOfField As Integer, ByRef objname As String, ByRef bgw As ComponentModel.BackgroundWorker)
        Dim keys() As String = customFields.Keys.ToArray()
        Array.Sort(keys)
        For Each key As String In keys
            If Not namedFields.Contains(key) Then
                Dim percent As Integer = CInt(numOfPart * (rowPointer / numOfField)) + (numOfPart * objectCount)
                bgw.ReportProgress(percent, "Describe " & objname & " (fields " & rowPointer.ToString("N0") & " / " & numOfField.ToString("N0") & ")")
                Dim fldinfo As Dictionary(Of String, String) = IIf(fieldMeta.ContainsKey(key), fieldMeta.Item(key), Nothing)
                renderField(worksheet, start, fldinfo, rowPointer, customFields.Item(key))
                rowPointer = rowPointer + 1
            End If
        Next
    End Sub

    Sub renderField(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range, ByRef fieldinfo As Dictionary(Of String, String),
                    ByVal rownum As Integer, ByVal fld As Partner.Field)
        Dim startCell As Excel.Range = start.Offset(rownum, 0)
        Dim dataRow As Excel.Range = worksheet.Range(startCell, startCell.Offset(0, 12))

        dataRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        dataRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
        dataRow.Font.Name = "Vernada"
        dataRow.Style.IndentLevel = 1
        dataRow.VerticalAlignment = Excel.Constants.xlCenter
        startCell.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        startCell.Offset(0, 11).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous

        startCell.Value = fld.label
        If fieldinfo IsNot Nothing Then
            Dim labels As String = ""
            For Each key As String In fieldinfo.Keys()
                If key <> "desc" Then
                    Dim trns As String = fieldinfo.Item(key)
                    If trns.Length > 0 Then labels = labels & "[" & key & "] " & trns & vbCrLf
                End If
            Next
            If labels.Length > 0 Then
                startCell.ClearComments()
                startCell.AddComment()
                startCell.Comment.Shape.TextFrame.AutoSize = True
                startCell.Comment.Shape.TextFrame.Characters.Font.Bold = False
                startCell.Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
                startCell.Comment.Text(labels)
            End If
        End If
        startCell.Offset(0, 1).Value = fld.name
        startCell.Offset(0, 1).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        With startCell.Offset(0, 2)
            .Value = fieldType.Item(CInt(fld.type))
            .Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
            If fld.type = Partner.fieldType.picklist Or fld.type = Partner.fieldType.reference Then
                .ClearComments()
                .AddComment()
                .Comment.Shape.TextFrame.AutoSize = True
                .Comment.Shape.TextFrame.Characters.Font.Bold = False
                .Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
                Dim comment As String = ""
                Dim i As Integer = 0
                If fld.type = Partner.fieldType.picklist Then
                    comment = "Pickist Values :" & vbCrLf
                    Dim picklists() As Partner.PicklistEntry = fld.picklistValues
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
        'startCell.Offset(0, 12).Value = IIf(fieldinfo.ContainsKey("desc"), fieldinfo.Item("desc"), "")
        If fieldinfo.ContainsKey("desc") Then
            startCell.Offset(0, 12).Value = fieldinfo.Item("desc")
        Else
            startCell.Offset(0, 12).Value = ""
        End If
        startCell.Offset(0, 12).Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        startCell.Offset(0, 12).Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
    End Sub

End Module
