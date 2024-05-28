/**************************************************************************
 *                                                                        *
 *  File:        RegisterControl.cs                                       *
 *  Copyright:   (c) 2024, Marco Galatanu                                 *
 *  E-mail:      marco-ionut.galatanu@student.tuiasi.ro                   *
 *  Description: Handles the registration logic                           *
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
using System.Windows.Forms;

namespace ChatApp
{
    /// <summary>
    /// The RegisterControl class handles the registration interface, including sending registration data
    /// and switching to the login view.
    /// </summary>
    public partial class RegisterControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToLogin;
        public event EventHandler<UserRegisterDataEventArgs> RegisterDataSend;

        /// <summary>
        /// Initializes a new instance of the RegisterControl class.
        /// </summary>
        public RegisterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event when the register button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
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
                string email = textBoxConfirmEmail.Text;
                string firstname = textBoxFirstname.Text;
                string lastname = textBoxLastname.Text;
                string phone = textBoxPhoneNumber.Text;
                // After successful registration, switch to login view
                SwitchToLogin?.Invoke(this, EventArgs.Empty);
                RegisterDataSend?.Invoke(this, new UserRegisterDataEventArgs(
                    username, password, 
                    email, firstname, 
                    lastname, phone
                ));
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        /// <summary>
        /// Handles the event when the switch to login button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void switchToLoginButton_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToLogin event
            SwitchToLogin?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Resets the registration screen by clearing all input fields.
        /// </summary>
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

        private void buttonRegisterHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "chm_help.chm");
        }
    }
}
