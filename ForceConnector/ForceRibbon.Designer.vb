Partial Class ForceRibbon
    Inherits Microsoft.Office.Tools.Ribbon.RibbonBase

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New()
        MyBase.New(Globals.Factory.GetRibbonFactory())

        'This call is required by the Component Designer.
        InitializeComponent()

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ForceConnectorTab = Me.Factory.CreateRibbonTab
        Me.ribbonForceConnector = Me.Factory.CreateRibbonGroup
        Me.btnAbout = Me.Factory.CreateRibbonButton
        Me.TableWizard = Me.Factory.CreateRibbonButton
        Me.UpdateCells = Me.Factory.CreateRibbonButton
        Me.InsertRows = Me.Factory.CreateRibbonButton
        Me.seperator1 = Me.Factory.CreateRibbonSeparator
        Me.QueryRows = Me.Factory.CreateRibbonButton
        Me.QueryTable = Me.Factory.CreateRibbonButton
        Me.DeleteRecords = Me.Factory.CreateRibbonButton
        Me.separator2 = Me.Factory.CreateRibbonSeparator
        Me.DescribeSobject = Me.Factory.CreateRibbonButton
        Me.Options = Me.Factory.CreateRibbonButton
        Me.Logout = Me.Factory.CreateRibbonButton
        Me.ribbonTranslation = Me.Factory.CreateRibbonGroup
        Me.btnDownloadCustomLabel = Me.Factory.CreateRibbonButton
        Me.btnDownloadCustomLabelTranslation = Me.Factory.CreateRibbonButton
        Me.btnDownloadTranslation = Me.Factory.CreateRibbonButton
        Me.btnUploadCustomLabel = Me.Factory.CreateRibbonButton
        Me.btnUpdateCustomLabelTranslation = Me.Factory.CreateRibbonButton
        Me.btnUpdateTranslation = Me.Factory.CreateRibbonButton
        Me.btnDownloadObjectTranslation = Me.Factory.CreateRibbonButton
        Me.btnUpdateObjectTranslation = Me.Factory.CreateRibbonButton
        Me.Separator1 = Me.Factory.CreateRibbonSeparator
        Me.ForceConnectorTab.SuspendLayout()
        Me.ribbonForceConnector.SuspendLayout()
        Me.ribbonTranslation.SuspendLayout()
        Me.SuspendLayout()
        '
        'ForceConnectorTab
        '
        Me.ForceConnectorTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.ForceConnectorTab.Groups.Add(Me.ribbonForceConnector)
        Me.ForceConnectorTab.Groups.Add(Me.ribbonTranslation)
        Me.ForceConnectorTab.Label = "TabAddIns"
        Me.ForceConnectorTab.Name = "ForceConnectorTab"
        '
        'ribbonForceConnector
        '
        Me.ribbonForceConnector.Items.Add(Me.btnAbout)
        Me.ribbonForceConnector.Items.Add(Me.TableWizard)
        Me.ribbonForceConnector.Items.Add(Me.UpdateCells)
        Me.ribbonForceConnector.Items.Add(Me.InsertRows)
        Me.ribbonForceConnector.Items.Add(Me.seperator1)
        Me.ribbonForceConnector.Items.Add(Me.QueryRows)
        Me.ribbonForceConnector.Items.Add(Me.QueryTable)
        Me.ribbonForceConnector.Items.Add(Me.DeleteRecords)
        Me.ribbonForceConnector.Items.Add(Me.separator2)
        Me.ribbonForceConnector.Items.Add(Me.DescribeSobject)
        Me.ribbonForceConnector.Items.Add(Me.Options)
        Me.ribbonForceConnector.Items.Add(Me.Logout)
        Me.ribbonForceConnector.Label = "Force.com Connector Next Generation"
        Me.ribbonForceConnector.Name = "ribbonForceConnector"
        '
        'btnAbout
        '
        Me.btnAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.btnAbout.Image = Global.ForceConnector.My.Resources.Resources.imgres
        Me.btnAbout.Label = "About"
        Me.btnAbout.Name = "btnAbout"
        Me.btnAbout.ShowImage = True
        '
        'TableWizard
        '
        Me.TableWizard.Label = "Table Query Wizard"
        Me.TableWizard.Name = "TableWizard"
        '
        'UpdateCells
        '
        Me.UpdateCells.Label = "Update Selected Cells"
        Me.UpdateCells.Name = "UpdateCells"
        Me.UpdateCells.ScreenTip = "send an Update call to salesforce.com passing the values in the selected cells"
        '
        'InsertRows
        '
        Me.InsertRows.Label = "Insert Selected Rows"
        Me.InsertRows.Name = "InsertRows"
        Me.InsertRows.ScreenTip = "Insert (new) one row of data into Salesforce.com"
        '
        'seperator1
        '
        Me.seperator1.Name = "seperator1"
        '
        'QueryRows
        '
        Me.QueryRows.Label = "Query Selected Rows"
        Me.QueryRows.Name = "QueryRows"
        Me.QueryRows.ScreenTip = "Query one or more rows (selected) of data from Salesforce.com"
        '
        'QueryTable
        '
        Me.QueryTable.Label = "Query Table Data"
        Me.QueryTable.Name = "QueryTable"
        Me.QueryTable.ScreenTip = "Run the Query in the first row of the current region, return table data from Sale" &
    "sforce"
        '
        'DeleteRecords
        '
        Me.DeleteRecords.Label = "Delete Records"
        Me.DeleteRecords.Name = "DeleteRecords"
        Me.DeleteRecords.ScreenTip = "Deleted selected records from salesforce.com"
        '
        'separator2
        '
        Me.separator2.Name = "separator2"
        '
        'DescribeSobject
        '
        Me.DescribeSobject.Label = "Describe Sforce Object "
        Me.DescribeSobject.Name = "DescribeSobject"
        Me.DescribeSobject.ScreenTip = "Describe valid columns for the sepecified Salesforce object"
        '
        'Options
        '
        Me.Options.Enabled = False
        Me.Options.Label = "Options"
        Me.Options.Name = "Options"
        Me.Options.ScreenTip = "Display option dialog box to config Force.com Connector"
        '
        'Logout
        '
        Me.Logout.Label = "Logout"
        Me.Logout.Name = "Logout"
        Me.Logout.ScreenTip = "Log out from salesforce.com"
        '
        'ribbonTranslation
        '
        Me.ribbonTranslation.Items.Add(Me.btnDownloadCustomLabel)
        Me.ribbonTranslation.Items.Add(Me.btnDownloadCustomLabelTranslation)
        Me.ribbonTranslation.Items.Add(Me.btnDownloadTranslation)
        Me.ribbonTranslation.Items.Add(Me.btnUploadCustomLabel)
        Me.ribbonTranslation.Items.Add(Me.btnUpdateCustomLabelTranslation)
        Me.ribbonTranslation.Items.Add(Me.btnUpdateTranslation)
        Me.ribbonTranslation.Items.Add(Me.btnDownloadObjectTranslation)
        Me.ribbonTranslation.Items.Add(Me.btnUpdateObjectTranslation)
        Me.ribbonTranslation.Items.Add(Me.Separator1)
        Me.ribbonTranslation.Label = "Force.com Translation Helper"
        Me.ribbonTranslation.Name = "ribbonTranslation"
        '
        'btnDownloadCustomLabel
        '
        Me.btnDownloadCustomLabel.Label = "Download CustomLabel"
        Me.btnDownloadCustomLabel.Name = "btnDownloadCustomLabel"
        '
        'btnDownloadCustomLabelTranslation
        '
        Me.btnDownloadCustomLabelTranslation.Label = "Download CustomLabel Translation"
        Me.btnDownloadCustomLabelTranslation.Name = "btnDownloadCustomLabelTranslation"
        '
        'btnDownloadTranslation
        '
        Me.btnDownloadTranslation.Label = "Download Translation"
        Me.btnDownloadTranslation.Name = "btnDownloadTranslation"
        '
        'btnUploadCustomLabel
        '
        Me.btnUploadCustomLabel.Label = "Upsert CustomLabel"
        Me.btnUploadCustomLabel.Name = "btnUploadCustomLabel"
        '
        'btnUpdateCustomLabelTranslation
        '
        Me.btnUpdateCustomLabelTranslation.Label = "Update CustomLabel Translation"
        Me.btnUpdateCustomLabelTranslation.Name = "btnUpdateCustomLabelTranslation"
        '
        'btnUpdateTranslation
        '
        Me.btnUpdateTranslation.Enabled = False
        Me.btnUpdateTranslation.Label = "Update Translation"
        Me.btnUpdateTranslation.Name = "btnUpdateTranslation"
        '
        'btnDownloadObjectTranslation
        '
        Me.btnDownloadObjectTranslation.Label = "Download Object Translation"
        Me.btnDownloadObjectTranslation.Name = "btnDownloadObjectTranslation"
        '
        'btnUpdateObjectTranslation
        '
        Me.btnUpdateObjectTranslation.Enabled = False
        Me.btnUpdateObjectTranslation.Label = "Update ObjectTranslation"
        Me.btnUpdateObjectTranslation.Name = "btnUpdateObjectTranslation"
        '
        'Separator1
        '
        Me.Separator1.Name = "Separator1"
        '
        'ForceRibbon
        '
        Me.Name = "ForceRibbon"
        Me.RibbonType = "Microsoft.Excel.Workbook"
        Me.Tabs.Add(Me.ForceConnectorTab)
        Me.ForceConnectorTab.ResumeLayout(False)
        Me.ForceConnectorTab.PerformLayout()
        Me.ribbonForceConnector.ResumeLayout(False)
        Me.ribbonForceConnector.PerformLayout()
        Me.ribbonTranslation.ResumeLayout(False)
        Me.ribbonTranslation.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ForceConnectorTab As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents ribbonForceConnector As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents UpdateCells As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents InsertRows As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents QueryRows As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DescribeSobject As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents QueryTable As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DeleteRecords As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Options As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Logout As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ribbonTranslation As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents btnUploadCustomLabel As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnDownloadCustomLabel As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnDownloadObjectTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnUpdateTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnDownloadTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnAbout As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents TableWizard As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents seperator1 As Microsoft.Office.Tools.Ribbon.RibbonSeparator
    Friend WithEvents separator2 As Microsoft.Office.Tools.Ribbon.RibbonSeparator
    Friend WithEvents Separator1 As Microsoft.Office.Tools.Ribbon.RibbonSeparator
    Friend WithEvents btnUpdateObjectTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnUpdateCustomLabelTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents btnDownloadCustomLabelTranslation As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property ForceRibbon() As ForceRibbon
        Get
            Return Me.GetRibbon(Of ForceRibbon)()
        End Get
    End Property
End Class
