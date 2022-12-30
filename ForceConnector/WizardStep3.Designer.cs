using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmWizardStep3 : System.Windows.Forms.Form
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
            this.lblSelectFields = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnBack = new System.Windows.Forms.Button();
            this._btnNext = new System.Windows.Forms.Button();
            this.lstField = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lblSelectFields
            // 
            this.lblSelectFields.AutoSize = true;
            this.lblSelectFields.Location = new System.Drawing.Point(14, 16);
            this.lblSelectFields.Name = "lblSelectFields";
            this.lblSelectFields.Size = new System.Drawing.Size(232, 22);
            this.lblSelectFields.TabIndex = 0;
            this.lblSelectFields.Text = "Select Fields to Include :";
            this.lblSelectFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(13, 448);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(120, 40);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _btnBack
            // 
            this._btnBack.Location = new System.Drawing.Point(222, 448);
            this._btnBack.Name = "_btnBack";
            this._btnBack.Size = new System.Drawing.Size(120, 40);
            this._btnBack.TabIndex = 3;
            this._btnBack.Text = "< Back";
            this._btnBack.UseVisualStyleBackColor = true;
            this._btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // _btnNext
            // 
            this._btnNext.Location = new System.Drawing.Point(358, 448);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(120, 40);
            this._btnNext.TabIndex = 4;
            this._btnNext.Text = "Next >";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lstField
            // 
            this.lstField.FullRowSelect = true;
            this.lstField.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstField.HideSelection = false;
            this.lstField.Location = new System.Drawing.Point(13, 67);
            this.lstField.Name = "lstField";
            this.lstField.Size = new System.Drawing.Size(465, 371);
            this.lstField.TabIndex = 5;
            this.lstField.UseCompatibleStateImageBehavior = false;
            // 
            // frmWizardStep3
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(492, 496);
            this.ControlBox = false;
            this.Controls.Add(this.lstField);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this._btnBack);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.lblSelectFields);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWizardStep3";
            this.ShowIcon = false;
            this.Text = "Table Query Wizard - Step 3 of 4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.Label lblSelectFields;
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

        internal System.Windows.Forms.ListView lstField;
    }
}