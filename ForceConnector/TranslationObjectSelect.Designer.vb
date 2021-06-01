<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TranslationObjectSelect
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
        Me.lstObject = New System.Windows.Forms.ListView()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblSelectTable = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lstObject
        '
        Me.lstObject.HideSelection = False
        Me.lstObject.Location = New System.Drawing.Point(8, 27)
        Me.lstObject.Margin = New System.Windows.Forms.Padding(2)
        Me.lstObject.Name = "lstObject"
        Me.lstObject.Size = New System.Drawing.Size(311, 310)
        Me.lstObject.TabIndex = 17
        Me.lstObject.UseCompatibleStateImageBehavior = False
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(227, 346)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(2)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(91, 31)
        Me.btnNext.TabIndex = 16
        Me.btnNext.Text = "Next >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(8, 346)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(91, 31)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblSelectTable
        '
        Me.lblSelectTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectTable.Location = New System.Drawing.Point(9, 5)
        Me.lblSelectTable.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSelectTable.Name = "lblSelectTable"
        Me.lblSelectTable.Size = New System.Drawing.Size(145, 20)
        Me.lblSelectTable.TabIndex = 14
        Me.lblSelectTable.Text = "Select the Objects :"
        Me.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TranslationObjectSelect
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(329, 387)
        Me.Controls.Add(Me.lstObject)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelectTable)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "TranslationObjectSelect"
        Me.Text = "Select Translation Target Objects"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstObject As Windows.Forms.ListView
    Friend WithEvents btnNext As Windows.Forms.Button
    Friend WithEvents btnCancel As Windows.Forms.Button
    Friend WithEvents lblSelectTable As Windows.Forms.Label
End Class
