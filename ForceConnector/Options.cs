using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    public partial class frmOption
    {
        public frmOption()
        {
            InitializeComponent();
            _btnOK.Name = "btnOK";
        }

        private void Options_Load(object sender, EventArgs e)
        {
            chkUseReference.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.USE_REFERENCE), RegDB.RegQueryBoolValue(ForceConnector.USE_REFERENCE), false));
            chkNoWarning.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.GOAHEAD), RegDB.RegQueryBoolValue(ForceConnector.GOAHEAD), false));
            chkNoLimit.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.NOLIMITS), RegDB.RegQueryBoolValue(ForceConnector.NOLIMITS), false));
            chkDisableAssignRule.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE), RegDB.RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE), true));
            chkSkipHidden.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.SKIPHIDDEN), RegDB.RegQueryBoolValue(ForceConnector.SKIPHIDDEN), false));
            chkDisableManaged.Checked = Conversions.ToBoolean(Interaction.IIf(RegDB.RegQueryBoolValue(ForceConnector.GET_MANAGED), RegDB.RegQueryBoolValue(ForceConnector.GET_MANAGED), true));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RegDB.RegSetValue(ForceConnector.USE_REFERENCE, Conversions.ToString(chkUseReference.Checked));
            RegDB.RegSetValue(ForceConnector.GOAHEAD, Conversions.ToString(chkNoWarning.Checked));
            RegDB.RegSetValue(ForceConnector.NOLIMITS, Conversions.ToString(chkNoLimit.Checked));
            RegDB.RegSetValue(ForceConnector.AUTOASSIGNRULE, Conversions.ToString(chkDisableAssignRule.Checked));
            RegDB.RegSetValue(ForceConnector.SKIPHIDDEN, Conversions.ToString(chkSkipHidden.Checked));
            RegDB.RegSetValue(ForceConnector.GET_MANAGED, Conversions.ToString(chkDisableManaged.Checked));
            Close();
        }
    }
}