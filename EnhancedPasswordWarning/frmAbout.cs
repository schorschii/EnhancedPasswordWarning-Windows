using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EnhancedPasswordWarning
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            // dark mode support
            if(DarkMode.IsSystemDarkModeActive())
            {
                DarkMode.UseImmersiveDarkMode(this.Handle);
                this.BackColor = Color.Black;
                this.lblAppName.ForeColor = Color.White;
                this.lblCopyright.ForeColor = Color.White;
                this.lblVersion.ForeColor = Color.White;
            }
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void llAppRepo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(llAppRepo.Text) { UseShellExecute = true });
        }
    }
}
