using System.Runtime.CompilerServices;
using Office = Microsoft.Office.Core;

namespace ForceConnector
{
    public partial class ForceRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        [System.Diagnostics.DebuggerNonUserCode()]
        public ForceRibbon() : base(Globals.Factory.GetRibbonFactory())
        {
            base.Load += ForceRibbon_Load;

            // This call is required by the Component Designer.
            InitializeComponent();
        }

        [System.Diagnostics.DebuggerNonUserCode()]
        public ForceRibbon(System.ComponentModel.IContainer container) : this()
        {

            // Required for Windows.Forms Class Composition Designer support
            if (container is object)
            {
                container.Add(this);
            }
        }

        // Component overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Component Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Component Designer
        // It can be modified using the Component Designer.
        // Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            ForceConnectorTab = Factory.CreateRibbonTab();
            ribbonForceConnector = Factory.CreateRibbonGroup();
            _btnAbout = Factory.CreateRibbonButton();
            _btnAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnAbout_Click);
            _TableWizard = Factory.CreateRibbonButton();
            _TableWizard.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(TableWizard_Click);
            _UpdateCells = Factory.CreateRibbonButton();
            _UpdateCells.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(UpdateCells_Click);
            _InsertRows = Factory.CreateRibbonButton();
            _InsertRows.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(InsertRows_Click);
            seperator1 = Factory.CreateRibbonSeparator();
            _QueryRows = Factory.CreateRibbonButton();
            _QueryRows.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(QueryRows_Click);
            _QueryTable = Factory.CreateRibbonButton();
            _QueryTable.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(QueryTable_Click);
            _DeleteRecords = Factory.CreateRibbonButton();
            _DeleteRecords.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(DeleteRecords_Click);
            separator2 = Factory.CreateRibbonSeparator();
            _DescribeSobject = Factory.CreateRibbonButton();
            _DescribeSobject.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(DescribeSobject_Click);
            _Options = Factory.CreateRibbonButton();
            _Options.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(Options_Click);
            _Logout = Factory.CreateRibbonButton();
            _Logout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(Logout_Click);
            ribbonTranslation = Factory.CreateRibbonGroup();
            _btnDownloadCustomLabel = Factory.CreateRibbonButton();
            _btnDownloadCustomLabel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnDownloadCustomLabel_Click);
            _btnDownloadCustomLabelTranslation = Factory.CreateRibbonButton();
            _btnDownloadCustomLabelTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnDownloadCustomLabelTranslation_Click);
            _btnDownloadTranslation = Factory.CreateRibbonButton();
            _btnDownloadTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnDownloadTranslation_Click);
            _btnUploadCustomLabel = Factory.CreateRibbonButton();
            _btnUploadCustomLabel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnUploadCustomLabel_Click);
            _btnUpdateCustomLabelTranslation = Factory.CreateRibbonButton();
            _btnUpdateCustomLabelTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnUpdateCustomLabelTranslation_Click);
            _btnUpdateTranslation = Factory.CreateRibbonButton();
            _btnUpdateTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnUpdateTranslation_Click);
            _btnDownloadObjectTranslation = Factory.CreateRibbonButton();
            _btnDownloadObjectTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnDownloadObjectTranslation_Click);
            _btnUpdateObjectTranslation = Factory.CreateRibbonButton();
            _btnUpdateObjectTranslation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(btnUpdateObjectTranslation_Click);
            Separator1 = Factory.CreateRibbonSeparator();
            ForceConnectorTab.SuspendLayout();
            ribbonForceConnector.SuspendLayout();
            ribbonTranslation.SuspendLayout();
            SuspendLayout();
            // 
            // ForceConnectorTab
            // 
            ForceConnectorTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            ForceConnectorTab.Groups.Add(ribbonForceConnector);
            ForceConnectorTab.Groups.Add(ribbonTranslation);
            ForceConnectorTab.Label = "TabAddIns";
            ForceConnectorTab.Name = "ForceConnectorTab";
            // 
            // ribbonForceConnector
            // 
            ribbonForceConnector.Items.Add(_btnAbout);
            ribbonForceConnector.Items.Add(_TableWizard);
            ribbonForceConnector.Items.Add(_UpdateCells);
            ribbonForceConnector.Items.Add(_InsertRows);
            ribbonForceConnector.Items.Add(seperator1);
            ribbonForceConnector.Items.Add(_QueryRows);
            ribbonForceConnector.Items.Add(_QueryTable);
            ribbonForceConnector.Items.Add(_DeleteRecords);
            ribbonForceConnector.Items.Add(separator2);
            ribbonForceConnector.Items.Add(_DescribeSobject);
            ribbonForceConnector.Items.Add(_Options);
            ribbonForceConnector.Items.Add(_Logout);
            ribbonForceConnector.Label = "Force.com Connector Next Generation";
            ribbonForceConnector.Name = "ribbonForceConnector";
            // 
            // btnAbout
            // 
            _btnAbout.ControlSize = Office.RibbonControlSize.RibbonControlSizeLarge;
            _btnAbout.Image = My.Resources.Resources.imgres;
            _btnAbout.Label = "About";
            _btnAbout.Name = "_btnAbout";
            _btnAbout.ShowImage = true;
            // 
            // TableWizard
            // 
            _TableWizard.Label = "Table Query Wizard";
            _TableWizard.Name = "_TableWizard";
            // 
            // UpdateCells
            // 
            _UpdateCells.Label = "Update Selected Cells";
            _UpdateCells.Name = "_UpdateCells";
            _UpdateCells.ScreenTip = "send an Update call to salesforce.com passing the values in the selected cells";
            // 
            // InsertRows
            // 
            _InsertRows.Label = "Insert Selected Rows";
            _InsertRows.Name = "_InsertRows";
            _InsertRows.ScreenTip = "Insert (new) one row of data into Salesforce.com";
            // 
            // seperator1
            // 
            seperator1.Name = "seperator1";
            // 
            // QueryRows
            // 
            _QueryRows.Label = "Query Selected Rows";
            _QueryRows.Name = "_QueryRows";
            _QueryRows.ScreenTip = "Query one or more rows (selected) of data from Salesforce.com";
            // 
            // QueryTable
            // 
            _QueryTable.Label = "Query Table Data";
            _QueryTable.Name = "_QueryTable";
            _QueryTable.ScreenTip = "Run the Query in the first row of the current region, return table data from Sale" + "sforce";
            // 
            // DeleteRecords
            // 
            _DeleteRecords.Label = "Delete Records";
            _DeleteRecords.Name = "_DeleteRecords";
            _DeleteRecords.ScreenTip = "Deleted selected records from salesforce.com";
            // 
            // separator2
            // 
            separator2.Name = "separator2";
            // 
            // DescribeSobject
            // 
            _DescribeSobject.Label = "Describe Sforce Object ";
            _DescribeSobject.Name = "_DescribeSobject";
            _DescribeSobject.ScreenTip = "Describe valid columns for the sepecified Salesforce object";
            // 
            // Options
            // 
            _Options.Enabled = false;
            _Options.Label = "Options";
            _Options.Name = "_Options";
            _Options.ScreenTip = "Display option dialog box to config Force.com Connector";
            // 
            // Logout
            // 
            _Logout.Label = "Logout";
            _Logout.Name = "_Logout";
            _Logout.ScreenTip = "Log out from salesforce.com";
            // 
            // ribbonTranslation
            // 
            ribbonTranslation.Items.Add(_btnDownloadCustomLabel);
            ribbonTranslation.Items.Add(_btnDownloadCustomLabelTranslation);
            ribbonTranslation.Items.Add(_btnDownloadTranslation);
            ribbonTranslation.Items.Add(_btnUploadCustomLabel);
            ribbonTranslation.Items.Add(_btnUpdateCustomLabelTranslation);
            ribbonTranslation.Items.Add(_btnUpdateTranslation);
            ribbonTranslation.Items.Add(_btnDownloadObjectTranslation);
            ribbonTranslation.Items.Add(_btnUpdateObjectTranslation);
            ribbonTranslation.Items.Add(Separator1);
            ribbonTranslation.Label = "Force.com Translation Helper";
            ribbonTranslation.Name = "ribbonTranslation";
            // 
            // btnDownloadCustomLabel
            // 
            _btnDownloadCustomLabel.Label = "Download CustomLabel";
            _btnDownloadCustomLabel.Name = "_btnDownloadCustomLabel";
            // 
            // btnDownloadCustomLabelTranslation
            // 
            _btnDownloadCustomLabelTranslation.Label = "Download CustomLabel Translation";
            _btnDownloadCustomLabelTranslation.Name = "_btnDownloadCustomLabelTranslation";
            // 
            // btnDownloadTranslation
            // 
            _btnDownloadTranslation.Label = "Download Translation";
            _btnDownloadTranslation.Name = "_btnDownloadTranslation";
            // 
            // btnUploadCustomLabel
            // 
            _btnUploadCustomLabel.Label = "Upsert CustomLabel";
            _btnUploadCustomLabel.Name = "_btnUploadCustomLabel";
            // 
            // btnUpdateCustomLabelTranslation
            // 
            _btnUpdateCustomLabelTranslation.Label = "Update CustomLabel Translation";
            _btnUpdateCustomLabelTranslation.Name = "_btnUpdateCustomLabelTranslation";
            // 
            // btnUpdateTranslation
            // 
            _btnUpdateTranslation.Enabled = false;
            _btnUpdateTranslation.Label = "Update Translation";
            _btnUpdateTranslation.Name = "_btnUpdateTranslation";
            // 
            // btnDownloadObjectTranslation
            // 
            _btnDownloadObjectTranslation.Label = "Download Object Translation";
            _btnDownloadObjectTranslation.Name = "_btnDownloadObjectTranslation";
            // 
            // btnUpdateObjectTranslation
            // 
            _btnUpdateObjectTranslation.Enabled = false;
            _btnUpdateObjectTranslation.Label = "Update ObjectTranslation";
            _btnUpdateObjectTranslation.Name = "_btnUpdateObjectTranslation";
            // 
            // Separator1
            // 
            Separator1.Name = "Separator1";
            // 
            // ForceRibbon
            // 
            Name = "ForceRibbon";
            RibbonType = "Microsoft.Excel.Workbook";
            Tabs.Add(ForceConnectorTab);
            ForceConnectorTab.ResumeLayout(false);
            ForceConnectorTab.PerformLayout();
            ribbonForceConnector.ResumeLayout(false);
            ribbonForceConnector.PerformLayout();
            ribbonTranslation.ResumeLayout(false);
            ribbonTranslation.PerformLayout();
            ResumeLayout(false);
        }

        internal Microsoft.Office.Tools.Ribbon.RibbonTab ForceConnectorTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ribbonForceConnector;
        private Microsoft.Office.Tools.Ribbon.RibbonButton _UpdateCells;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateCells
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _UpdateCells;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_UpdateCells != null)
                {
                    _UpdateCells.Click -= UpdateCells_Click;
                }

                _UpdateCells = value;
                if (_UpdateCells != null)
                {
                    _UpdateCells.Click += UpdateCells_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _InsertRows;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton InsertRows
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _InsertRows;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_InsertRows != null)
                {
                    _InsertRows.Click -= InsertRows_Click;
                }

                _InsertRows = value;
                if (_InsertRows != null)
                {
                    _InsertRows.Click += InsertRows_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _QueryRows;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton QueryRows
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _QueryRows;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_QueryRows != null)
                {
                    _QueryRows.Click -= QueryRows_Click;
                }

                _QueryRows = value;
                if (_QueryRows != null)
                {
                    _QueryRows.Click += QueryRows_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _DescribeSobject;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton DescribeSobject
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _DescribeSobject;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_DescribeSobject != null)
                {
                    _DescribeSobject.Click -= DescribeSobject_Click;
                }

                _DescribeSobject = value;
                if (_DescribeSobject != null)
                {
                    _DescribeSobject.Click += DescribeSobject_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _QueryTable;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton QueryTable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _QueryTable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_QueryTable != null)
                {
                    _QueryTable.Click -= QueryTable_Click;
                }

                _QueryTable = value;
                if (_QueryTable != null)
                {
                    _QueryTable.Click += QueryTable_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _DeleteRecords;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton DeleteRecords
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _DeleteRecords;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_DeleteRecords != null)
                {
                    _DeleteRecords.Click -= DeleteRecords_Click;
                }

                _DeleteRecords = value;
                if (_DeleteRecords != null)
                {
                    _DeleteRecords.Click += DeleteRecords_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _Options;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton Options
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Options;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Options != null)
                {
                    _Options.Click -= Options_Click;
                }

                _Options = value;
                if (_Options != null)
                {
                    _Options.Click += Options_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _Logout;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton Logout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Logout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Logout != null)
                {
                    _Logout.Click -= Logout_Click;
                }

                _Logout = value;
                if (_Logout != null)
                {
                    _Logout.Click += Logout_Click;
                }
            }
        }

        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ribbonTranslation;
        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnUploadCustomLabel;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnUploadCustomLabel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUploadCustomLabel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUploadCustomLabel != null)
                {
                    _btnUploadCustomLabel.Click -= btnUploadCustomLabel_Click;
                }

                _btnUploadCustomLabel = value;
                if (_btnUploadCustomLabel != null)
                {
                    _btnUploadCustomLabel.Click += btnUploadCustomLabel_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnDownloadCustomLabel;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDownloadCustomLabel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDownloadCustomLabel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDownloadCustomLabel != null)
                {
                    _btnDownloadCustomLabel.Click -= btnDownloadCustomLabel_Click;
                }

                _btnDownloadCustomLabel = value;
                if (_btnDownloadCustomLabel != null)
                {
                    _btnDownloadCustomLabel.Click += btnDownloadCustomLabel_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnDownloadObjectTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDownloadObjectTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDownloadObjectTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDownloadObjectTranslation != null)
                {
                    _btnDownloadObjectTranslation.Click -= btnDownloadObjectTranslation_Click;
                }

                _btnDownloadObjectTranslation = value;
                if (_btnDownloadObjectTranslation != null)
                {
                    _btnDownloadObjectTranslation.Click += btnDownloadObjectTranslation_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnUpdateTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnUpdateTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateTranslation != null)
                {
                    _btnUpdateTranslation.Click -= btnUpdateTranslation_Click;
                }

                _btnUpdateTranslation = value;
                if (_btnUpdateTranslation != null)
                {
                    _btnUpdateTranslation.Click += btnUpdateTranslation_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnDownloadTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDownloadTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDownloadTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDownloadTranslation != null)
                {
                    _btnDownloadTranslation.Click -= btnDownloadTranslation_Click;
                }

                _btnDownloadTranslation = value;
                if (_btnDownloadTranslation != null)
                {
                    _btnDownloadTranslation.Click += btnDownloadTranslation_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnAbout;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAbout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAbout != null)
                {
                    _btnAbout.Click -= btnAbout_Click;
                }

                _btnAbout = value;
                if (_btnAbout != null)
                {
                    _btnAbout.Click += btnAbout_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _TableWizard;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton TableWizard
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _TableWizard;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_TableWizard != null)
                {
                    _TableWizard.Click -= TableWizard_Click;
                }

                _TableWizard = value;
                if (_TableWizard != null)
                {
                    _TableWizard.Click += TableWizard_Click;
                }
            }
        }

        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator seperator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator Separator1;
        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnUpdateObjectTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnUpdateObjectTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateObjectTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateObjectTranslation != null)
                {
                    _btnUpdateObjectTranslation.Click -= btnUpdateObjectTranslation_Click;
                }

                _btnUpdateObjectTranslation = value;
                if (_btnUpdateObjectTranslation != null)
                {
                    _btnUpdateObjectTranslation.Click += btnUpdateObjectTranslation_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnUpdateCustomLabelTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnUpdateCustomLabelTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateCustomLabelTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateCustomLabelTranslation != null)
                {
                    _btnUpdateCustomLabelTranslation.Click -= btnUpdateCustomLabelTranslation_Click;
                }

                _btnUpdateCustomLabelTranslation = value;
                if (_btnUpdateCustomLabelTranslation != null)
                {
                    _btnUpdateCustomLabelTranslation.Click += btnUpdateCustomLabelTranslation_Click;
                }
            }
        }

        private Microsoft.Office.Tools.Ribbon.RibbonButton _btnDownloadCustomLabelTranslation;

        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDownloadCustomLabelTranslation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnDownloadCustomLabelTranslation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnDownloadCustomLabelTranslation != null)
                {
                    _btnDownloadCustomLabelTranslation.Click -= btnDownloadCustomLabelTranslation_Click;
                }

                _btnDownloadCustomLabelTranslation = value;
                if (_btnDownloadCustomLabelTranslation != null)
                {
                    _btnDownloadCustomLabelTranslation.Click += btnDownloadCustomLabelTranslation_Click;
                }
            }
        }
    }

    internal partial class ThisRibbonCollection
    {
        [System.Diagnostics.DebuggerNonUserCode()]
        internal ForceRibbon ForceRibbon
        {
            get
            {
                return GetRibbon<ForceRibbon>();
            }
        }
    }
}