using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class RegDB
    {
        private static Microsoft.Win32.RegistryKey SO_KEY;

        private static void confirmSubkey()
        {
            SO_KEY = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\OpenSource", true);
            if (SO_KEY is null)
            {
                SO_KEY = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\OpenSource");
            }
        }

        public static void RegSetValue(string regkey, string regval)
        {
            confirmSubkey();
            SO_KEY.SetValue(regkey, regval);
            SO_KEY.Close();
        }

        public static string RegQueryValue(string regkey)
        {
            confirmSubkey();
            return Conversions.ToString(SO_KEY.GetValue(regkey, ""));
        }

        public static bool RegQueryBoolValue(string regkey)
        {
            confirmSubkey();
            string value = Conversions.ToString(SO_KEY.GetValue(regkey, "False"));
            return Conversions.ToBoolean(Interaction.IIf(value == "True", true, false));
        }
    }
}