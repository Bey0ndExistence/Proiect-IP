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
            this.SuspendLayout();
            // 
            // listBoxMessages
            // 
            this.listBoxMessages.FormattingEnabled = true;
            this.listBoxMessages.Location = new System.Drawing.Point(13, 86);
            this.listBoxMessages.Name = "listBoxMessages";
            this.listBoxMessages.Size = new System.Drawing.Size(775, 316);
            this.listBoxMessages.TabIndex = 0;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(13, 408);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(693, 20);
            this.textBoxMessage.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(713, 406);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // labelChatUsername
            // 
            this.labelChatUsername.AutoSize = true;
            this.labelChatUsername.Location = new System.Drawing.Point(13, 29);
            this.labelChatUsername.Name = "labelChatUsername";
            this.labelChatUsername.Size = new System.Drawing.Size(99, 13);
            this.labelChatUsername.TabIndex = 3;
            this.labelChatUsername.Text = "labelChatUsername";
            // 
            // buttonExitChat
            // 
            this.buttonExitChat.Location = new System.Drawing.Point(712, 29);
            this.buttonExitChat.Name = "buttonExitChat";
            this.buttonExitChat.Size = new System.Drawing.Size(75, 23);
            this.buttonExitChat.TabIndex = 4;
            this.buttonExitChat.Text = "Exit";
            this.buttonExitChat.UseVisualStyleBackColor = true;
            this.buttonExitChat.Click += new System.EventHandler(this.buttonExitChat_Click);
            // 
            // ChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonExitChat);
            this.Controls.Add(this.labelChatUsername);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.listBoxMessages);
            this.Name = "ChatControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label labelChatUsername;
        private System.Windows.Forms.Button buttonExitChat;
    }
}
