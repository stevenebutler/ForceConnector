Option Explicit On

Module RegDB

    Dim SO_KEY As Microsoft.Win32.RegistryKey

    Private Sub confirmSubkey()
        SO_KEY = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\OpenSource", True)

        If SO_KEY Is Nothing Then
            SO_KEY = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\OpenSource")
        End If
    End Sub

    Public Sub RegSetValue(ByVal regkey As String, ByVal regval As String)
        confirmSubkey()
        SO_KEY.SetValue(regkey, regval)
        SO_KEY.Close()
    End Sub

    Public Function RegQueryValue(ByVal regkey As String) As String
        confirmSubkey()
        Return SO_KEY.GetValue(regkey, "")
    End Function

    Public Function RegQueryBoolValue(ByVal regkey As String) As Boolean
        confirmSubkey()
        Dim value As String = SO_KEY.GetValue(regkey, "False")
        Return IIf(value = "True", True, False)
    End Function

End Module
