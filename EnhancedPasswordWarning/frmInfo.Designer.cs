using System.Windows.Forms;

namespace EnhancedPasswordWarning
{
    partial class frmInfo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfo));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmNewPassword = new System.Windows.Forms.Label();
            this.txtConfirmNewPassword = new System.Windows.Forms.TextBox();
            this.lblExpiry = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnNewPassword_Click);
            // 
            // txtOldPassword
            // 
            resources.ApplyResources(this.txtOldPassword, "txtOldPassword");
            this.txtOldPassword.Name = "txtOldPassword";
            // 
            // lblOldPassword
            // 
            resources.ApplyResources(this.lblOldPassword, "lblOldPassword");
            this.lblOldPassword.Name = "lblOldPassword";
            // 
            // lblNewPassword
            // 
            resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
            this.lblNewPassword.Name = "lblNewPassword";
            // 
            // txtNewPassword
            // 
            resources.ApplyResources(this.txtNewPassword, "txtNewPassword");
            this.txtNewPassword.Name = "txtNewPassword";
            // 
            // lblConfirmNewPassword
            // 
            resources.ApplyResources(this.lblConfirmNewPassword, "lblConfirmNewPassword");
            this.lblConfirmNewPassword.Name = "lblConfirmNewPassword";
            // 
            // txtConfirmNewPassword
            // 
            resources.ApplyResources(this.txtConfirmNewPassword, "txtConfirmNewPassword");
            this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
            // 
            // lblExpiry
            // 
            resources.ApplyResources(this.lblExpiry, "lblExpiry");
            this.lblExpiry.Name = "lblExpiry";
            // 
            // frmInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblExpiry);
            this.Controls.Add(this.lblConfirmNewPassword);
            this.Controls.Add(this.txtConfirmNewPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblOldPassword);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInfo";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmInfo_HelpButtonClicked);
            this.Load += new System.EventHandler(this.frmInfo_Load);
            this.Shown += new System.EventHandler(this.frmInfo_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnCancel;
        private Button btnConfirm;
        private TextBox txtOldPassword;
        private Label lblOldPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmNewPassword;
        private TextBox txtConfirmNewPassword;
        private Label lblExpiry;
    }
}