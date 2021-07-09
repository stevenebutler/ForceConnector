using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmObjectList : System.Windows.Forms.Form
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
            lstObject = new System.Windows.Forms.ListView();
            _btnNext = new System.Windows.Forms.Button();
            _btnNext.Click += new EventHandler(btnNext_Click);
            _btnCancel = new System.Windows.Forms.Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            lblSelectTable = new System.Windows.Forms.Label();
            _chkStandard = new System.Windows.Forms.CheckBox();
            _chkStandard.CheckedChanged += new EventHandler(chkStandard_CheckedChanged);
            _chkCustom = new System.Windows.Forms.CheckBox();
            _chkCustom.CheckedChanged += new EventHandler(chkCustom_CheckedChanged);
            _chkSystem = new System.Windows.Forms.CheckBox();
            _chkSystem.CheckedChanged += new EventHandler(chkSystem_CheckedChanged);
            _cmbLang = new System.Windows.Forms.ComboBox();
            _cmbLang.SelectedIndexChanged += new EventHandler(cmbLang_SelectedIndexChanged);
            lblSelectLang = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // lstObject
            // 
            lstObject.HideSelection = false;
            lstObject.Location = new System.Drawing.Point(16, 68);
            lstObject.Name = "lstObject";
            lstObject.Size = new System.Drawing.Size(465, 440);
            lstObject.TabIndex = 10;
            lstObject.UseCompatibleStateImageBehavior = false;
            // 
            // btnNext
            // 
            _btnNext.Location = new System.Drawing.Point(270, 559);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new System.Drawing.Size(100, 30);
            _btnNext.TabIndex = 9;
            _btnNext.Text = "Next >";
            _btnNext.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new System.Drawing.Point(125, 559);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(100, 30);
            _btnCancel.TabIndex = 7;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSelectTable
            // 
            lblSelectTable.Location = new System.Drawing.Point(17, 14);
            lblSelectTable.Name = "lblSelectTable";
            lblSelectTable.Size = new System.Drawing.Size(217, 16);
            lblSelectTable.TabIndex = 6;
            lblSelectTable.Text = "Select the Objects to Describe :";
            lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkStandard
            // 
            _chkStandard.AutoSize = true;
            _chkStandard.Checked = true;
            _chkStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkStandard.Location = new System.Drawing.Point(19, 42);
            _chkStandard.Name = "_chkStandard";
            _chkStandard.Size = new System.Drawing.Size(136, 18);
            _chkStandard.TabIndex = 11;
            _chkStandard.Text = "Standard Objects";
            _chkStandard.UseVisualStyleBackColor = true;
            // 
            // chkCustom
            // 
            _chkCustom.AutoSize = true;
            _chkCustom.Checked = true;
            _chkCustom.CheckState = System.Windows.Forms.CheckState.Checked;
            _chkCustom.Location = new System.Drawing.Point(181, 42);
            _chkCustom.Name = "_chkCustom";
            _chkCustom.Size = new System.Drawing.Size(126, 18);
            _chkCustom.TabIndex = 12;
            _chkCustom.Text = "Custom Objects";
            _chkCustom.UseVisualStyleBackColor = true;
            // 
            // chkSystem
            // 
            _chkSystem.AutoSize = true;
            _chkSystem.Location = new System.Drawing.Point(342, 42);
            _chkSystem.Name = "_chkSystem";
            _chkSystem.Size = new System.Drawing.Size(124, 18);
            _chkSystem.TabIndex = 13;
            _chkSystem.Text = "System Objects";
            _chkSystem.UseVisualStyleBackColor = true;
            // 
            // cmbLang
            // 
            _cmbLang.FormattingEnabled = true;
            _cmbLang.Location = new System.Drawing.Point(186, 524);
            _cmbLang.Name = "_cmbLang";
            _cmbLang.Size = new System.Drawing.Size(197, 22);
            _cmbLang.TabIndex = 14;
            // 
            // lblSelectLang
            // 
            lblSelectLang.AutoSize = true;
            lblSelectLang.Location = new System.Drawing.Point(20, 527);
            lblSelectLang.Name = "lblSelectLang";
            lblSelectLang.Size = new System.Drawing.Size(156, 14);
            lblSelectLang.TabIndex = 15;
            lblSelectLang.Text = "Select Base Language :";
            // 
            // frmObjectList
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(497, 601);
            Controls.Add(lblSelectLang);
            Controls.Add(_cmbLang);
            Controls.Add(_chkSystem);
            Controls.Add(_chkCustom);
            Controls.Add(_chkStandard);
            Controls.Add(lstObject);
            Controls.Add(_btnNext);
            Controls.Add(_btnCancel);
            Controls.Add(lblSelectTable);
            Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            Name = "frmObjectList";
            Text = "Describe SObjects";
            TopMost = true;
            Load += new EventHandler(frmObjectList_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        internal System.Windows.Forms.ListView lstObject;
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

        internal System.Windows.Forms.Label lblSelectTable;
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

        private System.Windows.Forms.ComboBox _cmbLang;

        internal System.Windows.Forms.ComboBox cmbLang
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbLang;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbLang != null)
                {
                    _cmbLang.SelectedIndexChanged -= cmbLang_SelectedIndexChanged;
                }

                _cmbLang = value;
                if (_cmbLang != null)
                {
                    _cmbLang.SelectedIndexChanged += cmbLang_SelectedIndexChanged;
                }
            }
        }

        internal System.Windows.Forms.Label lblSelectLang;
    }
}