Option Explicit On

Imports System.Collections

Module Operation

    Public Sub QueryData()
        Try
            Dim frm As processDatabaseQueryTable = New processDatabaseQueryTable()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="QueryData Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub UpdateCells()
        If (RegQueryBoolValue(SKIPHIDDEN)) Then
            Call UpdateCells_New()
            Exit Sub
        End If

        Try
            Dim frm As processDatabaseUpdateRows = New processDatabaseUpdateRows()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="UpdateCells Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub UpdateCells_New()
        Try
            Dim frm As processDatabaseUpdateRowsNew = New processDatabaseUpdateRowsNew()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="UpdateCells Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Public Sub InsertRows()
        Try
            Dim frm As processDatabaseInsertRows = New processDatabaseInsertRows()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="InsertRows Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub QueryRows()
        Try
            Dim frm As processDatabaseQuerySelectedRows = New processDatabaseQuerySelectedRows()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="InsertRows Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    Sub DeleteRecords()
        Try
            Dim frm As processDatabaseDeleteRows = New processDatabaseDeleteRows()
            frm.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.Message, Title:="InsertRows Exception" & vbCrLf & ex.Message)
        End Try
    End Sub

    '******************************************************************************
    '******************************************************************************
    '* Operation
    '******************************************************************************
    '******************************************************************************

    '******************************************************************************
    '* Query Data Part
    '******************************************************************************
    Public Function BuildQueryString(ByRef excelApp As Excel.Application, ByRef g_table As Excel.Range, ByRef g_start As Excel.Range,
                                ByRef g_header As Excel.Range, ByRef refIds As Excel.Range, ByRef joinfield As String, ByRef oneeachrow As Boolean,
                                ByRef fieldMap As Dictionary(Of String, RESTful.Field), ByRef sels As String, ByRef where As String,
                                ByRef statusText As String) As Boolean
        With g_table
            ' remove old contents with value, formatting, comments
            If (.Rows.Count > 2) Then
                .Offset(2, 0).Resize(.Rows.Count - 2, .Columns.Count).Select()
                excelApp.Selection.Clear()
            End If
            g_start.Select()

            ' build-up field list to query
            sels = getSelectionList(g_header) ' it works with g_header only

            Dim api, opr, vlu As String, jw As Integer
            Dim field_obj As RESTful.Field
            oneeachrow = False  ' for "on" joins
            jw = 2

            ' build-up where statements
            Do While (.Cells(1, jw).value <> "") ' if it's not empty, assume its more query

                api = .Cells(1, jw).Comment.Text ' the field api name, value is label
                opr = .Cells(1, jw + 1).value ' the operator
                vlu = .Cells(1, jw + 2).value ' the criteria value(s)
                field_obj = fieldMap.Item(api) ' 5.46 get the field as an object

                ' operator
                ' add other aliases here if you like
                opr = LCase(opr) : Select Case opr
                    Case "equals" : opr = "="
                    Case "contains" : opr = "like"
                    Case "not equals" : opr = "!="
                    Case "less than" : opr = "<"
                    Case "greater than" : opr = ">"
                End Select

                ' 5.23 basic error check, 5.0 checks for this anyway but we know where the offending cell is
                If (opr = "like" And field_obj.type = "picklist") Then
                    statusText = "like (or contains) operator in cell " & g_table.Cells(1, jw + 1).AddressLocal &
                         " is not valid on picklist fields, " & vbCrLf & " use --> equals, not equals"
                    GoTo errors
                End If

                ' special case 'in' and a ref field
                If ((opr = "in" Or opr = "on") And field_obj.type = "reference") Then
                    If (opr = "on") Then oneeachrow = True
                    refIds = build_ref_range(vlu) ' list of IDs to use in join
                    joinfield = field_obj.name ' save for later, should be only one..

                    If refIds Is Nothing Then
                        statusText = "Range error, could not build a valid range from the string" &
                             vbCrLf & "--> " & vlu & " <--" & vbCrLf & " in the cell " & .Cells(1, jw + 2).AddressLocal &
                             "expected valid range (ex: 'A:A') or range name"
                        GoTo errors
                    End If

                Else
                    ' general case
                    ' Value ~ assemble the where clause using field, opr and values list
                    '   this loop has been re-written (ver 5.04) to properly
                    '   deal with comma seperated values i.e. -> field | operator |this,that|
                    '   should become (field operator 'this' OR field operator 'that')
                    '   unless it's multipicklist type then produce slightly different string for SOQL:
                    '     (field inclqudes ('this') or field includes ('that'))
                    '     (field excludes ('this') or field excludes ('that'))
                    '
                    Dim values As Object
                    values = Split(vlu, ",")
                    ' if values is empty and vlu is the nul string, still need to assemble the clause
                    Dim vu, clause As String
                    clause = ""
                    If (UBound(values) < 0 And vlu = "") Then ' case of one empty value
                        clause = field_obj.name & " " & opr & "''"
                        If field_obj.type = "date" Then ' special case, compare value is an empty date 5.49
                            clause = field_obj.name & " " & opr & " null"
                        End If
                    Else
                        For Each vu In values ' works for one or many non nul values

                            Dim str As String, referTo As String = ""
                            str = vu
                            str = escapeQueryString(str) ' escape some chars
                            If field_obj.referenceTo IsNot Nothing And field_obj.referenceTo.Length > 0 Then
                                referTo = field_obj.referenceTo(0)
                            End If
                            vu = NameToId(str, referTo) ' map strs to refid's 5.46
                            If Len(clause) > 0 Then clause = clause & " or " ' prepend an or
                            If opr Like "like" Then vu = "%" & vu & "%"  ' wrap like string with wildcard
                            If opr Like "begins with" Or opr Like "starts with" Then vu = vu & "%" ' wildcar at front
                            If opr Like "ends with" Then vu = "%" & vu ' wlidcard at end
                            If opr Like "regexp" Then opr = "like"  ' pass the user provided wildcard
                            Dim fmtVal As String
                            fmtVal = QueryValueFormat(field_obj.type, vu) ' format value for SOQL
                            If (field_obj.type = "multipicklist") Then fmtVal = "(" & fmtVal & ")" ' special case

                            If (opr Like "starts with" Or opr Like "begins with" Or opr Like "ends with") Then  ' remap these to 'like'
                                clause = clause & field_obj.name & " " & "Like" & " " & fmtVal '**** thanks to tim_bouscal!
                            Else
                                clause = clause & field_obj.name & " " & opr & " " & fmtVal ' assemble the clause
                            End If

                        Next vu
                    End If

                    If (UBound(values) > 0) Then clause = "(" & clause & ")" ' cant hurt
                    where = where & clause ': Debug.Print where
                    If (.Cells(1, jw + 3).value <> "") Then where = where & " and " ' to be ready for more

                End If

                jw = jw + 3 ' slide over to grab the next three cells

            Loop ' end loop while we have more WHERE clauses to add

        End With ' g_table
        GoTo done

