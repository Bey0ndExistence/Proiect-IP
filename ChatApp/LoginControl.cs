using System;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class LoginControl : UserControl
    {
        // Events to notify the main form about login success and switching to register view
        public event EventHandler LoginSuccessful;
        public event EventHandler SwitchToRegister;

        public LoginControl()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            // Implement your login logic here
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // For demonstration purposes, assume any non-empty username and password is a successful login
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // Raise the LoginSuccessful event
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Please enter both username and password.");
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToRegister event
            SwitchToRegister?.Invoke(this, EventArgs.Empty);
        }

        public void resetLoginScreen()
        {
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
        }
    }
}
