Option Explicit On

Imports System.Windows.Forms

Module TableWizard
    Dim excelApp As Excel.Application
    Dim fields As List(Of RESTful.Field) = New List(Of RESTful.Field)()

    Sub QueryWizard()
        excelApp = ThisAddIn.excelApp

        Dim rng As Excel.Range

WizardStep1:
        ' Wizard Step 1 of 4, Select start position
        rng = WizardStep1()
        If rng Is Nothing Then GoTo done
        rng.Select()

WizardStep2:
        ' Wizard Step 2 of 4, Select SObject to Query
        Dim step2 As frmWizardStep2 = New frmWizardStep2(rng)
        step2.ShowDialog()
        If Not step2.complete Then GoTo done
        If step2.gotoStep1 Then GoTo WizardStep1
        If Not step2.gotoStep3 Then GoTo done

WizardStep3:
        ' Wizard Step 3 of 4, Select Fields to Include
        Dim step3 As frmWizardStep3 = New frmWizardStep3(fields, rng)
        step3.ShowDialog()
        If Not step3.complete Then GoTo done
        If step3.gotoStep2 Then GoTo WizardStep2
        If Not step3.gotoStep4 Then GoTo done
        ' Draw fields onto the sheet
        drawWizard(rng, excelApp, fields.ToArray())

        ' Wizard Step 4 of 4, Add Query Clauses
        Dim step4 As frmWizardStep4 = New frmWizardStep4(step3.mapField, rng)
        step4.ShowDialog()
        If Not step4.complete Then GoTo done

        QueryData()

done:
    End Sub

    Function WizardStep1() As Excel.Range
        Dim rnData As Excel.Range
        On Error Resume Next
        rnData = excelApp.InputBox("Where do you want to put the Sforce Table Query?", "Table Query Wizard - Step 1 of 4", "$A$1", Type:=8)
        Err.Clear()
        If rnData Is Nothing Then Return Nothing
        WizardStep1 = rnData.Cells(1, 1)
    End Function

    Sub drawWizard(ByRef rng As Excel.Range, ByRef excelApp As Excel.Application, ByRef flds As RESTful.Field())
        Try
            Dim table As Excel.Range
            Dim start As Excel.Range
            Dim objName As String
            Dim pos As Integer = 0

            table = excelApp.ActiveCell.CurrentRegion
            start = table.Cells(1, 1)
            objName = start.Value

            With table
                For Each fld As RESTful.Field In flds
                    If fld.name = "Id" Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

                For Each fld As RESTful.Field In flds
                    If IsRequired(fld) Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

                For Each fld As RESTful.Field In flds
                    If IsNameField(fld) Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

                For Each fld As RESTful.Field In flds
                    If IsStandard(fld) Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

                For Each fld As RESTful.Field In flds
                    If IsCustom(fld) Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

                For Each fld As RESTful.Field In flds
                    If IsReadOnly(fld) Then
                        pos = drawField(start.Offset(1, pos), fld, pos)
                    End If
                Next

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Function drawField(cel As Excel.Range, fld As RESTful.Field, pos As Integer) As Integer
        cel.Value = fld.label
        cel.WrapText = True

        ' Clear it out and left over comments
        If Not (cel.Comment Is Nothing) Then cel.Comment.Delete()

        Dim commentStr As String
        Dim commentHeight As Integer = 60

        commentStr = "API Name: " & fld.name & vbCrLf
        If Not fld.updateable Then commentStr = commentStr & "Read Only Field" & vbCrLf
        If IsRequired(fld) Then commentStr = commentStr & "Required on Insert" & vbCrLf
        If fld.name = "Id" Then commentStr = commentStr & "Primary Object Identifier" & vbCrLf

        Dim fieldType As String = fld.type

        Select Case fieldType
            Case "picklist", "multipicklist"
                commentStr = commentStr & "Type: " & fieldType & vbCrLf
                For Each pickval As RESTful.PicklistEntry In fld.picklistValues
                    commentStr = commentStr & pickval.value & vbCrLf
                Next
                Dim h As Integer = (fld.length * 12) + 1
                If h > 60 Then commentHeight = h
            Case Else
                commentStr = commentStr & "Type: " & fieldType & vbCrLf
        End Select

        If commentStr = "" Then Return 0
        cel.AddComment()
        cel.Comment.Text(commentStr)
        'cel.Comment.Shape.Height = commentHeight
        'cel.Comment.Shape.TextFrame.Characters.Font.Name = "Consolas"
        cel.Comment.Shape.TextFrame.Characters.Font.Bold = False
        cel.Comment.Shape.TextFrame.AutoSize = True
        drawField = pos + 1
        Return drawField
    End Function

End Module