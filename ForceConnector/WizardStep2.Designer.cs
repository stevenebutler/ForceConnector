using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmWizardStep2 : System.Windows.Forms.Form
    {

        // Form overrides dispose to clean up the component list.
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

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.lblSelectTable = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnBack = new System.Windows.Forms.Button();
            this._btnNext = new System.Windows.Forms.Button();
            this._lstObject = new System.Windows.Forms.ListView();
            this._chkSystem = new System.Windows.Forms.CheckBox();
            this._chkCustom = new System.Windows.Forms.CheckBox();
            this._chkStandard = new System.Windows.Forms.CheckBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this._txtSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSelectTable
            // 
            this.lblSelectTable.Location = new System.Drawing.Point(14, 16);
            this.lblSelectTable.Name = "lblSelectTable";
            this.lblSelectTable.Size = new System.Drawing.Size(274, 31);
            this.lblSelectTable.TabIndex = 0;
            this.lblSelectTable.Text = "Select a Table to Query :";
            this.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(13, 533);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(120, 40);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnBack
            // 
            this._btnBack.Location = new System.Drawing.Point(326, 533);
            this._btnBack.Name = "_btnBack";
            this._btnBack.Size = new System.Drawing.Size(120, 40);
            this._btnBack.TabIndex = 3;
            this._btnBack.Text = "< Back";
            this._btnBack.UseVisualStyleBackColor = true;
            this._btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // _btnNext
            // 
            this._btnNext.Location = new System.Drawing.Point(452, 533);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(120, 40);
            this._btnNext.TabIndex = 4;
            this._btnNext.Text = "Next >";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // _lstObject
            // 
            this._lstObject.FullRowSelect = true;
            this._lstObject.HideSelection = false;
            this._lstObject.Location = new System.Drawing.Point(13, 155);
            this._lstObject.Name = "_lstObject";
            this._lstObject.Size = new System.Drawing.Size(559, 349);
            this._lstObject.TabIndex = 5;
            this._lstObject.UseCompatibleStateImageBehavior = false;
            this._lstObject.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstObject_ColumnClick);
            // 
            // _chkSystem
            // 
            this._chkSystem.AutoSize = true;
            this._chkSystem.Location = new System.Drawing.Point(394, 113);
            this._chkSystem.Name = "_chkSystem";
            this._chkSystem.Size = new System.Drawing.Size(177, 26);
            this._chkSystem.TabIndex = 16;
            this._chkSystem.Text = "System Objects";
            this._chkSystem.UseVisualStyleBackColor = true;
            this._chkSystem.CheckedChanged += new System.EventHandler(this.chkSystem_CheckedChanged);
            // 
            // _chkCustom
            // 
            this._chkCustom.AutoSize = true;
            this._chkCustom.Checked = true;
            this._chkCustom.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkCustom.Location = new System.Drawing.Point(210, 113);
            this._chkCustom.Name = "_chkCustom";
            this._chkCustom.Size = new System.Drawing.Size(178, 26);
            this._chkCustom.TabIndex = 15;
            this._chkCustom.Text = "Custom Objects";
            this._chkCustom.UseVisualStyleBackColor = true;
            this._chkCustom.CheckedChanged += new System.EventHandler(this.chkCustom_CheckedChanged);
            // 
            // _chkStandard
            // 
            this._chkStandard.AutoSize = true;
            this._chkStandard.Checked = true;
            this._chkStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkStandard.Location = new System.Drawing.Point(13, 113);
            this._chkStandard.Name = "_chkStandard";
            this._chkStandard.Size = new System.Drawing.Size(191, 26);
            this._chkStandard.TabIndex = 14;
            this._chkStandard.Text = "Standard Objects";
            this._chkStandard.UseVisualStyleBackColor = true;
            this._chkStandard.CheckedChanged += new System.EventHandler(this.chkStandard_CheckedChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(14, 70);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(165, 22);
            this.lblSearch.TabIndex = 17;
            this.lblSearch.Text = "Search sObject : ";
            // 
            // _txtSearch
            // 
            this._txtSearch.Location = new System.Drawing.Point(185, 70);
            this._txtSearch.Name = "_txtSearch";
            this._txtSearch.Size = new System.Drawing.Size(374, 29);
            this._txtSearch.TabIndex = 18;
            this._txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // frmWizardStep2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 585);
            this.ControlBox = false;
            this.Controls.Add(this._txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this._chkSystem);
            this.Controls.Add(this._chkCustom);
            this.Controls.Add(this._chkStandard);
            this.Controls.Add(this._lstObject);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this._btnBack);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.lblSelectTable);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWizardStep2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table Query Wizard - Step 2 of 4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.Label lblSelectTable;
        private System.Windows.Forms.Button _btnCancel;

        internal System.Windows.Forms.Button btnCancel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCancel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCancel != null)
                {
                    _btnCancel.Click -= btnCancel_Click;
                }

                _btnCancel = value;
                if (_btnCancel != null)
                {
                    _btnCancel.Click += btnCancel_Click;
                }
            }
        }

        private System.Windows.Forms.Button _btnBack;

        internal System.Windows.Forms.Button btnBack
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBack;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBack != null)
                {
                    _btnBack.Click -= btnBack_Click;
                }

                _btnBack = value;
                if (_btnBack != null)
                {
                    _btnBack.Click += btnBack_Click;
                }
            }
        }

        private System.Windows.Forms.Button _btnNext;

        internal System.Windows.Forms.Button btnNext
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnNext;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnNext != null)
                {
                    _btnNext.Click -= btnNext_Click;
                }

                _btnNext = value;
                if (_btnNext != null)
                {
                    _btnNext.Click += btnNext_Click;
                }
            }
        }

        private System.Windows.Forms.ListView _lstObject;

        internal System.Windows.Forms.ListView lstObject
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lstObject;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lstObject != null)
                {
                    _lstObject.ColumnClick -= lstObject_ColumnClick;
                }

                _lstObject = value;
                if (_lstObject != null)
                {
                    _lstObject.ColumnClick += lstObject_ColumnClick;
                }
            }
        }

        private System.Windows.Forms.CheckBox _chkSystem;

        internal System.Windows.Forms.CheckBox chkSystem
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkSystem;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkSystem != null)
                {
                    _chkSystem.CheckedChanged -= chkSystem_CheckedChanged;
                }

                _chkSystem = value;
                if (_chkSystem != null)
                {
                    _chkSystem.CheckedChanged += chkSystem_CheckedChanged;
                }
            }
        }

        private System.Windows.Forms.CheckBox _chkCustom;

        internal System.Windows.Forms.CheckBox chkCustom
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkCustom;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkCustom != null)
                {
                    _chkCustom.CheckedChanged -= chkCustom_CheckedChanged;
                }

                _chkCustom = value;
                if (_chkCustom != null)
                {
                    _chkCustom.CheckedChanged += chkCustom_CheckedChanged;
                }
            }
        }

        private System.Windows.Forms.CheckBox _chkStandard;

        internal System.Windows.Forms.CheckBox chkStandard
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkStandard;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkStandard != null)
                {
                    _chkStandard.CheckedChanged -= chkStandard_CheckedChanged;
                }

                _chkStandard = value;
                if (_chkStandard != null)
                {
                    _chkStandard.CheckedChanged += chkStandard_CheckedChanged;
                }
            }
        }

        internal System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox _txtSearch;

        internal System.Windows.Forms.TextBox txtSearch
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSearch;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSearch != null)
                {
                    _txtSearch.TextChanged -= txtSearch_TextChanged;
                }

                _txtSearch = value;
                if (_txtSearch != null)
                {
                    _txtSearch.TextChanged += txtSearch_TextChanged;
                }
            }
        }
    }
}