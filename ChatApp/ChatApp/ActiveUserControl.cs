﻿using System;
using System.IO;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class ActiveUserControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToLogin;
        public event EventHandler SwitchToChat;

        public event EventHandler<string> SendActiveUser;
        public event EventHandler LogOut;

        public event EventHandler LoadChat;

        private string selectedActiveUser; // Field to store the selected active user

        public ActiveUserControl()
        {
            InitializeComponent();
        }

        // Method to update the list of active users
        public void UpdateActiveUsers(string[] activeUsers)
        {
            listBoxActiveUsers.Items.Clear();
            listBoxActiveUsers.Items.AddRange(activeUsers);
        }

        private void listBoxActiveUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if an item is selected in the ListBox
            if (listBoxActiveUsers.SelectedIndex != -1)
            {
                // Get the selected active user
                selectedActiveUser = listBoxActiveUsers.SelectedItem.ToString();
            }
        }

        private void buttonChatWithActiveUser_Click(object sender, EventArgs e)
        {
            SendActiveUser?.Invoke(this, selectedActiveUser);
            LoadChat?.Invoke(this, EventArgs.Empty);
            // Raise the SwitchToChat event
            SwitchToChat?.Invoke(this, EventArgs.Empty);
            //Console.WriteLine($"User selected: {selectedActiveUser}");
        }
        private void buttonActiveUsersExit_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToLogin event
            SwitchToLogin?.Invoke(this, EventArgs.Empty);
            LogOut?.Invoke(this, EventArgs.Empty);
        }

        public void updateLoggedUsername(string username)
        {
            labelLoggedUsername.Text = "You are logged in as " + username;
        }
    }
}
