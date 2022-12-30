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
            this.grpAddClause = new System.Windows.Forms.GroupBox();
            this._btnAddClause = new System.Windows.Forms.Button();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblOperator = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.lblField = new System.Windows.Forms.Label();
            this.grpQueryClauses = new System.Windows.Forms.GroupBox();
            this._btnClearClause = new System.Windows.Forms.Button();
            this._btnClearAll = new System.Windows.Forms.Button();
            this.lstClause = new System.Windows.Forms.ListView();
            this._btnRunQuery = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this.grpAddClause.SuspendLayout();
            this.grpQueryClauses.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAddClause
            // 
            this.grpAddClause.Controls.Add(this._btnAddClause);
            this.grpAddClause.Controls.Add(this.lblValue);
            this.grpAddClause.Controls.Add(this.lblOperator);
            this.grpAddClause.Controls.Add(this.txtValue);
            this.grpAddClause.Controls.Add(this.cmbOperator);
            this.grpAddClause.Controls.Add(this.cmbField);
            this.grpAddClause.Controls.Add(this.lblField);
            this.grpAddClause.Location = new System.Drawing.Point(13, 13);
            this.grpAddClause.Name = "grpAddClause";
            this.grpAddClause.Size = new System.Drawing.Size(869, 156);
            this.grpAddClause.TabIndex = 0;
            this.grpAddClause.TabStop = false;
            this.grpAddClause.Text = "Add Clause";
            // 
            // _btnAddClause
            // 
            this._btnAddClause.Location = new System.Drawing.Point(679, 110);
            this._btnAddClause.Name = "_btnAddClause";
            this._btnAddClause.Size = new System.Drawing.Size(184, 40);
            this._btnAddClause.TabIndex = 6;
            this._btnAddClause.Text = "Add to Query";
            this._btnAddClause.UseVisualStyleBackColor = true;
            this._btnAddClause.Click += new System.EventHandler(this.btnAddClause_Click);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(517, 36);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(162, 22);
            this.lblValue.TabIndex = 5;
            this.lblValue.Text = "3. Enter Value(s)";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOperator
            // 
            this.lblOperator.AutoSize = true;
            this.lblOperator.Location = new System.Drawing.Point(339, 36);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(151, 22);
            this.lblOperator.TabIndex = 4;
            this.lblOperator.Text = "2. Set Operator";
            this.lblOperator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(521, 75);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(342, 29);
            this.txtValue.TabIndex = 3;
            // 
            // cmbOperator
            // 
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Items.AddRange(new object[] {
            "equals",
            "not equals",
            "like",
            "starts with",
            "ends with",
            "less than",
            "greater than",
            "includes",
            "excludes",
            "regexp"});
            this.cmbOperator.Location = new System.Drawing.Point(343, 74);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(172, 30);
            this.cmbOperator.TabIndex = 2;
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(10, 74);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(327, 30);
            this.cmbField.TabIndex = 1;
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.Location = new System.Drawing.Point(6, 36);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(154, 22);
            this.lblField.TabIndex = 0;
            this.lblField.Text = "1. Select a Field";
            this.lblField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpQueryClauses
            // 
            this.grpQueryClauses.Controls.Add(this._btnClearClause);
            this.grpQueryClauses.Controls.Add(this._btnClearAll);
            this.grpQueryClauses.Controls.Add(this.lstClause);
            this.grpQueryClauses.Location = new System.Drawing.Point(13, 191);
            this.grpQueryClauses.Name = "grpQueryClauses";
            this.grpQueryClauses.Size = new System.Drawing.Size(869, 257);
            this.grpQueryClauses.TabIndex = 1;
            this.grpQueryClauses.TabStop = false;
            this.grpQueryClauses.Text = "Query Clauses";
            // 
            // _btnClearClause
            // 
            this._btnClearClause.Location = new System.Drawing.Point(434, 192);
            this._btnClearClause.Name = "_btnClearClause";
            this._btnClearClause.Size = new System.Drawing.Size(230, 50);
            this._btnClearClause.TabIndex = 2;
            this._btnClearClause.Text = "Clear Selected Clause";
            this._btnClearClause.UseVisualStyleBackColor = true;
            this._btnClearClause.Click += new System.EventHandler(this.btnClearClause_Click);
            // 
            // _btnClearAll
            // 
            this._btnClearAll.Location = new System.Drawing.Point(670, 192);
            this._btnClearAll.Name = "_btnClearAll";
            this._btnClearAll.Size = new System.Drawing.Size(193, 50);
            this._btnClearAll.TabIndex = 1;
            this._btnClearAll.Text = "Clear All Clauses";
            this._btnClearAll.UseVisualStyleBackColor = true;
            this._btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // lstClause
            // 
            this.lstClause.HideSelection = false;
            this.lstClause.Location = new System.Drawing.Point(7, 26);
            this.lstClause.Name = "lstClause";
            this.lstClause.Size = new System.Drawing.Size(856, 160);
            this.lstClause.TabIndex = 0;
            this.lstClause.UseCompatibleStateImageBehavior = false;
            // 
            // _btnRunQuery
            // 
            this._btnRunQuery.Location = new System.Drawing.Point(742, 466);
            this._btnRunQuery.Name = "_btnRunQuery";
            this._btnRunQuery.Size = new System.Drawing.Size(140, 40);
            this._btnRunQuery.TabIndex = 2;
            this._btnRunQuery.Text = "Run Query";
            this._btnRunQuery.UseVisualStyleBackColor = true;
            this._btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(596, 466);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(140, 40);
            this._btnClose.TabIndex = 3;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmWizardStep4
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(894, 519);
            this.ControlBox = false;
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnRunQuery);
            this.Controls.Add(this.grpQueryClauses);
            this.Controls.Add(this.grpAddClause);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWizardStep4";
            this.ShowIcon = false;
            this.Text = "Table Query Wizard - Step 4 of 4";
            this.grpAddClause.ResumeLayout(false);
            this.grpAddClause.PerformLayout();
            this.grpQueryClauses.ResumeLayout(false);
            this.ResumeLayout(false);

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