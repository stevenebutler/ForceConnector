<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOption
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
        Me.chkUseReference = New System.Windows.Forms.CheckBox()
        Me.chkNoWarning = New System.Windows.Forms.CheckBox()
        Me.chkNoLimit = New System.Windows.Forms.CheckBox()
        Me.chkDisableAssignRule = New System.Windows.Forms.CheckBox()
        Me.chkSkipHidden = New System.Windows.Forms.CheckBox()
        Me.chkDisableManaged = New System.Windows.Forms.CheckBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'chkUseReference
        '
        Me.chkUseReference.AutoSize = True
        Me.chkUseReference.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseReference.Location = New System.Drawing.Point(12, 14)
        Me.chkUseReference.Name = "chkUseReference"
        Me.chkUseReference.Size = New System.Drawing.Size(175, 18)
        Me.chkUseReference.TabIndex = 14
        Me.chkUseReference.Text = "Use Reference Name/Id"
        Me.chkUseReference.UseVisualStyleBackColor = True
        '
        'chkNoWarning
        '
        Me.chkNoWarning.AutoSize = True
        Me.chkNoWarning.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoWarning.Location = New System.Drawing.Point(12, 49)
        Me.chkNoWarning.Name = "chkNoWarning"
        Me.chkNoWarning.Size = New System.Drawing.Size(213, 18)
        Me.chkNoWarning.TabIndex = 15
        Me.chkNoWarning.Text = "Not Show Warning Dialog Box"
        Me.chkNoWarning.UseVisualStyleBackColor = True
        '
        'chkNoLimit
        '
        Me.chkNoLimit.AutoSize = True
        Me.chkNoLimit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoLimit.Location = New System.Drawing.Point(12, 82)
        Me.chkNoLimit.Name = "chkNoLimit"
        Me.chkNoLimit.Size = New System.Drawing.Size(118, 18)
        Me.chkNoLimit.TabIndex = 16
        Me.chkNoLimit.Text = "No Query Limit"
        Me.chkNoLimit.UseVisualStyleBackColor = True
        '
        'chkDisableAssignRule
        '
        Me.chkDisableAssignRule.AutoSize = True
        Me.chkDisableAssignRule.Checked = True
        Me.chkDisableAssignRule.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDisableAssignRule.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisableAssignRule.Location = New System.Drawing.Point(12, 117)
        Me.chkDisableAssignRule.Name = "chkDisableAssignRule"
        Me.chkDisableAssignRule.Size = New System.Drawing.Size(186, 18)
        Me.chkDisableAssignRule.TabIndex = 17
        Me.chkDisableAssignRule.Text = "Supress Auto Assign Rule"
        Me.chkDisableAssignRule.UseVisualStyleBackColor = True
        '
        'chkSkipHidden
        '
        Me.chkSkipHidden.AutoSize = True
        Me.chkSkipHidden.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSkipHidden.Location = New System.Drawing.Point(12, 151)
        Me.chkSkipHidden.Name = "chkSkipHidden"
        Me.chkSkipHidden.Size = New System.Drawing.Size(197, 18)
        Me.chkSkipHidden.TabIndex = 18
        Me.chkSkipHidden.Text = "Skip Hidden Columns/Rows"
        Me.chkSkipHidden.UseVisualStyleBackColor = True
        '
        'chkDisableManaged
        '
        Me.chkDisableManaged.AutoSize = True
        Me.chkDisableManaged.Checked = True
        Me.chkDisableManaged.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDisableManaged.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisableManaged.Location = New System.Drawing.Point(12, 185)
        Me.chkDisableManaged.Name = "chkDisableManaged"
        Me.chkDisableManaged.Size = New System.Drawing.Size(276, 18)
        Me.chkDisableManaged.TabIndex = 19
        Me.chkDisableManaged.Text = "Not Use Managed Data (for Translation)"
        Me.chkDisableManaged.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(119, 221)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 30)
        Me.btnOK.TabIndex = 20
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmOption
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(318, 260)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.chkDisableManaged)
        Me.Controls.Add(Me.chkSkipHidden)
        Me.Controls.Add(Me.chkDisableAssignRule)
        Me.Controls.Add(Me.chkNoLimit)
        Me.Controls.Add(Me.chkNoWarning)
        Me.Controls.Add(Me.chkUseReference)
        Me.Name = "frmOption"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkUseReference As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoWarning As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoLimit As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableAssignRule As System.Windows.Forms.CheckBox
    Friend WithEvents chkSkipHidden As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableManaged As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
