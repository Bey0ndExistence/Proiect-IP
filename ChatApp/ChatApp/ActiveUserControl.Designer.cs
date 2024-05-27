namespace ChatApp
{
    partial class ActiveUserControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxActiveUsers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxActiveUsers = new System.Windows.Forms.ListBox();
            this.buttonActiveUsersExit = new System.Windows.Forms.Button();
            this.buttonChatWithActiveUser = new System.Windows.Forms.Button();
            this.labelLoggedUsername = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxActiveUsers
            // 
            this.listBoxActiveUsers.DisplayMember = "test";
            this.listBoxActiveUsers.FormattingEnabled = true;
            this.listBoxActiveUsers.ItemHeight = 16;
            this.listBoxActiveUsers.Location = new System.Drawing.Point(72, 62);
            this.listBoxActiveUsers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxActiveUsers.Name = "listBoxActiveUsers";
            this.listBoxActiveUsers.Size = new System.Drawing.Size(923, 356);
            this.listBoxActiveUsers.TabIndex = 0;
            this.listBoxActiveUsers.ValueMember = "test";
            this.listBoxActiveUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxActiveUsers_SelectedIndexChanged);
            // 
            // buttonActiveUsersExit
            // 
            this.buttonActiveUsersExit.Location = new System.Drawing.Point(484, 466);
            this.buttonActiveUsersExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonActiveUsersExit.Name = "buttonActiveUsersExit";
            this.buttonActiveUsersExit.Size = new System.Drawing.Size(100, 28);
            this.buttonActiveUsersExit.TabIndex = 1;
            this.buttonActiveUsersExit.Text = "Exit";
            this.buttonActiveUsersExit.UseVisualStyleBackColor = true;
            this.buttonActiveUsersExit.Click += new System.EventHandler(this.buttonActiveUsersExit_Click);
            // 
            // buttonChatWithActiveUser
            // 
            this.buttonChatWithActiveUser.Location = new System.Drawing.Point(484, 431);
            this.buttonChatWithActiveUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChatWithActiveUser.Name = "buttonChatWithActiveUser";
            this.buttonChatWithActiveUser.Size = new System.Drawing.Size(100, 28);
            this.buttonChatWithActiveUser.TabIndex = 2;
            this.buttonChatWithActiveUser.Text = "Chat";
            this.buttonChatWithActiveUser.UseVisualStyleBackColor = true;
            this.buttonChatWithActiveUser.Click += new System.EventHandler(this.buttonChatWithActiveUser_Click);
            // 
            // labelLoggedUsername
            // 
            this.labelLoggedUsername.AutoSize = true;
            this.labelLoggedUsername.Location = new System.Drawing.Point(69, 24);
            this.labelLoggedUsername.Name = "labelLoggedUsername";
            this.labelLoggedUsername.Size = new System.Drawing.Size(103, 17);
            this.labelLoggedUsername.TabIndex = 3;
            this.labelLoggedUsername.Text = "labelUsername";
            // 
            // ActiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLoggedUsername);
            this.Controls.Add(this.buttonChatWithActiveUser);
            this.Controls.Add(this.buttonActiveUsersExit);
            this.Controls.Add(this.listBoxActiveUsers);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ActiveUserControl";
            this.Size = new System.Drawing.Size(1067, 554);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button buttonActiveUsersExit;
        private System.Windows.Forms.Button buttonChatWithActiveUser;
        private System.Windows.Forms.Label labelLoggedUsername;
    }
}
