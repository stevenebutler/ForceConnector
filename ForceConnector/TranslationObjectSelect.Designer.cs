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
            this.lstObject = new System.Windows.Forms.ListView();
            this._btnNext = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.lblSelectTable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstObject
            // 
            this.lstObject.HideSelection = false;
            this.lstObject.Location = new System.Drawing.Point(8, 27);
            this.lstObject.Margin = new System.Windows.Forms.Padding(2);
            this.lstObject.Name = "lstObject";
            this.lstObject.Size = new System.Drawing.Size(311, 310);
            this.lstObject.TabIndex = 17;
            this.lstObject.UseCompatibleStateImageBehavior = false;
            // 
            // _btnNext
            // 
            this._btnNext.Location = new System.Drawing.Point(227, 346);
            this._btnNext.Margin = new System.Windows.Forms.Padding(2);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(91, 31);
            this._btnNext.TabIndex = 16;
            this._btnNext.Text = "Next >";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(8, 346);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(91, 31);
            this._btnCancel.TabIndex = 15;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSelectTable
            // 
            this.lblSelectTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectTable.Location = new System.Drawing.Point(9, 5);
            this.lblSelectTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectTable.Name = "lblSelectTable";
            this.lblSelectTable.Size = new System.Drawing.Size(145, 20);
            this.lblSelectTable.TabIndex = 14;
            this.lblSelectTable.Text = "Select the Objects :";
            this.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TranslationObjectSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(329, 387);
            this.Controls.Add(this.lstObject);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this.lblSelectTable);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TranslationObjectSelect";
            this.Text = "Select Translation Target Objects";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TranslationObjectSelect_Load);
            this.ResumeLayout(false);

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