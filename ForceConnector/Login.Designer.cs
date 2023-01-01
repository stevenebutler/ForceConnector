// Imports ForceConnector.SOAP

using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmLogin : System.Windows.Forms.Form
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.labelDestination = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblSecurityKey = new System.Windows.Forms.Label();
            this.txtSecurityToken = new System.Windows.Forms.TextBox();
            this.cmbDestination = new System.Windows.Forms.ComboBox();
            this.cmbVersion = new System.Windows.Forms.ComboBox();
            this._btnLogin = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._usingOAuth2 = new System.Windows.Forms.RadioButton();
            this._usingSOAP = new System.Windows.Forms.RadioButton();
            this.loginUsing = new System.Windows.Forms.GroupBox();
            this._btnNext = new System.Windows.Forms.Button();
            this.responseBox = new System.Windows.Forms.TextBox();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpAPI = new System.Windows.Forms.GroupBox();
            this._SOAPAPI = new System.Windows.Forms.RadioButton();
            this._RESTFULAPI = new System.Windows.Forms.RadioButton();
            this.loginUsing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.grpAPI.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.Location = new System.Drawing.Point(20, 208);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(184, 39);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username :";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(20, 280);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(184, 27);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password :";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDestination
            // 
            this.labelDestination.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDestination.Location = new System.Drawing.Point(20, 153);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(184, 34);
            this.labelDestination.TabIndex = 3;
            this.labelDestination.Text = "Destination :";
            this.labelDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(469, 151);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(101, 33);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "API :";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(225, 208);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(238, 32);
            this.txtUsername.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(225, 272);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(238, 32);
            this.txtPassword.TabIndex = 6;
            // 
            // lblSecurityKey
            // 
            this.lblSecurityKey.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecurityKey.Location = new System.Drawing.Point(15, 339);
            this.lblSecurityKey.Name = "lblSecurityKey";
            this.lblSecurityKey.Size = new System.Drawing.Size(195, 34);
            this.lblSecurityKey.TabIndex = 7;
            this.lblSecurityKey.Text = "Security Token :";
            this.lblSecurityKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSecurityToken
            // 
            this.txtSecurityToken.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSecurityToken.Location = new System.Drawing.Point(225, 339);
            this.txtSecurityToken.Name = "txtSecurityToken";
            this.txtSecurityToken.PasswordChar = '*';
            this.txtSecurityToken.Size = new System.Drawing.Size(238, 32);
            this.txtSecurityToken.TabIndex = 8;
            // 
            // cmbDestination
            // 
            this.cmbDestination.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Items.AddRange(new object[] {
            "Production",
            "Sandbox"});
            this.cmbDestination.Location = new System.Drawing.Point(225, 152);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.Size = new System.Drawing.Size(238, 33);
            this.cmbDestination.TabIndex = 9;
            this.cmbDestination.Text = "Production";
            // 
            // cmbVersion
            // 
            this.cmbVersion.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVersion.FormattingEnabled = true;
            this.cmbVersion.Items.AddRange(new object[] {
            "56",
            "55",
            "54",
            "53",
            "52",
            "51"});
            this.cmbVersion.Location = new System.Drawing.Point(591, 152);
            this.cmbVersion.Name = "cmbVersion";
            this.cmbVersion.Size = new System.Drawing.Size(62, 33);
            this.cmbVersion.TabIndex = 10;
            this.cmbVersion.Text = "56";
            // 
            // _btnLogin
            // 
            this._btnLogin.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnLogin.Location = new System.Drawing.Point(537, 406);
            this._btnLogin.Name = "_btnLogin";
            this._btnLogin.Size = new System.Drawing.Size(118, 54);
            this._btnLogin.TabIndex = 11;
            this._btnLogin.Text = "Login";
            this._btnLogin.UseVisualStyleBackColor = true;
            this._btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnCancel.Location = new System.Drawing.Point(405, 489);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(126, 48);
            this._btnCancel.TabIndex = 12;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _usingOAuth2
            // 
            this._usingOAuth2.AutoSize = true;
            this._usingOAuth2.Checked = true;
            this._usingOAuth2.Location = new System.Drawing.Point(6, 32);
            this._usingOAuth2.Name = "_usingOAuth2";
            this._usingOAuth2.Size = new System.Drawing.Size(141, 29);
            this._usingOAuth2.TabIndex = 13;
            this._usingOAuth2.TabStop = true;
            this._usingOAuth2.Text = "OAuth 2.0";
            this._usingOAuth2.UseVisualStyleBackColor = true;
            this._usingOAuth2.CheckedChanged += new System.EventHandler(this.usingOAuth2_CheckedChanged);
            // 
            // _usingSOAP
            // 
            this._usingSOAP.AutoSize = true;
            this._usingSOAP.Location = new System.Drawing.Point(6, 66);
            this._usingSOAP.Name = "_usingSOAP";
            this._usingSOAP.Size = new System.Drawing.Size(241, 29);
            this._usingSOAP.TabIndex = 14;
            this._usingSOAP.Text = "Username/Password";
            this._usingSOAP.UseVisualStyleBackColor = true;
            this._usingSOAP.CheckedChanged += new System.EventHandler(this.usingSOAP_CheckedChanged);
            // 
            // loginUsing
            // 
            this.loginUsing.Controls.Add(this._usingOAuth2);
            this.loginUsing.Controls.Add(this._usingSOAP);
            this.loginUsing.Location = new System.Drawing.Point(122, 16);
            this.loginUsing.Name = "loginUsing";
            this.loginUsing.Size = new System.Drawing.Size(310, 112);
            this.loginUsing.TabIndex = 15;
            this.loginUsing.TabStop = false;
            this.loginUsing.Text = "Login Using";
            // 
            // _btnNext
            // 
            this._btnNext.Enabled = false;
            this._btnNext.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnNext.Location = new System.Drawing.Point(537, 489);
            this._btnNext.Name = "_btnNext";
            this._btnNext.Size = new System.Drawing.Size(120, 48);
            this._btnNext.TabIndex = 16;
            this._btnNext.Text = "Next";
            this._btnNext.UseVisualStyleBackColor = true;
            this._btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // responseBox
            // 
            this.responseBox.BackColor = System.Drawing.SystemColors.Control;
            this.responseBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.responseBox.Location = new System.Drawing.Point(20, 406);
            this.responseBox.Multiline = true;
            this.responseBox.Name = "responseBox";
            this.responseBox.Size = new System.Drawing.Size(489, 54);
            this.responseBox.TabIndex = 17;
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::ForceConnector.My.Resources.Resources.imgres;
            this.PictureBox1.InitialImage = global::ForceConnector.My.Resources.Resources.imgres;
            this.PictureBox1.Location = new System.Drawing.Point(16, 32);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(99, 98);
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // grpAPI
            // 
            this.grpAPI.Controls.Add(this._SOAPAPI);
            this.grpAPI.Controls.Add(this._RESTFULAPI);
            this.grpAPI.Location = new System.Drawing.Point(460, 16);
            this.grpAPI.Name = "grpAPI";
            this.grpAPI.Size = new System.Drawing.Size(195, 112);
            this.grpAPI.TabIndex = 18;
            this.grpAPI.TabStop = false;
            this.grpAPI.Text = "Using API";
            // 
            // _SOAPAPI
            // 
            this._SOAPAPI.AutoSize = true;
            this._SOAPAPI.Enabled = false;
            this._SOAPAPI.Location = new System.Drawing.Point(21, 66);
            this._SOAPAPI.Name = "_SOAPAPI";
            this._SOAPAPI.Size = new System.Drawing.Size(134, 29);
            this._SOAPAPI.TabIndex = 1;
            this._SOAPAPI.TabStop = true;
            this._SOAPAPI.Text = "SOAP API";
            this._SOAPAPI.UseVisualStyleBackColor = true;
            this._SOAPAPI.CheckedChanged += new System.EventHandler(this.SOAPAPI_CheckedChanged);
            // 
            // _RESTFULAPI
            // 
            this._RESTFULAPI.AutoSize = true;
            this._RESTFULAPI.Checked = true;
            this._RESTFULAPI.Location = new System.Drawing.Point(21, 32);
            this._RESTFULAPI.Name = "_RESTFULAPI";
            this._RESTFULAPI.Size = new System.Drawing.Size(157, 29);
            this._RESTFULAPI.TabIndex = 0;
            this._RESTFULAPI.TabStop = true;
            this._RESTFULAPI.Text = "RESTful API";
            this._RESTFULAPI.UseVisualStyleBackColor = true;
            this._RESTFULAPI.CheckedChanged += new System.EventHandler(this.RESTFULAPI_CheckedChanged);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(674, 554);
            this.Controls.Add(this.grpAPI);
            this.Controls.Add(this.responseBox);
            this.Controls.Add(this._btnNext);
            this.Controls.Add(this.loginUsing);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnLogin);
            this.Controls.Add(this.cmbVersion);
            this.Controls.Add(this.cmbDestination);
            this.Controls.Add(this.txtSecurityToken);
            this.Controls.Add(this.lblSecurityKey);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.PictureBox1);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login to Salesforce";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.loginUsing.ResumeLayout(false);
            this.loginUsing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.grpAPI.ResumeLayout(false);
            this.grpAPI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Label lblUsername;
        internal System.Windows.Forms.Label lblPassword;
        internal System.Windows.Forms.Label labelDestination;
        internal System.Windows.Forms.Label lblVersion;
        internal System.Windows.Forms.TextBox txtUsername;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.Label lblSecurityKey;
        internal System.Windows.Forms.TextBox txtSecurityToken;
        internal System.Windows.Forms.ComboBox cmbDestination;
        internal System.Windows.Forms.ComboBox cmbVersion;
        private System.Windows.Forms.Button _btnLogin;

        internal System.Windows.Forms.Button btnLogin
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnLogin;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnLogin != null)
                {
                    _btnLogin.Click -= btnLogin_Click;
                }

                _btnLogin = value;
                if (_btnLogin != null)
                {
                    _btnLogin.Click += btnLogin_Click;
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

        private System.Windows.Forms.RadioButton _usingOAuth2;

        internal System.Windows.Forms.RadioButton usingOAuth2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _usingOAuth2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_usingOAuth2 != null)
                {
                    _usingOAuth2.CheckedChanged -= usingOAuth2_CheckedChanged;
                }

                _usingOAuth2 = value;
                if (_usingOAuth2 != null)
                {
                    _usingOAuth2.CheckedChanged += usingOAuth2_CheckedChanged;
                }
            }
        }

        private System.Windows.Forms.RadioButton _usingSOAP;

        internal System.Windows.Forms.RadioButton usingSOAP
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _usingSOAP;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_usingSOAP != null)
                {
                    _usingSOAP.CheckedChanged -= usingSOAP_CheckedChanged;
                }

                _usingSOAP = value;
                if (_usingSOAP != null)
                {
                    _usingSOAP.CheckedChanged += usingSOAP_CheckedChanged;
                }
            }
        }

        internal System.Windows.Forms.GroupBox loginUsing;
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

        internal System.Windows.Forms.TextBox responseBox;
        internal System.Windows.Forms.GroupBox grpAPI;
        private System.Windows.Forms.RadioButton _SOAPAPI;

        internal System.Windows.Forms.RadioButton SOAPAPI
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _SOAPAPI;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_SOAPAPI != null)
                {
                    _SOAPAPI.CheckedChanged -= SOAPAPI_CheckedChanged;
                }

                _SOAPAPI = value;
                if (_SOAPAPI != null)
                {
                    _SOAPAPI.CheckedChanged += SOAPAPI_CheckedChanged;
                }
            }
        }

        private System.Windows.Forms.RadioButton _RESTFULAPI;

        internal System.Windows.Forms.RadioButton RESTFULAPI
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _RESTFULAPI;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_RESTFULAPI != null)
                {
                    _RESTFULAPI.CheckedChanged -= RESTFULAPI_CheckedChanged;
                }

                _RESTFULAPI = value;
                if (_RESTFULAPI != null)
                {
                    _RESTFULAPI.CheckedChanged += RESTFULAPI_CheckedChanged;
                }
            }
        }
    }
}