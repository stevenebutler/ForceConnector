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
            lblSelectFields = new System.Windows.Forms.Label();
            _btnCancel = new System.Windows.Forms.Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _btnBack = new System.Windows.Forms.Button();
            _btnBack.Click += new EventHandler(btnBack_Click);
            _btnNext = new System.Windows.Forms.Button();
            _btnNext.Click += new EventHandler(btnNext_Click);
            lstField = new System.Windows.Forms.ListView();
            SuspendLayout();
            // 
            // lblSelectFields
            // 
            lblSelectFields.AutoSize = true;
            lblSelectFields.Location = new System.Drawing.Point(14, 16);
            lblSelectFields.Name = "lblSelectFields";
            lblSelectFields.Size = new System.Drawing.Size(161, 14);
            lblSelectFields.TabIndex = 0;
            lblSelectFields.Text = "Select Fields to Include :";
            lblSelectFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new System.Drawing.Point(13, 448);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(120, 40);
            _btnCancel.TabIndex = 2;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            _btnBack.Location = new System.Drawing.Point(222, 448);
            _btnBack.Name = "_btnBack";
            _btnBack.Size = new System.Drawing.Size(120, 40);
            _btnBack.TabIndex = 3;
            _btnBack.Text = "< Back";
            _btnBack.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            _btnNext.Location = new System.Drawing.Point(358, 448);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new System.Drawing.Size(120, 40);
            _btnNext.TabIndex = 4;
            _btnNext.Text = "Next >";
            _btnNext.UseVisualStyleBackColor = true;
            // 
            // lstField
            // 
            lstField.FullRowSelect = true;
            lstField.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lstField.HideSelection = false;
            lstField.Location = new System.Drawing.Point(13, 42);
            lstField.Name = "lstField";
            lstField.Size = new System.Drawing.Size(465, 396);
            lstField.TabIndex = 5;
            lstField.UseCompatibleStateImageBehavior = false;
            // 
            // frmWizardStep3
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(492, 496);
            ControlBox = false;
            Controls.Add(lstField);
            Controls.Add(_btnNext);
            Controls.Add(_btnBack);
            Controls.Add(_btnCancel);
            Controls.Add(lblSelectFields);
            Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmWizardStep3";
            ShowIcon = false;
            Text = "Table Query Wizard - Step 3 of 4";
            ResumeLayout(false);
            PerformLayout();
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