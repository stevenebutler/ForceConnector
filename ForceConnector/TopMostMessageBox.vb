Imports System.Windows.Forms

Public Class TopMostMessageBox
    Public Shared Function Show(ByVal title As String, ByVal message As String,
                                ByVal buttons As MessageBoxButtons, ByVal icons As MessageBoxIcon) As DialogResult
        ' Create a host form that is a TopMost window which will be the
        ' parent of the MessageBox.
        Dim topmostForm As Form = New Form()
        ' new form should not be visible so position it off the visible screen and make it as small as possible
        topmostForm.Size = New System.Drawing.Size(1, 1)
        topmostForm.StartPosition = FormStartPosition.Manual
        Dim rect As System.Drawing.Rectangle = SystemInformation.VirtualScreen
        topmostForm.Location = New System.Drawing.Point(rect.Bottom + 10, rect.Right + 10)
        topmostForm.Show()
        ' Make this form the active form and make it TopMost
        topmostForm.Focus()
        topmostForm.BringToFront()
        topmostForm.TopMost = True
        ' Finally show the MessageBox with the form just created as its owner
        Dim result As DialogResult = MessageBox.Show(topmostForm, message, title, buttons, icons)
        'clean it up all the way
        topmostForm.Dispose()
        Return result
    End Function
End Class
