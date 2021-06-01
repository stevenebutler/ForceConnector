Imports System.Data
Imports System.Windows.Forms

Public Class frmObjectList
    Public objectList As List(Of String) = New List(Of String)
    Public langs As List(Of String)
    Public baseLanguage As String = ThisAddIn.userLang

    Public success As Boolean = False

    Dim objects As List(Of Dictionary(Of String, String)) = New List(Of Dictionary(Of String, String))
    Dim standards() As String = {"Account", "Campaign", "Case", "Contact", "Contract", "Event",
        "Lead", "Opportunity", "Pricebook2", "Product2", "Profile", "Quote", "Task", "User", "UserRole"}

    Private Sub frmObjectList_Load(sender As Object, e As EventArgs) Handles Me.Load
        setLanguage()

        If ThisAddIn.usingRESTful Then
            Dim lgr As RESTful.DescribeGlobalSObjectResult() = RESTAPI.getSObjectList()
            With Me.lstObject
                .View = View.Details
                .MultiSelect = True
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
                .Sort()
            End With
        Else
            Dim lgr As Partner.DescribeGlobalSObjectResult() = SOAPAPI.getSObjectList()
            With Me.lstObject
                .View = View.Details
                .MultiSelect = True
                .Columns.Add("sObject Name", 200, HorizontalAlignment.Left)
                .Columns.Add("sObject API Name", 265, HorizontalAlignment.Left)
                For Each gr As Partner.DescribeGlobalSObjectResult In lgr
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
                .Sort()
            End With
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If lstObject.SelectedItems Is Nothing Then
            MessageBox.Show("You must select a object!")
        Else
            For Each itm As ListViewItem In lstObject.SelectedItems
                objectList.Add(itm.SubItems(1).Text)
            Next
            success = True
            Me.Close()
        End If
    End Sub

    Private Sub chkCustom_CheckedChanged(sender As Object, e As EventArgs) Handles chkCustom.CheckedChanged
        resetList()
    End Sub

    Private Sub chkStandard_CheckedChanged(sender As Object, e As EventArgs) Handles chkStandard.CheckedChanged
        resetList()
    End Sub

    Private Sub chkSystem_CheckedChanged(sender As Object, e As EventArgs) Handles chkSystem.CheckedChanged
        resetList()
    End Sub

    Private Sub resetList()
        Dim objs() As Dictionary(Of String, String) = objects.ToArray()
        lstObject.Clear()

        With Me.lstObject
            .View = View.Details
            .MultiSelect = True
            .Columns.Add("sObject Name", 200, HorizontalAlignment.Left)
            .Columns.Add("sObject API Name", 265, HorizontalAlignment.Left)
            For Each obj As Dictionary(Of String, String) In objs
                If chkStandard.Checked And standards.Contains(obj.Item("name")) Then
                    Dim line As New ListViewItem()
                    line.Text = obj.Item("label")
                    line.SubItems.Add(obj.Item("name"))
                    .Items.Add(line)
                End If
                If chkCustom.Checked And obj.Item("custom") = True Then
                    Dim line As New ListViewItem()
                    line.Text = obj.Item("label")
                    line.SubItems.Add(obj.Item("name"))
                    .Items.Add(line)
                End If
                If chkSystem.Checked Then
                    If Not standards.Contains(obj.Item("name")) And obj.Item("custom") = False Then
                        Dim line As New ListViewItem()
                        line.Text = obj.Item("label")
                        line.SubItems.Add(obj.Item("name"))
                        .Items.Add(line)
                    End If
                End If
            Next
            lstObject.Sort()
        End With
    End Sub

    Private Sub setLanguage()
        cmbLang.DisplayMember = "Label"
        cmbLang.ValueMember = "Value"
        Dim langTable As DataTable = New DataTable()
        langTable.Columns.Add("Label", GetType(String))
        langTable.Columns.Add("Value", GetType(String))
        For Each key As String In langs.ToArray()
            langTable.Rows.Add(langSet.Item(key), key)
        Next
        cmbLang.DataSource = langTable
        cmbLang.SelectedItem = ThisAddIn.userLang
    End Sub

    Private Sub cmbLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLang.SelectedIndexChanged
        Me.baseLanguage = cmbLang.SelectedValue
    End Sub
End Class