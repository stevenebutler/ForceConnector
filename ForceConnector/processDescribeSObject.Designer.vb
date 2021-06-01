<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class processDescribeSObject
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
        Me.bgw = New System.ComponentModel.BackgroundWorker()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.btnAction = New System.Windows.Forms.Button()
        Me.progressDownload = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'bgw
        '
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(12, 45)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(143, 16)
        Me.lblMessage.TabIndex = 5
        Me.lblMessage.Text = "Process Messages..."
        '
        'btnAction
        '
        Me.btnAction.Enabled = False
        Me.btnAction.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAction.Location = New System.Drawing.Point(192, 80)
        Me.btnAction.Name = "btnAction"
        Me.btnAction.Size = New System.Drawing.Size(100, 30)
        Me.btnAction.TabIndex = 4
        Me.btnAction.Text = "[Action]"
        Me.btnAction.UseVisualStyleBackColor = True
        '
        'progressDownload
        '
        Me.progressDownload.Location = New System.Drawing.Point(12, 12)
        Me.progressDownload.Name = "progressDownload"
        Me.progressDownload.Size = New System.Drawing.Size(460, 23)
        Me.progressDownload.TabIndex = 3
        '
        'processDescribeSObject
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(484, 121)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.btnAction)
        Me.Controls.Add(Me.progressDownload)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "processDescribeSObject"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Describe sObjects"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents bgw As ComponentModel.BackgroundWorker
    Friend WithEvents lblMessage As Windows.Forms.Label
    Friend WithEvents btnAction As Windows.Forms.Button
    Friend WithEvents progressDownload As Windows.Forms.ProgressBar
End Class
