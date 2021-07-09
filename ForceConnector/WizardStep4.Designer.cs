using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    [DesignerGenerated()]
    public partial class frmWizardStep4 : System.Windows.Forms.Form
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
            grpAddClause = new System.Windows.Forms.GroupBox();
            _btnAddClause = new System.Windows.Forms.Button();
            _btnAddClause.Click += new EventHandler(btnAddClause_Click);
            lblValue = new System.Windows.Forms.Label();
            lblOperator = new System.Windows.Forms.Label();
            txtValue = new System.Windows.Forms.TextBox();
            cmbOperator = new System.Windows.Forms.ComboBox();
            cmbField = new System.Windows.Forms.ComboBox();
            lblField = new System.Windows.Forms.Label();
            grpQueryClauses = new System.Windows.Forms.GroupBox();
            _btnClearClause = new System.Windows.Forms.Button();
            _btnClearClause.Click += new EventHandler(btnClearClause_Click);
            _btnClearAll = new System.Windows.Forms.Button();
            _btnClearAll.Click += new EventHandler(btnClearAll_Click);
            lstClause = new System.Windows.Forms.ListView();
            _btnRunQuery = new System.Windows.Forms.Button();
            _btnRunQuery.Click += new EventHandler(btnRunQuery_Click);
            _btnClose = new System.Windows.Forms.Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            grpAddClause.SuspendLayout();
            grpQueryClauses.SuspendLayout();
            SuspendLayout();
            // 
            // grpAddClause
            // 
            grpAddClause.Controls.Add(_btnAddClause);
            grpAddClause.Controls.Add(lblValue);
            grpAddClause.Controls.Add(lblOperator);
            grpAddClause.Controls.Add(txtValue);
            grpAddClause.Controls.Add(cmbOperator);
            grpAddClause.Controls.Add(cmbField);
            grpAddClause.Controls.Add(lblField);
            grpAddClause.Location = new System.Drawing.Point(13, 13);
            grpAddClause.Name = "grpAddClause";
            grpAddClause.Size = new System.Drawing.Size(846, 131);
            grpAddClause.TabIndex = 0;
            grpAddClause.TabStop = false;
            grpAddClause.Text = "Add Clause";
            // 
            // btnAddClause
            // 
            _btnAddClause.Location = new System.Drawing.Point(699, 80);
            _btnAddClause.Name = "_btnAddClause";
            _btnAddClause.Size = new System.Drawing.Size(140, 40);
            _btnAddClause.TabIndex = 6;
            _btnAddClause.Text = "Add to Query";
            _btnAddClause.UseVisualStyleBackColor = true;
            // 
            // lblValue
            // 
            lblValue.AutoSize = true;
            lblValue.Location = new System.Drawing.Point(499, 26);
            lblValue.Name = "lblValue";
            lblValue.Size = new System.Drawing.Size(112, 14);
            lblValue.TabIndex = 5;
            lblValue.Text = "3. Enter Value(s)";
            lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOperator
            // 
            lblOperator.AutoSize = true;
            lblOperator.Location = new System.Drawing.Point(343, 26);
            lblOperator.Name = "lblOperator";
            lblOperator.Size = new System.Drawing.Size(105, 14);
            lblOperator.TabIndex = 4;
            lblOperator.Text = "2. Set Operator";
            lblOperator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtValue
            // 
            txtValue.Location = new System.Drawing.Point(499, 48);
            txtValue.Name = "txtValue";
            txtValue.Size = new System.Drawing.Size(340, 22);
            txtValue.TabIndex = 3;
            // 
            // cmbOperator
            // 
            cmbOperator.FormattingEnabled = true;
            cmbOperator.Items.AddRange(new object[] { "equals", "not equals", "like", "starts with", "ends with", "less than", "greater than", "includes", "excludes", "regexp" });
            cmbOperator.Location = new System.Drawing.Point(343, 48);
            cmbOperator.Name = "cmbOperator";
            cmbOperator.Size = new System.Drawing.Size(150, 22);
            cmbOperator.TabIndex = 2;
            // 
            // cmbField
            // 
            cmbField.FormattingEnabled = true;
            cmbField.Location = new System.Drawing.Point(7, 48);
            cmbField.Name = "cmbField";
            cmbField.Size = new System.Drawing.Size(330, 22);
            cmbField.TabIndex = 1;
            // 
            // lblField
            // 
            lblField.AutoSize = true;
            lblField.Location = new System.Drawing.Point(7, 26);
            lblField.Name = "lblField";
            lblField.Size = new System.Drawing.Size(106, 14);
            lblField.TabIndex = 0;
            lblField.Text = "1. Select a Field";
            lblField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpQueryClauses
            // 
            grpQueryClauses.Controls.Add(_btnClearClause);
            grpQueryClauses.Controls.Add(_btnClearAll);
            grpQueryClauses.Controls.Add(lstClause);
            grpQueryClauses.Location = new System.Drawing.Point(13, 151);
            grpQueryClauses.Name = "grpQueryClauses";
            grpQueryClauses.Size = new System.Drawing.Size(846, 240);
            grpQueryClauses.TabIndex = 1;
            grpQueryClauses.TabStop = false;
            grpQueryClauses.Text = "Query Clauses";
            // 
            // btnClearClause
            // 
            _btnClearClause.Location = new System.Drawing.Point(473, 192);
            _btnClearClause.Name = "_btnClearClause";
            _btnClearClause.Size = new System.Drawing.Size(200, 40);
            _btnClearClause.TabIndex = 2;
            _btnClearClause.Text = "Clear Selected Clause";
            _btnClearClause.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            _btnClearAll.Location = new System.Drawing.Point(679, 192);
            _btnClearAll.Name = "_btnClearAll";
            _btnClearAll.Size = new System.Drawing.Size(160, 40);
            _btnClearAll.TabIndex = 1;
            _btnClearAll.Text = "Clear All Clauses";
            _btnClearAll.UseVisualStyleBackColor = true;
            // 
            // lstClause
            // 
            lstClause.HideSelection = false;
            lstClause.Location = new System.Drawing.Point(7, 26);
            lstClause.Name = "lstClause";
            lstClause.Size = new System.Drawing.Size(832, 160);
            lstClause.TabIndex = 0;
            lstClause.UseCompatibleStateImageBehavior = false;
            // 
            // btnRunQuery
            // 
            _btnRunQuery.Location = new System.Drawing.Point(712, 397);
            _btnRunQuery.Name = "_btnRunQuery";
            _btnRunQuery.Size = new System.Drawing.Size(140, 40);
            _btnRunQuery.TabIndex = 2;
            _btnRunQuery.Text = "Run Query";
            _btnRunQuery.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            _btnClose.Location = new System.Drawing.Point(20, 397);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new System.Drawing.Size(140, 40);
            _btnClose.TabIndex = 3;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // frmWizardStep4
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(872, 453);
            ControlBox = false;
            Controls.Add(_btnClose);
            Controls.Add(_btnRunQuery);
            Controls.Add(grpQueryClauses);
            Controls.Add(grpAddClause);
            Font = new System.Drawing.Font("Verdana", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Conversions.ToByte(0));
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmWizardStep4";
            ShowIcon = false;
            Text = "Table Query Wizard - Step 4 of 4";
            grpAddClause.ResumeLayout(false);
            grpAddClause.PerformLayout();
            grpQueryClauses.ResumeLayout(false);
            ResumeLayout(false);
        }

        internal System.Windows.Forms.GroupBox grpAddClause;
        internal System.Windows.Forms.TextBox txtValue;
        internal System.Windows.Forms.ComboBox cmbOperator;
        internal System.Windows.Forms.ComboBox cmbField;
        internal System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Button _btnAddClause;

        internal System.Windows.Forms.Button btnAddClause
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAddClause;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAddClause != null)
                {
                    _btnAddClause.Click -= btnAddClause_Click;
                }

                _btnAddClause = value;
                if (_btnAddClause != null)
                {
                    _btnAddClause.Click += btnAddClause_Click;
                }
            }
        }

        internal System.Windows.Forms.Label lblValue;
        internal System.Windows.Forms.Label lblOperator;
        internal System.Windows.Forms.GroupBox grpQueryClauses;
        private System.Windows.Forms.Button _btnClearClause;

        internal System.Windows.Forms.Button btnClearClause
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClearClause;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClearClause != null)
                {
                    _btnClearClause.Click -= btnClearClause_Click;
                }

                _btnClearClause = value;
                if (_btnClearClause != null)
                {
                    _btnClearClause.Click += btnClearClause_Click;
                }
            }
        }

        private System.Windows.Forms.Button _btnClearAll;

        internal System.Windows.Forms.Button btnClearAll
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClearAll;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClearAll != null)
                {
                    _btnClearAll.Click -= btnClearAll_Click;
                }

                _btnClearAll = value;
                if (_btnClearAll != null)
                {
                    _btnClearAll.Click += btnClearAll_Click;
                }
            }
        }

        private System.Windows.Forms.Button _btnRunQuery;

        internal System.Windows.Forms.Button btnRunQuery
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRunQuery;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRunQuery != null)
                {
                    _btnRunQuery.Click -= btnRunQuery_Click;
                }

                _btnRunQuery = value;
                if (_btnRunQuery != null)
                {
                    _btnRunQuery.Click += btnRunQuery_Click;
                }
            }
        }

        private System.Windows.Forms.Button _btnClose;

        internal System.Windows.Forms.Button btnClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClose != null)
                {
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
                }
            }
        }

        internal System.Windows.Forms.ListView lstClause;
    }
}