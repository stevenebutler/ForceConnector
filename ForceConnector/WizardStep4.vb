Imports System.Data
Imports System.Windows.Forms

Public Class frmWizardStep4
    Public complete As Boolean = False

    Dim mapField As Dictionary(Of String, RESTful.Field)
    Dim rng As Excel.Range
    Dim rngOrigin As Excel.Range
    Dim objectLabel As String
    Dim objectName As String

    Public Sub New(ByRef mapField As Dictionary(Of String, RESTful.Field), ByVal rng As Excel.Range)
        Dim fieldTable As New DataTable()
        With fieldTable.Columns
            .Add("Label", GetType(String))
            .Add("Value", GetType(String))
        End With

        ' This call is required by the designer.
        InitializeComponent()

        Me.mapField = mapField
        Me.rng = rng
        Me.rngOrigin = rng
        Me.objectLabel = rng.Value
        Me.objectName = rng.Comment.Text

        ' Add any initialization after the InitializeComponent() call.
        Dim keys As Dictionary(Of String, RESTful.Field).KeyCollection = mapField.Keys
        For Each key As String In keys
            Dim fld As RESTful.Field = mapField(key)
            fieldTable.Rows.Add(fld.label & " (" & fld.name & ")", fld.name)
            'cmbField.Items.Add(key)
        Next
        cmbField.DisplayMember = "Label"
        cmbField.ValueMember = "Value"
        cmbField.DataSource = fieldTable

        lstClause.View = View.Details
        lstClause.Columns.Add("Field", 300, HorizontalAlignment.Left)
        lstClause.Columns.Add("Operator", 200, HorizontalAlignment.Left)
        lstClause.Columns.Add("Value", 300, HorizontalAlignment.Left)
        lstClause.Columns.Add("Field API", 0, HorizontalAlignment.Left)

        'With Me.lstClause
        '    ' this clause make MALFORMED ERROR
        '    Dim line As New ListViewItem()
        '    line.Text = "IsDeleted"
        '    line.SubItems.Add("not equal")
        '    line.SubItems.Add("True")
        '    line.SubItems.Add("IsDeleted")
        '    .Items.Add(line)
        'End With
    End Sub

    Private Sub btnAddClause_Click(sender As Object, e As EventArgs) Handles btnAddClause.Click
        Dim dt As DataTable = cmbField.DataSource
        Dim dr As DataRow = dt.Rows.Item(cmbField.SelectedIndex)
        If cmbField.SelectedValue Is Nothing Or cmbOperator.SelectedItem Is Nothing Then
            MessageBox.Show("You must select the Field and Operator.")
            Exit Sub
        End If

        AddClause(dr, cmbOperator.SelectedItem, txtValue.Text)
    End Sub

    Private Sub btnClearClause_Click(sender As Object, e As EventArgs) Handles btnClearClause.Click
        Dim selItems As ListView.SelectedListViewItemCollection = Me.lstClause.SelectedItems

        If selItems.Count > 0 Then
            For Each itm As ListViewItem In selItems
                Me.lstClause.Items.Remove(itm)
            Next
        End If
    End Sub

    Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
        Me.lstClause.Clear()

        rngOrigin.EntireRow.Clear()
        rngOrigin.EntireRow.ClearComments()
        rngOrigin.Value = objectLabel
        rngOrigin.AddComment() 'error when already has comment
        rngOrigin.Comment.Text(objectName)
        rngOrigin.Comment.Shape.Height = 20
        rngOrigin.Comment.Shape.Width = 200
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnRunQuery_Click(sender As Object, e As EventArgs) Handles btnRunQuery.Click
        rngOrigin.EntireRow.Clear()
        rngOrigin.EntireRow.ClearComments()
        rngOrigin.Value = objectLabel
        rngOrigin.AddComment() 'error when already has comment
        rngOrigin.Comment.Text(objectName)
        rngOrigin.Comment.Shape.Height = 20
        rngOrigin.Comment.Shape.Width = 200

        Dim listItems As ListView.ListViewItemCollection = Me.lstClause.Items
        If listItems.Count > 0 Then
            For Each itm As ListViewItem In listItems
                rng.Offset(0, 1).Value = itm.SubItems(0).Text
                rng.Offset(0, 1).AddComment()
                rng.Offset(0, 1).Comment.Text(itm.SubItems(3).Text)
                rng.Offset(0, 1).Comment.Shape.Height = 20
                rng.Offset(0, 1).Comment.Shape.Width = 200
                rng.Offset(0, 2).Value = itm.SubItems(1).Text
                If itm.SubItems(2).Text IsNot Nothing Then
                    rng.Offset(0, 3).Value = itm.SubItems(2).Text
                End If

                rng = rng.Offset(0, 3)
            Next
        Else
            rng.Offset(0, 1).Value = "RECORD ID"
            rng.Offset(0, 1).AddComment()
            rng.Offset(0, 1).Comment.Text("Id")
            rng.Offset(0, 1).Comment.Shape.Height = 20
            rng.Offset(0, 1).Comment.Shape.Width = 200
            rng.Offset(0, 2).Value = "not equals"
            rng.Offset(0, 3).Value = ""
        End If

        complete = True
        Me.Close()
    End Sub

    Sub AddClause(dr As DataRow, op As Object, val As String)
        With Me.lstClause
            Dim line As New ListViewItem()
            line.Text = dr.ItemArray(0)
            line.SubItems.Add(op)
            If val IsNot Nothing Then
                line.SubItems.Add(val)
            Else
                line.SubItems.Add("")
            End If
            line.SubItems.Add(dr.ItemArray(1))
            .Items.Add(line)
        End With
    End Sub

End Class