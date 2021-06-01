<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWizardStep2
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
        Me.lblSelectTable = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.lstObject = New System.Windows.Forms.ListView()
        Me.chkSystem = New System.Windows.Forms.CheckBox()
        Me.chkCustom = New System.Windows.Forms.CheckBox()
        Me.chkStandard = New System.Windows.Forms.CheckBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblSelectTable
        '
        Me.lblSelectTable.Location = New System.Drawing.Point(14, 16)
        Me.lblSelectTable.Name = "lblSelectTable"
        Me.lblSelectTable.Size = New System.Drawing.Size(162, 14)
        Me.lblSelectTable.TabIndex = 0
        Me.lblSelectTable.Text = "Select a Table to Query :"
        Me.lblSelectTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(13, 466)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 40)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(222, 466)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(120, 40)
        Me.btnBack.TabIndex = 3
        Me.btnBack.Text = "< Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(358, 466)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(120, 40)
        Me.btnNext.TabIndex = 4
        Me.btnNext.Text = "Next >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'lstObject
        '
        Me.lstObject.FullRowSelect = True
        Me.lstObject.HideSelection = False
        Me.lstObject.Location = New System.Drawing.Point(13, 103)
        Me.lstObject.Name = "lstObject"
        Me.lstObject.Size = New System.Drawing.Size(465, 349)
        Me.lstObject.TabIndex = 5
        Me.lstObject.UseCompatibleStateImageBehavior = False
        '
        'chkSystem
        '
        Me.chkSystem.AutoSize = True
        Me.chkSystem.Location = New System.Drawing.Point(335, 79)
        Me.chkSystem.Name = "chkSystem"
        Me.chkSystem.Size = New System.Drawing.Size(124, 18)
        Me.chkSystem.TabIndex = 16
        Me.chkSystem.Text = "System Objects"
        Me.chkSystem.UseVisualStyleBackColor = True
        '
        'chkCustom
        '
        Me.chkCustom.AutoSize = True
        Me.chkCustom.Checked = True
        Me.chkCustom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCustom.Location = New System.Drawing.Point(178, 79)
        Me.chkCustom.Name = "chkCustom"
        Me.chkCustom.Size = New System.Drawing.Size(126, 18)
        Me.chkCustom.TabIndex = 15
        Me.chkCustom.Text = "Custom Objects"
        Me.chkCustom.UseVisualStyleBackColor = True
        '
        'chkStandard
        '
        Me.chkStandard.AutoSize = True
        Me.chkStandard.Checked = True
        Me.chkStandard.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStandard.Location = New System.Drawing.Point(17, 79)
        Me.chkStandard.Name = "chkStandard"
        Me.chkStandard.Size = New System.Drawing.Size(136, 18)
        Me.chkStandard.TabIndex = 14
        Me.chkStandard.Text = "Standard Objects"
        Me.chkStandard.UseVisualStyleBackColor = True
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(14, 45)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(115, 14)
        Me.lblSearch.TabIndex = 17
        Me.lblSearch.Text = "Search sObject : "
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(129, 42)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(349, 22)
        Me.txtSearch.TabIndex = 18
        '
        'frmWizardStep2
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(492, 515)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.chkSystem)
        Me.Controls.Add(Me.chkCustom)
        Me.Controls.Add(Me.chkStandard)
        Me.Controls.Add(Me.lstObject)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblSelectTable)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWizardStep2"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Table Query Wizard - Step 2 of 4"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSelectTable As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lstObject As System.Windows.Forms.ListView
    Friend WithEvents chkSystem As Windows.Forms.CheckBox
    Friend WithEvents chkCustom As Windows.Forms.CheckBox
    Friend WithEvents chkStandard As Windows.Forms.CheckBox
    Friend WithEvents lblSearch As Windows.Forms.Label
    Friend WithEvents txtSearch As Windows.Forms.TextBox
End Class
