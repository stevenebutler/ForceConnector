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
            _btnClose = new System.Windows.Forms.Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            lblComments3 = new System.Windows.Forms.Label();
            lblBrand = new System.Windows.Forms.Label();
            lblAuthor = new System.Windows.Forms.Label();
            imageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)imageBox).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            _btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            _btnClose.Location = new System.Drawing.Point(171, 295);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new System.Drawing.Size(103, 43);
            _btnClose.TabIndex = 8;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // lblComments3
            // 
            lblComments3.AutoSize = true;
            lblComments3.Font = new System.Drawing.Font("Arial", 12.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblComments3.Location = new System.Drawing.Point(9, 259);
            lblComments3.Name = "lblComments3";
            lblComments3.Size = new System.Drawing.Size(390, 18);
            lblComments3.TabIndex = 12;
            lblComments3.Text = "https://github.com/good-ghost/Forcedotcom_Connector";
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblBrand.Location = new System.Drawing.Point(6, 188);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new System.Drawing.Size(115, 22);
            lblBrand.TabIndex = 13;
            lblBrand.Text = "Brand Name";
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.Font = new System.Drawing.Font("Arial", 12.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblAuthor.Location = new System.Drawing.Point(149, 228);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new System.Drawing.Size(282, 18);
            lblAuthor.TabIndex = 14;
            lblAuthor.Text = "author : mingyoon.woo at gmail dot com";
            // 
            // imageBox
            // 
            imageBox.BackgroundImage = My.Resources.Resources.opensource;
            imageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            imageBox.InitialImage = My.Resources.Resources.opensource;
            imageBox.Location = new System.Drawing.Point(11, 14);
            imageBox.Name = "imageBox";
            imageBox.Size = new System.Drawing.Size(423, 160);
            imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            imageBox.TabIndex = 9;
            imageBox.TabStop = false;
            // 
            // frmAbout
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(446, 355);
            ControlBox = false;
            Controls.Add(lblAuthor);
            Controls.Add(lblBrand);
            Controls.Add(lblComments3);
            Controls.Add(imageBox);
            Controls.Add(_btnClose);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAbout";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "About Force.com Connector...";
            ((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
            Load += new EventHandler(frmAbout_Load);
            ResumeLayout(false);
            PerformLayout();
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