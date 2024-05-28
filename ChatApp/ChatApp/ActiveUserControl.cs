/**************************************************************************
 *                                                                        *
 *  File:        ActiveUserControl.cs                                     *
 *  Copyright:   (c) 2024, Marco Galatanu                                 *
 *  E-mail:      marco-ionut.galatanu@student.tuiasi.ro                   *
 *  Description: Handles the view and selection of active users logic     *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/
using System;
using System.IO;
using System.Windows.Forms;

namespace ChatApp
{
    /// <summary>
    /// UserControl responsible for displaying the list of active users and handling user interactions related to active users.
    /// </summary>
    public partial class ActiveUserControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToLogin;
        public event EventHandler SwitchToChat;

        public event EventHandler<string> SendActiveUser;
        public event EventHandler LogOut;

        public event EventHandler LoadChat;

        private string selectedActiveUser; // Field to store the selected active user

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveUserControl"/> class.
        /// </summary>
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
            if (selectedActiveUser!= null && selectedActiveUser.EndsWith(" *"))
            {
                listBoxActiveUsers.Items[listBoxActiveUsers.Items.IndexOf(selectedActiveUser)] = selectedActiveUser.Substring(0, selectedActiveUser.Length - 2);
            }
            SendActiveUser?.Invoke(this, selectedActiveUser);
            // when opening a new chat load the file into listBoxMessages
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

        /// <summary>
        /// Updates the label indicating the currently logged-in user.
        /// </summary>
        /// <param name="username">The username of the currently logged-in user.</param>
        public void updateLoggedUsername(string username)
        {
            labelLoggedUsername.Text = "You are logged in as " + username;
        }

        /// <summary>
        /// Displays a notification indicating a new message from the given sender.
        /// </summary>
        /// <param name="sender">The sender of the new message.</param>
        public void messageNotification(string sender)
        {
            if (selectedActiveUser != null && !selectedActiveUser.EndsWith(" *"))
            {
                var index = listBoxActiveUsers.Items.IndexOf(sender);
                if (index >= 0)
                    listBoxActiveUsers.Items[index] += " *";
            }
        }

        /// <summary>
        /// Removes the message notification for the currently selected active user.
        /// </summary>
        public void removeMessageNotification()
        {
            if (selectedActiveUser != null && selectedActiveUser.EndsWith(" *"))
            {
                var index = listBoxActiveUsers.Items.IndexOf(selectedActiveUser);
                if (index >= 0)
                    listBoxActiveUsers.Items[index] = selectedActiveUser.Substring(0, selectedActiveUser.Length - 2);
            }
        }

        /// <summary>
        /// Resets the selected active user to null.
        /// </summary>

        public void resetSelectedActiveUser()
        {
            if (selectedActiveUser != null)
            {
                selectedActiveUser = null;
            }
        }

        private void buttonActiveUserHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "chm_help.chm");
        }
    }
}