errors:
        Return False
done:
        Return True
    End Function

    ' do the query and draw the rows we got back
    Public Function queryDataDraw(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet,
                                  ByRef g_header As Excel.Range, ByRef g_body As Excel.Range, ByRef g_ids As Excel.Range,
                                  ByRef g_objectType As String, ByRef g_sfd As RESTful.DescribeSObjectResult,
                                  ByVal sels As String, ByVal where As String, ByVal outrow As Long, ByVal totals As Long,
                                  ByRef bgw As ComponentModel.BackgroundWorker) As Long
        Dim queryData As RESTful.QueryResult
        Dim recordList As ArrayList = New ArrayList()
        Dim no_more As Boolean = False
        Dim statusBarText = "Select Data From " & g_objectType
        excelApp.StatusBar = Left(statusBarText, 128)

        Try
            queryData = RESTAPI.Query("SELECT " & sels & " FROM " & g_objectType & " " & where)

            If queryData.totalSize = 0 Then
                ' output something, like... "#N/F"
                g_body.Cells(outrow, g_ids.Column - g_body.Column + 1).value = "#N/F"
                GoTo done
            End If

            recordList.AddRange(queryData.records)
            Dim size As Integer = If(recordList.Count >= 50, 50, recordList.Count)
            Dim so As Object
            Do Until no_more
                Dim records As Object() = recordList.GetRange(0, size).ToArray()
                recordList.RemoveRange(0, size)
                recordList.TrimToSize()

                excelApp.ScreenUpdating = False
                For Each so In records
                    Call formatWriteRow(worksheet, g_header, g_body, g_sfd, so, outrow)
                    outrow = outrow + 1

                    If outrow > excelLimit Then
                        Throw New Exception("Can not retrieve over than " & excelLimit.ToString() & " rows by Excel limitation.")
                    End If

                    Dim percent As Integer = CInt((outrow / totals) * 100)
                    If percent > 100 Then percent = 100
                    bgw.ReportProgress(percent, "Download records (" & outrow.ToString("N0") & " / " & totals.ToString("N0") & ")")

                    '' check at regular intervals for CancellationPending
                    If bgw.CancellationPending Then
                        bgw.ReportProgress(percent, "Cancelling...")
                        Exit For
                    End If
                Next so

                excelApp.ScreenUpdating = True
                ' 0.1 second delay for screen drawing, without it screen will be updated at next loop.
                Threading.Thread.Sleep(100)

                '' check at regular intervals for CancellationPending
                If bgw.CancellationPending Then Exit Do

                If recordList.Count < 50 Then
                    Try
                        If Not queryData.done Then
                            queryData = RESTAPI.QueryMore(queryData.nextRecordsUrl)
                            If queryData.totalSize = 0 Then
                                no_more = True
                                size = recordList.Count
                            Else
                                recordList.AddRange(queryData.records)
                                size = recordList.Count
                            End If
                        Else
                            If recordList.Count = 0 Then no_more = True
                            size = recordList.Count
                        End If
                    Catch ex As Exception
                        no_more = True
                    End Try
                End If

                ScrollAtBottom(excelApp.ActiveWindow, outrow)
            Loop

            so = Nothing

        Catch ex As Exception
            g_body.Cells(outrow, g_ids.Column - g_body.Column + 1).value = "#Err"
            excelApp.ScreenUpdating = True
            Throw New Exception("queryDataDraw Exception" & vbCrLf & ex.Message)
        End Try
