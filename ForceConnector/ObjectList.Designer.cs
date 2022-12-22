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
            this.lstObject = new System.Windows.Forms.ListView();
            this._btnNext = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.lblSelectTable = new System.Windows.Forms.Label();
            this._chkStandard = new System.Windows.Forms.CheckBox();
            this._chkCustom = new System.Windows.Forms.CheckBox();
            this._chkSystem = new System.Windows.Forms.CheckBox();
            this._cmbLang = new System.Windows.Forms.ComboBox();
            this.lblSelectLang = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstObject
            // 
            this.lstObject.HideSelection = false;
            this.lstObject.Location = new System.Drawing.Point(16, 68);
            this.lstObject.Name = "lstObject";
            this.lstObject.Size = new System.Drawing.Size(465, 440);
            this.lstObject.TabIndex = 10;
            this.lstObject.UseCompatibleStateImageBehavior = false;
            // 
            // _btnNext
            // 
            this._btnNext.Location = new System.Drawing.Point(270, 559);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(100, 30);
            this._btnNext.TabIndex = 9;
            this._btnNext.Text = "Next >";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(125, 559);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(100, 30);
            this._btnCancel.TabIndex = 7;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSelectTable
            // 
            this.lblSelectTable.Location = new System.Drawing.Point(17, 14);
            this.lblSelectTable.Name = "lblSelectTable";
            this.lblSelectTable.Size = new System.Drawing.Size(217, 16);
            this.lblSelectTable.TabIndex = 6;
            this.lblSelectTable.Text = "Select the Objects to Describe :";
            this.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _chkStandard
            // 
            this._chkStandard.AutoSize = true;
            this._chkStandard.Checked = true;
            this._chkStandard.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkStandard.Location = new System.Drawing.Point(19, 42);
            this._chkStandard.Name = "_chkStandard";
            this._chkStandard.Size = new System.Drawing.Size(136, 18);
            this._chkStandard.TabIndex = 11;
            this._chkStandard.Text = "Standard Objects";
            this._chkStandard.UseVisualStyleBackColor = true;
            this._chkStandard.CheckedChanged += new System.EventHandler(this.chkStandard_CheckedChanged);
            // 
            // _chkCustom
            // 
            this._chkCustom.AutoSize = true;
            this._chkCustom.Checked = true;
            this._chkCustom.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkCustom.Location = new System.Drawing.Point(181, 42);
            this._chkCustom.Name = "_chkCustom";
            this._chkCustom.Size = new System.Drawing.Size(126, 18);
            this._chkCustom.TabIndex = 12;
            this._chkCustom.Text = "Custom Objects";
            this._chkCustom.UseVisualStyleBackColor = true;
            this._chkCustom.CheckedChanged += new System.EventHandler(this.chkCustom_CheckedChanged);
            // 
            // _chkSystem
            // 
            this._chkSystem.AutoSize = true;
            this._chkSystem.Location = new System.Drawing.Point(342, 42);
            this._chkSystem.Name = "_chkSystem";
            this._chkSystem.Size = new System.Drawing.Size(124, 18);
            this._chkSystem.TabIndex = 13;
            this._chkSystem.Text = "System Objects";
            this._chkSystem.UseVisualStyleBackColor = true;
            this._chkSystem.CheckedChanged += new System.EventHandler(this.chkSystem_CheckedChanged);
            // 
            // _cmbLang
            // 
            this._cmbLang.FormattingEnabled = true;
            this._cmbLang.Location = new System.Drawing.Point(186, 524);
            this._cmbLang.Name = "_cmbLang";
            this._cmbLang.Size = new System.Drawing.Size(295, 22);
            this._cmbLang.TabIndex = 14;
            this._cmbLang.SelectedIndexChanged += new System.EventHandler(this.cmbLang_SelectedIndexChanged);
            // 
            // lblSelectLang
            // 
            this.lblSelectLang.AutoSize = true;
            this.lblSelectLang.Location = new System.Drawing.Point(20, 527);
            this.lblSelectLang.Name = "lblSelectLang";
            this.lblSelectLang.Size = new System.Drawing.Size(156, 14);
            this.lblSelectLang.TabIndex = 15;
            this.lblSelectLang.Text = "Select Base Language :";
            // 
            // frmObjectList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(497, 601);
            this.Controls.Add(this.lblSelectLang);
            this.Controls.Add(this._cmbLang);
            this.Controls.Add(this._chkSystem);
            this.Controls.Add(this._chkCustom);
            this.Controls.Add(this._chkStandard);
            this.Controls.Add(this.lstObject);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.lblSelectTable);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmObjectList";
            this.Text = "Describe SObjects";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmObjectList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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