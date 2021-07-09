using System.Windows.Forms;

namespace ForceConnector
{
    public class TopMostMessageBox
    {
        public static DialogResult Show(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icons)
        {
            // Create a host form that is a TopMost window which will be the
            // parent of the MessageBox.
            var topmostForm = new Form();
            // new form should not be visible so position it off the visible screen and make it as small as possible
            topmostForm.Size = new System.Drawing.Size(1, 1);
            topmostForm.StartPosition = FormStartPosition.Manual;
            var rect = SystemInformation.VirtualScreen;
            topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10, rect.Right + 10);
            topmostForm.Show();
            // Make this form the active form and make it TopMost
            topmostForm.Focus();
            topmostForm.BringToFront();
            topmostForm.TopMost = true;
            // Finally show the MessageBox with the form just created as its owner
            var result = MessageBox.Show(topmostForm, message, title, buttons, icons);
            // clean it up all the way
            topmostForm.Dispose();
            return result;
        }
    }
}