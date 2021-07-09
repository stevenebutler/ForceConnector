using Microsoft.VisualBasic;

namespace ForceConnector
{
    [ComClass(ClassId, InterfaceId, EventsId)]
    public class SalesForceAddInApi
    {

        #region COM GUIDs
        // These  GUIDs provide the COM identity for this class 
        // and its COM interfaces. If you change them, existing 
        // clients will no longer be able to access the class.
        public const string ClassId = "e5fcccc8-a685-4980-a79a-eab37f2c7caf";
        public const string InterfaceId = "bacb17f7-9b85-4137-861d-c9a2d899b564";
        public const string EventsId = "6894737e-cfa7-410c-be80-03ad4de47656";
        #endregion

        // A creatable COM class must have a Public Sub New() 
        // with no parameters, otherwise, the class will not be 
        // registered in the COM registry and cannot be created 
        // via CreateObject.
        public SalesForceAddInApi() : base()
        {
        }

        public void QuerySelectedRowsApi()
        {
            ForceConnector.QuerySelectedRows();
        }

        public void QueryTableDataApi()
        {
            ForceConnector.QueryTableData();
        }

        public void UpdateSelectedCellsApi()
        {
            ForceConnector.UpdateSelectedCells();
        }

        public void InsertSelectedRowsApi()
        {
            ForceConnector.InsertSelectedRows();
        }

        public void DeleteSelectedRecordsApi()
        {
            ForceConnector.DeleteSelectedRecords();
        }
    }
}