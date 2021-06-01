Imports ForceConnector.MiniMETA

Module METAAPI

    Dim metaClient As MetadataPortTypeClient
    Dim metaSessionHeader As MiniMETA.SessionHeader
    Dim allOrNoneHeader As MiniMETA.AllOrNoneHeader

    Public Sub DownloadCustomLabels()
        Try
            Dim frm As processCustomLabelDownload = New processCustomLabelDownload()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="DownloadCustomLabel Exception" & vbCrLf & ex.Message)
        End Try

        ThisAddIn.excelApp.StatusBar = "Download CustomLabel Translations completed"
    End Sub

    Public Sub DownloadCustomLabelTranslations()
        Try
            Dim frm As processCustomLabelTranslationDownload = New processCustomLabelTranslationDownload()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="DownloadCustomLabelTranslations() Exception")
        End Try

        ThisAddIn.excelApp.StatusBar = "Download CustomLabel Translations completed"
    End Sub

    Public Sub UploadCustomLabels()
        Try
            Dim frm As processCustomLabelUpload = New processCustomLabelUpload()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="UploadCustomLabels() Exception")
        End Try
        ThisAddIn.excelApp.StatusBar = "Upload New CustomLabels completed"
    End Sub

    Public Sub UpdateCustomLabelTranslations()
        Try
            Dim frm As processCustomLabelTranslationUpload = New processCustomLabelTranslationUpload()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="UpdateCustomLabelTranslation() Exception")
        End Try
        ThisAddIn.excelApp.StatusBar = "Update CustomLabel Translations completed"
    End Sub

    Public Sub DownloadObjectTranslations()
        Dim statusText As String = ""

        Try
            ' Query CustomObjectTranslation Metadata
            Dim m_files() As FileProperties
            Dim objectMap As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))
            Dim selectedObject() As String

            m_files = listMetadata({"CustomObjectTranslation"})
            If m_files IsNot Nothing Then
                For Each fp As FileProperties In m_files
                    If Not fp.fullName.Contains("__mdt") Then
                        Dim tmp() As String = fp.fullName.Split("-") ' split "Account-en_US" to "Account, en_US", finally get "Account, {en_US, ko, ...}"
                        If objectMap.ContainsKey(tmp(0)) Then
                            Dim lang As List(Of String) = objectMap.Item(tmp(0))
                            lang.Add(tmp(1))
                            If lang.Count > 1 Then lang.Sort()
                            'objectMap.Add(tmp(0), lang)
                        Else
                            Dim lang As List(Of String) = New List(Of String)
                            lang.Add(tmp(1))
                            If lang.Count > 1 Then lang.Sort()
                            objectMap.Add(tmp(0), lang)
                        End If
                    End If
                Next
            Else
                statusText = "No ObjectTranslation Data!"
                GoTo errors
            End If

            If objectMap.Keys.Count > 0 Then
                Dim keys As List(Of String) = objectMap.Keys.ToList
                keys.Sort()
                Dim selectForm As TranslationObjectSelect = New TranslationObjectSelect(keys.ToArray)

                selectForm.ShowDialog()
                selectedObject = selectForm.selectedList.ToArray

                If selectedObject.Length = 0 Then
                    statusText = "No object selected"
                    GoTo errors
                End If
            Else
                statusText = "No Available ObjectTranslation Data!"
                GoTo errors
            End If

            Dim frm As processObjectTranslationDownload = New processObjectTranslationDownload()
            frm.objectMap = objectMap
            frm.selectedObject = selectedObject
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="DownloadObjectTranslations() Exception")
        End Try
errors:
        If statusText <> "" Then
            MsgBox(statusText, Title:="Download Objects Translation")
        End If