done:
        Return outrow
    End Function

    '******************************************************************************
    '* Update Selected Rows Part
    '******************************************************************************
    Public Function UpdateLimitCheck(ByRef s As Excel.Range, ByRef statusText As String) As Boolean
        If (s.Areas.Count > 1) Then
            statusText = "Cannot run on multiple selections"
            Return False
        End If

        If (RegQueryBoolValue(NOLIMITS)) Then Return True

        ' adjust these limits to meet your requirements, or flip NOLIMITS in the options dialog
        If (s.Rows.Count > maxRows Or s.Columns.Count > maxCols) Then
            statusText = "Selection too large, cannot run on > " & maxRows.ToString() & "rows and > " & maxCols.ToString() & " cols"
            Return False
        End If

        Return True
    End Function

    Public Sub updateRange(ByRef excelApp As Excel.Application, ByRef g_header As Excel.Range, ByRef g_objectType As String,
                           ByRef g_start As Excel.Range, ByRef g_sfd As RESTful.DescribeSObjectResult,
                           ByRef g_ids As Excel.Range, ByRef todo As Excel.Range, ByRef someFailed As Boolean,
                           ByRef row_counter As Long, ByRef totals As Long, ByRef bgw As ComponentModel.BackgroundWorker)
        Dim fieldMap As Dictionary(Of String, RESTful.Field) = getFieldMap(g_sfd.fields)
        Dim srMap As Dictionary(Of String, RESTful.SaveResult) = New Dictionary(Of String, RESTful.SaveResult)
        Dim recordSet As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
        Dim idlist As List(Of String) = New List(Of String)
        Dim rec As List(Of Object) = New List(Of Object)
        Dim srs() As RESTful.SaveResult

        Try
            Dim c As Excel.Range
            For Each c In excelApp.Intersect(g_ids, todo.EntireRow)
                idlist.Add(FixID(c.Value))
            Next c
            If idlist.Count = 0 Then GoTo done ' how ?

            Dim percent As Integer = CInt((row_counter / totals) * 100)
            bgw.ReportProgress(percent, "Building record block from row " & row_counter.ToString("N0"))

            Dim i As Integer = 0
            For Each id In idlist.ToArray

                recordSet = New Dictionary(Of String, Object)
                recordSet.Add("attributes", New RESTful.Attributes(g_objectType))
                recordSet.Add("Id", id)

                For Each j In todo.Columns
                    'field name
                    Dim fld As String = getAPINameFromCell(g_header.Cells(1, 1 + j.Column - g_start.Column))
                    Dim field As RESTful.Field = fieldMap.Item(fld)
                    ' only updatable columns add to recordSet
                    If Not field.updateable Then GoTo nextcol
                    Dim target As Excel.Range
                    target = todo.Offset(i, j.Column - todo.Column)
                    recordSet.Add(fld, toVBtype(target.Cells(1, 1), field))
nextcol:
                Next j
                ' if recordSet does not contains any updatable columns, cancel update
                If rec.Count = 0 And recordSet.Count = 2 Then
                    Throw New Exception("No updatable columns selected, operation canceled.")
                End If

                rec.Add(recordSet)
                i = i + 1
            Next id

            srs = RESTAPI.UpdateRecords(rec.ToArray)
            For Each sr As RESTful.SaveResult In srs
                srMap.Add(sr.id, sr)
            Next

            percent = CInt(((row_counter + i) / totals) * 100)
            bgw.ReportProgress(percent, "Updating (" & i.ToString() & ") records from row " & row_counter.ToString("N0"))
            row_counter = row_counter + i

            updateResultHandler(excelApp, todo, someFailed, rec, srMap)

        Catch ex As Exception
            fieldMap = Nothing
            srMap = Nothing
            srs = Nothing
            Throw New Exception("updateRange Exception" & vbCrLf & ex.Message)
        End Try
