﻿using System;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class ChatControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToActiveUsers;
        public ChatControl()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Handle sending messages
            string message = textBoxMessage.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                listBoxMessages.Items.Add(message);
                textBoxMessage.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a message.");
            }
        }

        private void buttonExitChat_Click(object sender, EventArgs e)
        {
            // Raise the SwitchToLogin event
            SwitchToActiveUsers?.Invoke(this, EventArgs.Empty);
        }

        public void ResetFields()
        {
            foreach (Control control in Controls)
            {
                if (control is ListBox)
                {
                    ((ListBox)control).Text = string.Empty;
                }
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
            }
        }

    }
}