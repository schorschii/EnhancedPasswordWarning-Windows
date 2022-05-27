using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace EnhancedPasswordWarning
{
    public class DarkMode
    {
        [DllImport("DwmApi")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static bool IsSystemDarkModeActive()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var registryValueObject = key?.GetValue("AppsUseLightTheme");
            if (registryValueObject == null)
            {
                return false;
            }
            var registryValue = (int)registryValueObject;
            return registryValue > 0 ? false : true;
        }

        public static void UseImmersiveDarkMode(IntPtr handle)
        {
            if(DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, new[] { 1 }, 4) != 0)
               DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, new[] { 1 }, 4);
        }
    }
}