done:
        fieldMap = Nothing
        srMap = Nothing
        srs = Nothing
    End Sub

    Private Sub updateResultHandler(ByRef excelApp As Excel.Application, ByRef todo As Excel.Range, ByRef someFailed As Boolean,
                                    ByVal rec As List(Of Object), ByVal srMap As Dictionary(Of String, RESTful.SaveResult))
        Dim r As Object
        Dim s As RESTful.SaveResult
        Dim i As Integer = 0
        For Each r In rec.ToArray
            s = srMap.Item(r.Item("Id"))
            Dim thisrow As Excel.Range, firstcel As Excel.Range
            thisrow = excelApp.Intersect(todo.Offset(i, 1).EntireRow, todo)
            firstcel = thisrow.Offset(0, 0).Cells(1, 1)

            ' find out what is wrong with this record
            If Not s.success Then
                'Debug.Print r.ErrorMessage
                ' turns out that if one field fails, the entire row fails
                thisrow.Interior.ColorIndex = 6
                For Each c In thisrow.Cells
                    If Not (c.Comment Is Nothing) Then c.Comment.Delete()
                Next c
                firstcel.AddComment()
                Dim errMsg As String = "Update Row Failed:" & Chr(10)
                For Each err As RESTful.SalesforceError In s.errors
                    errMsg = errMsg & err.statusCode & ", " & err.message & Chr(10)
                Next
                firstcel.Comment.Text(errMsg)
                firstcel.Comment.Shape.Height = 60 ' is this enough
                someFailed = True ' will message this later
            Else
                ' clear out the color on this row only
                ' also remove any comments which may now be incorrect
                ' for this entire row, need to clear on each col of the selection
                thisrow.Interior.ColorIndex = 0
                For Each c In thisrow.Cells
                    If Not (c.Comment Is Nothing) Then c.Comment.Delete()
                Next c
            End If
            i = i + 1
        Next r
    End Sub

    Public Sub calcUpdateRange(ByRef xlSelection As Excel.Range, ByRef totalRow As Long, ByRef totalCol As Long,
                               ByVal blnSkipHidden As Boolean, ByVal blnNoLimits As Boolean)
        Dim lngRows As Long
        Dim lngCols As Long
        Dim lngHiddenRows As Long
        Dim lngHiddenCols As Long

        With xlSelection
            lngRows = .Rows.Count
            lngCols = .Columns.Count
        End With

        '// have we set the "Skip hidden fields" option?
        If blnSkipHidden = True Then
            '// count hidden rows
            For Each xlRow In xlSelection.Rows
                If xlRow.Hidden Then lngHiddenRows = lngHiddenRows + 1
            Next xlRow
            '// count hidden columns
            For Each xlColumn In xlSelection.Columns
                If xlColumn.Hidden Then lngHiddenCols = lngHiddenCols + 1
            Next xlColumn
        End If

        totalRow = lngRows - lngHiddenRows
        totalCol = lngCols - lngHiddenCols

        '// have we set the "Disregard reasonable limits" option?
        If Not blnNoLimits Then
            '// let's see if the selection is within the confines
            If totalRow > maxRows Then
                Throw New Exception("You can't process more than " & maxRows & " rows.")
            End If
            If totalCol > maxCols Then
                Throw New Exception("You can't process more than " & maxCols & " columns.")
            End If
        End If
    End Sub

    Public Sub updateResultHandlerNew(ByRef worksheet As Excel.Worksheet, ByRef intFailedRows As Long,
                                       ByVal records As List(Of Object), ByVal strArryCells As String())

        Dim srMap As Dictionary(Of String, RESTful.SaveResult) = New Dictionary(Of String, RESTful.SaveResult)
        Dim srs As RESTful.SaveResult()
        Dim xlTempRow As Excel.Range
        Dim xlCell As Excel.Range
        Dim rec As Object()

        '// now, let's do the update
        rec = records.ToArray()
        srs = RESTAPI.UpdateRecords(rec)

        For Each sr As RESTful.SaveResult In srs
            srMap.Add(sr.id, sr)
        Next

        Dim i As Integer = 0
        For Each r In rec.ToArray
            '// check if the update was okay
            Dim sr As RESTful.SaveResult = srMap.Item(r.Item("Id"))
            xlTempRow = worksheet.Range(strArryCells(i))
            If Not sr.success Then
                xlTempRow.Interior.ColorIndex = 6
                For Each xlCell In xlTempRow.Cells
                    If Not (xlCell.Comment Is Nothing) Then xlCell.Comment.Delete()
                Next xlCell
                With xlTempRow.Cells(1, 1)
                    Dim errMsg As String = "Update Row Failed:" & vbLf
                    For Each err As RESTful.SalesforceError In sr.errors
                        errMsg = errMsg & err.statusCode & ", " & err.message & vbLf
                    Next
                    .AddComment
                    .Comment.Text(errMsg)
                    .Comment.Shape.Height = 60
                End With
                intFailedRows = intFailedRows + 1
            Else
                xlTempRow.Interior.ColorIndex = 0
                For Each xlCell In xlTempRow.Cells
                    If Not (xlCell.Comment Is Nothing) Then xlCell.Comment.Delete()
                Next xlCell
            End If
            i = i + 1
        Next r
    End Sub

    '******************************************************************************
    '* Insert Selected Rows Part
    '******************************************************************************
    Sub insertSelectedRange(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet, ByRef g_table As Excel.Range,
                            ByRef g_header As Excel.Range, ByRef g_sfd As RESTful.DescribeSObjectResult, ByRef g_objectType As String,
                            ByRef g_ids As Excel.Range, ByRef todo As Excel.Range, ByRef someFailed As Boolean,
                            ByRef row_counter As Long, ByRef totals As Long, ByRef bgw As ComponentModel.BackgroundWorker)

        Dim fieldMap As Dictionary(Of String, RESTful.Field) = getFieldMap(g_sfd.fields)
        Dim xlSelection As Excel.Range
        Dim records As List(Of Object) = New List(Of Object)
        Dim recarray As List(Of String) = New List(Of String)

        Try
            xlSelection = excelApp.Selection

            Dim row_pointer As Long = todo.Row - xlSelection.Row
            Dim percent As Integer = CInt((row_counter / totals) * 100)
            Dim msg As String = (row_pointer + 1).ToString("N0") & " -> " & (row_pointer + todo.Rows.Count).ToString("N0") &
                                " of " & xlSelection.Rows.Count.ToString("N0")

            bgw.ReportProgress(percent, "Create the record block :" & msg)

            Dim IDcol As Integer

            todo.Interior.ColorIndex = 36 ' show where we are working

            Dim rw As Excel.Range
            For Each rw In todo.Rows
                ' don't insert if there is no "new" label in the id column
                If Not (objectid(excelApp, worksheet, g_ids, rw.Row, True) Like "[nN][eE][wW]*") Then GoTo nextrow

                Dim attributes As RESTful.Attributes = New RESTful.Attributes(g_objectType)
                Dim record As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
                record.Add("attributes", attributes)

                Dim j As Integer
                For j = 1 To g_header.Count
                    Dim name As String
                    name = getAPINameFromCell(g_header.Cells(1, j))
                    If name <> "Id" Then
                        ' don't overwrite the id on this row, needs to be empty when passed to create
                        ' find the field, TODO i have a routine to find the field, could refactor this loop.
                        Dim fld As RESTful.Field = fieldMap.Item(name)

                        ' excelApp.StatusBar = "loading value for " & name
                        Dim celVal As String = CStr(g_table.Cells(rw.Row + 1 - g_table.Row, j).value)
                        If Not String.IsNullOrEmpty(celVal) Then
                            ' here we have a value check it and load it into the fld value
                            ' 5.10 dont load field values unless the field is createable
                            If (fld.createable) Then
                                Dim fieldValue As Object = toVBtype(g_table.Cells(rw.Row + 1 - g_table.Row, j), fld)
                                If fld.type = "reference" Then
                                    record.Add(name, NameToId(fieldValue, fld.referenceTo(0)))
                                Else
                                    record.Add(name, fieldValue)
                                End If
                            End If
                        End If
                    Else
                        IDcol = j ' save this location for later
                        recarray.Add(g_table.Cells(rw.Row + 1 - g_table.Row, IDcol).Address)
                    End If

                Next j

                ' if recordSet does not contains any updatable columns, cancel update
                If records.Count = 0 And record.Count = 2 Then
                    Throw New Exception("No insertable columns selected, operation canceled.")
                End If

                records.Add(record)
                row_counter = row_counter + 1
