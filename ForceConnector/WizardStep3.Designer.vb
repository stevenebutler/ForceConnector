<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWizardStep3
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
        Me.lblSelectFields = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lstField = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'lblSelectFields
        '
        Me.lblSelectFields.AutoSize = True
        Me.lblSelectFields.Location = New System.Drawing.Point(14, 16)
        Me.lblSelectFields.Name = "lblSelectFields"
        Me.lblSelectFields.Size = New System.Drawing.Size(161, 14)
        Me.lblSelectFields.TabIndex = 0
        Me.lblSelectFields.Text = "Select Fields to Include :"
        Me.lblSelectFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(13, 448)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(222, 448)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(120, 40)
        Me.btnBack.TabIndex = 3
        Me.btnBack.Text = "< Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(358, 448)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(120, 40)
        Me.btnNext.TabIndex = 4
        Me.btnNext.Text = "Next >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'lstField
        '
        Me.lstField.FullRowSelect = True
        Me.lstField.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstField.HideSelection = False
        Me.lstField.Location = New System.Drawing.Point(13, 42)
        Me.lstField.Name = "lstField"
        Me.lstField.Size = New System.Drawing.Size(465, 396)
        Me.lstField.TabIndex = 5
        Me.lstField.UseCompatibleStateImageBehavior = False
        '
        'frmWizardStep3
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(492, 496)
        Me.ControlBox = False
        Me.Controls.Add(Me.lstField)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelectFields)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWizardStep3"
        Me.ShowIcon = False
        Me.Text = "Table Query Wizard - Step 3 of 4"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSelectFields As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lstField As System.Windows.Forms.ListView
End Class
