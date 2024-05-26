﻿namespace ChatApp
{
    partial class ChatApp
    {
        private System.ComponentModel.IContainer components = null;
        private LoginControl loginControl;
        private RegisterControl registerControl;
        private ChatControl chatControl;
        private ActiveUserControl activeUsersControl;

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
            this.loginControl = new global::ChatApp.LoginControl();
            this.registerControl = new global::ChatApp.RegisterControl();
            this.chatControl = new global::ChatApp.ChatControl();
            this.activeUsersControl = new global::ChatApp.ActiveUserControl(); // Add this line
            this.SuspendLayout();
            // 
            // loginControl
            // 
            this.loginControl.Location = new System.Drawing.Point(0, 0);
            this.loginControl.Name = "loginControl";
            this.loginControl.Size = new System.Drawing.Size(800, 450);
            this.loginControl.TabIndex = 0;
            // 
            // registerControl
            // 
            this.registerControl.Location = new System.Drawing.Point(0, 0);
            this.registerControl.Name = "registerControl";
            this.registerControl.Size = new System.Drawing.Size(800, 450);
            this.registerControl.TabIndex = 1;
            this.registerControl.Visible = false;
            // 
            // chatControl
            // 
            this.chatControl.Location = new System.Drawing.Point(0, 0);
            this.chatControl.Name = "chatControl";
            this.chatControl.Size = new System.Drawing.Size(800, 450);
            this.chatControl.TabIndex = 2;
            this.chatControl.Visible = false;
            // 
            // activeUsersControl
            // 
            this.activeUsersControl.Location = new System.Drawing.Point(0, 0);
            this.activeUsersControl.Name = "activeUsersControl";
            this.activeUsersControl.Size = new System.Drawing.Size(800, 450);
            this.activeUsersControl.TabIndex = 3;
            this.activeUsersControl.Visible = false; // Set visible to false initially
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loginControl);
            this.Controls.Add(this.registerControl);
            this.Controls.Add(this.chatControl);
            this.Controls.Add(this.activeUsersControl); // Add activeUsersControl to the controls collection
            this.Name = "Form1";
            this.Text = "Chat Application";
            this.ResumeLayout(false);
        }
    }
}