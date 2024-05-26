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
            this.SuspendLayout();
            // 
            // listBoxActiveUsers
            // 
            this.listBoxActiveUsers.DisplayMember = "test";
            this.listBoxActiveUsers.FormattingEnabled = true;
            this.listBoxActiveUsers.Location = new System.Drawing.Point(54, 50);
            this.listBoxActiveUsers.Name = "listBoxActiveUsers";
            this.listBoxActiveUsers.Size = new System.Drawing.Size(693, 290);
            this.listBoxActiveUsers.TabIndex = 0;
            this.listBoxActiveUsers.ValueMember = "test";
            this.listBoxActiveUsers.SelectedIndexChanged += new System.EventHandler(this.listBoxActiveUsers_SelectedIndexChanged);
            // 
            // buttonActiveUsersExit
            // 
            this.buttonActiveUsersExit.Location = new System.Drawing.Point(363, 379);
            this.buttonActiveUsersExit.Name = "buttonActiveUsersExit";
            this.buttonActiveUsersExit.Size = new System.Drawing.Size(75, 23);
            this.buttonActiveUsersExit.TabIndex = 1;
            this.buttonActiveUsersExit.Text = "Exit";
            this.buttonActiveUsersExit.UseVisualStyleBackColor = true;
            this.buttonActiveUsersExit.Click += new System.EventHandler(this.buttonActiveUsersExit_Click);
            // 
            // buttonChatWithActiveUser
            // 
            this.buttonChatWithActiveUser.Location = new System.Drawing.Point(363, 350);
            this.buttonChatWithActiveUser.Name = "buttonChatWithActiveUser";
            this.buttonChatWithActiveUser.Size = new System.Drawing.Size(75, 23);
            this.buttonChatWithActiveUser.TabIndex = 2;
            this.buttonChatWithActiveUser.Text = "Chat";
            this.buttonChatWithActiveUser.UseVisualStyleBackColor = true;
            this.buttonChatWithActiveUser.Click += new System.EventHandler(this.buttonChatWithActiveUser_Click);
            // 
            // ActiveUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonChatWithActiveUser);
            this.Controls.Add(this.buttonActiveUsersExit);
            this.Controls.Add(this.listBoxActiveUsers);
            this.Name = "ActiveUserControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button buttonActiveUsersExit;
        private System.Windows.Forms.Button buttonChatWithActiveUser;
    }
}
