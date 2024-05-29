/**************************************************************************
 *                                                                        *
 *  File:        LoginControl.cs                                          *
 *  Copyright:   (c) 2024, Marco Galatanu                                 *
 *  E-mail:      marco-ionut.galatanu@student.tuiasi.ro                   *
 *  Description: Handles the login logic                                  *
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
    /// The LoginControl class handles the login interface, including sending login credentials
    /// and switching to the register view.
    /// </summary>
    public partial class LoginControl : UserControl
    {
        // Events to notify the main form about login success and switching to register view
        //public event EventHandler LoginSuccessful;
        public event EventHandler SwitchToRegister;

        public event EventHandler<UserCredentialsEventArgs> LoginSendCredentials;

        /// <summary>
        /// Initializes a new instance of the LoginControl class.
        /// </summary>
        public LoginControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event when the login button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void loginButton_Click(object sender, EventArgs e)
        {
            // Implement your login logic here
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // For demonstration purposes, assume any non-empty username and password is a successful login
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // Raise the LoginSuccessful event
                // LoginSuccessful?.Invoke(this, EventArgs.Empty);
                LoginSendCredentials?.Invoke(this, new UserCredentialsEventArgs(username, password));
            }
            else
            {
                MessageBox.Show("Please enter both username and password.");
            }
        }

        /// <summary>
        /// Handles the event when the register button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void registerButton_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToRegister event
            SwitchToRegister?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Resets the login screen by clearing the username and password fields.
        /// </summary>
        public void resetLoginScreen()
        {
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
        }

        private void buttonLoginHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "chm_help.chm");
        }
    }
}
