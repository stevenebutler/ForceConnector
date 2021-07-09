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
            lblUsername = new System.Windows.Forms.Label();
            lblPassword = new System.Windows.Forms.Label();
            labelDestination = new System.Windows.Forms.Label();
            lblVersion = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            lblSecurityKey = new System.Windows.Forms.Label();
            txtSecurityToken = new System.Windows.Forms.TextBox();
            cmbDestination = new System.Windows.Forms.ComboBox();
            cmbVersion = new System.Windows.Forms.ComboBox();
            _btnLogin = new System.Windows.Forms.Button();
            _btnLogin.Click += new EventHandler(btnLogin_Click);
            _btnCancel = new System.Windows.Forms.Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            _usingOAuth2 = new System.Windows.Forms.RadioButton();
            _usingOAuth2.CheckedChanged += new EventHandler(usingOAuth2_CheckedChanged);
            _usingSOAP = new System.Windows.Forms.RadioButton();
            _usingSOAP.CheckedChanged += new EventHandler(usingSOAP_CheckedChanged);
            loginUsing = new System.Windows.Forms.GroupBox();
            _btnNext = new System.Windows.Forms.Button();
            _btnNext.Click += new EventHandler(btnNext_Click);
            responseBox = new System.Windows.Forms.TextBox();
            PictureBox1 = new System.Windows.Forms.PictureBox();
            grpAPI = new System.Windows.Forms.GroupBox();
            _SOAPAPI = new System.Windows.Forms.RadioButton();
            _SOAPAPI.CheckedChanged += new EventHandler(SOAPAPI_CheckedChanged);
            _RESTFULAPI = new System.Windows.Forms.RadioButton();
            _RESTFULAPI.CheckedChanged += new EventHandler(RESTFULAPI_CheckedChanged);
            loginUsing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            grpAPI.SuspendLayout();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblUsername.Location = new System.Drawing.Point(11, 121);
            lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new System.Drawing.Size(122, 17);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username :";
            lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            lblPassword.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblPassword.Location = new System.Drawing.Point(11, 153);
            lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new System.Drawing.Size(122, 17);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password :";
            lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDestination
            // 
            labelDestination.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            labelDestination.Location = new System.Drawing.Point(11, 86);
            labelDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDestination.Name = "labelDestination";
            labelDestination.Size = new System.Drawing.Size(122, 17);
            labelDestination.TabIndex = 3;
            labelDestination.Text = "Destination :";
            labelDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVersion
            // 
            lblVersion.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblVersion.Location = new System.Drawing.Point(272, 86);
            lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(42, 17);
            lblVersion.TabIndex = 4;
            lblVersion.Text = "API :";
            lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUsername
            // 
            txtUsername.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            txtUsername.Location = new System.Drawing.Point(133, 119);
            txtUsername.Margin = new System.Windows.Forms.Padding(4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(251, 24);
            txtUsername.TabIndex = 5;
            // 
            // txtPassword
            // 
            txtPassword.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            txtPassword.Location = new System.Drawing.Point(133, 151);
            txtPassword.Margin = new System.Windows.Forms.Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new System.Drawing.Size(251, 24);
            txtPassword.TabIndex = 6;
            // 
            // lblSecurityKey
            // 
            lblSecurityKey.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            lblSecurityKey.Location = new System.Drawing.Point(11, 185);
            lblSecurityKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSecurityKey.Name = "lblSecurityKey";
            lblSecurityKey.Size = new System.Drawing.Size(122, 17);
            lblSecurityKey.TabIndex = 7;
            lblSecurityKey.Text = "Security Token :";
            lblSecurityKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSecurityToken
            // 
            txtSecurityToken.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            txtSecurityToken.Location = new System.Drawing.Point(133, 183);
            txtSecurityToken.Margin = new System.Windows.Forms.Padding(4);
            txtSecurityToken.Name = "txtSecurityToken";
            txtSecurityToken.PasswordChar = '*';
            txtSecurityToken.Size = new System.Drawing.Size(251, 24);
            txtSecurityToken.TabIndex = 8;
            // 
            // cmbDestination
            // 
            cmbDestination.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            cmbDestination.FormattingEnabled = true;
            cmbDestination.Items.AddRange(new object[] { "Production", "Sandbox" });
            cmbDestination.Location = new System.Drawing.Point(133, 84);
            cmbDestination.Margin = new System.Windows.Forms.Padding(4);
            cmbDestination.Name = "cmbDestination";
            cmbDestination.Size = new System.Drawing.Size(100, 24);
            cmbDestination.TabIndex = 9;
            cmbDestination.Text = "Production";
            // 
            // cmbVersion
            // 
            cmbVersion.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            cmbVersion.FormattingEnabled = true;
            cmbVersion.Items.AddRange(new object[] { "51", "50", "49", "48" });
            cmbVersion.Location = new System.Drawing.Point(313, 84);
            cmbVersion.Margin = new System.Windows.Forms.Padding(4);
            cmbVersion.Name = "cmbVersion";
            cmbVersion.Size = new System.Drawing.Size(71, 24);
            cmbVersion.TabIndex = 10;
            cmbVersion.Text = "51";
            // 
            // btnLogin
            // 
            _btnLogin.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            _btnLogin.Location = new System.Drawing.Point(284, 215);
            _btnLogin.Margin = new System.Windows.Forms.Padding(4);
            _btnLogin.Name = "_btnLogin";
            _btnLogin.Size = new System.Drawing.Size(100, 30);
            _btnLogin.TabIndex = 11;
            _btnLogin.Text = "Login";
            _btnLogin.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            _btnCancel.Location = new System.Drawing.Point(86, 267);
            _btnCancel.Margin = new System.Windows.Forms.Padding(4);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new System.Drawing.Size(100, 30);
            _btnCancel.TabIndex = 12;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // usingOAuth2
            // 
            _usingOAuth2.AutoSize = true;
            _usingOAuth2.Checked = true;
            _usingOAuth2.Location = new System.Drawing.Point(9, 19);
            _usingOAuth2.Name = "_usingOAuth2";
            _usingOAuth2.Size = new System.Drawing.Size(99, 21);
            _usingOAuth2.TabIndex = 13;
            _usingOAuth2.TabStop = true;
            _usingOAuth2.Text = "OAuth 2.0";
            _usingOAuth2.UseVisualStyleBackColor = true;
            // 
            // usingSOAP
            // 
            _usingSOAP.AutoSize = true;
            _usingSOAP.Location = new System.Drawing.Point(9, 41);
            _usingSOAP.Name = "_usingSOAP";
            _usingSOAP.Size = new System.Drawing.Size(169, 21);
            _usingSOAP.TabIndex = 14;
            _usingSOAP.Text = "Username/Password";
            _usingSOAP.UseVisualStyleBackColor = true;
            // 
            // loginUsing
            // 
            loginUsing.Controls.Add(_usingOAuth2);
            loginUsing.Controls.Add(_usingSOAP);
            loginUsing.Location = new System.Drawing.Point(76, 5);
            loginUsing.Name = "loginUsing";
            loginUsing.Size = new System.Drawing.Size(180, 68);
            loginUsing.TabIndex = 15;
            loginUsing.TabStop = false;
            loginUsing.Text = "Login Using";
            // 
            // btnNext
            // 
            _btnNext.Enabled = false;
            _btnNext.Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            _btnNext.Location = new System.Drawing.Point(209, 267);
            _btnNext.Margin = new System.Windows.Forms.Padding(4);
            _btnNext.Name = "_btnNext";
            _btnNext.Size = new System.Drawing.Size(100, 30);
            _btnNext.TabIndex = 16;
            _btnNext.Text = "Next";
            _btnNext.UseVisualStyleBackColor = true;
            // 
            // responseBox
            // 
            responseBox.BackColor = System.Drawing.SystemColors.Control;
            responseBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            responseBox.Location = new System.Drawing.Point(16, 215);
            responseBox.Multiline = true;
            responseBox.Name = "responseBox";
            responseBox.Size = new System.Drawing.Size(230, 30);
            responseBox.TabIndex = 17;
            // 
            // PictureBox1
            // 
            PictureBox1.Image = My.Resources.Resources.imgres;
            PictureBox1.InitialImage = My.Resources.Resources.imgres;
            PictureBox1.Location = new System.Drawing.Point(8, 9);
            PictureBox1.Margin = new System.Windows.Forms.Padding(4);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new System.Drawing.Size(64, 64);
            PictureBox1.TabIndex = 0;
            PictureBox1.TabStop = false;
            // 
            // grpAPI
            // 
            grpAPI.Controls.Add(_SOAPAPI);
            grpAPI.Controls.Add(_RESTFULAPI);
            grpAPI.Location = new System.Drawing.Point(260, 4);
            grpAPI.Name = "grpAPI";
            grpAPI.Size = new System.Drawing.Size(124, 69);
            grpAPI.TabIndex = 18;
            grpAPI.TabStop = false;
            grpAPI.Text = "Using API";
            // 
            // SOAPAPI
            // 
            _SOAPAPI.AutoSize = true;
            _SOAPAPI.Enabled = false;
            _SOAPAPI.Location = new System.Drawing.Point(5, 42);
            _SOAPAPI.Name = "_SOAPAPI";
            _SOAPAPI.Size = new System.Drawing.Size(93, 21);
            _SOAPAPI.TabIndex = 1;
            _SOAPAPI.TabStop = true;
            _SOAPAPI.Text = "SOAP API";
            _SOAPAPI.UseVisualStyleBackColor = true;
            // 
            // RESTFULAPI
            // 
            _RESTFULAPI.AutoSize = true;
            _RESTFULAPI.Checked = true;
            _RESTFULAPI.Location = new System.Drawing.Point(5, 20);
            _RESTFULAPI.Name = "_RESTFULAPI";
            _RESTFULAPI.Size = new System.Drawing.Size(109, 21);
            _RESTFULAPI.TabIndex = 0;
            _RESTFULAPI.TabStop = true;
            _RESTFULAPI.Text = "RESTful API";
            _RESTFULAPI.UseVisualStyleBackColor = true;
            // 
            // frmLogin
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(396, 311);
            Controls.Add(grpAPI);
            Controls.Add(responseBox);
            Controls.Add(_btnNext);
            Controls.Add(loginUsing);
            Controls.Add(_btnCancel);
            Controls.Add(_btnLogin);
            Controls.Add(cmbVersion);
            Controls.Add(cmbDestination);
            Controls.Add(txtSecurityToken);
            Controls.Add(lblSecurityKey);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblVersion);
            Controls.Add(labelDestination);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(PictureBox1);
            Font = new System.Drawing.Font("Verdana", 10.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Login to Salesforce";
            loginUsing.ResumeLayout(false);
            loginUsing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            grpAPI.ResumeLayout(false);
            grpAPI.PerformLayout();
            Load += new EventHandler(LoginForm_Load);
            ResumeLayout(false);
            PerformLayout();
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