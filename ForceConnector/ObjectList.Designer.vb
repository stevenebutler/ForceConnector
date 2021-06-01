<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmObjectList
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
        Me.lstObject = New System.Windows.Forms.ListView()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblSelectTable = New System.Windows.Forms.Label()
        Me.chkStandard = New System.Windows.Forms.CheckBox()
        Me.chkCustom = New System.Windows.Forms.CheckBox()
        Me.chkSystem = New System.Windows.Forms.CheckBox()
        Me.cmbLang = New System.Windows.Forms.ComboBox()
        Me.lblSelectLang = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lstObject
        '
        Me.lstObject.HideSelection = False
        Me.lstObject.Location = New System.Drawing.Point(16, 68)
        Me.lstObject.Name = "lstObject"
        Me.lstObject.Size = New System.Drawing.Size(465, 440)
        Me.lstObject.TabIndex = 10
        Me.lstObject.UseCompatibleStateImageBehavior = False
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(270, 559)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(100, 30)
        Me.btnNext.TabIndex = 9
        Me.btnNext.Text = "Next >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(125, 559)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblSelectTable
        '
        Me.lblSelectTable.Location = New System.Drawing.Point(17, 14)
        Me.lblSelectTable.Name = "lblSelectTable"
        Me.lblSelectTable.Size = New System.Drawing.Size(217, 16)
        Me.lblSelectTable.TabIndex = 6
        Me.lblSelectTable.Text = "Select the Objects to Describe :"
        Me.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkStandard
        '
        Me.chkStandard.AutoSize = True
        Me.chkStandard.Checked = True
        Me.chkStandard.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStandard.Location = New System.Drawing.Point(19, 42)
        Me.chkStandard.Name = "chkStandard"
        Me.chkStandard.Size = New System.Drawing.Size(136, 18)
        Me.chkStandard.TabIndex = 11
        Me.chkStandard.Text = "Standard Objects"
        Me.chkStandard.UseVisualStyleBackColor = True
        '
        'chkCustom
        '
        Me.chkCustom.AutoSize = True
        Me.chkCustom.Checked = True
        Me.chkCustom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCustom.Location = New System.Drawing.Point(181, 42)
        Me.chkCustom.Name = "chkCustom"
        Me.chkCustom.Size = New System.Drawing.Size(126, 18)
        Me.chkCustom.TabIndex = 12
        Me.chkCustom.Text = "Custom Objects"
        Me.chkCustom.UseVisualStyleBackColor = True
        '
        'chkSystem
        '
        Me.chkSystem.AutoSize = True
        Me.chkSystem.Location = New System.Drawing.Point(342, 42)
        Me.chkSystem.Name = "chkSystem"
        Me.chkSystem.Size = New System.Drawing.Size(124, 18)
        Me.chkSystem.TabIndex = 13
        Me.chkSystem.Text = "System Objects"
        Me.chkSystem.UseVisualStyleBackColor = True
        '
        'cmbLang
        '
        Me.cmbLang.FormattingEnabled = True
        Me.cmbLang.Location = New System.Drawing.Point(186, 524)
        Me.cmbLang.Name = "cmbLang"
        Me.cmbLang.Size = New System.Drawing.Size(197, 22)
        Me.cmbLang.TabIndex = 14
        '
        'lblSelectLang
        '
        Me.lblSelectLang.AutoSize = True
        Me.lblSelectLang.Location = New System.Drawing.Point(20, 527)
        Me.lblSelectLang.Name = "lblSelectLang"
        Me.lblSelectLang.Size = New System.Drawing.Size(156, 14)
        Me.lblSelectLang.TabIndex = 15
        Me.lblSelectLang.Text = "Select Base Language :"
        '
        'frmObjectList
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(497, 601)
        Me.Controls.Add(Me.lblSelectLang)
        Me.Controls.Add(Me.cmbLang)
        Me.Controls.Add(Me.chkSystem)
        Me.Controls.Add(Me.chkCustom)
        Me.Controls.Add(Me.chkStandard)
        Me.Controls.Add(Me.lstObject)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelectTable)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmObjectList"
        Me.Text = "Describe SObjects"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lstObject As System.Windows.Forms.ListView
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblSelectTable As System.Windows.Forms.Label
    Friend WithEvents chkStandard As Windows.Forms.CheckBox
    Friend WithEvents chkCustom As Windows.Forms.CheckBox
    Friend WithEvents chkSystem As Windows.Forms.CheckBox
    Friend WithEvents cmbLang As Windows.Forms.ComboBox
    Friend WithEvents lblSelectLang As Windows.Forms.Label
End Class
