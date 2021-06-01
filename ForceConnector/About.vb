Public Class frmAbout
    Private Sub frmAbout_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim brand As String = "Force.com Connector NG " & ThisAddIn.Ver
        Me.lblBrand.Text = brand
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class