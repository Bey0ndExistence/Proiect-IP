using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

using MessageNamespace;

namespace ChatApp
{
    public partial class ChatControl : UserControl
    {
        // Events to notify the main form about switching to login view
        public event EventHandler SwitchToActiveUsers;

        public event EventHandler<string> SendMessage;
        /*        public event EventHandler<string> ReceiveMessage;
        */
        public event EventHandler LoadChat;
        public event EventHandler SaveChat;

        public event EventHandler RemoveMessageNotification;
        public ChatControl()
        {
            InitializeComponent();
        }

        // new method
        /*private void buttonSend_Click(object sender, EventArgs e)
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
        }*/

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Handle sending messages
            string message = textBoxMessage.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                /*listBoxMessages.Items.Add(message);
                textBoxMessage.Clear();*/
                SaveMessageToFile(labelLoggedUsername.Text.Split(' ').Last(),
                    labelChatUsername.Text.Split(' ').Last(), message);
                LoadListBoxFromFile(labelLoggedUsername.Text.Split(' ').Last(),
                    labelChatUsername.Text.Split(' ').Last());

                //send over socket the message
                SendMessage?.Invoke(this, message);
                textBoxMessage.Text = "";
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

            RemoveMessageNotification?.Invoke(this, EventArgs.Empty);
        }

        /*public void ReceiveMessage(string sender, string message)
        {
            string s = $"[{sender}]: {message}";
            listBoxMessages.Items.Add(s);
            Console.WriteLine($"Message received: {s}");
        }*/
        // new method
        public void ReceiveMessage(string sender, string receiver, string message)
        {
            string s = $"[{sender}]: {message}";
            // listBoxMessages.Items.Add(s);
            SaveMessageToFile(sender, receiver, s);
            if (sender == labelChatUsername.Text.Split(' ').Last())
                LoadListBoxFromFile(sender, receiver);
            Console.WriteLine($"Message received: {s}");
        }

        /*public void SaveListBoxToFile(string sender, string receiver)
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
        }*/

        public void SaveMessageToFile(string sender, string receiver, string message)
        {
            string directoryPath = "chat";
            string fileName1 = $"{sender}-{receiver}.txt";
            string fileName2 = $"{receiver}-{sender}.txt";
            string filePath;

            // Ensure the 'chat' directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Check if either combination of file names exists
            if (File.Exists(Path.Combine(directoryPath, fileName1)))
            {
                filePath = Path.Combine(directoryPath, fileName1);
            }
            else if (File.Exists(Path.Combine(directoryPath, fileName2)))
            {
                filePath = Path.Combine(directoryPath, fileName2);
            }
            else
            {
                // If neither file exists, use the first combination
                filePath = Path.Combine(directoryPath, fileName1);
            }

            // Append ListBox content to the file
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(message);
            }
            
        }

        public void LoadListBoxFromFile(string sender, string receiver)
        {
            string directoryPath = "chat";
            string fileName1 = $"{sender}-{receiver}.txt";
            string fileName2 = $"{receiver}-{sender}.txt";
            string filePath1 = Path.Combine(directoryPath, fileName1);
            string filePath2 = Path.Combine(directoryPath, fileName2);
            string filePathToUse = null;

            // Determine which file exists
            if (File.Exists(filePath1))
            {
                filePathToUse = filePath1;
            }
            else if (File.Exists(filePath2))
            {
                filePathToUse = filePath2;
            }
            else
            {
                // Create a new file with the first combination if neither exists
                filePathToUse = filePath1;
                try
                {
                    // Ensure the 'chat' directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Create an empty file
                    File.Create(filePathToUse).Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while creating the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            try
            {
                // Clear the ListBox before loading new content
                listBoxMessages.Items.Clear();

                // Read the file content and add to the ListBox
                string[] lines = File.ReadAllLines(filePathToUse);
                foreach (var line in lines)
                {
                    listBoxMessages.Items.Add(line);
                }

                // No need to delete the file as per the new method
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void updateLoggedUsername(string username)
        {
            labelLoggedUsername.Text = "You are logged in as " + username;
        }

        public void updateChatPartnerUsername(string username)
        {
            labelChatUsername.Text = "You are chatting with " + username;
        }
    }
}
