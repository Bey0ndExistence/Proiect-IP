using System;
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

        public void updateLoggedUsername(string username)
        {
            labelLoggedUsername.Text = "You are logged in as " + username;
        }

        public void messageNotification(string sender)
        {
            if (selectedActiveUser != null && !selectedActiveUser.EndsWith(" *"))
            {
                var index = listBoxActiveUsers.Items.IndexOf(sender);
                if (index >= 0)
                    listBoxActiveUsers.Items[index] += " *";
            }
        }

        public void removeMessageNotification()
        {
            if (selectedActiveUser != null && selectedActiveUser.EndsWith(" *"))
            {
                var index = listBoxActiveUsers.Items.IndexOf(selectedActiveUser);
                if (index >= 0)
                    listBoxActiveUsers.Items[index] = selectedActiveUser.Substring(0, selectedActiveUser.Length - 2);
            }
        }

        public void resetSelectedActiveUser()
        {
            if (selectedActiveUser != null)
            {
                selectedActiveUser = null;
            }
        }
    }
}