nextrow:
            Next rw

            If records.Count < 1 Then  ' no records to insert
                ErrorBox("No records to Insert in this block, enter the string 'New' on one or more rows")
                todo.Interior.ColorIndex = 0 ' clear out color
                GoTo done
            End If

            bgw.ReportProgress(percent, "Insert the record block :" & msg)

            insertResultHandler(excelApp, worksheet, records, recarray, todo, someFailed)

        Catch ex As Exception
            Throw New Exception("insertSelectedRange Exception" & vbCrLf & ex.Message)
        End Try
done:
    End Sub

    Private Sub insertResultHandler(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet,
                                    ByRef records As List(Of Object), ByRef recarray As List(Of String),
                                    ByRef todo As Excel.Range, ByRef someFailed As Boolean)

        Dim srs As RESTful.SaveResult()
        srs = RESTAPI.CreateRecords(records.ToArray())
        todo.Interior.ColorIndex = 0 ' clear out color

        Dim sr As RESTful.SaveResult

        Dim i As Integer = 0
        For i = 0 To UBound(recarray.ToArray())
            sr = srs(i)
            Dim firstcel As Excel.Range = worksheet.Range(recarray.Item(i))
            Dim thisrow As Excel.Range
            thisrow = excelApp.Intersect(firstcel.EntireRow, todo)

            ' find out what is wrong with this record
            If Not sr.success Then
                'Debug.Print r.ErrorMessage
                ' turns out that if one field fails, the entire row fails
                thisrow.Interior.ColorIndex = 6
                For Each c In thisrow.Cells
                    If Not (c.Comment Is Nothing) Then c.Comment.Delete()
                Next c
                Try
                    firstcel.AddComment()
                Catch ex As Exception
                    firstcel.ClearComments()
                    firstcel.AddComment()
                End Try
                Dim errMsg As String = "Insert Row Failed:" & Chr(10)
                For Each err As RESTful.SalesforceError In sr.errors
                    errMsg = errMsg & err.statusCode & ", " & err.message & Chr(10)
                Next
                firstcel.Comment.Text(errMsg)
                firstcel.Comment.Shape.Height = 60 ' is this enough
                someFailed = True ' will message this later
            Else
                firstcel.Value = sr.id
                ' clear out the color on this row only
                ' also remove any comments which may now be incorrect
                ' for this entire row, need to clear on each col of the selection
                thisrow.Interior.ColorIndex = 0
                For Each c In thisrow.Cells
                    If Not (c.Comment Is Nothing) Then c.Comment.Delete()
                Next c
            End If
        Next i ' 5.43 end
        ' no return value from this func
    End Sub

    '******************************************************************************
    '* Query Selected Rows Part
    '******************************************************************************
    Function querySelectedRow(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet,
                              ByRef g_header As Excel.Range, ByRef g_body As Excel.Range, ByRef g_ids As Excel.Range,
                              ByRef g_objectType As String, ByRef g_sfd As RESTful.DescribeSObjectResult,
                              ByVal sels As String, ByRef todo As Excel.Range,
                              ByRef outrow As Long, ByRef totals As Long, ByRef bgw As ComponentModel.BackgroundWorker) As Boolean
        Try
            Dim flds() As String = sels.Replace(" ", "").Split(",")
            Dim percent As Integer = 0
            Dim i As Integer = 0
            Dim idlist() As String
            ReDim idlist(todo.Rows.Count - 1)

            percent = CInt((outrow / totals) * 100)
            If percent > 100 Then percent = 100
            bgw.ReportProgress(percent, "Download " & todo.Count.ToString() & " records from row " & outrow.ToString("N0"))

            For Each rw As Excel.Range In todo.Rows
                idlist(i) = objectid(excelApp, worksheet, g_ids, rw.Row, True)
                i = i + 1
            Next rw

            Dim qrs() As Object = RESTAPI.RetrieveRecords(g_objectType, idlist, flds)

            Dim sd As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            For Each qr As Object In qrs
                If Not sd.ContainsKey(qr.Item("Id")) Then
                    sd.Add(qr.Item("Id"), qr)
                End If
            Next

            For Each rw In todo.Rows
                Dim so As Object = sd.Item(objectid(excelApp, worksheet, g_ids, rw.Row, True))

                Call formatWriteRow(worksheet, g_header, g_body, g_sfd, so, rw.row - 2, False)

                outrow = outrow + 1
                percent = CInt((outrow / totals) * 100)
                If percent > 100 Then percent = 100
                bgw.ReportProgress(percent, "Write record (" & outrow.ToString("N0") & " / " & totals.ToString("N0") & ")")
            Next rw

            sd = Nothing
            qrs = Nothing
            Return True
        Catch ex As Exception
            Throw New Exception("querySelectedRow Exception" & vbCrLf & ex.Message)
        End Try
        Return False
    End Function

    '******************************************************************************
    '* Delete Selected Rows Part
    '******************************************************************************
    Function deleteSelectedRange(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet,
                                 ByRef g_ids As Excel.Range, ByRef g_objectType As String,
                                 ByRef todo As Excel.Range)
        Try
            Dim idlist() As String
            Dim drMap As Dictionary(Of String, RESTful.DeleteResult) = New Dictionary(Of String, RESTful.DeleteResult)

            ReDim idlist(todo.Rows.Count - 1)
            Dim i As Integer = 0, rw As Excel.Range
            For Each rw In todo.Rows
                idlist(i) = objectid(excelApp, worksheet, g_ids, rw.Row, True)
                i = i + 1
            Next rw

            Dim drs() As RESTful.DeleteResult = RESTAPI.DeleteRecords(g_objectType, idlist)
            Dim dr As RESTful.DeleteResult
            For Each dr In drs
                If dr.id IsNot Nothing Then
                    drMap.Add(dr.id, dr)
                End If
            Next

            ' TODO when one fails we don't need to skip all of the rest...
            ' do it like the create calls?

            For Each rw In todo.Rows ' draw results
                If drMap.ContainsKey(rw.Cells(1, 1).value) Then
                    excelApp.Intersect(g_ids, worksheet.Rows(rw.Row)).Value = "deleted"
                Else
                    rw.Cells(1, 1).AddComment()
                    Dim errMsg As String = "Delete Row Failed"
                    rw.Cells(1, 1).Comment.Text(errMsg)
                    rw.Cells(1, 1).Comment.Shape.Height = 60 ' is this enough
                End If
            Next rw
            Return True
        Catch ex As Exception
            Throw New Exception("deleteSelectedRange Exception" & vbCrLf & ex.Message)
        End Try
        Return False
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' Common Functions Block
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ''' <summary>
    ''' set global ranges, labels for the current active region
    ''' used by most other calls which operate on a Range of data
    ''' except sfDescribe which creates a default table layout
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' 
    Function setDataRanges(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet, ByRef g_table As Excel.Range, ByRef g_start As Excel.Range,
                        ByRef g_header As Excel.Range, ByRef g_body As Excel.Range, ByRef g_objectType As String,
                        ByRef g_ids As Excel.Range, ByRef g_sfd As RESTful.DescribeSObjectResult, ByRef statusText As String) As Boolean
        Dim result As Boolean = False
        Try
            excelApp.StatusBar = "build data ranges..."

            Try
                g_table = excelApp.ActiveCell.CurrentRegion
            Catch ex As Exception
                statusText = "Oops, Could not find an active Worksheet"
                GoTo errors
            End Try

            g_start = g_table.Cells(1, 1)
            ' see how many rows we have before setting body... 5.20
            If g_table.Rows.Count = 2 Then
                ' body is going to be outside the table... so place it where we need
                g_body = worksheet.Range(g_table.Cells(3, 1).AddressLocal) ' 5.20
            Else
                g_body = worksheet.Range(g_table.Cells(3, 1), g_table.Cells(g_table.Rows.Count, g_table.Columns.Count))
            End If

            ' trim the g_header Range down if g_table.columns.count is greater than
            ' the number of non blank cells in row 2 !!
            Dim k As Integer
            For k = 1 To g_table.Columns.Count
                If Not IsNothing(g_table.Cells(2, k).Value2) And Not IsNothing(g_table.Cells(2, k).Comment) Then
                    If g_table.Cells(2, k).Comment.Text.Substring(0, 8) = "API Name" Then ' expand the Range to hold non blank cells
                        g_header = worksheet.Range(g_table.Cells(2, 1), g_table.Cells(2, k))
                    End If
                End If
            Next k

            g_objectType = g_start.Comment.Text
            If (g_objectType = "") Then
                statusText = "could not locate a object name in cell " & g_start.Address & vbCrLf &
                     "use Describe Sforce Object menu item to select a valid object"
                GoTo errors
            End If

            excelApp.StatusBar = "Query " & g_objectType & " table description"

            Dim gcol As Integer = getObjectIdColumn(g_header, statusText)
            If gcol = Nothing Then GoTo errors

            g_ids = excelApp.Intersect(g_body, g_body.Columns(gcol))
            g_sfd = RESTAPI.DescribeSObject(g_objectType)

        Catch ex As Exception
            statusText = "set_Range Exception" & vbCrLf & ex.Message
        End Try
        '  Debug.Print "gbody is at " & g_body.AddressLocal
        GoTo done
