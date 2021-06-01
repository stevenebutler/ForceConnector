Imports System.Collections
Imports System.Windows.Forms

Public Class frmWizardStep3
    Dim fields As List(Of RESTful.Field)
    Dim rng As Excel.Range
    Public mapField As New Dictionary(Of String, RESTful.Field)

    Public complete As Boolean = False
    Public gotoStep2 As Boolean = False
    Public gotoStep4 As Boolean = False

    Public Sub New(ByRef flds As List(Of RESTful.Field), ByRef rng As Excel.Range)
        Me.fields = flds
        Me.rng = rng
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim objectName As String = rng.Comment.Text
        Dim gr As RESTful.DescribeSObjectResult = RESTAPI.DescribeSObject(objectName)

        With Me.lstField
            .View = View.Details
            .MultiSelect = True
            .Columns.Add("Field Label", 200, HorizontalAlignment.Left)
            .Columns.Add("Field API Name", 265, HorizontalAlignment.Left)
            For Each fld As RESTful.Field In gr.fields
                mapField.Add(fld.name, fld)    ' if sObject have duplicated label, issue the error here, use api name instead of label
                Dim line As New ListViewItem()
                line.Text = fld.label
                line.SubItems.Add(fld.name)
                line.Selected = True
                .Items.Add(line)

            Next
            .FullRowSelect = True
        End With
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        gotoStep2 = True
        complete = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        fields.Clear()
        Dim hasId As Boolean = False

        If lstField.SelectedItems Is Nothing Then
            MessageBox.Show("You must select the fields!")
            GoTo done
        Else
            Dim lfield As ListView.SelectedListViewItemCollection = lstField.SelectedItems

            For Each itm As ListViewItem In lfield
                Dim apiname As String = itm.SubItems(1).Text
                fields.Add(mapField(apiname))
                If apiname.ToLower() = "id" Then hasId = True
            Next

            If Not hasId Then GoTo notHasId
            gotoStep4 = True
            complete = True
            Me.Close()
            GoTo done
        End If
notHasId:
        MsgBox("Selected field set must include Id field!")
        GoTo done
done:
    End Sub

End Class