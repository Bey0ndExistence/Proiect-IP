namespace ChatApp
{
    partial class ChatControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxMessages;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;

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
            this.listBoxMessages = new System.Windows.Forms.ListBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.labelChatUsername = new System.Windows.Forms.Label();
            this.buttonExitChat = new System.Windows.Forms.Button();
            this.labelLoggedUsername = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxMessages
            // 
            this.listBoxMessages.FormattingEnabled = true;
            this.listBoxMessages.ItemHeight = 16;
            this.listBoxMessages.Location = new System.Drawing.Point(17, 106);
            this.listBoxMessages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxMessages.Name = "listBoxMessages";
            this.listBoxMessages.Size = new System.Drawing.Size(1032, 388);
            this.listBoxMessages.TabIndex = 0;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(17, 502);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(923, 22);
            this.textBoxMessage.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(951, 500);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(100, 28);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // labelChatUsername
            // 
            this.labelChatUsername.AutoSize = true;
            this.labelChatUsername.Location = new System.Drawing.Point(14, 47);
            this.labelChatUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelChatUsername.Name = "labelChatUsername";
            this.labelChatUsername.Size = new System.Drawing.Size(132, 17);
            this.labelChatUsername.TabIndex = 3;
            this.labelChatUsername.Text = "labelChatUsername";
            // 
            // buttonExitChat
            // 
            this.buttonExitChat.Location = new System.Drawing.Point(949, 36);
            this.buttonExitChat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonExitChat.Name = "buttonExitChat";
            this.buttonExitChat.Size = new System.Drawing.Size(100, 28);
            this.buttonExitChat.TabIndex = 4;
            this.buttonExitChat.Text = "Exit";
            this.buttonExitChat.UseVisualStyleBackColor = true;
            this.buttonExitChat.Click += new System.EventHandler(this.buttonExitChat_Click);
            // 
            // labelLoggedUsername
            // 
            this.labelLoggedUsername.AutoSize = true;
            this.labelLoggedUsername.Location = new System.Drawing.Point(14, 21);
            this.labelLoggedUsername.Name = "labelLoggedUsername";
            this.labelLoggedUsername.Size = new System.Drawing.Size(103, 17);
            this.labelLoggedUsername.TabIndex = 5;
            this.labelLoggedUsername.Text = "labelUsername";
            // 
            // ChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLoggedUsername);
            this.Controls.Add(this.buttonExitChat);
            this.Controls.Add(this.labelChatUsername);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.listBoxMessages);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ChatControl";
            this.Size = new System.Drawing.Size(1067, 554);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label labelChatUsername;
        private System.Windows.Forms.Button buttonExitChat;
        private System.Windows.Forms.Label labelLoggedUsername;
    }
}
