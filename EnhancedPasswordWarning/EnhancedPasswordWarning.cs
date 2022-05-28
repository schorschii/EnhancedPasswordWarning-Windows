using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.Timers;
using System.Windows.Forms;

namespace EnhancedPasswordWarning
{
    public class EnhancedPasswordWarning
    {
        protected internal string userName = Environment.UserName;
        protected internal string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

        protected internal int pwdExpiryUnixSecs = 0;
        protected internal int warnDays = 0;

        NotifyIcon niInfo = new NotifyIcon();
        ToolStripItem miInfo;
        frmInfo frmInfo;

        public EnhancedPasswordWarning()
        {
            // load config
            try
            {
                // Creates or loads an INI file in the same directory as your executable
                // named EXE.ini (where EXE is the name of the executable)
                var iniFile = new IniFile();
                string strWarnDays = iniFile.Read("warnDays");
                if(strWarnDays != null) this.warnDays = int.Parse(strWarnDays);

                Debug.WriteLine("Loaded .ini: warnDays:" + this.warnDays);
            } catch(Exception ex)
            {
                // invalid configuration - use default
                Debug.WriteLine("Unable to load .ini: " + ex.Message);
            }

            // init taskbar icon
            this.niInfo.Icon = Properties.Resources.key_blue;
            this.niInfo.Visible = true;
            this.niInfo.DoubleClick += new EventHandler(OpenWindow);
            this.niInfo.BalloonTipClicked += new EventHandler(OpenWindow);

            // init taskbar icon context menu
            ContextMenuStrip cms = new ContextMenuStrip();
            this.miInfo = new ToolStripMenuItem(Properties.strings.password_warning);
            this.miInfo.Enabled = false;
            cms.Items.Add(this.miInfo);

            cms.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem miRefresh = new ToolStripMenuItem(Properties.strings.refresh_expiry);
            miRefresh.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                RefreshExpiry(true);
            });
            cms.Items.Add(miRefresh);

            ToolStripMenuItem miSetNewPassword = new ToolStripMenuItem(Properties.strings.create_new_password);
            miSetNewPassword.Click += new EventHandler(OpenWindow);
            cms.Items.Add(miSetNewPassword);

            cms.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem miExit = new ToolStripMenuItem(Properties.strings.exit);
            miExit.Click += new EventHandler(delegate (Object o, EventArgs a)
            {
                Application.Exit();
            });
            cms.Items.Add(miExit);

            this.niInfo.ContextMenuStrip = cms;

            // initial refresh
            RefreshExpiry();

