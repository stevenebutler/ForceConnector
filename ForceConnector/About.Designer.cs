using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmAbout : System.Windows.Forms.Form
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
            this._btnClose = new System.Windows.Forms.Button();
            this.lblComments3 = new System.Windows.Forms.Label();
            this.lblBrand = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.imageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnClose
            // 
            this._btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnClose.Location = new System.Drawing.Point(171, 295);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(103, 43);
            this._btnClose.TabIndex = 8;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblComments3
            // 
            this.lblComments3.AutoSize = true;
            this.lblComments3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComments3.Location = new System.Drawing.Point(9, 259);
            this.lblComments3.Name = "lblComments3";
            this.lblComments3.Size = new System.Drawing.Size(390, 18);
            this.lblComments3.TabIndex = 12;
            this.lblComments3.Text = "https://github.com/good-ghost/Forcedotcom_Connector";
            this.lblComments3.Click += new System.EventHandler(this.lblComments3_Click);
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrand.Location = new System.Drawing.Point(6, 188);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(115, 22);
            this.lblBrand.TabIndex = 13;
            this.lblBrand.Text = "Brand Name";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.Location = new System.Drawing.Point(149, 228);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(282, 18);
            this.lblAuthor.TabIndex = 14;
            this.lblAuthor.Text = "author : mingyoon.woo at gmail dot com";
            // 
            // imageBox
            // 
            this.imageBox.BackgroundImage = global::ForceConnector.My.Resources.Resources.opensource;
            this.imageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox.InitialImage = global::ForceConnector.My.Resources.Resources.opensource;
            this.imageBox.Location = new System.Drawing.Point(11, 14);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(423, 160);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox.TabIndex = 9;
            this.imageBox.TabStop = false;
            // 
            // frmAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(446, 355);
            this.ControlBox = false;
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblBrand);
            this.Controls.Add(this.lblComments3);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this._btnClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Force.com Connector...";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button _btnClose;

        internal System.Windows.Forms.Button btnClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClose != null)
                {
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
                }
            }
        }

        internal System.Windows.Forms.PictureBox imageBox;
        internal System.Windows.Forms.Label lblComments3;
        internal System.Windows.Forms.Label lblBrand;
        internal System.Windows.Forms.Label lblAuthor;
    }
}