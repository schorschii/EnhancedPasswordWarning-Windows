using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Windows.Forms;

namespace EnhancedPasswordWarning
{
    public partial class frmInfo : Form
    {
        EnhancedPasswordWarning epw;

        public frmInfo()
        {
            CommonInit();
            this.epw = new EnhancedPasswordWarning();
        }

        public frmInfo(EnhancedPasswordWarning epw)
        {
            CommonInit();
            this.epw = epw;
            this.lblExpiry.Text = epw.GetExpiryText(epw.GetExpiryDays(epw.pwdExpiryUnixSecs));
        }

        private void CommonInit()
        {
            InitializeComponent();

            // dark mode support
            if (DarkMode.IsSystemDarkModeActive())
            {
                DarkMode.UseImmersiveDarkMode(this.Handle);
                this.BackColor = Color.Black;
                this.lblOldPassword.ForeColor = Color.White;
                this.lblNewPassword.ForeColor = Color.White;
                this.lblConfirmNewPassword.ForeColor = Color.White;
                this.lblExpiry.ForeColor = Color.White;
            }
        }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            // translations
            this.Text = String.Format(Properties.strings.change_password_for, epw.userName);
            btnCancel.Text = Properties.strings.cancel;
            btnConfirm.Text = Properties.strings.ok;
            lblOldPassword.Text = Properties.strings.old_password;
            lblNewPassword.Text = Properties.strings.new_password;
            lblConfirmNewPassword.Text = Properties.strings.confirm_new_password;

            // position bottom right
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);
        }

        private void btnNewPassword_Click(object sender, EventArgs e)
        {
            if(txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                MessageBox.Show(this, Properties.strings.passwords_do_not_match, Properties.strings.error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ChangePassword(epw.domainName, epw.userName, txtOldPassword.Text, txtNewPassword.Text);
                MessageBox.Show(this, Properties.strings.password_changed_successfully, Properties.strings.success, MessageBoxButtons.OK, MessageBoxIcon.Information);

                epw.RefreshExpiry();

                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, Properties.strings.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ChangePassword(string domain, string userName, string oldPassword, string newPassword)
        {
            using(var context = new PrincipalContext(ContextType.Domain, domain))
            using(var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
            {
                user.ChangePassword(oldPassword, newPassword);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmInfo_Shown(object sender, EventArgs e)
        {
            // initial focus
            this.txtOldPassword.Focus();
        }

        private void frmInfo_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            frmAbout aboutWindow = new frmAbout();
            aboutWindow.ShowDialog();
            e.Cancel = true;
        }
    }
}