<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblComments3 = New System.Windows.Forms.Label()
        Me.lblBrand = New System.Windows.Forms.Label()
        Me.lblAuthor = New System.Windows.Forms.Label()
        Me.imageBox = New System.Windows.Forms.PictureBox()
        CType(Me.imageBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(171, 295)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(103, 43)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblComments3
        '
        Me.lblComments3.AutoSize = True
        Me.lblComments3.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComments3.Location = New System.Drawing.Point(9, 259)
        Me.lblComments3.Name = "lblComments3"
        Me.lblComments3.Size = New System.Drawing.Size(390, 18)
        Me.lblComments3.TabIndex = 12
        Me.lblComments3.Text = "https://github.com/good-ghost/Forcedotcom_Connector"
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBrand.Location = New System.Drawing.Point(6, 188)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(115, 22)
        Me.lblBrand.TabIndex = 13
        Me.lblBrand.Text = "Brand Name"
        '
        'lblAuthor
        '
        Me.lblAuthor.AutoSize = True
        Me.lblAuthor.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthor.Location = New System.Drawing.Point(149, 228)
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.Size = New System.Drawing.Size(282, 18)
        Me.lblAuthor.TabIndex = 14
        Me.lblAuthor.Text = "author : mingyoon.woo at gmail dot com"
        '
        'imageBox
        '
        Me.imageBox.BackgroundImage = Global.ForceConnector.My.Resources.Resources.opensource
        Me.imageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imageBox.InitialImage = Global.ForceConnector.My.Resources.Resources.opensource
        Me.imageBox.Location = New System.Drawing.Point(11, 14)
        Me.imageBox.Name = "imageBox"
        Me.imageBox.Size = New System.Drawing.Size(423, 160)
        Me.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imageBox.TabIndex = 9
        Me.imageBox.TabStop = False
        '
        'frmAbout
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(446, 355)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblAuthor)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.lblComments3)
        Me.Controls.Add(Me.imageBox)
        Me.Controls.Add(Me.btnClose)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAbout"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About Force.com Connector..."
        CType(Me.imageBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents imageBox As System.Windows.Forms.PictureBox
    Friend WithEvents lblComments3 As System.Windows.Forms.Label
    Friend WithEvents lblBrand As System.Windows.Forms.Label
    Friend WithEvents lblAuthor As System.Windows.Forms.Label
End Class
