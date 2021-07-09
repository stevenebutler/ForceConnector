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
            lblSelectTable = new System.Windows.Forms.Label();
            _btnCancel = new System.Windows.Forms.Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _btnBack = new System.Windows.Forms.Button();
            _btnBack.Click += new EventHandler(btnBack_Click);
            _btnNext = new System.Windows.Forms.Button();
            _btnNext.Click += new EventHandler(btnNext_Click);
            _lstObject = new System.Windows.Forms.ListView();
            _lstObject.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lstObject_ColumnClick);
            _chkSystem = new System.Windows.Forms.CheckBox();
            _chkSystem.CheckedChanged += new EventHandler(chkSystem_CheckedChanged);
            _chkCustom = new System.Windows.Forms.CheckBox();
            _chkCustom.CheckedChanged += new EventHandler(chkCustom_CheckedChanged);
            _chkStandard = new System.Windows.Forms.CheckBox();
            _chkStandard.CheckedChanged += new EventHandler(chkStandard_CheckedChanged);
            lblSearch = new System.Windows.Forms.Label();
            _txtSearch = new System.Windows.Forms.TextBox();
            _txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            SuspendLayout();
            // 
            // lblSelectTable
            // 
            lblSelectTable.Location = new System.Drawing.Point(14, 16);
            lblSelectTable.Name = "lblSelectTable";
            lblSelectTable.Size = new System.Drawing.Size(162, 14);
            lblSelectTable.TabIndex = 0;
            lblSelectTable.Text = "Select a Table to Query :";
            lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new System.Drawing.Point(13, 466);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(120, 40);
            _btnCancel.TabIndex = 2;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            _btnBack.Location = new System.Drawing.Point(222, 466);
            _btnBack.Name = "_btnBack";
            _btnBack.Size = new System.Drawing.Size(120, 40);
            _btnBack.TabIndex = 3;
            _btnBack.Text = "< Back";
            _btnBack.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            _btnNext.Location = new System.Drawing.Point(358, 466);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new System.Drawing.Size(120, 40);
            _btnNext.TabIndex = 4;
            _btnNext.Text = "Next >";
            _btnNext.UseVisualStyleBackColor = true;
            // 
            // lstObject
            // 
            _lstObject.FullRowSelect = true;
            _lstObject.HideSelection = false;
            _lstObject.Location = new System.Drawing.Point(13, 103);
            _lstObject.Name = "_lstObject";
            _lstObject.Size = new System.Drawing.Size(465, 349);
            _lstObject.TabIndex = 5;
            _lstObject.UseCompatibleStateImageBehavior = false;
            // 
            // chkSystem
            // 
            _chkSystem.AutoSize = true;
            _chkSystem.Location = new System.Drawing.Point(335, 79);
            _chkSystem.Name = "_chkSystem";
            _chkSystem.Size = new System.Drawing.Size(124, 18);
            _chkSystem.TabIndex = 16;
            _chkSystem.Text = "System Objects";
            _chkSystem.UseVisualStyleBackColor = true;
            // 
            // chkCustom
            // 
            _chkCustom.AutoSize = true;
            _chkCustom.Checked = true;
            _chkCustom.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkCustom.Location = new System.Drawing.Point(178, 79);
            _chkCustom.Name = "_chkCustom";
            _chkCustom.Size = new System.Drawing.Size(126, 18);
            _chkCustom.TabIndex = 15;
            _chkCustom.Text = "Custom Objects";
            _chkCustom.UseVisualStyleBackColor = true;
            // 
            // chkStandard
            // 
            _chkStandard.AutoSize = true;
            _chkStandard.Checked = true;
            _chkStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkStandard.Location = new System.Drawing.Point(17, 79);
            _chkStandard.Name = "_chkStandard";
            _chkStandard.Size = new System.Drawing.Size(136, 18);
            _chkStandard.TabIndex = 14;
            _chkStandard.Text = "Standard Objects";
            _chkStandard.UseVisualStyleBackColor = true;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new System.Drawing.Point(14, 45);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new System.Drawing.Size(115, 14);
            lblSearch.TabIndex = 17;
            lblSearch.Text = "Search sObject : ";
            // 
            // txtSearch
            // 
            _txtSearch.Location = new System.Drawing.Point(129, 42);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.Size = new System.Drawing.Size(349, 22);
            _txtSearch.TabIndex = 18;
            // 
            // frmWizardStep2
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(492, 515);
            ControlBox = false;
            Controls.Add(_txtSearch);
            Controls.Add(lblSearch);
            Controls.Add(_chkSystem);
            Controls.Add(_chkCustom);
            Controls.Add(_chkStandard);
            Controls.Add(_lstObject);
            Controls.Add(_btnNext);
            Controls.Add(_btnBack);
            Controls.Add(_btnCancel);
            Controls.Add(lblSelectTable);
            Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmWizardStep2";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Table Query Wizard - Step 2 of 4";
            ResumeLayout(false);
            PerformLayout();
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