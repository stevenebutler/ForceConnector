<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWizardStep4
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
        Me.grpAddClause = New System.Windows.Forms.GroupBox()
        Me.btnAddClause = New System.Windows.Forms.Button()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.lblOperator = New System.Windows.Forms.Label()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.cmbOperator = New System.Windows.Forms.ComboBox()
        Me.cmbField = New System.Windows.Forms.ComboBox()
        Me.lblField = New System.Windows.Forms.Label()
        Me.grpQueryClauses = New System.Windows.Forms.GroupBox()
        Me.btnClearClause = New System.Windows.Forms.Button()
        Me.btnClearAll = New System.Windows.Forms.Button()
        Me.lstClause = New System.Windows.Forms.ListView()
        Me.btnRunQuery = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grpAddClause.SuspendLayout()
        Me.grpQueryClauses.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAddClause
        '
        Me.grpAddClause.Controls.Add(Me.btnAddClause)
        Me.grpAddClause.Controls.Add(Me.lblValue)
        Me.grpAddClause.Controls.Add(Me.lblOperator)
        Me.grpAddClause.Controls.Add(Me.txtValue)
        Me.grpAddClause.Controls.Add(Me.cmbOperator)
        Me.grpAddClause.Controls.Add(Me.cmbField)
        Me.grpAddClause.Controls.Add(Me.lblField)
        Me.grpAddClause.Location = New System.Drawing.Point(13, 13)
        Me.grpAddClause.Name = "grpAddClause"
        Me.grpAddClause.Size = New System.Drawing.Size(846, 131)
        Me.grpAddClause.TabIndex = 0
        Me.grpAddClause.TabStop = False
        Me.grpAddClause.Text = "Add Clause"
        '
        'btnAddClause
        '
        Me.btnAddClause.Location = New System.Drawing.Point(699, 80)
        Me.btnAddClause.Name = "btnAddClause"
        Me.btnAddClause.Size = New System.Drawing.Size(140, 40)
        Me.btnAddClause.TabIndex = 6
        Me.btnAddClause.Text = "Add to Query"
        Me.btnAddClause.UseVisualStyleBackColor = True
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(499, 26)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(112, 14)
        Me.lblValue.TabIndex = 5
        Me.lblValue.Text = "3. Enter Value(s)"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOperator
        '
        Me.lblOperator.AutoSize = True
        Me.lblOperator.Location = New System.Drawing.Point(343, 26)
        Me.lblOperator.Name = "lblOperator"
        Me.lblOperator.Size = New System.Drawing.Size(105, 14)
        Me.lblOperator.TabIndex = 4
        Me.lblOperator.Text = "2. Set Operator"
        Me.lblOperator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(499, 48)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(340, 22)
        Me.txtValue.TabIndex = 3
        '
        'cmbOperator
        '
        Me.cmbOperator.FormattingEnabled = True
        Me.cmbOperator.Items.AddRange(New Object() {"equals", "not equals", "like", "starts with", "ends with", "less than", "greater than", "includes", "excludes", "regexp"})
        Me.cmbOperator.Location = New System.Drawing.Point(343, 48)
        Me.cmbOperator.Name = "cmbOperator"
        Me.cmbOperator.Size = New System.Drawing.Size(150, 22)
        Me.cmbOperator.TabIndex = 2
        '
        'cmbField
        '
        Me.cmbField.FormattingEnabled = True
        Me.cmbField.Location = New System.Drawing.Point(7, 48)
        Me.cmbField.Name = "cmbField"
        Me.cmbField.Size = New System.Drawing.Size(330, 22)
        Me.cmbField.TabIndex = 1
        '
        'lblField
        '
        Me.lblField.AutoSize = True
        Me.lblField.Location = New System.Drawing.Point(7, 26)
        Me.lblField.Name = "lblField"
        Me.lblField.Size = New System.Drawing.Size(106, 14)
        Me.lblField.TabIndex = 0
        Me.lblField.Text = "1. Select a Field"
        Me.lblField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpQueryClauses
        '
        Me.grpQueryClauses.Controls.Add(Me.btnClearClause)
        Me.grpQueryClauses.Controls.Add(Me.btnClearAll)
        Me.grpQueryClauses.Controls.Add(Me.lstClause)
        Me.grpQueryClauses.Location = New System.Drawing.Point(13, 151)
        Me.grpQueryClauses.Name = "grpQueryClauses"
        Me.grpQueryClauses.Size = New System.Drawing.Size(846, 240)
        Me.grpQueryClauses.TabIndex = 1
        Me.grpQueryClauses.TabStop = False
        Me.grpQueryClauses.Text = "Query Clauses"
        '
        'btnClearClause
        '
        Me.btnClearClause.Location = New System.Drawing.Point(473, 192)
        Me.btnClearClause.Name = "btnClearClause"
        Me.btnClearClause.Size = New System.Drawing.Size(200, 40)
        Me.btnClearClause.TabIndex = 2
        Me.btnClearClause.Text = "Clear Selected Clause"
        Me.btnClearClause.UseVisualStyleBackColor = True
        '
        'btnClearAll
        '
        Me.btnClearAll.Location = New System.Drawing.Point(679, 192)
        Me.btnClearAll.Name = "btnClearAll"
        Me.btnClearAll.Size = New System.Drawing.Size(160, 40)
        Me.btnClearAll.TabIndex = 1
        Me.btnClearAll.Text = "Clear All Clauses"
        Me.btnClearAll.UseVisualStyleBackColor = True
        '
        'lstClause
        '
        Me.lstClause.HideSelection = False
        Me.lstClause.Location = New System.Drawing.Point(7, 26)
        Me.lstClause.Name = "lstClause"
        Me.lstClause.Size = New System.Drawing.Size(832, 160)
        Me.lstClause.TabIndex = 0
        Me.lstClause.UseCompatibleStateImageBehavior = False
        '
        'btnRunQuery
        '
        Me.btnRunQuery.Location = New System.Drawing.Point(712, 397)
        Me.btnRunQuery.Name = "btnRunQuery"
        Me.btnRunQuery.Size = New System.Drawing.Size(140, 40)
        Me.btnRunQuery.TabIndex = 2
        Me.btnRunQuery.Text = "Run Query"
        Me.btnRunQuery.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(20, 397)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(140, 40)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmWizardStep4
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(872, 453)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRunQuery)
        Me.Controls.Add(Me.grpQueryClauses)
        Me.Controls.Add(Me.grpAddClause)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWizardStep4"
        Me.ShowIcon = False
        Me.Text = "Table Query Wizard - Step 4 of 4"
        Me.grpAddClause.ResumeLayout(False)
        Me.grpAddClause.PerformLayout()
        Me.grpQueryClauses.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpAddClause As System.Windows.Forms.GroupBox
    Friend WithEvents txtValue As System.Windows.Forms.TextBox
    Friend WithEvents cmbOperator As System.Windows.Forms.ComboBox
    Friend WithEvents cmbField As System.Windows.Forms.ComboBox
    Friend WithEvents lblField As System.Windows.Forms.Label
    Friend WithEvents btnAddClause As System.Windows.Forms.Button
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents lblOperator As System.Windows.Forms.Label
    Friend WithEvents grpQueryClauses As System.Windows.Forms.GroupBox
    Friend WithEvents btnClearClause As System.Windows.Forms.Button
    Friend WithEvents btnClearAll As System.Windows.Forms.Button
    Friend WithEvents btnRunQuery As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lstClause As System.Windows.Forms.ListView
End Class