errors:
        Return False
done:
        Return True
    End Function

    Function getObjectIdColumn(ByRef g_header As Excel.Range, ByRef statusText As String) As Integer ' have a map of labels and one is the id
        Dim j As Integer : For j = 1 To g_header.Count
            With g_header.Cells(1, j)
                Dim apiname As String = getAPIName(.Comment.Text)
                If apiname.ToLower() = "id" Then
                    Return j
                End If
            End With
        Next j

        statusText = "no Object Id found in the column header row"
        Return Nothing
    End Function

    Function getSelectionList(ByRef g_header As Excel.Range) As String
        Dim c As Excel.Range, sels As String = ""
        For Each c In g_header.Cells
            Dim apiname As String = getAPIName(c.Comment.Text)
            If apiname <> "" Then
                sels = sels & apiname & ", "
            End If
        Next c
        sels = RTrim(sels)
        If (Mid(sels, Len(sels), 1) = ",") Then sels = Left(sels, Len(sels) - 1)  ' remove the final comma

        Return sels
    End Function

    Function objectid(ByRef excelApp As Excel.Application, ByRef worksheet As Excel.Worksheet, ByRef g_ids As Excel.Range,
                      ByVal row As Object, Optional quiet As Boolean = True) As String
        Dim tempId As String = ""
        Dim t As Excel.Range = excelApp.Intersect(g_ids, worksheet.Rows(row))
        On Error Resume Next
        tempId = t.Value
        If Len(tempId) = 15 Then
            tempId = FixID(tempId)
        ElseIf LCase(tempId) = "new" Then
            tempId = tempId ' no change
        ElseIf Len(tempId) < 15 Then
            '  Debug.Print tempId
            If quiet Then MsgBox("unrecognized object id >" & tempId & "<")
        End If
        ' Debug.Print "object id is " & tempId
        Return tempId
    End Function

    Sub formatWriteRow(ByRef worksheet As Excel.Worksheet, ByRef g_header As Excel.Range, ByRef g_body As Excel.Range,
                       ByRef g_sfd As RESTful.DescribeSObjectResult, ByVal so As Object, ByVal row As Integer, Optional isInsert As Boolean = True)

        Dim fields As Dictionary(Of String, RESTful.Field) = getFieldMap(g_sfd.fields)
        Dim maxRowHght : maxRowHght = worksheet.StandardHeight * 3

        With g_body
            For j As Integer = 1 To g_header.Count
                Dim name As String, fmt As String, rheight As Integer
                name = getAPINameFromCell(g_header.Cells(1, j))
                Dim field As RESTful.Field = fields.Item(name)

                ' for query selected row, skip writing the Id column
                If Not isInsert And field.type = "id" Then GoTo nextcol

                fmt = typeToFormat(field.type)
                rheight = .Cells(row, j).RowHeight  ' before height

                ' map owner id to names (5.29)
                ' only do this if the option flag is set... or should it be default
                ' if querybool(SPELL_USERNAME) then ...
                '
                If field.type = "reference" Then
                    .Cells(row, j).value = IdToName(so.Item(name))
                Else
                    ' need to preserve text fields as text in excel or we may
                    ' lose any leading zeros... !!!
                    ' therefore we need to respect the field type here
                    ' gotcha: the format must be set both before and after as the value
                    ' assignment appears to trump some formats
                    .Cells(row, j).NumberFormat = fmt
                    ' 6.02 by MO'L
                    ' Check type. Do not trim if it's a date or datetime as this will convert the date to text and lose international formatting: MO'L
                    Select Case field.type
                        Case "date", "datetime"
                            .Cells(row, j).value = so.Item(name)
                        Case "address"
                            If so.Item(name) IsNot Nothing Then
                                If so.Item(name).count = 0 Then Exit Select
                                Dim addr As Dictionary(Of String, Object) = so.Item(name)
                                Dim full_address As String = ""
                                full_address = addr.Item("street") & ", " & addr.Item("city") & ", " & addr.Item("state") & " " & addr.Item("postalCode") & ", " & addr.Item("country")
                                .Cells(row, j).value = full_address
                            End If
                        Case "location"
                            If so.Item(name) IsNot Nothing Then
                                If so.Item(name).count = 0 Then Exit Select
                                Dim loc As Dictionary(Of String, Object) = so.Item(name)
                                Dim location As String = ""
                                location = loc.Item("latitude") & ", " & loc.Item("longitude")
                                .Cells(row, j).value = location
                            End If
                        Case Else
                            .Cells(row, j).value = Left(so.Item(name), 32767)
                    End Select
                    .Cells(row, j).NumberFormat = fmt  ' some formats like to be applied after

                    If TypeOf so.Item(name) Is String Then
                        If IsHyperlink(field, so.Item(name)) Then Call AddHyperlink(.Cells(row, j), so.Item(name)) ' 6.09
                    End If

                    ' do something about the auto resizing, just to try to avoid blowup in long text
                    ' fields as they are loaded into the cells, but dont mess with it if the user
                    ' has set a height first
                    If .Cells(row, j).RowHeight > maxRowHght And .Cells(row, j).RowHeight > rheight + 1 Then
                        .Cells(row, j).RowHeight = maxRowHght ' set some default max
                    End If

                End If
nextcol:

            Next j
        End With
    End Sub
End Module