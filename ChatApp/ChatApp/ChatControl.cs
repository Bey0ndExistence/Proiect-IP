using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChatApp
{
    public partial class ChatControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToActiveUsers;

        public event EventHandler<string> SendMessage;
/*        public event EventHandler<string> ReceiveMessage;
*/
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

                //send over socket the message
                SendMessage?.Invoke(this, message);
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
        public void ReceiveMessage(string sender, string message)
        {
            string s = $"{sender}:{message}";
            listBoxMessages.Items.Add(s);
            Console.WriteLine($"Message received: {s}");
        }
    }
}