done:
        ThisAddIn.excelApp.StatusBar = "Download Object Translations completed"
    End Sub

    Public Sub UpdateObjectTranslations()
        MsgBox("Under Development", Title:="No Action")
    End Sub

    Public Sub DownloadTranslations()
        Try
            Dim frm As processTranslationDownload = New processTranslationDownload()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="DownloadTranslations() Exception")
        End Try
        ThisAddIn.excelApp.StatusBar = "Download Translations completed"
    End Sub

    Public Sub UpdateTranslations()
        MsgBox("Under Development", Title:="No Action")
    End Sub

    '******************************************************************************
    '******************************************************************************
    '* Operation
    '******************************************************************************
    '******************************************************************************

    '******************************************************************************
    '* Custom Label Part
    '******************************************************************************
    Sub setCustomLabelLayout(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range)
        ' headline rendering
        Dim titleRange As Excel.Range = worksheet.Range("A1:F1")
        titleRange.Merge()
        titleRange.RowHeight = 26
        titleRange.Font.Size = 20
        titleRange.Font.Name = "Consolas"
        titleRange.Font.Bold = True
        titleRange.Value = "Custom Label"

        start = worksheet.Range("A3")

        renderCustomLabelHeader(worksheet)
    End Sub

    Sub renderCustomLabelHeader(ByRef worksheet As Excel.Worksheet)
        Dim headerRow As Excel.Range = worksheet.Range("A2:F2")
        ' Id, FullName, 
        headerRow.Font.Bold = True
        headerRow.Font.Name = "Vernada"
        headerRow.Font.ColorIndex = 2
        headerRow.HorizontalAlignment = Excel.Constants.xlCenter
        headerRow.VerticalAlignment = Excel.Constants.xlCenter
        headerRow.Interior.Color = RGB(0, 176, 240)
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
        worksheet.Range("A2").Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("F2").Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("A2").Value = "FullName"
        worksheet.Range("A2").ColumnWidth = 20
        worksheet.Range("B2").Value = "Language"
        worksheet.Range("B2").ColumnWidth = 10
        worksheet.Range("C2").Value = "Protected"
        worksheet.Range("C2").ColumnWidth = 10
        worksheet.Range("D2").Value = "Category"
        worksheet.Range("D2").ColumnWidth = 20
        worksheet.Range("E2").Value = "Short Description"
        worksheet.Range("E2").ColumnWidth = 20
        worksheet.Range("F2").Value = "Value"
        worksheet.Range("F2").ColumnWidth = 30
    End Sub

    Public Function queryCustomLabel(ByRef excelApp As Excel.Application, ByRef todo As Excel.Range) As Boolean
        Try
            Dim i As Integer = 0
            Dim fullNames() As String
            ReDim fullNames(todo.Rows.Count - 1)
            For Each rw As Excel.Range In todo.Rows
                fullNames(i) = rw.Cells(1, 1).Value
                i = i + 1
            Next rw

            excelApp.StatusBar = "Query CustomLabel from salesforce>" & UBound(fullNames) + 1
            Dim srs() As MiniMETA.Metadata = readMetadata("CustomLabel", fullNames)

            If srs.Length > 0 Then
                Dim cld As Dictionary(Of String, MiniMETA.Metadata) = New Dictionary(Of String, MiniMETA.Metadata)
                For Each sr As MiniMETA.Metadata In srs ' just make restults into dict
                    cld.Add(sr.fullName, sr)
                Next
                excelApp.StatusBar = "back from retrieve data at salesforce"

                i = 1
                For Each rw As Excel.Range In todo.Rows
                    If rw.Cells(1, 1).Value IsNot Nothing Then
                        todo.Rows(i).Interior.ColorIndex = 36
                        Dim md As MiniMETA.CustomLabel
                        md = CType(cld.Item(rw.Cells(1, 1).Value), MiniMETA.CustomLabel)
                        rw.Cells(1, 1).AddComment()
                        rw.Cells(1, 1).Comment.Text("registered")
                        rw.Cells(1, 2).value = md.language
                        rw.Cells(1, 3).value = md.protected
                        rw.Cells(1, 4).value = md.categories
                        rw.Cells(1, 5).value = md.shortDescription
                        rw.Cells(1, 6).value = md.value

                        todo.Rows(i).Interior.ColorIndex = 0
                        i = i + 1
                    End If
                Next rw

                cld = Nothing
                srs = Nothing
            End If
            Return True
        Catch ex As Exception
            Throw New Exception("queryCustomLabel Exception" & vbCrLf & ex.Message)
        End Try
        Return False
    End Function

    Sub uploadCustomLabel(ByRef excelApp As Excel.Application, ByRef todo As Excel.Range, ByRef someFailed As Boolean)
        Dim urs As MiniMETA.UpsertResult()
        Dim urMap As Dictionary(Of String, MiniMETA.UpsertResult) = New Dictionary(Of String, MiniMETA.UpsertResult)

        Dim metadatas As List(Of MiniMETA.CustomLabel) = New List(Of MiniMETA.CustomLabel)
        Dim recarray As List(Of String) = New List(Of String)

        Try
            excelApp.StatusBar = "Upload :" & todo.Row - excelApp.Selection.row + 1 & " -> " &
                todo.Row - excelApp.Selection.row + todo.Rows.Count & " of " & CStr(excelApp.Selection.Rows.Count)
            todo.Interior.ColorIndex = 36 ' show where we are working

            For Each rw As Excel.Range In todo.Rows
                Dim meta As MiniMETA.CustomLabel = New MiniMETA.CustomLabel

                meta.fullName = rw.Cells(1, 1).Value
                meta.language = rw.Cells(1, 2).Value
                meta.protected = rw.Cells(1, 3).Value
                meta.categories = rw.Cells(1, 4).Value
                meta.shortDescription = rw.Cells(1, 5).Value
                meta.value = rw.Cells(1, 6).Value

                metadatas.Add(meta)
            Next rw

            If metadatas.Count < 1 Then  ' no metadatas to upload
                ErrorBox("No CustomLabels to Upload")
                todo.Interior.ColorIndex = 0 ' clear out color
                GoTo done
            End If

            urs = upsertMetadata(metadatas.ToArray())
            todo.Interior.ColorIndex = 0 ' clear out color

            Dim ur As MiniMETA.UpsertResult
            For Each ur In urs
                urMap.Add(ur.fullName, ur)
            Next

            For Each rw In todo.Rows
                Dim fullname As String = rw.Cells(1, 1).Value
                ur = urMap.Item(fullname)

                If Not ur.success Then
                    rw.Interior.ColorIndex = 6
                    If Not (rw.Cells(1, 1).Comment Is Nothing) Then rw.Cells(1, 1).Comment.Delete()
                    rw.Cells(1, 1).AddComment()
                    Dim errMsg As String = "Upload CustomLabel Failed:" & Chr(10)
                    For Each err As MiniMETA.Error In ur.errors
                        errMsg = errMsg & err.statusCode & ", " & err.message & Chr(10)
                    Next
                    rw.Cells(1, 1).Comment.Text(errMsg)
                    rw.Cells(1, 1).Comment.Shape.Height = 60
                    someFailed = True ' will message this later
                Else
                    If rw.Cells(1, 1).Comment Is Nothing Then
                        rw.Cells(1, 1).AddComment()
                        rw.Cells(1, 1).Comment.Text("registered")
                    Else
                        Dim comments As String = rw.Cells(1, 1).Comment.Text
                        If comments.Contains("registered") Then
                            rw.Cells(1, 1).Comment.Text(comments & vbCrLf & "updated")
                        End If
                    End If
                    rw.Interior.ColorIndex = 0
                End If
            Next rw

        Catch ex As Exception
            Throw New Exception("uploadCustomLabel Exception" & vbCrLf & ex.Message)
        End Try
