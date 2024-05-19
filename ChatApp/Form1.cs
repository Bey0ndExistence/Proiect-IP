using System;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Attach event handlers to the user controls
            loginControl.LoginSuccessful += SwitchToActiveUsers;
            loginControl.SwitchToRegister += SwitchToRegister;
            registerControl.SwitchToLogin += SwitchToLogin;
            activeUsersControl.SwitchToLogin += SwitchToLogin;
            activeUsersControl.SwitchToChat += SwitchToChat;
            chatControl.SwitchToActiveUsers += SwitchToActiveUsers;
        }

        private void SwitchToActiveUsers(object sender, EventArgs e)
        {
            // Show the active users control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = true;
            registerControl.Visible = false;
            chatControl.Visible = false;
        }

        private void SwitchToRegister(object sender, EventArgs e)
        {
            // Show the register control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = false;
            registerControl.Visible = true;
            chatControl.Visible = false;
            registerControl.ResetFields();
        }

        private void SwitchToLogin(object sender, EventArgs e)
        {
            // Show the login control and hide all the other controls
            loginControl.Visible = true;
            activeUsersControl.Visible = false;
            registerControl.Visible = false;
            chatControl.Visible = false;
            loginControl.ResetFields();
        }
        private void SwitchToChat(object sender, EventArgs e)
        {
            // Show the chat control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = false;
            registerControl.Visible = false;
            chatControl.Visible = true;
            chatControl.ResetFields();
        }
    }
}
