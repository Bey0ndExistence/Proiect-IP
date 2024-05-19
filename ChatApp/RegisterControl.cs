using System;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class RegisterControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToLogin;

        public RegisterControl()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            // Implement your registration logic here
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // For demonstration purposes, assume any non-empty username and password is a successful registration
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // After successful registration, switch to login view
                SwitchToLogin?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private void switchToLoginButton_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToLogin event
            SwitchToLogin?.Invoke(this, EventArgs.Empty);
        }

        public void resetRegisterScreen()
        {
            textBoxConfirmEmail.Text = "";
            textBoxConfirmPassword.Text = "";
            textBoxEmail.Text = "";
            textBoxFirstname.Text = "";
            textBoxLastname.Text = "";
            textBoxPassword.Text = "";
            textBoxPhoneNumber.Text = "";
            textBoxUsername.Text = "";
        }
    }
}
