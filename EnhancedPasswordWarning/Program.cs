using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace EnhancedPasswordWarning
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // testing other languages
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EnhancedPasswordWarning epw = new EnhancedPasswordWarning();

            Application.Run();
        }

    }
}