Option Explicit On

Imports Microsoft.Office.Interop.Excel
Imports System.Diagnostics
Imports System.Threading.Tasks
Imports System.Windows.Forms

Module Util

    Public Function checkSession() As Boolean
        If ThisAddIn.accessToken <> "" And ThisAddIn.id <> "" Then
            Try
                Dim result As RESTful.ConnectionInfo = RESTAPI.getConnectionInfo()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End If
        Return False
    End Function

    Public Function IsRequired(fld As RESTful.Field) As Boolean
        IsRequired = fld.name <> "Id" And fld.nillable = False And fld.defaultedOnCreate = False And fld.createable = True
    End Function

    Public Function IsNameField(fld As RESTful.Field) As Boolean
        IsNameField = fld.nameField And Not IsRequired(fld) And Not (fld.custom) And fld.updateable
    End Function

    Public Function IsStandard(fld As RESTful.Field) As Boolean
        IsStandard = Not IsNameField(fld) And Not IsRequired(fld) And Not (fld.custom) And fld.updateable
    End Function

    Public Function IsCustom(fld As RESTful.Field) As Boolean
        IsCustom = fld.custom And fld.updateable And Not IsRequired(fld)
    End Function

    Public Function IsReadOnly(fld As RESTful.Field) As Boolean
        IsReadOnly = fld.name <> "Id" And Not (fld.updateable) And Not IsRequired(fld)
    End Function

    Public Function IsHyperlink(fld As RESTful.Field, val As String) As Boolean
        If (fld.type <> "string") Then Return False
        If (Left(val, 5) = "_HL1_") Then Return True
        Return False
    End Function

    Public Function IsObject(obj As Object) As Boolean
        If obj.Item("attributes") IsNot Nothing Then Return True
        Return False
    End Function

    Public Sub AddHyperlink(cel, val) ' trim down the value
        Dim link As String = Right(val, Len(val) - 5)
        link = Left(link, Len(link) - 5)
        Dim p As Integer = InStr(link, "_HL2_")
        If p > 0 Then
            cel.value = Mid(link, p + 5)
            worksheet.Hyperlinks.Add(cel, Left(link, p - 1))
        End If
    End Sub
    '
    Public Sub displayUserName(uname As String)
        Globals.Ribbons.ForceRibbon.ribbonForceConnector.Label = ThisAddIn.ribbonBoxName & " (" & uname & ")"
    End Sub

    Public Sub ErrorBox(ByVal msg As String)
        TopMostMessageBox.Show("Error", msg, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    '
    ' we may need to trim the range down to valid ID's ?
    '
    ' trim top and bottom of this range to try to capture the region of valid
    ' object id's
    ' if we are given a range like "A:A" we can be smart by removing the
    ' top invalid items and triming the blank cells at the tail of the range
    Public Function build_ref_range(str As String) As Excel.Range
        Dim t As Excel.Range = Nothing, r As Excel.Range = Nothing
        On Error Resume Next
        r = worksheet.Range(str) ' if this is not a valid range description send a msg
        If r Is Nothing Then GoTo done ' Range method did not work
        On Error GoTo done

        For Each c As Excel.Range In r
            If (c Is Nothing) Then GoTo done
            If IsError(c) Then GoTo done
            Select Case Len(c.Value)
                Case 15, 18
                    ' sometimes a text string like 'opportunity id' will be just
                    ' 15 or 18 long, to avoid adding this, check that the string we
                    ' are looking at has some numeric chars and is not all alpha.
                    If c.Value Like "*[0-9][0-9]*" Then    ' two adjacent numbers
                        If (t Is Nothing) Then t = c ' special case first time thru
                        t = worksheet.Union(t, c) ' normal case, extend the range down
                    End If
            End Select
        Next c

        ' check that the range is made of one area...
        If t.Areas.Count > 1 Then
            MsgBox("Range " & t.Address & " is made of more than one area")
        End If

done:
        Return t
    End Function

    '
    ' slightly different than above for query strings
    ' SFDC_escape_q
    Public Function escapeQueryString(s As String) As String
        Dim InI As Integer
        '  "&|!()[]^""~*?:'" should really deal with all of these, just lazy i guess
        For InI = 1 To Len(s) Step 1
            ' Debug.Print Mid(s, InI, 1): Debug.Print Asc(Mid(s, InI, 1))
            Select Case Asc(Mid(s, InI, 1))
                Case 39 ' this is the tick ->'<-
                    s = Left(s, InI - 1) & Chr(92) & Chr(39) & Right(s, Len(s) - InI)
                    InI = InI + 1
            End Select

        Next InI

        Return Trim(s)
    End Function

    '
    ' adjust the format of the value for types as expected by API
    ' sfQueryValueFormat
    Public Function QueryValueFormat(typ, vlu) As String
        Select Case typ
            Case "datetime", "date"
                '
                ' 5.12 allow strings like
                '   today, today - 1 , today - 150, today + 30
                ' to be translated into vba dates for the query...
                '
                If (InStr(LCase(vlu), "today")) Then
                    Dim today As Date
                    today = New Date()
                    Dim daychange As Object, incr As Integer
                    incr = 0
                    If (InStr(LCase(vlu), "-")) Then
                        daychange = Split(vlu, "-")
                        incr = 0 - Int(daychange(1))
                    End If
                    If (InStr(LCase(vlu), "+")) Then
                        daychange = Split(vlu, "+")
                        incr = Int(daychange(1))
                    End If
                    vlu = DateAdd("d", incr, today)
                End If ' 5.12 end

                Return Format(vlu, "yyyy-mm-ddTHH:MM:SS.000Z")

            Case "double", "currency", "percent"  ' add percent per Scot S. 5.67
                If (InStr(vlu, ".")) Then
                    Return Val(vlu) ' if the double has a decimal already, dont need to add .0
                Else
                    Return Val(vlu) & ".0"
                End If
            Case "boolean"
                Return IIf((Val(vlu) Or "true" = LCase(vlu)), "TRUE", "FALSE")

            Case "int" ' 6.11 by scot stony
                Return vlu

            Case Else ' all which look like string, including but not limited to
                Return "'" & vlu & "'" ' string, picklist, id, reference, textarea, combobox email

        End Select
    End Function

    Public Function getFieldMap(ByVal field As RESTful.Field()) As Dictionary(Of String, RESTful.Field)
        Dim fields As Dictionary(Of String, RESTful.Field) = New Dictionary(Of String, RESTful.Field)
        For Each fld As RESTful.Field In field
            fields.Add(fld.name, fld)
        Next

        Return fields
    End Function

    Public Function getAPINameFromCell(ByVal cell As Excel.Range) As String
        Return getAPIName(cell.Comment.Text)
    End Function

    Public Function getAPIName(ByVal commentText As String) As String
        Dim idx As Integer = If(commentText.IndexOf(vbCrLf) = -1, commentText.Length, commentText.IndexOf(vbCrLf))
        Return commentText.Substring(10, idx - 10)
    End Function

    Public Function typeToFormat(sfType As String) As String
        typeToFormat = "General" ' default
        Select Case sfType

            Case "date", "datetime" ' re-written for 5.66
                typeToFormat = "yyyy-mm-dd"
                If (sfType = "datetime") Then
                    typeToFormat = typeToFormat + " hh:mm" ' 5.15
                End If
            Case "string", "picklist", "phone" ' , "textarea"
                typeToFormat = "@"
            Case "currency"
                typeToFormat = "$#,##0_);($#,##0)" ' format as currency, no cents (added in 5.15)

        End Select

    End Function

    Public Function IsMissing(ByVal cond As Object) As Boolean
        If cond = True Then Return True Else Return False

    End Function

    '
    ' Converts a 15 character ID to an 18 character, case-insensitive one ...
    ' got this one from sforce community
    ' thanks go to Scot Stoney
    '
    Public Function FixID(InID As String) As String
        FixID = ""
        If Len(InID) = 18 Then
            FixID = InID
            Exit Function
        End If
        Dim InChars As String, InI As Integer, InUpper As String
        Dim InCnt As Integer
        InChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345"
        InUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        InCnt = 0
        For InI = 15 To 1 Step -1
            InCnt = 2 * InCnt + Math.Sign(InStr(1, InUpper, Mid(InID, InI, 1), vbBinaryCompare))
            If InI Mod 5 = 1 Then
                FixID = Mid(InChars, InCnt + 1, 1) + FixID
                InCnt = 0
            End If
        Next InI
        FixID = InID + FixID
    End Function

    '
    ' look at the field type and variant type, cast the into a return value
    '
    Public Function toVBtype(cel As Excel.Range, field As RESTful.Field) As Object
        ' special case, must set value to an uninitialized variant
        ' to indicate to the COM toolkit that we want to "nil" the field
        ' toolkit translates this to passing "fieldsToNull" in the actual SOAP request
        If String.IsNullOrEmpty(cel.Value) Then
            Return vbNullChar
        End If

        Select Case field.type
            Case "int"
                Dim i As Integer
                i = Int(Val(cel.Value))
                toVBtype = i

            Case "percent"
                toVBtype = Val(cel.Value) ' normal case

            Case "double", "currency"
                ' val() does not use i18n conventions, use CDbl instead, 6.08
                toVBtype = CDbl(cel.Value)  ' normal case
                ' 6.01 truncate to the number of digits, Field3 likes it's numbers formated
                If (field.scale = 0) Then
                    toVBtype = Int(toVBtype)
                Else ' If (field.Scale > 0) Then
                    Dim z As Integer = InStr(cel.Value, excelApp.International(XlApplicationInternational.xlDecimalSeparator))
                    If (z > 0) Then  ' need to remove any extra decimal places
                        toVBtype = CDbl(Left(cel.Value, z + field.scale))
                    End If
                End If

            Case "datetime", "date"
                If cel.Value.GetType().Name = "DateTime" Then
                    Dim dt As DateTime = cel.Value.ToString
                    toVBtype = dt.ToString("s")
                Else
                    toVBtype = cel.Value
                End If
            Case "boolean"
                toVBtype = cel.Value
            Case "reference"
                ' deal with user names in a reference id field here 5.29
                ' and record types, and others that ref_id can deal with 5.34
                ' need to map a name into the actual ID prior to passing to update
                ' ref_id routine will return the passed in value if we don't map
                ' the ReferenceTo type provided (User,Group,Profile... etc) as a fallback
                toVBtype = cel.Value
                If Len(toVBtype) = 15 Or Len(toVBtype) = 18 Then
                    If field.referenceTo.Length > 0 Then _
                        toVBtype = NameToId(cel.Value, field.referenceTo(0)) ' how to check for multi reference type???????????????
                Else
                    Throw New Exception("Invalid Id format for " & field.name)
                End If
            Case Else ' all other types (so far),  work with this "string" type
                toVBtype = "" & cel.Value.ToString()

        End Select
    End Function

    Public Sub ScrollAtBottom(ByRef win As Excel.Window, ByVal outrow As Long)
        Dim sr As Long
        sr = outrow - win.VisibleRange.Rows.Count + 2
        If sr < 1 Then sr = 1
        win.ScrollRow = sr
    End Sub

    ' given a users name or string name of a reference type
    ' return the id or any other case or
    ' if it's not a reference at all, ref_to will be nul, make that case
    '  return the name_string, this is working in 5.46
    Public Function NameToId(name_string As String, objectName As String) As String
        If Not RegQueryBoolValue(USE_REFERENCE) Then Return name_string

        Dim names As String()
        Dim records As Object(), record As Object

        Select Case Len(name_string)
            Case 15, 18 ' length matches, and two adjacent numbers, looks like an ID 5.51
                ' kick out now or we may turn a real ID into a name string
                If (name_string Like "*[0-9][0-9]*") Then Return name_string
        End Select

        On Error Resume Next
        Select Case objectName
            Case "User"
                If ThisAddIn.UserNames.ContainsKey(name_string) Then
                    Return ThisAddIn.UserNames.Item(name_string)
                End If
                names = Split(name_string, " ")
                records = QueryAll("SELECT FirstName, LastName, Id FROM " &
                                    "User WHERE FirstName = '" & names(0) & "' AND LastName = '" & names(1) & "'")
                For Each record In records
                    Dim refId = record.Item("Id")
                    ThisAddIn.UserNames.Add(refId, name_string)
                    ThisAddIn.UserNames.Add(name_string, refId)
                Next record

                If ThisAddIn.UserNames.ContainsKey(name_string) Then
                    Return ThisAddIn.UserNames.Item(name_string)
                Else
                    Return name_string
                End If

            ' use of this type of reference should be controled by an option
            ' it could cause a serious performance problem on long queries
            ' and is not going to return unique strings if there are dups in the database
            ' should check for dups!!! and then return the ID passed in rather than guessing.
            ' as it does now TODO
            Case "RecordType"
                If ThisAddIn.RecordTypes.ContainsKey(name_string) Then
                    Return ThisAddIn.RecordTypes.Item(name_string)
                End If
                records = QueryAll("SELECT Id, Name FROM RecordType WHERE Name = '" & name_string & "'")
                For Each record In records
                    Dim refId = record.Item("Id")
                    ThisAddIn.RecordTypes.Add(refId, name_string)
                    ThisAddIn.RecordTypes.Add(name_string, refId)
                Next record

                If ThisAddIn.RecordTypes.ContainsKey(name_string) Then
                    Return ThisAddIn.RecordTypes.Item(name_string)
                Else
                    Return name_string
                End If

            Case "Profile"
                If ThisAddIn.Profiles.ContainsKey(name_string) Then
                    Return ThisAddIn.Profiles.Item(name_string)
                End If
                records = QueryAll("SELECT Id, Name FROM Profile WHERE Name = '" & name_string & "'")
                For Each record In records
                    Dim refId = record.Item("Id")
                    ThisAddIn.Profiles.Add(refId, name_string)
                    ThisAddIn.Profiles.Add(name_string, refId)
                Next record

                If ThisAddIn.Profiles.ContainsKey(name_string) Then
                    Return ThisAddIn.Profiles.Item(name_string)
                Else
                    Return name_string
                End If
            Case "Group"
                If ThisAddIn.Groups.ContainsKey(name_string) Then
                    Return ThisAddIn.Groups.Item(name_string)
                End If
                records = QueryAll("SELECT Id, Name FROM Group WHERE Name = '" & name_string & "'")
                For Each record In records
                    Dim refId = record.Item("Id")
                    ThisAddIn.Groups.Add(refId, name_string)
                    ThisAddIn.Groups.Add(name_string, refId)
                Next record

                If ThisAddIn.Groups.ContainsKey(name_string) Then
                    Return ThisAddIn.Groups.Item(name_string)
                Else
                    Return name_string
                End If

            Case "UserRole"
                If ThisAddIn.Roles.ContainsKey(name_string) Then
                    Return ThisAddIn.Roles.Item(name_string)
                End If
                records = QueryAll("SELECT Id, Name FROM UserRole WHERE Name = '" & name_string & "'")
                For Each record In records
                    Dim refId = record.Item("Id")
                    ThisAddIn.Roles.Add(refId, name_string)
                    ThisAddIn.Roles.Add(name_string, refId)
                Next record

                If ThisAddIn.Roles.ContainsKey(name_string) Then
                    Return ThisAddIn.Roles.Item(name_string)
                Else
                    Return name_string
                End If
                ' and we arrive here for not a ref_to at all in 5.46
            Case Else ' 5.37 don't know how to map this type, so restore the value passed in
                Return name_string ' assume it was correct and we got called by mistake.

        End Select

    End Function

    '
    ' lookup ID and return the string name and add the name to a dict
    ' works on user id's and record types currently, can be extended
    ' for roles, profiles, groups, etc.
    '
    Public Function IdToName(objectid As String) As String
        If Not RegQueryBoolValue(USE_REFERENCE) Then Return objectid

        Dim records As Object(), record As Object
        Dim keyPrefixes As String() = {"005", "012", "00e", "00G", "00E"}
        Dim prefix As String = Left(objectid, 3)

        If objectid = "" Then Return "" ' we get here for Converted Account Id in leads which are not converted...
        If Not keyPrefixes.Contains(prefix) Then Return objectid

        On Error Resume Next
        Select Case prefix
            Case "005" ' User
                ' would be nice to look up and return the org info here...
                ' since this is located int the session object we can pull it
                If ThisAddIn.UserNames.ContainsKey(objectid) Then
                    Return ThisAddIn.UserNames.Item(objectid)
                End If
                records = QueryAll("SELECT FirstName, LastName, Id FROM User WHERE Id = '" & objectid & "' ")
                For Each record In records
                    Debug.Assert(objectid = record.Item("Id")) ' better be true
                    Dim refName = record.Item("FirstName") & " " & record.Item("LastName")
                    ThisAddIn.UserNames.Add(record.Item("Id"), refName)
                    ThisAddIn.UserNames.Add(refName, record.Item("Id"))
                Next record

                If ThisAddIn.UserNames.ContainsKey(objectid) Then
                    Return ThisAddIn.UserNames.Item(objectid)
                Else
                    Return objectid
                End If
            Case "012" ' RecordType
                If ThisAddIn.RecordTypes.ContainsKey(objectid) Then
                    Return ThisAddIn.RecordTypes.Item(objectid)
                End If
                records = QueryAll("SELECT Id, Name FROM RecordType WHERE Id = '" & objectid & "'")
                For Each record In records
                    Dim refName = record.Item("Name")
                    ThisAddIn.RecordTypes.Add(record.Item("Id"), refName)
                    ThisAddIn.RecordTypes.Add(refName, record.Item("Id"))
                Next record

                If ThisAddIn.RecordTypes.ContainsKey(objectid) Then
                    Return ThisAddIn.RecordTypes.Item(objectid)
                Else
                    Return objectid
                End If
            Case "00e" ' Profile
                If ThisAddIn.Profiles.ContainsKey(objectid) Then
                    Return ThisAddIn.Profiles.Item(objectid)
                End If
                records = QueryAll("SELECT Id, Name FROM Profile WHERE Id = '" & objectid & "'")
                For Each record In records
                    Dim refName = record.Item("Name")
                    ThisAddIn.Profiles.Add(record.Item("Id"), refName)
                    ThisAddIn.Profiles.Add(refName, record.Item("Id"))
                Next record

                If ThisAddIn.Profiles.ContainsKey(objectid) Then
                    Return ThisAddIn.Profiles.Item(objectid)
                Else
                    Return objectid
                End If
            Case "00G" ' Group
                If ThisAddIn.Groups.ContainsKey(objectid) Then
                    Return ThisAddIn.Groups.Item(objectid)
                End If
                records = QueryAll("SELECT Id, Name FROM Group WHERE Id = '" & objectid & "'")
                For Each record In records
                    Dim refName = record.Item("Name")

                    ' 5.40 sometimes the group name is empty stash the id instead of an empty name
                    If (record.Item("Name") = "") Then Return record.Item("Id")

                    ThisAddIn.Groups.Add(record.Item("Id"), refName)
                    ThisAddIn.Groups.Add(refName, record.Item("Id"))
                Next record

                If ThisAddIn.Groups.ContainsKey(objectid) Then
                    Return ThisAddIn.Groups.Item(objectid)
                Else
                    Return objectid
                End If
            Case "00E" ' UserRole
                If ThisAddIn.Roles.ContainsKey(objectid) Then
                    Return ThisAddIn.Roles.Item(objectid)
                End If
                records = QueryAll("SELECT Id, Name FROM UserRole WHERE Id = '" & objectid & "'")
                For Each record In records
                    Dim refName = record.Item("Name")
                    ThisAddIn.Roles.Add(record.Item("Id"), refName)
                    ThisAddIn.Roles.Add(refName, record.Item("Id"))
                Next record

                If ThisAddIn.Roles.ContainsKey(objectid) Then
                    Return ThisAddIn.Roles.Item(objectid)
                Else
                    Return objectid
                End If
            Case Else
                Return objectid ' too small, throw it back

        End Select

    End Function

    Public Function QueryAll(ByVal q As String) As Object()
        Dim qrs As RESTful.QueryResult = RESTAPI.Query(q)
        Dim records As List(Of Object) = New List(Of Object)
        Dim no_more As Boolean = False ' check can use QueryResult.done

        Try
            If qrs.totalSize > 0 Then
                records.Concat(qrs.records)
            End If

            Do Until qrs.done
                Try
                    qrs = RESTAPI.QueryMore(qrs.nextRecordsUrl)
                    If qrs.totalSize > 0 Then
                        records.Concat(qrs.records)
                    End If
                Catch ex As Exception
                    GoTo done
                End Try
            Loop
        Catch ex As Exception
            MsgBox(ex.Message, Title:="Salesforce.QueryAll Error")
        End Try
done:
        Return records.ToArray()
    End Function
End Module
