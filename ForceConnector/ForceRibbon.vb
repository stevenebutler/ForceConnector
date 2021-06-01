Imports Microsoft.Office.Tools.Ribbon

Public Class ForceRibbon
    Private Sub ForceRibbon_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load

    End Sub


    ''' <summary>
    '''  Data Connector Module
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAbout_Click(sender As Object, e As RibbonControlEventArgs) Handles btnAbout.Click
        OpenAbout()
    End Sub

    Private Sub TableWizard_Click(sender As Object, e As RibbonControlEventArgs) Handles TableWizard.Click
        QueryTableWizard()
    End Sub

    Private Sub UpdateCells_Click(sender As Object, e As RibbonControlEventArgs) Handles UpdateCells.Click
        UpdateSelectedCells()
    End Sub

    Private Sub InsertRows_Click(sender As Object, e As RibbonControlEventArgs) Handles InsertRows.Click
        InsertSelectedRows()
    End Sub

    Private Sub QueryRows_Click(sender As Object, e As RibbonControlEventArgs) Handles QueryRows.Click
        QuerySelectedRows()
    End Sub

    Private Sub DescribeSobject_Click(sender As Object, e As RibbonControlEventArgs) Handles DescribeSobject.Click
        DescribeSforceObject()
    End Sub

    Private Sub QueryTable_Click(sender As Object, e As RibbonControlEventArgs) Handles QueryTable.Click
        QueryTableData()
    End Sub

    Private Sub DeleteRecords_Click(sender As Object, e As RibbonControlEventArgs) Handles DeleteRecords.Click
        DeleteSelectedRecords()
    End Sub

    Private Sub Options_Click(sender As Object, e As RibbonControlEventArgs) Handles Options.Click
        OptionsForm()
    End Sub

    Private Sub Logout_Click(sender As Object, e As RibbonControlEventArgs) Handles Logout.Click
        LogoutFrom() 'sfLogout
    End Sub


    ''' <summary>
    '''  Translation Helper Module
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Private Sub btnDownloadCustomLabel_Click(sender As Object, e As RibbonControlEventArgs) Handles btnDownloadCustomLabel.Click
        METAAPI.DownloadCustomLabels()
    End Sub

    Private Sub btnDownloadObjectTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnDownloadObjectTranslation.Click
        METAAPI.DownloadObjectTranslations()
    End Sub

    Private Sub btnDownloadTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnDownloadTranslation.Click
        METAAPI.DownloadTranslations()
    End Sub

    Private Sub btnUploadCustomLabel_Click(sender As Object, e As RibbonControlEventArgs) Handles btnUploadCustomLabel.Click
        METAAPI.UploadCustomLabels()
    End Sub

    Private Sub btnUpdateObjectTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnUpdateObjectTranslation.Click
        METAAPI.UpdateObjectTranslations()
    End Sub

    Private Sub btnUpdateTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnUpdateTranslation.Click
        METAAPI.UpdateTranslations()
    End Sub

    Private Sub btnDownloadCustomLabelTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnDownloadCustomLabelTranslation.Click
        METAAPI.DownloadCustomLabelTranslations()
    End Sub

    Private Sub btnUpdateCustomLabelTranslation_Click(sender As Object, e As RibbonControlEventArgs) Handles btnUpdateCustomLabelTranslation.Click
        METAAPI.UpdateCustomLabelTranslations()
    End Sub

End Class
