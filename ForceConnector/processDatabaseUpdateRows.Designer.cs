using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class processDatabaseUpdateRows : System.Windows.Forms.Form
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
            _bgw = new System.ComponentModel.BackgroundWorker();
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            lblMessage = new System.Windows.Forms.Label();
            _btnAction = new System.Windows.Forms.Button();
            _btnAction.Click += new EventHandler(btnAction_Click);
            progressDownload = new System.Windows.Forms.ProgressBar();
            SuspendLayout();
            // 
            // bgw
            // 
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblMessage.Location = new System.Drawing.Point(12, 45);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new System.Drawing.Size(143, 16);
            lblMessage.TabIndex = 5;
            lblMessage.Text = "Process Messages...";
            // 
            // btnAction
            // 
            _btnAction.Enabled = false;
            _btnAction.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            _btnAction.Location = new System.Drawing.Point(192, 80);
            _btnAction.Name = "_btnAction";
            _btnAction.Size = new System.Drawing.Size(100, 30);
            _btnAction.TabIndex = 4;
            _btnAction.Text = "[Action]";
            _btnAction.UseVisualStyleBackColor = true;
            // 
            // progressDownload
            // 
            progressDownload.Location = new System.Drawing.Point(12, 12);
            progressDownload.Name = "progressDownload";
            progressDownload.Size = new System.Drawing.Size(460, 23);
            progressDownload.TabIndex = 3;
            // 
            // processDatabaseUpdateRows
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(484, 121);
            ControlBox = false;
            Controls.Add(lblMessage);
            Controls.Add(_btnAction);
            Controls.Add(progressDownload);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Name = "processDatabaseUpdateRows";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Update Selected Rows";
            TopMost = true;
            Load += new EventHandler(processDatabaseUpdateRows_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.ComponentModel.BackgroundWorker _bgw;

        internal System.ComponentModel.BackgroundWorker bgw
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _bgw;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_bgw != null)
                {
                    _bgw.DoWork -= bgw_DoWork;
                    _bgw.ProgressChanged -= bgw_ProgressChanged;
                    _bgw.RunWorkerCompleted -= bgw_RunWorkerCompleted;
                }

                _bgw = value;
                if (_bgw != null)
                {
                    _bgw.DoWork += bgw_DoWork;
                    _bgw.ProgressChanged += bgw_ProgressChanged;
                    _bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
                }
            }
        }

        internal System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button _btnAction;

        internal System.Windows.Forms.Button btnAction
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAction;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAction != null)
                {
                    _btnAction.Click -= btnAction_Click;
                }

                _btnAction = value;
                if (_btnAction != null)
                {
                    _btnAction.Click += btnAction_Click;
                }
            }
        }

        internal System.Windows.Forms.ProgressBar progressDownload;
    }
}