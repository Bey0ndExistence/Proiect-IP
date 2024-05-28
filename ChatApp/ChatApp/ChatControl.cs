using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MessageNamespace;

namespace ChatApp
{
    /// <summary>
    /// The ChatControl class handles the chat interface, including sending and receiving messages,
    /// and managing chat history through file operations.
    /// </summary>
    public partial class ChatControl : UserControl
    {
        // Events to notify the main form about switching to different views and actions
        public event EventHandler SwitchToActiveUsers;
        public event EventHandler<string> SendMessage;
        public event EventHandler LoadChat;
        public event EventHandler SaveChat;
        public event EventHandler RemoveMessageNotification;

        /// <summary>
        /// Initializes a new instance of the ChatControl class.
        /// </summary>
        public ChatControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event when the send button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            // Handle sending messages
            string message = textBoxMessage.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                SaveMessageToFile(labelLoggedUsername.Text.Split(' ').Last(),
                    labelChatUsername.Text.Split(' ').Last(), message);
                LoadListBoxFromFile(labelLoggedUsername.Text.Split(' ').Last(),
                    labelChatUsername.Text.Split(' ').Last());

                // Send the message over the socket
                SendMessage?.Invoke(this, message);
                textBoxMessage.Text = "";
            }
            else
            {
                MessageBox.Show("Please enter a message.");
            }
        }

        /// <summary>
        /// Handles the event when the exit chat button is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonExitChat_Click(object sender, EventArgs e)
        {
            SaveChat?.Invoke(this, EventArgs.Empty);
            // Raise the SwitchToActiveUsers event
            SwitchToActiveUsers?.Invoke(this, EventArgs.Empty);

            RemoveMessageNotification?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Receives a message and updates the chat history.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="receiver">The receiver of the message.</param>
        /// <param name="message">The message content.</param>
        public void ReceiveMessage(string sender, string receiver, string message)
        {
            string formattedMessage = $"[{sender}]: {message}";
            SaveMessageToFile(sender, receiver, formattedMessage);
            if (sender == labelChatUsername.Text.Split(' ').Last())
                LoadListBoxFromFile(sender, receiver);
            Console.WriteLine($"Message received: {formattedMessage}");
        }

        /// <summary>
        /// Saves a message to a file.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="receiver">The receiver of the message.</param>
        /// <param name="message">The message content.</param>
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

            // Append the message to the file
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(message);
            }
        }

        /// <summary>
        /// Loads chat history from a file and updates the ListBox.
        /// </summary>
        /// <param name="sender">The sender of the messages.</param>
        /// <param name="receiver">The receiver of the messages.</param>
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the label to show the logged-in username.
        /// </summary>
        /// <param name="username">The username of the logged-in user.</param>
        public void updateLoggedUsername(string username)
        {
            labelLoggedUsername.Text = "You are logged in as " + username;
        }

        /// <summary>
        /// Updates the label to show the chat partner's username.
        /// </summary>
        /// <param name="username">The username of the chat partner.</param>
        public void updateChatPartnerUsername(string username)
        {
            labelChatUsername.Text = "You are chatting with " + username;
        }
    }
}
