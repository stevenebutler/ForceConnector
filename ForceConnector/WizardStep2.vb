Imports System.Windows.Forms
Public Class frmWizardStep2
    Public complete As Boolean = False
    Public gotoStep1 As Boolean = False
    Public gotoStep3 As Boolean = False
    Dim rng As Excel.Range

    Dim objects As List(Of Dictionary(Of String, String)) = New List(Of Dictionary(Of String, String))
    Dim standards() As String = {"Account", "Campaign", "Case", "Contact", "Contract", "Event",
        "Lead", "Opportunity", "Pricebook2", "Product2", "Profile", "Quote", "Task", "User", "UserRole"}

    Public Sub New(ByRef r As Excel.Range)
        rng = r
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim lgr As RESTful.DescribeGlobalSObjectResult() = RESTAPI.getSObjectList()

        With Me.lstObject
            .Columns.Clear()
            .View = View.Details
            .MultiSelect = False
            .Columns.Add("sObject Name", 200, HorizontalAlignment.Left)
            .Columns.Add("sObject API Name", 265, HorizontalAlignment.Left)
            For Each gr As RESTful.DescribeGlobalSObjectResult In lgr
                Dim obj = New Dictionary(Of String, String) From {
                    {"name", gr.name},
                    {"label", gr.label},
                    {"custom", gr.custom}
                }
                objects.Add(obj)
                If standards.Contains(gr.name) Or gr.custom Then
                    Dim line As New ListViewItem()
                    line.Text = gr.label
                    line.SubItems.Add(gr.name)
                    .Items.Add(line)
                End If
            Next
        End With
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        gotoStep1 = True
        complete = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If lstObject.SelectedItems Is Nothing Then
            MessageBox.Show("You must select a object!")
        Else
            Dim itm As ListViewItem = lstObject.SelectedItems.Item(0)
            rng.Value = itm.SubItems(0).Text
            rng.AddComment() 'error when already has comment
            rng.Comment.Text(itm.SubItems(1).Text)
            rng.Comment.Shape.Height = 20
            rng.Comment.Shape.Width = 200

            gotoStep3 = True
            complete = True
            Me.Close()
        End If
    End Sub

    Private Sub chkCustom_CheckedChanged(sender As Object, e As EventArgs) Handles chkCustom.CheckedChanged
        resetList("")
    End Sub

    Private Sub chkStandard_CheckedChanged(sender As Object, e As EventArgs) Handles chkStandard.CheckedChanged
        resetList("")
    End Sub

    Private Sub chkSystem_CheckedChanged(sender As Object, e As EventArgs) Handles chkSystem.CheckedChanged
        resetList("")
    End Sub

    Private Sub resetList(ByVal search As String)
        Dim objs() As Dictionary(Of String, String) = objects.ToArray()
        lstObject.Clear()

        With Me.lstObject
            .Columns.Clear()
            .View = View.Details
            .MultiSelect = False
            .Columns.Add("sObject Name", 200, HorizontalAlignment.Left)
            .Columns.Add("sObject API Name", 265, HorizontalAlignment.Left)
            For Each obj As Dictionary(Of String, String) In objs
                Dim label As String = obj.Item("label")
                Dim value As String = obj.Item("name")

                If label.IndexOf(search, 0, StringComparison.CurrentCultureIgnoreCase) > -1 _
                    Or value.IndexOf(search, 0, StringComparison.CurrentCultureIgnoreCase) > -1 Then

                    If chkStandard.Checked And standards.Contains(value) Then
                        Dim line As New ListViewItem()
                        line.Text = label
                        line.SubItems.Add(value)
                        .Items.Add(line)
                    End If
                    If chkCustom.Checked And obj.Item("custom") = True Then
                        Dim line As New ListViewItem()
                        line.Text = label
                        line.SubItems.Add(value)
                        .Items.Add(line)
                    End If
                    If chkSystem.Checked Then
                        If Not standards.Contains(value) And obj.Item("custom") = False Then
                            Dim line As New ListViewItem()
                            line.Text = label
                            line.SubItems.Add(value)
                            .Items.Add(line)
                        End If
                    End If
                End If
            Next
            lstObject.Sort()
        End With
    End Sub

    Private Sub lstObject_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lstObject.ColumnClick
        Select Case e.Column
            Case 0
                If lstObject.Columns.Item(0).ListView.Sorting <> SortOrder.Descending Then
                    lstObject.Columns.Item(0).ListView.Sorting = SortOrder.Descending
                ElseIf lstObject.Columns.Item(0).ListView.Sorting <> SortOrder.Ascending Then
                    lstObject.Columns.Item(0).ListView.Sorting = SortOrder.Ascending
                End If
            Case 1
                If lstObject.Columns.Item(1).ListView.Sorting <> SortOrder.Descending Then
                    lstObject.Columns.Item(1).ListView.Sorting = SortOrder.Descending
                ElseIf lstObject.Columns.Item(1).ListView.Sorting <> SortOrder.Ascending Then
                    lstObject.Columns.Item(1).ListView.Sorting = SortOrder.Ascending
                End If
        End Select
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim searchText As String = txtSearch.Text
        If searchText.Length > 2 Then
            resetList(searchText)
        End If

    End Sub
End Class