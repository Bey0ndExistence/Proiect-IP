using System;
using System.IO;
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
        //public event EventHandler LoadChat;
        public event EventHandler SaveChat;
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
            SaveChat?.Invoke(this, EventArgs.Empty);
            // Raise the SwitchToLogin event
            SwitchToActiveUsers?.Invoke(this, EventArgs.Empty);
        }
        public void ReceiveMessage(string sender, string message)
        {
            string s = $"[{sender}]: {message}";
            listBoxMessages.Items.Add(s);
            Console.WriteLine($"Message received: {s}");
        }

        public void SaveListBoxToFile(string sender, string receiver)
        {
            string directoryPath = "chat";
            string fileName = $"{sender}-{receiver}.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            // Ensure the 'chat' directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Append ListBox content to the file
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                foreach (var item in listBoxMessages.Items)
                {
                    sw.WriteLine(item);
                }
            }
            // Clear the ListBox before loading new content
            listBoxMessages.Items.Clear();
        }

        public void LoadListBoxFromFile(string sender, string receiver)
        {
            string directoryPath = "chat";
            string fileName = $"{sender}-{receiver}.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if the file exists
            if (File.Exists(filePath))
            {
                try
                {
                    // Read the file content and add to the ListBox
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        listBoxMessages.Items.Add(line);
                    }

                    // Delete the file after reading its contents
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while reading the file or deleting it: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Chat history file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
