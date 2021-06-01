Imports System.Windows.Forms

Public Class TranslationObjectSelect
    'Public objectList() As String
    Public selectedList As List(Of String) = New List(Of String)

    Public Sub New(ByVal objectList() As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.'=
        With Me.lstObject
            .View = View.Details
            .MultiSelect = True
            .Columns.Add("sObject Name", 310, HorizontalAlignment.Left)
            For Each obj As String In objectList
                Dim line As New ListViewItem()
                line.Text = obj
                .Items.Add(line)
            Next
        End With
    End Sub

    Private Sub TranslationObjectSelect_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If lstObject.SelectedItems Is Nothing Then
            MessageBox.Show("You must select a object!")
        Else
            For Each itm As ListViewItem In lstObject.SelectedItems
                selectedList.Add(itm.Text)
            Next
            Me.Close()
        End If
    End Sub
End Class