            // timer for periodically refresh every 24hrs
            // (for computers which are not shutted down, suspended only)
            System.Timers.Timer tmrRefresh = new System.Timers.Timer();
            tmrRefresh.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tmrRefresh.Interval = 24 * 60 * 60 * 1000;
            tmrRefresh.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            RefreshExpiry();
        }

        private void OpenWindow(object sender, EventArgs e)
        {
            if(frmInfo != null)
            {
                //frmInfo.Close();
                frmInfo.Dispose();
            }
            this.frmInfo = new frmInfo(this);
            this.frmInfo.Show();
        }

        public double GetExpiryDays(int pwdExpiryUnixSecs)
        {
            DateTime expiryDate = UnixTimeStampToDateTime(pwdExpiryUnixSecs);
            TimeSpan expiry = expiryDate - DateTime.UtcNow;
            return expiry.TotalDays;
        }

        public void RefreshExpiry(bool showErrors = false)
        {
            // cached value from settings
            this.pwdExpiryUnixSecs = Properties.Settings.Default.pwdExpiryUnixSecs;
            Debug.WriteLine("Loaded from settings: " + this.pwdExpiryUnixSecs);

            // try to get current value from LDAP
            try
            {
                this.pwdExpiryUnixSecs = GetPasswordExpiryLdap(this.userName);
                Debug.WriteLine("Loaded from LDAP: " + this.pwdExpiryUnixSecs);
                if (showErrors)
                {
                    MessageBox.Show(null, GetExpiryText(GetExpiryDays(this.pwdExpiryUnixSecs)), Properties.strings.success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error query LDAP: " + ex.Message);
                if(showErrors)
                {
                    MessageBox.Show(null, ex.Message, Properties.strings.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // display expiry
            double expiryDays = GetExpiryDays(this.pwdExpiryUnixSecs);
            RefreshDisplay(expiryDays);

            // show warning
            if(expiryDays > 0 && expiryDays <= this.warnDays)
            {
                String expiryText = GetExpiryText(expiryDays);
                this.niInfo.ShowBalloonTip(
                    5000,
                    Properties.strings.password_warning,
                    expiryText + " " + Properties.strings.click_here_to_change,
                    ToolTipIcon.Warning
                );
            }
        }

        private void RefreshDisplay(double expiryDays)
        {
            String expiryText = GetExpiryText(expiryDays);

            if(expiryDays > 0)
            {
                if(expiryDays > this.warnDays)
                {
                    niInfo.Icon = Properties.Resources.key_green;
                }
                else if(expiryDays > Math.Round((double)(this.warnDays / 2)))
                {
                    niInfo.Icon = Properties.Resources.key_yellow;
                }
                else
                {
                    niInfo.Icon = Properties.Resources.key_red;
                }
            }
            else
            {
                niInfo.Icon = Properties.Resources.key_blue;
            }

            niInfo.Text = expiryText;
            if(miInfo != null)
            {
                miInfo.Text = expiryText;
            }
            if(frmInfo != null)
            {
                frmInfo.Text = this.userName;
            }
        }

        public string GetExpiryText(double expiryDays)
        {
            String expiryText = Properties.strings.password_does_not_expire;
            if(expiryDays > 0)
            {
                expiryText = String.Format(Properties.strings.password_expires_in_days, Math.Round(expiryDays));
            }
            return expiryText;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        /* returns the password expiry date (seconds, unix timestamp) */
        private int GetPasswordExpiryLdap(string userName)
        {
            DirectoryEntry de = new DirectoryEntry();

            // query user
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectClass=User)(samaccountname=" + userName + "))";
            ds.PropertiesToLoad.Add("pwdLastSet");
            SearchResult rs = ds.FindOne();
            if(rs == null)
            {
                throw new Exception("User not found");
            }
            if(rs.GetDirectoryEntry().Properties["pwdLastSet"].Value == null
                || rs.GetDirectoryEntry().Properties["pwdLastSet"].Count != 1)
            {
                throw new Exception("Query error: pwdLastSet is not set");
            }

            int pwdLastSetUnixSecs = (int)AdsiUtils.AdsDateValue(
                rs.GetDirectoryEntry().Properties["pwdLastSet"][0] ?? 0
            ).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            // query password policy
            ds = new DirectorySearcher(de);
            ds.Filter = "(objectClass=*)";
            ds.PropertiesToLoad.Add("maxPwdAge");
            rs = ds.FindOne();
            if(rs == null)
            {
                throw new Exception("Base not found");
            }
            if(rs.GetDirectoryEntry().Properties["maxPwdAge"].Value == null
                || rs.GetDirectoryEntry().Properties["maxPwdAge"].Count != 1)
            {
                throw new Exception("Query error: maxPwdAge is not set");
            }

            int maxPwdAgeSecs = (int)TimeSpan.FromTicks(
                Math.Abs(
                    AdsiUtils.AdsTicksValue(
                        rs.GetDirectoryEntry().Properties["maxPwdAge"][0] ?? 0
                    )
                )
            ).TotalSeconds;

            int pwdExpiryUnixSecs = pwdLastSetUnixSecs + maxPwdAgeSecs;

            Properties.Settings.Default.pwdExpiryUnixSecs = pwdExpiryUnixSecs;
            Properties.Settings.Default.Save();

            return pwdExpiryUnixSecs;
        }
    }
}
