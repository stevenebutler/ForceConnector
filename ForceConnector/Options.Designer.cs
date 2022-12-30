using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmOption : System.Windows.Forms.Form
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
            this.chkUseReference = new System.Windows.Forms.CheckBox();
            this.chkNoWarning = new System.Windows.Forms.CheckBox();
            this.chkNoLimit = new System.Windows.Forms.CheckBox();
            this.chkDisableAssignRule = new System.Windows.Forms.CheckBox();
            this.chkSkipHidden = new System.Windows.Forms.CheckBox();
            this.chkDisableManaged = new System.Windows.Forms.CheckBox();
            this._btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkUseReference
            // 
            this.chkUseReference.AutoSize = true;
            this.chkUseReference.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseReference.Location = new System.Drawing.Point(12, 14);
            this.chkUseReference.Name = "chkUseReference";
            this.chkUseReference.Size = new System.Drawing.Size(252, 26);
            this.chkUseReference.TabIndex = 14;
            this.chkUseReference.Text = "Use Reference Name/Id";
            this.chkUseReference.UseVisualStyleBackColor = true;
            // 
            // chkNoWarning
            // 
            this.chkNoWarning.AutoSize = true;
            this.chkNoWarning.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNoWarning.Location = new System.Drawing.Point(12, 49);
            this.chkNoWarning.Name = "chkNoWarning";
            this.chkNoWarning.Size = new System.Drawing.Size(305, 26);
            this.chkNoWarning.TabIndex = 15;
            this.chkNoWarning.Text = "Not Show Warning Dialog Box";
            this.chkNoWarning.UseVisualStyleBackColor = true;
            // 
            // chkNoLimit
            // 
            this.chkNoLimit.AutoSize = true;
            this.chkNoLimit.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNoLimit.Location = new System.Drawing.Point(12, 82);
            this.chkNoLimit.Name = "chkNoLimit";
            this.chkNoLimit.Size = new System.Drawing.Size(172, 26);
            this.chkNoLimit.TabIndex = 16;
            this.chkNoLimit.Text = "No Query Limit";
            this.chkNoLimit.UseVisualStyleBackColor = true;
            // 
            // chkDisableAssignRule
            // 
            this.chkDisableAssignRule.AutoSize = true;
            this.chkDisableAssignRule.Checked = true;
            this.chkDisableAssignRule.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisableAssignRule.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisableAssignRule.Location = new System.Drawing.Point(12, 117);
            this.chkDisableAssignRule.Name = "chkDisableAssignRule";
            this.chkDisableAssignRule.Size = new System.Drawing.Size(263, 26);
            this.chkDisableAssignRule.TabIndex = 17;
            this.chkDisableAssignRule.Text = "Supress Auto Assign Rule";
            this.chkDisableAssignRule.UseVisualStyleBackColor = true;
            // 
            // chkSkipHidden
            // 
            this.chkSkipHidden.AutoSize = true;
            this.chkSkipHidden.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSkipHidden.Location = new System.Drawing.Point(12, 151);
            this.chkSkipHidden.Name = "chkSkipHidden";
            this.chkSkipHidden.Size = new System.Drawing.Size(284, 26);
            this.chkSkipHidden.TabIndex = 18;
            this.chkSkipHidden.Text = "Skip Hidden Columns/Rows";
            this.chkSkipHidden.UseVisualStyleBackColor = true;
            // 
            // chkDisableManaged
            // 
            this.chkDisableManaged.AutoSize = true;
            this.chkDisableManaged.Checked = true;
            this.chkDisableManaged.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisableManaged.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisableManaged.Location = new System.Drawing.Point(12, 185);
            this.chkDisableManaged.Name = "chkDisableManaged";
            this.chkDisableManaged.Size = new System.Drawing.Size(393, 26);
            this.chkDisableManaged.TabIndex = 19;
            this.chkDisableManaged.Text = "Not Use Managed Data (for Translation)";
            this.chkDisableManaged.UseVisualStyleBackColor = true;
            // 
            // _btnOK
            // 
            this._btnOK.Location = new System.Drawing.Point(119, 221);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(80, 30);
            this._btnOK.TabIndex = 20;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmOption
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(423, 263);
            this.ControlBox = false;
            this.Controls.Add(this._btnOK);
            this.Controls.Add(this.chkDisableManaged);
            this.Controls.Add(this.chkSkipHidden);
            this.Controls.Add(this.chkDisableAssignRule);
            this.Controls.Add(this.chkNoLimit);
            this.Controls.Add(this.chkNoWarning);
            this.Controls.Add(this.chkUseReference);
            this.Name = "frmOption";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.CheckBox chkUseReference;
        internal System.Windows.Forms.CheckBox chkNoWarning;
        internal System.Windows.Forms.CheckBox chkNoLimit;
        internal System.Windows.Forms.CheckBox chkDisableAssignRule;
        internal System.Windows.Forms.CheckBox chkSkipHidden;
        internal System.Windows.Forms.CheckBox chkDisableManaged;
        private System.Windows.Forms.Button _btnOK;

        internal System.Windows.Forms.Button btnOK
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnOK;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnOK != null)
                {
                    _btnOK.Click -= btnOK_Click;
                }

                _btnOK = value;
                if (_btnOK != null)
                {
                    _btnOK.Click += btnOK_Click;
                }
            }
        }
    }
}