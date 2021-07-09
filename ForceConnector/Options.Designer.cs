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
            chkUseReference = new System.Windows.Forms.CheckBox();
            chkNoWarning = new System.Windows.Forms.CheckBox();
            chkNoLimit = new System.Windows.Forms.CheckBox();
            chkDisableAssignRule = new System.Windows.Forms.CheckBox();
            chkSkipHidden = new System.Windows.Forms.CheckBox();
            chkDisableManaged = new System.Windows.Forms.CheckBox();
            _btnOK = new System.Windows.Forms.Button();
            _btnOK.Click += new EventHandler(btnOK_Click);
            SuspendLayout();
            // 
            // chkUseReference
            // 
            chkUseReference.AutoSize = true;
            chkUseReference.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkUseReference.Location = new System.Drawing.Point(12, 14);
            chkUseReference.Name = "chkUseReference";
            chkUseReference.Size = new System.Drawing.Size(175, 18);
            chkUseReference.TabIndex = 14;
            chkUseReference.Text = "Use Reference Name/Id";
            chkUseReference.UseVisualStyleBackColor = true;
            // 
            // chkNoWarning
            // 
            chkNoWarning.AutoSize = true;
            chkNoWarning.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkNoWarning.Location = new System.Drawing.Point(12, 49);
            chkNoWarning.Name = "chkNoWarning";
            chkNoWarning.Size = new System.Drawing.Size(213, 18);
            chkNoWarning.TabIndex = 15;
            chkNoWarning.Text = "Not Show Warning Dialog Box";
            chkNoWarning.UseVisualStyleBackColor = true;
            // 
            // chkNoLimit
            // 
            chkNoLimit.AutoSize = true;
            chkNoLimit.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkNoLimit.Location = new System.Drawing.Point(12, 82);
            chkNoLimit.Name = "chkNoLimit";
            chkNoLimit.Size = new System.Drawing.Size(118, 18);
            chkNoLimit.TabIndex = 16;
            chkNoLimit.Text = "No Query Limit";
            chkNoLimit.UseVisualStyleBackColor = true;
            // 
            // chkDisableAssignRule
            // 
            chkDisableAssignRule.AutoSize = true;
            chkDisableAssignRule.Checked = true;
            chkDisableAssignRule.CheckState = System.Windows.Forms.CheckState.Checked;
            chkDisableAssignRule.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkDisableAssignRule.Location = new System.Drawing.Point(12, 117);
            chkDisableAssignRule.Name = "chkDisableAssignRule";
            chkDisableAssignRule.Size = new System.Drawing.Size(186, 18);
            chkDisableAssignRule.TabIndex = 17;
            chkDisableAssignRule.Text = "Supress Auto Assign Rule";
            chkDisableAssignRule.UseVisualStyleBackColor = true;
            // 
            // chkSkipHidden
            // 
            chkSkipHidden.AutoSize = true;
            chkSkipHidden.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkSkipHidden.Location = new System.Drawing.Point(12, 151);
            chkSkipHidden.Name = "chkSkipHidden";
            chkSkipHidden.Size = new System.Drawing.Size(197, 18);
            chkSkipHidden.TabIndex = 18;
            chkSkipHidden.Text = "Skip Hidden Columns/Rows";
            chkSkipHidden.UseVisualStyleBackColor = true;
            // 
            // chkDisableManaged
            // 
            chkDisableManaged.AutoSize = true;
            chkDisableManaged.Checked = true;
            chkDisableManaged.CheckState = System.Windows.Forms.CheckState.Checked;
            chkDisableManaged.Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            chkDisableManaged.Location = new System.Drawing.Point(12, 185);
            chkDisableManaged.Name = "chkDisableManaged";
            chkDisableManaged.Size = new System.Drawing.Size(276, 18);
            chkDisableManaged.TabIndex = 19;
            chkDisableManaged.Text = "Not Use Managed Data (for Translation)";
            chkDisableManaged.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            _btnOK.Location = new System.Drawing.Point(119, 221);
            _btnOK.Name = "_btnOK";
            _btnOK.Size = new System.Drawing.Size(80, 30);
            _btnOK.TabIndex = 20;
            _btnOK.Text = "OK";
            _btnOK.UseVisualStyleBackColor = true;
            // 
            // frmOption
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(318, 260);
            ControlBox = false;
            Controls.Add(_btnOK);
            Controls.Add(chkDisableManaged);
            Controls.Add(chkSkipHidden);
            Controls.Add(chkDisableAssignRule);
            Controls.Add(chkNoLimit);
            Controls.Add(chkNoWarning);
            Controls.Add(chkUseReference);
            Name = "frmOption";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Options";
            Load += new EventHandler(Options_Load);
            ResumeLayout(false);
            PerformLayout();
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