<ComClass(SalesForceAddInApi.ClassId, SalesForceAddInApi.InterfaceId, SalesForceAddInApi.EventsId)>
Public Class SalesForceAddInApi

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "e5fcccc8-a685-4980-a79a-eab37f2c7caf"
    Public Const InterfaceId As String = "bacb17f7-9b85-4137-861d-c9a2d899b564"
    Public Const EventsId As String = "6894737e-cfa7-410c-be80-03ad4de47656"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub QuerySelectedRowsApi()
        QuerySelectedRows()
    End Sub

    Public Sub QueryTableDataApi()
        QueryTableData()
    End Sub

    Public Sub UpdateSelectedCellsApi()
        UpdateSelectedCells()
    End Sub
    Public Sub InsertSelectedRowsApi()
        InsertSelectedRows()
    End Sub

    Public Sub DeleteSelectedRecordsApi()
        DeleteSelectedRecords()
    End Sub


End Class


