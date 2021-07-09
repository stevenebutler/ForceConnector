using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class TranslationObjectSelect : System.Windows.Forms.Form
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
            SuspendLayout();
            // 
            // lstObject
            // 
            lstObject.HideSelection = false;
            lstObject.Location = new System.Drawing.Point(8, 27);
            lstObject.Margin = new System.Windows.Forms.Padding(2);
            lstObject.Name = "lstObject";
            lstObject.Size = new System.Drawing.Size(311, 310);
            lstObject.TabIndex = 17;
            lstObject.UseCompatibleStateImageBehavior = false;
            // 
            // btnNext
            // 
            _btnNext.Location = new System.Drawing.Point(227, 346);
            _btnNext.Margin = new System.Windows.Forms.Padding(2);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new System.Drawing.Size(91, 31);
            _btnNext.TabIndex = 16;
            _btnNext.Text = "Next >";
            _btnNext.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new System.Drawing.Point(8, 346);
            _btnCancel.Margin = new System.Windows.Forms.Padding(2);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(91, 31);
            _btnCancel.TabIndex = 15;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSelectTable
            // 
            lblSelectTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblSelectTable.Location = new System.Drawing.Point(9, 5);
            lblSelectTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblSelectTable.Name = "lblSelectTable";
            lblSelectTable.Size = new System.Drawing.Size(145, 20);
            lblSelectTable.TabIndex = 14;
            lblSelectTable.Text = "Select the Objects :";
            lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TranslationObjectSelect
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(329, 387);
            Controls.Add(lstObject);
            Controls.Add(_btnNext);
            Controls.Add(_btnCancel);
            Controls.Add(lblSelectTable);
            Margin = new System.Windows.Forms.Padding(2);
            Name = "TranslationObjectSelect";
            Text = "Select Translation Target Objects";
            Load += new EventHandler(TranslationObjectSelect_Load);
            ResumeLayout(false);
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
    }
}