Public Class frmOption
    Private Sub Options_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.chkUseReference.Checked = IIf(RegQueryBoolValue(ForceConnector.USE_REFERENCE), RegQueryBoolValue(ForceConnector.USE_REFERENCE), False)
        Me.chkNoWarning.Checked = IIf(RegQueryBoolValue(ForceConnector.GOAHEAD), RegQueryBoolValue(ForceConnector.GOAHEAD), False)
        Me.chkNoLimit.Checked = IIf(RegQueryBoolValue(ForceConnector.NOLIMITS), RegQueryBoolValue(ForceConnector.NOLIMITS), False)
        Me.chkDisableAssignRule.Checked = IIf(RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE), RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE), True)
        Me.chkSkipHidden.Checked = IIf(RegQueryBoolValue(ForceConnector.SKIPHIDDEN), RegQueryBoolValue(ForceConnector.SKIPHIDDEN), False)
        Me.chkDisableManaged.Checked = IIf(RegQueryBoolValue(ForceConnector.GET_MANAGED), RegQueryBoolValue(ForceConnector.GET_MANAGED), True)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        RegSetValue(ForceConnector.USE_REFERENCE, Me.chkUseReference.Checked)
        RegSetValue(ForceConnector.GOAHEAD, Me.chkNoWarning.Checked)
        RegSetValue(ForceConnector.NOLIMITS, Me.chkNoLimit.Checked)
        RegSetValue(ForceConnector.AUTOASSIGNRULE, Me.chkDisableAssignRule.Checked)
        RegSetValue(ForceConnector.SKIPHIDDEN, Me.chkSkipHidden.Checked)
        RegSetValue(ForceConnector.GET_MANAGED, Me.chkDisableManaged.Checked)

        Me.Close()
    End Sub

End Class