using Microsoft.Office.Tools.Ribbon;

namespace ForceConnector
{
    public partial class ForceRibbon
    {
        private void ForceRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }


        /// <summary>
    /// Data Connector Module
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void btnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.OpenAbout();
        }

        private void TableWizard_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.QueryTableWizard();
        }

        private void UpdateCells_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.UpdateSelectedCells();
        }

        private void InsertRows_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.InsertSelectedRows();
        }

        private void QueryRows_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.QuerySelectedRows();
        }

        private void DescribeSobject_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.DescribeSforceObject();
        }

        private void QueryTable_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.QueryTableData();
        }

        private void DeleteRecords_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.DeleteSelectedRecords();
        }

        private void Options_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.OptionsForm();
        }

        private void Logout_Click(object sender, RibbonControlEventArgs e)
        {
            ForceConnector.LogoutFrom(); // sfLogout
        }


        /// <summary>
    /// Translation Helper Module
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

        private void btnDownloadCustomLabel_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.DownloadCustomLabels();
        }

        private void btnDownloadObjectTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.DownloadObjectTranslations();
        }

        private void btnDownloadTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.DownloadTranslations();
        }

        private void btnUploadCustomLabel_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.UploadCustomLabels();
        }

        private void btnUpdateObjectTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.UpdateObjectTranslations();
        }

        private void btnUpdateTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.UpdateTranslations();
        }

        private void btnDownloadCustomLabelTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.DownloadCustomLabelTranslations();
        }

        private void btnUpdateCustomLabelTranslation_Click(object sender, RibbonControlEventArgs e)
        {
            METAAPI.UpdateCustomLabelTranslations();
        }
    }
}