'Imports ForceConnector.SOAP

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.labelDestination = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblSecurityKey = New System.Windows.Forms.Label()
        Me.txtSecurityToken = New System.Windows.Forms.TextBox()
        Me.cmbDestination = New System.Windows.Forms.ComboBox()
        Me.cmbVersion = New System.Windows.Forms.ComboBox()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.usingOAuth2 = New System.Windows.Forms.RadioButton()
        Me.usingSOAP = New System.Windows.Forms.RadioButton()
        Me.loginUsing = New System.Windows.Forms.GroupBox()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.responseBox = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.grpAPI = New System.Windows.Forms.GroupBox()
        Me.SOAPAPI = New System.Windows.Forms.RadioButton()
        Me.RESTFULAPI = New System.Windows.Forms.RadioButton()
        Me.loginUsing.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAPI.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblUsername
        '
        Me.lblUsername.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsername.Location = New System.Drawing.Point(11, 121)
        Me.lblUsername.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(122, 17)
        Me.lblUsername.TabIndex = 1
        Me.lblUsername.Text = "Username :"
        Me.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPassword
        '
        Me.lblPassword.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.Location = New System.Drawing.Point(11, 153)
        Me.lblPassword.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(122, 17)
        Me.lblPassword.TabIndex = 2
        Me.lblPassword.Text = "Password :"
        Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'labelDestination
        '
        Me.labelDestination.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelDestination.Location = New System.Drawing.Point(11, 86)
        Me.labelDestination.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelDestination.Name = "labelDestination"
        Me.labelDestination.Size = New System.Drawing.Size(122, 17)
        Me.labelDestination.TabIndex = 3
        Me.labelDestination.Text = "Destination :"
        Me.labelDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(272, 86)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(42, 17)
        Me.lblVersion.TabIndex = 4
        Me.lblVersion.Text = "API :"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtUsername
        '
        Me.txtUsername.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(133, 119)
        Me.txtUsername.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(251, 24)
        Me.txtUsername.TabIndex = 5
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(133, 151)
        Me.txtPassword.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(251, 24)
        Me.txtPassword.TabIndex = 6
        '
        'lblSecurityKey
        '
        Me.lblSecurityKey.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSecurityKey.Location = New System.Drawing.Point(11, 185)
        Me.lblSecurityKey.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSecurityKey.Name = "lblSecurityKey"
        Me.lblSecurityKey.Size = New System.Drawing.Size(122, 17)
        Me.lblSecurityKey.TabIndex = 7
        Me.lblSecurityKey.Text = "Security Token :"
        Me.lblSecurityKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSecurityToken
        '
        Me.txtSecurityToken.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSecurityToken.Location = New System.Drawing.Point(133, 183)
        Me.txtSecurityToken.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSecurityToken.Name = "txtSecurityToken"
        Me.txtSecurityToken.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSecurityToken.Size = New System.Drawing.Size(251, 24)
        Me.txtSecurityToken.TabIndex = 8
        '
        'cmbDestination
        '
        Me.cmbDestination.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDestination.FormattingEnabled = True
        Me.cmbDestination.Items.AddRange(New Object() {"Production", "Sandbox"})
        Me.cmbDestination.Location = New System.Drawing.Point(133, 84)
        Me.cmbDestination.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbDestination.Name = "cmbDestination"
        Me.cmbDestination.Size = New System.Drawing.Size(100, 24)
        Me.cmbDestination.TabIndex = 9
        Me.cmbDestination.Text = "Production"
        '
        'cmbVersion
        '
        Me.cmbVersion.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbVersion.FormattingEnabled = True
        Me.cmbVersion.Items.AddRange(New Object() {"51", "50", "49", "48"})
        Me.cmbVersion.Location = New System.Drawing.Point(313, 84)
        Me.cmbVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbVersion.Name = "cmbVersion"
        Me.cmbVersion.Size = New System.Drawing.Size(71, 24)
        Me.cmbVersion.TabIndex = 10
        Me.cmbVersion.Text = "51"
        '
        'btnLogin
        '
        Me.btnLogin.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.Location = New System.Drawing.Point(284, 215)
        Me.btnLogin.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(100, 30)
        Me.btnLogin.TabIndex = 11
        Me.btnLogin.Text = "Login"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(86, 267)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'usingOAuth2
        '
        Me.usingOAuth2.AutoSize = True
        Me.usingOAuth2.Checked = True
        Me.usingOAuth2.Location = New System.Drawing.Point(9, 19)
        Me.usingOAuth2.Name = "usingOAuth2"
        Me.usingOAuth2.Size = New System.Drawing.Size(99, 21)
        Me.usingOAuth2.TabIndex = 13
        Me.usingOAuth2.TabStop = True
        Me.usingOAuth2.Text = "OAuth 2.0"
        Me.usingOAuth2.UseVisualStyleBackColor = True
        '
        'usingSOAP
        '
        Me.usingSOAP.AutoSize = True
        Me.usingSOAP.Location = New System.Drawing.Point(9, 41)
        Me.usingSOAP.Name = "usingSOAP"
        Me.usingSOAP.Size = New System.Drawing.Size(169, 21)
        Me.usingSOAP.TabIndex = 14
        Me.usingSOAP.Text = "Username/Password"
        Me.usingSOAP.UseVisualStyleBackColor = True
        '
        'loginUsing
        '
        Me.loginUsing.Controls.Add(Me.usingOAuth2)
        Me.loginUsing.Controls.Add(Me.usingSOAP)
        Me.loginUsing.Location = New System.Drawing.Point(76, 5)
        Me.loginUsing.Name = "loginUsing"
        Me.loginUsing.Size = New System.Drawing.Size(180, 68)
        Me.loginUsing.TabIndex = 15
        Me.loginUsing.TabStop = False
        Me.loginUsing.Text = "Login Using"
        '
        'btnNext
        '
        Me.btnNext.Enabled = False
        Me.btnNext.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.Location = New System.Drawing.Point(209, 267)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(100, 30)
        Me.btnNext.TabIndex = 16
        Me.btnNext.Text = "Next"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'responseBox
        '
        Me.responseBox.BackColor = System.Drawing.SystemColors.Control
        Me.responseBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.responseBox.Location = New System.Drawing.Point(16, 215)
        Me.responseBox.Multiline = True
        Me.responseBox.Name = "responseBox"
        Me.responseBox.Size = New System.Drawing.Size(230, 30)
        Me.responseBox.TabIndex = 17
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ForceConnector.My.Resources.Resources.imgres
        Me.PictureBox1.InitialImage = Global.ForceConnector.My.Resources.Resources.imgres
        Me.PictureBox1.Location = New System.Drawing.Point(8, 9)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'grpAPI
        '
        Me.grpAPI.Controls.Add(Me.SOAPAPI)
        Me.grpAPI.Controls.Add(Me.RESTFULAPI)
        Me.grpAPI.Location = New System.Drawing.Point(260, 4)
        Me.grpAPI.Name = "grpAPI"
        Me.grpAPI.Size = New System.Drawing.Size(124, 69)
        Me.grpAPI.TabIndex = 18
        Me.grpAPI.TabStop = False
        Me.grpAPI.Text = "Using API"
        '
        'SOAPAPI
        '
        Me.SOAPAPI.AutoSize = True
        Me.SOAPAPI.Enabled = False
        Me.SOAPAPI.Location = New System.Drawing.Point(5, 42)
        Me.SOAPAPI.Name = "SOAPAPI"
        Me.SOAPAPI.Size = New System.Drawing.Size(93, 21)
        Me.SOAPAPI.TabIndex = 1
        Me.SOAPAPI.TabStop = True
        Me.SOAPAPI.Text = "SOAP API"
        Me.SOAPAPI.UseVisualStyleBackColor = True
        '
        'RESTFULAPI
        '
        Me.RESTFULAPI.AutoSize = True
        Me.RESTFULAPI.Checked = True
        Me.RESTFULAPI.Location = New System.Drawing.Point(5, 20)
        Me.RESTFULAPI.Name = "RESTFULAPI"
        Me.RESTFULAPI.Size = New System.Drawing.Size(109, 21)
        Me.RESTFULAPI.TabIndex = 0
        Me.RESTFULAPI.TabStop = True
        Me.RESTFULAPI.Text = "RESTful API"
        Me.RESTFULAPI.UseVisualStyleBackColor = True
        '
        'frmLogin
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(396, 311)
        Me.Controls.Add(Me.grpAPI)
        Me.Controls.Add(Me.responseBox)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.loginUsing)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.cmbVersion)
        Me.Controls.Add(Me.cmbDestination)
        Me.Controls.Add(Me.txtSecurityToken)
        Me.Controls.Add(Me.lblSecurityKey)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtUsername)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.labelDestination)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblUsername)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login to Salesforce"
        Me.loginUsing.ResumeLayout(False)
        Me.loginUsing.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAPI.ResumeLayout(False)
        Me.grpAPI.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents labelDestination As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblSecurityKey As System.Windows.Forms.Label
    Friend WithEvents txtSecurityToken As System.Windows.Forms.TextBox
    Friend WithEvents cmbDestination As System.Windows.Forms.ComboBox
    Friend WithEvents cmbVersion As System.Windows.Forms.ComboBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents usingOAuth2 As System.Windows.Forms.RadioButton
    Friend WithEvents usingSOAP As System.Windows.Forms.RadioButton
    Friend WithEvents loginUsing As System.Windows.Forms.GroupBox
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents responseBox As System.Windows.Forms.TextBox
    Friend WithEvents grpAPI As Windows.Forms.GroupBox
    Friend WithEvents SOAPAPI As Windows.Forms.RadioButton
    Friend WithEvents RESTFULAPI As Windows.Forms.RadioButton
End Class