done:
    End Sub

    '******************************************************************************
    '* sObject Translation Download Part
    '******************************************************************************
    Sub setObjectTranslationLayout(ByRef worksheet As Excel.Worksheet, ByVal objName As String, ByRef start As Excel.Range)
        ' headline rendering
        Dim titleRange As Excel.Range = worksheet.Range("A1:B1")
        titleRange.Merge()
        titleRange.RowHeight = 26
        titleRange.Font.Size = 20
        titleRange.Font.Name = "Consolas"
        titleRange.Font.Bold = True
        titleRange.Value = objName & " Translation"

        renderObjectTranslationHeader(worksheet, start)
    End Sub

    Sub renderObjectTranslationHeader(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range)
        Dim headerRow As Excel.Range = worksheet.Range("A2:B2")
        start = worksheet.Range("A3")
        headerRow.Font.Bold = True
        headerRow.Font.Name = "Vernada"
        headerRow.Font.ColorIndex = 2
        headerRow.HorizontalAlignment = Excel.Constants.xlCenter
        headerRow.VerticalAlignment = Excel.Constants.xlCenter
        headerRow.Interior.Color = RGB(0, 176, 240)
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
        worksheet.Range("A2").Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("A2").Value = "Key"
        worksheet.Range("A2").ColumnWidth = 60
        worksheet.Range("B2").Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("B2").Value = "Value"
        worksheet.Range("B2").ColumnWidth = 30
    End Sub

    Sub renderBaseObjectDescribe(ByRef m_baseObject As Dictionary(Of String, String), ByVal objName As String)
        ' VB.net SOAP deserializer doesn't support comment tag, so make temporary object for original label information.
        ' Also CustomObject metadata did not contains original label information. It can be get from partner's SObjectDescribe.
        ' {"The maximum message size quota for incoming messages (65536) has been exceeded. To increase the quota, use the MaxReceivedMessageSize property on the appropriate binding element."}
        Try
            m_baseObject = New Dictionary(Of String, String)
            Dim metas() As MiniMETA.Metadata = readMetadata("CustomObject", {objName})
            Dim meta As MiniMETA.CustomObject = CType(metas(0), MiniMETA.CustomObject)
            Dim prefix As String = meta.fullName

            m_baseObject.Add(prefix, If(meta.label = Nothing, "", meta.label))
            If meta.nameField IsNot Nothing Then
                m_baseObject.Add(prefix & ".fields.NameField", If(meta.nameField.label = Nothing, "", meta.nameField.label))
            End If
            If meta.fieldSets IsNot Nothing Then
                For Each fs As MiniMETA.FieldSet In meta.fieldSets
                    m_baseObject.Add(prefix & ".fieldSets." & fs.fullName, fs.label)
                Next
            End If
            If meta.recordTypes IsNot Nothing Then
                For Each rt As MiniMETA.RecordType In meta.recordTypes
                    m_baseObject.Add(prefix & ".recordTypes." & rt.fullName, rt.label)
                Next
            End If
            If meta.sharingReasons IsNot Nothing Then
                For Each sr As MiniMETA.SharingReason In meta.sharingReasons
                    m_baseObject.Add(prefix & ".sharingReasons." & sr.fullName, sr.label)
                Next
            End If
            If meta.validationRules IsNot Nothing Then
                For Each vr As MiniMETA.ValidationRule In meta.validationRules
                    m_baseObject.Add(prefix & ".validationRules." & vr.fullName, vr.errorMessage)
                Next
            End If
            If meta.webLinks IsNot Nothing Then
                For Each wl As MiniMETA.WebLink In meta.webLinks
                    m_baseObject.Add(prefix & ".." & wl.fullName, wl.masterLabel)
                Next
            End If

            Dim gr As RESTful.DescribeSObjectResult = RESTAPI.DescribeSObject(objName)
            Dim fields() As RESTful.Field = gr.fields
            For Each fld As RESTful.Field In fields
                m_baseObject.Add(prefix & ".fields." & fld.name, fld.label)
                If fld.picklistValues IsNot Nothing Then
                    For Each pv As RESTful.PicklistEntry In fld.picklistValues
                        m_baseObject.Add(prefix & ".fields." & fld.name & ".picklist." & pv.label, pv.label)
                    Next
                End If
                If fld.relationshipName IsNot Nothing Then
                    m_baseObject.Add(prefix & ".fields." & fld.name & ".relationship", fld.relationshipName)
                End If
                If fld.inlineHelpText IsNot Nothing Then
                    m_baseObject.Add(prefix & ".fields." & fld.name & ".help", fld.inlineHelpText)
                End If
            Next
        Catch ex As Exception
            Throw New Exception("renderObjectDescribe Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub renderObjectTranslation(ByRef excelApp As Excel.Application, ByRef m_head As Excel.Range, ByRef m_body As Excel.Range,
                                ByRef m_rows As Long, ByRef m_baseObject As Dictionary(Of String, String),
                                ByVal objName As String, ByVal lang As String, ByVal meta As MiniMETA.Metadata)
        Dim ot As MiniMETA.CustomObjectTranslation = CType(meta, MiniMETA.CustomObjectTranslation)
        Dim prefix As String = objName
        Dim m_langCol As Integer = getLanguageColumn(m_head, lang)

        If ot.caseValues IsNot Nothing Then
            For Each cv As ObjectNameCaseValue In ot.caseValues
                If Not cv.plural Then
                    renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix, cv.value)
                End If
            Next
        End If
        If ot.nameFieldLabel IsNot Nothing Then
            renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".NameField", ot.nameFieldLabel)
        End If
        If ot.fields IsNot Nothing Then
            For Each fld As MiniMETA.CustomFieldTranslation In ot.fields
                Dim keyword As String = prefix & ".fields." & fld.name
                If fld.label IsNot Nothing Then
                    renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword, fld.label)
                Else
                    If fld.caseValues IsNot Nothing Then
                        For Each cv As ObjectNameCaseValue In fld.caseValues
                            If Not cv.plural Then
                                keyword = prefix & ".fields.caseValues." & fld.name
                                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword, cv.value)
                            Else
                                keyword = prefix & ".fields.caseValues.plural." & fld.name
                                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword, cv.value)
                            End If
                        Next
                    End If
                End If
                If fld.picklistValues IsNot Nothing Then
                    For Each pv As MiniMETA.PicklistValueTranslation In fld.picklistValues
                        Dim masterLabel As String = pv.masterLabel.Replace("~", "{{tilde}}")
                        renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword & ".picklist." & masterLabel, If(pv.translation = Nothing, "<!-- " & pv.masterLabel & " -->", pv.translation))
                    Next
                End If
                If fld.relationshipLabel IsNot Nothing Then
                    renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword & ".relationship", fld.relationshipLabel)
                End If
                If fld.help IsNot Nothing Then
                    renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, keyword & ".help", fld.help)
                End If
            Next
        End If
        If ot.fieldSets IsNot Nothing Then
            For Each fs As MiniMETA.FieldSetTranslation In ot.fieldSets
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".fieldSets." & fs.name, If(fs.label = Nothing, "<!-- " & fs.name & " -->", fs.label))
            Next
        End If
        ' no layout definition in CustomObject, need to describe from ????
        'If ot.layouts IsNot Nothing Then
        '    For Each lo As MiniMETA.LayoutTranslation In ot.layouts
        '        If lo.sections IsNot Nothing Then
        '            For Each ls As MiniMETA.LayoutSectionTranslation In lo.sections
        '                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".layouts." & lo.layout & ".section." & ls.section, If(ls.label = Nothing, ls.section, ls.label))
        '            Next
        '        End If
        '    Next
        'End If
        ' no quickAction definition in CustomObject, need to describe from ????
        'If ot.quickActions IsNot Nothing Then
        '    For Each qa As MiniMETA.QuickActionTranslation In ot.quickActions
        '        renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".quickActions." & qa.name, If(qa.label = Nothing, qa.name, qa.label))
        '    Next
        'End If
        If ot.recordTypes IsNot Nothing Then
            For Each rt As MiniMETA.RecordTypeTranslation In ot.recordTypes
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".recordTypes." & rt.name, If(rt.label = Nothing, "<!-- " & rt.name & " -->", rt.label))
            Next
        End If
        If ot.sharingReasons IsNot Nothing Then
            For Each sr As MiniMETA.SharingReasonTranslation In ot.sharingReasons
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".sharingReasons." & sr.name, If(sr.label = Nothing, "<!--" & sr.name & "-->", sr.label))
            Next
        End If
        If ot.standardFields IsNot Nothing Then
            For Each sf As MiniMETA.StandardFieldTranslation In ot.standardFields
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".standardFields." & sf.name, If(sf.label = Nothing, "<!-- " & sf.name & " -->", sf.label))
            Next
        End If
        If ot.validationRules IsNot Nothing Then
            For Each vr As MiniMETA.ValidationRuleTranslation In ot.validationRules
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".validationRules." & vr.name, If(vr.errorMessage = Nothing, "<!-- " & vr.name & " validation rule's error message -->", vr.errorMessage))
            Next
        End If
        If ot.webLinks IsNot Nothing Then
            For Each wl As MiniMETA.WebLinkTranslation In ot.webLinks
                renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".webLinks." & wl.name, If(wl.label = Nothing, "<!-- " & wl.name & " -->", wl.label))
            Next
        End If
        ' no workflowTasks definition in CustomObject, need to describe from ????
        'If ot.workflowTasks IsNot Nothing Then
        '    For Each wt As WorkflowTaskTranslation In ot.workflowTasks
        '        renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".workflowTasks." & wt.name, If(wt.subject = Nothing, wt.name, wt.subject))
        '        If wt.description IsNot Nothing Then
        '            renderObjectItem(excelApp, m_body, m_rows, m_baseObject, m_langCol, prefix & ".workflowTasks." & wt.name & ".description", wt.description)
        '        End If
        '    Next
        'End If
    End Sub

    '******************************************************************************
    '* General Translation Download Part
    '******************************************************************************
    Sub setTranslationLayout(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range)
        ' headline rendering
        Dim titleRange As Excel.Range = worksheet.Range("A1")
        titleRange.RowHeight = 26
        titleRange.Font.Size = 20
        titleRange.Font.Name = "Consolas"
        titleRange.Font.Bold = True
        titleRange.Value = "Translations"

        renderTranslationHeader(worksheet, start)
    End Sub

    Sub renderTranslationHeader(ByRef worksheet As Excel.Worksheet, ByRef start As Excel.Range)
        Dim headerRow As Excel.Range = worksheet.Range("A2")
        start = worksheet.Range("A3")
        headerRow.Font.Bold = True
        headerRow.Font.Name = "Vernada"
        headerRow.Font.ColorIndex = 2
        headerRow.HorizontalAlignment = Excel.Constants.xlCenter
        headerRow.VerticalAlignment = Excel.Constants.xlCenter
        headerRow.Interior.Color = RGB(0, 176, 240)
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlDot
        headerRow.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlDouble
        worksheet.Range("A2").Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous
        worksheet.Range("A2").Value = "Key"
        worksheet.Range("A2").ColumnWidth = 60
    End Sub

    Sub renderTranslations(ByRef excelApp As Excel.Application, ByRef m_head As Excel.Range, ByRef m_body As Excel.Range,
                           ByRef m_rows As Long, ByVal lang As String, ByVal meta As MiniMETA.Metadata)
        Dim trns As MiniMETA.Translations = CType(meta, MiniMETA.Translations)
        Dim m_langCol As Integer = getLanguageColumn(m_head, lang)
        Dim key As String = ""
        Dim label As String = ""

        If trns.customApplications IsNot Nothing Then
            For Each ca As MiniMETA.CustomApplicationTranslation In trns.customApplications
                key = "customApplications." & ca.name
                label = If(ca.label = Nothing, "<!-- " & ca.name & " -->", ca.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If

        If trns.customPageWebLinks IsNot Nothing Then
            For Each cpwl As MiniMETA.CustomPageWebLinkTranslation In trns.customPageWebLinks
                key = "customPageWebLinks." & cpwl.name
                label = If(cpwl.label = Nothing, "<!-- " & cpwl.name & " -->", cpwl.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
        If trns.customTabs IsNot Nothing Then
            For Each ct As MiniMETA.CustomTabTranslation In trns.customTabs
                key = "customTabs." & ct.name
                label = If(ct.label = Nothing, "<!-- " & ct.name & " -->", ct.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
        If trns.flowDefinitions IsNot Nothing Then
            For Each fd As MiniMETA.FlowDefinitionTranslation In trns.flowDefinitions
                key = "flowDefinitions." & fd.fullName
                label = If(fd.label = Nothing, "<!-- " & fd.fullName & " -->", fd.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                If fd.flows IsNot Nothing Then
                    For Each fl As MiniMETA.FlowTranslation In fd.flows
                        Dim flkey As String = "flowDefinitions." & fd.fullName
                        If fl.choices IsNot Nothing Then
                            For Each choice As MiniMETA.FlowChoiceTranslation In fl.choices
                                key = flkey & ".choices." & choice.name
                                label = If(choice.choiceText = Nothing, "<!-- " & choice.name & " choice text -->", choice.choiceText)
                                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                If choice.userInput IsNot Nothing Then
                                    key = flkey & ".choices." & choice.name & ".userInput"
                                    label = If(choice.userInput.promptText = Nothing, "<!-- " & choice.name & " user input prompt text -->", choice.userInput.promptText)
                                    renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                    If choice.userInput.validationRule IsNot Nothing Then
                                        key = flkey & ".choices." & choice.name & ".userInput.validationRule"
                                        label = If(choice.userInput.validationRule.errorMessage = Nothing, "<!-- " & choice.name & " user input validation rules' error message -->", choice.userInput.validationRule.errorMessage)
                                        renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                    End If
                                End If
                            Next
                        End If
                        If fl.screens IsNot Nothing Then
                            For Each screen As MiniMETA.FlowScreenTranslation In fl.screens
                                key = flkey & ".screens." & screen.name & ".pausedText"
                                label = If(screen.pausedText = Nothing, "<!-- " & screen.name & " paused text -->", screen.pausedText)
                                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                key = flkey & ".screens." & screen.name & ".helpText"
                                label = If(screen.helpText = Nothing, "<!-- " & screen.name & " help text -->", screen.helpText)
                                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                If screen.fields IsNot Nothing Then
                                    For Each field As FlowScreenFieldTranslation In screen.fields
                                        key = flkey & ".screens." & screen.name & ".fields." & field.name & ".fieldText"
                                        label = If(field.fieldText = Nothing, "<!-- " & field.name & " field text -->", field.fieldText)
                                        renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                        key = flkey & ".screens." & screen.name & ".fields." & field.name & ".helpText"
                                        label = If(field.helpText = Nothing, "<!-- " & field.name & " help text -->", field.helpText)
                                        renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                        If field.validationRule IsNot Nothing Then
                                            key = flkey & ".screens." & screen.name & ".fields." & field.name & ".validationRule"
                                            label = If(field.validationRule.errorMessage = Nothing, "<!-- " & field.name & " validation rule's error message -->", field.validationRule.errorMessage)
                                            renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                                        End If
                                    Next
                                End If
                            Next
                        End If
                        If fl.stages IsNot Nothing Then
                            For Each stage As MiniMETA.FlowStageTranslation In fl.stages
                                key = flkey & ".stages." & stage.name
                                label = If(stage.label = Nothing, "<!-- " & stage.name & " label -->", stage.label)
                                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                            Next
                        End If
                        If fl.textTemplates IsNot Nothing Then
                            For Each tt As FlowTextTemplateTranslation In fl.textTemplates
                                key = flkey & ".textTemplates." & tt.name
                                label = If(tt.text = Nothing, "<!-- " & tt.name & " text -->", tt.text)
                                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                            Next
                        End If
                    Next
                End If
            Next
        End If
        If trns.prompts IsNot Nothing Then
            For Each prompt As MiniMETA.PromptTranslation In trns.prompts
                key = "prompts." & prompt.name
                label = If(prompt.label = Nothing, "<!-- " & prompt.name & " -->", prompt.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
                key = "prompts." & prompt.name & ".description"
                label = If(prompt.description = Nothing, "<!-- " & prompt.name & " description -->", prompt.description)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
        If trns.quickActions IsNot Nothing Then
            For Each qa As MiniMETA.GlobalQuickActionTranslation In trns.quickActions
                key = "quickActions." & qa.name
                label = If(qa.label = Nothing, "<!-- " & qa.name & " -->", qa.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
        If trns.reportTypes IsNot Nothing Then
            For Each rt As MiniMETA.ReportTypeTranslation In trns.reportTypes
                key = "reportTypes." & rt.name
                label = If(rt.label = Nothing, "<!-- " & rt.name & " -->", rt.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
        If trns.scontrols IsNot Nothing Then
            For Each sc As MiniMETA.ScontrolTranslation In trns.scontrols
                key = "scontrols." & sc.name
                label = If(sc.label = Nothing, "<!-- " & sc.name & " -->", sc.label)
                renderItem(excelApp, m_body, m_rows, m_langCol, key, label)
            Next
        End If
    End Sub

    '******************************************************************************
    '********* Parsing General Translation Part (UNDER DEVELOPMENT)
    '******************************************************************************
    Sub updateGeneralTranslation(ByRef excelApp As Excel.Application, ByRef m_head As Excel.Range, ByRef m_body As Excel.Range, ByRef m_langSet As List(Of String))
        Try
            ' prompt has child as description
            Dim cas As List(Of MiniMETA.CustomApplicationTranslation) = New List(Of MiniMETA.CustomApplicationTranslation)
            Dim cpwls As List(Of MiniMETA.CustomPageWebLinkTranslation) = New List(Of MiniMETA.CustomPageWebLinkTranslation)
            Dim cts As List(Of MiniMETA.CustomTabTranslation) = New List(Of MiniMETA.CustomTabTranslation)
            Dim fds As List(Of MiniMETA.FlowDefinitionTranslation) = New List(Of MiniMETA.FlowDefinitionTranslation)
            Dim ps As List(Of MiniMETA.PromptTranslation) = New List(Of MiniMETA.PromptTranslation)
            Dim qas As List(Of MiniMETA.GlobalQuickActionTranslation) = New List(Of MiniMETA.GlobalQuickActionTranslation)
            Dim rts As List(Of MiniMETA.ReportTypeTranslation) = New List(Of MiniMETA.ReportTypeTranslation)
            Dim scs As List(Of MiniMETA.ScontrolTranslation) = New List(Of MiniMETA.ScontrolTranslation)

            Dim fdMap As Dictionary(Of String, MiniMETA.FlowDefinitionTranslation) = New Dictionary(Of String, FlowDefinitionTranslation)

            Dim todo As Excel.Range = excelApp.Selection ' save the selection
            If todo.Columns.Count > 1 Then
                ErrorBox("Only one language can upload!")
                GoTo done
            End If
            Dim langCell As Excel.Range = excelApp.Intersect(todo.Cells(1, 1).EntireColumn, m_head)
            If langCell Is Nothing Then
                ErrorBox("Could not find the translatable language")
                GoTo done
            End If
            If Not m_langSet.Contains(langCell.Value) Then
                ErrorBox("Select area's language""" & langCell.Value & """ does not supported!")
                GoTo done
            End If

            Dim rw As Excel.Range
            For Each rw In todo.Rows
                Dim nameCell As Excel.Range = excelApp.Intersect(m_body.Cells(1, 1).EntireColumn, rw.EntireRow)
                Dim name As String = nameCell.Value
                Dim childs() As String = name.Split(".")

                Select Case childs(0)
                    Case "customApplications"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim ca As MiniMETA.CustomApplicationTranslation = New MiniMETA.CustomApplicationTranslation
                            ca.name = childs(1)
                            ca.label = rw.val
                            cas.Add(ca)
                        End If
                    Case "customPageWebLinks"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim cpwl As MiniMETA.CustomPageWebLinkTranslation = New MiniMETA.CustomPageWebLinkTranslation
                            cpwl.name = childs(1)
                            cpwl.label = rw.val
                            cpwls.Add(cpwl)
                        End If
                    Case "customTabs"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim ct As MiniMETA.CustomTabTranslation = New MiniMETA.CustomTabTranslation
                            ct.name = childs(1)
                            ct.label = rw.val
                            cts.Add(ct)
                        End If
                    Case "flowDefinitions"
                        parsingFlowDefinitionTranslation(fdMap, childs, rw.Value)
                    Case "prompts"
                        parsingPromptTranslation(ps, childs, rw.Value)
                    Case "quickActions"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim qa As MiniMETA.GlobalQuickActionTranslation = New MiniMETA.GlobalQuickActionTranslation
                            qa.name = childs(1)
                            qa.label = val
                            qas.Add(qa)
                        End If
                    Case "reportTypes"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim rt As MiniMETA.ReportTypeTranslation = New MiniMETA.ReportTypeTranslation
                            rt.name = childs(1)
                            rt.label = val
                            rts.Add(rt)
                        End If
                    Case "scontrols"
                        Dim val As String = rw.Value
                        If Not val.Contains("<!--") And val.Length > 0 Then
                            Dim sc As MiniMETA.ScontrolTranslation = New MiniMETA.ScontrolTranslation
                            sc.name = childs(1)
                            sc.label = val
                            scs.Add(sc)
                        End If
                End Select
            Next

            Dim meta As MiniMETA.Translations = New MiniMETA.Translations
            meta.fullName = langCell.Value

            If cas.Count > 0 Then meta.customApplications = cas.ToArray
            If cpwls.Count > 0 Then meta.customPageWebLinks = cpwls.ToArray
            If cts.Count > 0 Then meta.customTabs = cts.ToArray
            'If fds.Count > 0 Then meta.flowDefinitions = fds.ToArray
            If fdMap.Count > 0 Then meta.flowDefinitions = fdMap.Values.ToArray
            If ps.Count > 0 Then meta.prompts = ps.ToArray
            If qas.Count > 0 Then meta.quickActions = qas.ToArray
            If rts.Count > 0 Then meta.reportTypes = rts.ToArray
            If scs.Count > 0 Then meta.scontrols = scs.ToArray

            Dim srs() As MiniMETA.SaveResult = updateMetadata({meta})

            For Each sr As MiniMETA.SaveResult In srs
                If Not sr.success Then
                    Dim msg As String = ""
                    For Each err As MiniMETA.Error In sr.errors
                        msg = msg & vbCrLf & err.message
                    Next
                    MsgBox(msg, Title:=sr.fullName)
                End If
            Next
            GoTo done
        Catch ex As Exception
            Throw New Exception("updateGeneralTranslation Exception" & vbCrLf & ex.Message)
        End Try
done:
    End Sub

    Sub parsingFlowDefinitionTranslation(ByRef fdm As Dictionary(Of String, MiniMETA.FlowDefinitionTranslation), ByVal childs() As String, ByVal value As String)
        If Not value.Contains("<!--") And value.Length > 0 Then
            If childs.Length = 2 Then
                Dim fd As MiniMETA.FlowDefinitionTranslation
                If fdm.ContainsKey(childs(1)) Then
                    fd = fdm.Item(childs(1))
                    fd.fullName = childs(1)
                    fd.label = value
                    fdm.Item(childs(1)) = fd
                Else
                    fd = New MiniMETA.FlowDefinitionTranslation
                    fd.fullName = childs(1)
                    fd.label = value
                    fdm.Add(fd.fullName, fd)
                End If
            ElseIf childs.Length > 2 Then
                Dim fd As MiniMETA.FlowDefinitionTranslation
                Dim flows() As MiniMETA.FlowTranslation
                If fdm.ContainsKey(childs(1)) Then
                    fd = fdm.Item(childs(1))
                    flows = If(fd.flows Is Nothing, {}, fd.flows)
                Else
                    fd = New MiniMETA.FlowDefinitionTranslation
                    flows = {}
                End If

                Select Case childs(3)
                    Case "choices"
                        If flows.Length > 0 Then
                        End If
                    Case "screens"

                    Case "stages"

                    Case "textTemplates"

                End Select
            End If
        End If
    End Sub

    Sub parsingPromptTranslation(ByRef ps As List(Of MiniMETA.PromptTranslation), ByVal childs() As String, ByVal value As String)
        If Not value.Contains("<!--") And value.Length > 0 Then
            Dim hasPrompt As Boolean = False
            If childs.Length = 3 Then
                For i As Integer = 0 To ps.Count - 1
                    Dim p As MiniMETA.PromptTranslation = ps(i)
                    If p.name = childs(1) Then
                        hasPrompt = True
                        p.description = value
                        ps.Item(i) = p
                    End If
                Next
                If Not hasPrompt Then
                    Dim p As MiniMETA.PromptTranslation = New MiniMETA.PromptTranslation
                    p.name = childs(1)
                    p.description = value
                    ps.Add(p)
                End If
            ElseIf childs.Length = 2 Then
                For i As Integer = 0 To ps.Count - 1
                    Dim p As MiniMETA.PromptTranslation = ps(i)
                    If p.name = childs(1) Then
                        hasPrompt = True
                        p.label = value
                        ps.Item(i) = p
                    End If
                Next
                If Not hasPrompt Then
                    Dim p As MiniMETA.PromptTranslation = New MiniMETA.PromptTranslation
                    p.name = childs(1)
                    p.label = value
                    ps.Add(p)
                End If
            End If
        End If
    End Sub



    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' Common Functions Block
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Function describeMetadata() As DescribeMetadataResult
        Dim api As Double = ThisAddIn.api
        Dim dmr As DescribeMetadataResult

        If setMetaBinding() Then
            dmr = metaClient.describeMetadata(metaSessionHeader, api)
            Return dmr
        End If
        Throw New Exception("describeMetadata Exception, no session")
    End Function

    Function listMetadata(ByVal types As String()) As FileProperties()
        Dim api As Double = ThisAddIn.api
        Dim fileObjs() As FileProperties

        If setMetaBinding() Then
            Dim metaTypes As List(Of ListMetadataQuery) = New List(Of ListMetadataQuery)
            For Each mtype As String In types
                Dim metaType As ListMetadataQuery = New ListMetadataQuery()
                metaType.type = mtype
                metaType.folder = vbNullString
                metaTypes.Add(metaType)
            Next

            fileObjs = metaClient.listMetadata(metaSessionHeader, metaTypes.ToArray(), api)
            Return fileObjs
        End If
        Throw New Exception("listMetadata Exception, no session")
    End Function

    Function readMetadata(ByVal type As String, ByVal fullNames As String()) As Metadata()
        Dim metas() As Metadata

        If setMetaBinding() Then
            metas = metaClient.readMetadata(metaSessionHeader, type, fullNames)
            Return metas
        End If
        Throw New Exception("readMetadata Exception, no session")
    End Function

    Function upsertMetadata(ByVal metadatas As Metadata()) As UpsertResult()
        Dim urs() As UpsertResult

        If setMetaBinding() Then
            urs = metaClient.upsertMetadata(metaSessionHeader, allOrNoneHeader, metadatas.ToArray())
            Return urs
        End If
        Throw New Exception("upsertMetadata Exception, no session")
    End Function

    Function updateMetadata(ByVal metadatas As Metadata()) As SaveResult()
        Dim srs() As SaveResult

        If setMetaBinding() Then
            srs = metaClient.updateMetadata(metaSessionHeader, metadatas.ToArray())
            Return srs
        End If
        Throw New Exception("updateMetadata Exception, no session")
    End Function

    Function setMetaBinding() As Boolean
        If Not checkSession() Then
            If Not LoginToSalesforce() Then GoTo done
        End If

        If checkSession() Then
            If metaClient Is Nothing Then
                metaClient = ThisAddIn.metaClient
                'metaClient = New MetadataPortTypeClient("Metadata", ThisAddIn.conInfo.urls.metadata)
            End If
            If metaSessionHeader Is Nothing Then
                metaSessionHeader = ThisAddIn.metaSessionHeader
                'metaSessionHeader = New MiniMETA.SessionHeader
                'metaSessionHeader.sessionId = ThisAddIn.accessToken
            End If
            allOrNoneHeader = New MiniMETA.AllOrNoneHeader
            allOrNoneHeader.allOrNone = False
            Return True
        End If
done:
        Return False
    End Function

    '' BackgroundWorker implemented routines
    Function getMetaWorkSheet(ByRef workbook As Excel.Workbook, ByVal metaname As String, Optional clear As Boolean = True) As Excel.Worksheet
        Dim currentsheet As Excel.Worksheet = workbook.ActiveSheet
        Try
            Dim find_sheet As Boolean = False
            For Each cs As Excel.Worksheet In workbook.Sheets
                If cs.Name = metaname Then
                    find_sheet = True
                    currentsheet = cs
                    currentsheet.Activate()

                    Dim totalSheets As Integer = workbook.Sheets.Count
                    'CType(ThisAddIn.excelApp.ActiveSheet, Excel.Worksheet).Move(After:=ThisAddIn.excelApp.Worksheets(totalSheets))
                    CType(currentsheet, Excel.Worksheet).Move(After:=workbook.Worksheets(totalSheets))

                    If clear Then
                        'Dim allRange As Excel.Range = ThisAddIn.excelApp.ActiveCell.CurrentRegion
                        Dim allRange As Excel.Range = currentsheet.ActiveCell.CurrentRegion
                        allRange = allRange.Resize(allRange.Rows.Count, allRange.Columns.Count)
                        allRange.Select()
                        currentsheet.Selection.Clear()
                    End If
                End If
            Next
            If Not find_sheet Then
                Dim newsheet As Excel.Worksheet
                newsheet = CType(workbook.Worksheets.Add(), Excel.Worksheet)
                newsheet.Name = metaname
                currentsheet = newsheet
                currentsheet.Activate()
            End If
            'excelApp.ActiveWindow.DisplayGridlines = False

            Return currentsheet
        Catch ex As Exception
            Throw New Exception("getMetaWorkSheet Exception" & vbCrLf & ex.Message)
        End Try

    End Function

    Sub setWorkArea(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet, ByRef m_table As Excel.Range, ByRef m_head As Excel.Range,
                    ByRef m_body As Excel.Range, ByRef m_start As Excel.Range, ByRef m_rows As Integer, ByRef m_metaType As String)
        Try
            excelApp.StatusBar = "build data Ranges"

            Try
                If excelApp.ActiveCell.CurrentRegion.Count = 1 And excelApp.ActiveCell.CurrentRegion.Value Is Nothing Then
                    worksheet.Range("A1").Select()
                    m_table = excelApp.ActiveCell.CurrentRegion
                Else
                    m_table = excelApp.ActiveCell.CurrentRegion
                End If
            Catch ex As Exception
                Throw New Exception("Oops, Could not find an active Worksheet")
            End Try

            m_start = m_table.Cells(1, 1)
            m_rows = m_table.Rows.Count

            If m_table.Rows.Count = 2 Then
                m_body = worksheet.Range(m_table.Cells(3, 1).AddressLocal) ' 5.20
            Else
                m_body = worksheet.Range(m_table.Cells(3, 1), m_table.Cells(m_table.Rows.Count, m_table.Columns.Count))
            End If

            Dim k As Integer
            For k = 1 To m_table.Columns.Count
                If Not IsNothing(m_table.Cells(2, k).Value2) Then
                    m_head = worksheet.Range(m_table.Cells(2, 1), m_table.Cells(2, k))
                End If
            Next k

            m_metaType = m_start.Value
            If (m_metaType = "") Then Throw New Exception("could not locate a metadata name in cell " & m_start.Address & vbCrLf)

            excelApp.StatusBar = "Query " & m_metaType & " table description"

        Catch ex As Exception
            Throw New Exception("setWorkArea Exception" & vbCrLf & ex.Message)
        End Try
done:
    End Sub

    Sub getTranslations(ByRef m_langSet As List(Of String))
        m_langSet = New List(Of String)
        Try
            Dim fileObjs() As FileProperties = listMetadata({"Translations"})
            If fileObjs IsNot Nothing Then
                For Each obj As FileProperties In fileObjs
                    Dim fullName As String = obj.fullName
                    If Not m_langSet.Contains(fullName) Then m_langSet.Add(fullName)
                Next
            End If

        Catch ex As Exception
            'Throw New Exception("getTranslations" & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub setLanguageHeaders(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet,
                           ByRef m_head As Excel.Range, ByRef m_langSet As List(Of String))
        m_head.Select()
        Dim columnCount As Integer = excelApp.Selection.Columns.Count
        Dim addedCol As Integer = 0
        Dim lastCell As Excel.Range = m_head.Cells(1, columnCount)
        If m_langSet.Count > 1 Then m_langSet.Sort()
        For i As Integer = 0 To m_langSet.Count - 1
            Dim notFound As Boolean = True
            For j As Integer = 1 To columnCount
                If m_head.Cells(1, j).Value = m_langSet.Item(i) Then
                    notFound = False
                End If
            Next
            If notFound Then
                addedCol = addedCol + 1
                lastCell.Copy(lastCell.Offset(0, 1))
                lastCell = lastCell.Offset(0, 1)
                lastCell.Value = m_langSet.Item(i)
                lastCell.ColumnWidth = 30
            End If
        Next
        m_head = worksheet.Range(m_head.Cells(1, 1), m_head.Cells(1, columnCount + addedCol))
        m_head.Select()
    End Sub

    Function getLanguageColumn(ByRef m_head As Excel.Range, ByVal lang As String) As Integer
        Dim j As Integer
        For j = 1 To m_head.Count
            With m_head.Cells(1, j)
                Dim apiname As String = .Value
                If apiname.ToLower() = lang.ToLower() Then
                    Return j
                End If
            End With
        Next j
        Return Nothing
    End Function

    Function renderObjectItem(ByRef excelApp As Excel.Application, ByRef m_body As Excel.Range, ByRef m_rows As Long,
                              ByRef m_baseObject As Dictionary(Of String, String), ByRef m_langCol As Long,
                              ByVal keyword As String, ByVal trsnvalue As String) As Boolean
        Dim value As String = ""
        Try
            If m_baseObject.ContainsKey(keyword) Then value = m_baseObject.Item(keyword)
            'Dim myValue As Object = excelApp.WorksheetFunction.VLookup(keyword, m_body, 1, False)
            Dim keyCol As Excel.Range = m_body.Cells(1, 1)
            Dim findRowIdx As Object = excelApp.WorksheetFunction.Match(keyword, keyCol.EntireColumn, False)
            m_body.Cells(findRowIdx - 2, 2) = value
            m_body.Cells(findRowIdx - 2, m_langCol) = trsnvalue
        Catch ex As Exception
            m_body.Cells(m_rows - 1, 1) = keyword
            m_body.Cells(m_rows - 1, 2) = value
            m_body.Cells(m_rows - 1, m_langCol) = trsnvalue
            m_rows = m_rows + 1
            m_body = m_body.Resize(m_body.Rows.Count + 1, m_body.Columns.Count)
        End Try
        Return True
    End Function

    Public Function renderItem(ByRef excelApp As Excel.Application, ByRef m_body As Excel.Range, ByRef m_rows As Long,
                               ByRef m_langCol As Long, ByVal keyword As String, ByVal value As String) As Boolean
        Try
            'Dim myValue As Object = excelApp.WorksheetFunction.VLookup(keyword, m_body, 1, False)
            Dim keyCol As Excel.Range = m_body.Cells(1, 1)
            Dim findRowIdx As Object = excelApp.WorksheetFunction.Match(keyword, keyCol.EntireColumn, False)
            m_body.Cells(findRowIdx - 2, m_langCol) = value
        Catch ex As Exception
            m_body.Cells(m_rows - 1, 1) = keyword
            m_body.Cells(m_rows - 1, m_langCol) = value
            m_rows = m_rows + 1
            m_body = m_body.Resize(m_body.Rows.Count + 1, m_body.Columns.Count)
        End Try
        Return True
    End Function

End Module
