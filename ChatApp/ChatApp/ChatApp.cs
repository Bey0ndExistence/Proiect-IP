using System;
using System.Windows.Forms;

namespace ChatApp
{
    using ClientHandle;
    using MessageNamespace;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// This is the main class for the ChatApp client application.
    /// It handles the initialization of the form, user login, registration, 
    /// and switching between different controls (login, register, active users, chat).
    /// </summary>
    public partial class ChatApp : Form
    {
        string _ip;
        int _port;
        string _name;
        ClientHandle _client;
        string _currentActiveUser;
        bool _loggedIn;

        /// <summary>
        /// Initializes a new instance of the ChatApp class.
        /// Sets up event handlers for user controls and initializes default values.
        /// </summary>
        public ChatApp()
        {
            InitializeComponent();

            // Attach event handlers to the user controls
            // loginControl.LoginSuccessful += SwitchToActiveUsers;
            loginControl.SwitchToRegister += SwitchToRegister;
            registerControl.SwitchToLogin += SwitchToLogin;
            activeUsersControl.SwitchToLogin += SwitchToLogin;
            activeUsersControl.SwitchToChat += SwitchToChat;
            chatControl.SwitchToActiveUsers += SwitchToActiveUsers;

            loginControl.LoginSendCredentials += LogInDataSendClicked;
            activeUsersControl.SendActiveUser += GetActiveUser;
            chatControl.SendMessage += SendMessageClicked;
            registerControl.RegisterDataSend += RegisterDataSendClicked;
            activeUsersControl.LogOut += LogOutDataSendClicked;

            chatControl.RemoveMessageNotification += RemoveNotif;

            // new method
            // chatControl.SaveChat += SaveToFile;
            activeUsersControl.LoadChat += LoadFromFile;

            _ip = "127.0.0.1";
            _port = 5678;
            _name = "anonimushihihihaaaa";
            /*_client = new ClientHandle(_name);
            _client.Start(_ip, _port);
*/
            _currentActiveUser = null;
            _loggedIn = false;
        }

        /// <summary>
        /// Event handler for selecting an active user to chat with.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The selected user's name.</param>
        private void GetActiveUser(object sender, string e)
        {
            _currentActiveUser = e;
            MessageBox.Show("Select user to chat with");
        }

        /// <summary>
        /// Event handler for sending a chat message.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The message to be sent.</param>
        private async void SendMessageClicked(object sender, string e)
        {
            if (_currentActiveUser != null)
            {
                _client.ChatMessage(_currentActiveUser, e);
            }
            MessageBox.Show("Send Message Request");
            // new method
            Invoke((Action)(() =>
                    chatControl.LoadListBoxFromFile(_name, _currentActiveUser)));
        }

        /// <summary>
        /// Event handler for logging in the user.
        /// Sends login credentials to the server and processes the response.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The user credentials.</param>
        private async void LogInDataSendClicked(object sender, UserCredentialsEventArgs e)
        {
            _client = new ClientHandle(e.Username);
            // _client.MessageReceived += OnMessageReceived; // Subscribe to the event
            _client.Start(_ip, _port);

            _client.LogInMessage(e.Username, e.Password);
            MessageBox.Show("Send Log In Request");

            Message msg = await _client.GetServerResponse();
            if (msg.Type == MessageType.UpdateOnlineUsers)
            {
                Console.WriteLine("Eroare: NU");
                loginControl.Visible = false;
                activeUsersControl.Visible = true;
                registerControl.Visible = false;
                chatControl.Visible = false;
                _currentActiveUser = null;
                _name = e.Username;

                Invoke((Action)(() => activeUsersControl.updateLoggedUsername(_name)));

                string[] users = msg.Body["userList"].Split(new string[] { ", " }, StringSplitOptions.None);

                activeUsersControl.UpdateActiveUsers(users);
                MessageBox.Show("User Loged In Successfuly");

                _loggedIn = true;
                // Start polling for messages
                Task.Run(() => PollMessages());
            }
            else
            {
                MessageBox.Show($"Error creating account: {msg.Body["Error message"]}");
                _client.Close();
            }
        }

        /// <summary>
        /// Event handler for registering a new user.
        /// Sends registration data to the server and processes the response.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The user registration data.</param>
        private async void RegisterDataSendClicked(object sender, UserRegisterDataEventArgs e)
        {
            string name;
            e.Data.TryGetValue("username", out name);
            _client = new ClientHandle(name);
            // _client.MessageReceived += OnMessageReceived; // Subscribe to the event
            _client.Start(_ip, _port);
            _client.RegisterMessage(e.Data);
            Message msg = await _client.GetServerResponse();
            if (msg.Type == MessageType.Register)
            {
                MessageBox.Show("User Created Successfuly");
            }
            else
            {
                MessageBox.Show($"Error creating account: {msg.Body["Error message"]}");
            }
            //MessageBox.Show($"------\n{msg.Type} \n {msg.Body.Values}\n------");
            _client.Close();
            /*MessageBox.Show("Send Register Request");*/
        }

        /// <summary>
        /// Event handler for logging out the user.
        /// Sends a logout message to the server and closes the connection.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private async void LogOutDataSendClicked(object sender, EventArgs e)
        {
            _client.LogOutMessage();
            MessageBox.Show("Send Log Out Request");
            _loggedIn = false;
            _client.Close();
        }

        /// <summary>
        /// Switches the view to the active users control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SwitchToActiveUsers(object sender, EventArgs e)
        {
            // Show the active users control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = true;
            registerControl.Visible = false;
            chatControl.Visible = false;

            _currentActiveUser = null;
        }

        /// <summary>
        /// Switches the view to the register control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SwitchToRegister(object sender, EventArgs e)
        {
            // Show the register control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = false;
            registerControl.Visible = true;
            chatControl.Visible = false;
            registerControl.resetRegisterScreen();

            _currentActiveUser = null;
        }

        /// <summary>
        /// Switches the view to the login control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SwitchToLogin(object sender, EventArgs e)
        {
            // Show the login control and hide all the other controls
            loginControl.Visible = true;
            activeUsersControl.Visible = false;
            registerControl.Visible = false;
            chatControl.Visible = false;
            loginControl.resetLoginScreen();

            _currentActiveUser = null;
        }

        /// <summary>
        /// Switches the view to the chat control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SwitchToChat(object sender, EventArgs e)
        {
            // Show the chat control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = false;
            registerControl.Visible = false;
            chatControl.Visible = true;
            Invoke((Action)(() => chatControl.updateLoggedUsername(_name)));
            Invoke((Action)(() => chatControl.updateChatPartnerUsername(_currentActiveUser)));
        }

        /// <summary>
        /// Polls messages from the server while the user is logged in.
        /// Updates the UI based on the messages received.
        /// </summary>
        private async Task PollMessages()
        {
            Console.WriteLine("Pooling messages...");
            while (_loggedIn)
            {
                Message msg = await _client.GetServerResponse();
                if (msg.Type == MessageType.UpdateOnlineUsers)
                {
                    Invoke((Action)(() =>
                    {
                        string[] users = msg.Body["userList"].Split(new string[] { ", " }, StringSplitOptions.None);
                        activeUsersControl.UpdateActiveUsers(users);
                        if (_currentActiveUser != null)
                        {
                            if (!msg.Body["userList"].Contains(_currentActiveUser))
                            {
                                loginControl.Visible = false;
                                activeUsersControl.Visible = true;
                                registerControl.Visible = false;
                                chatControl.Visible = false;

                                MessageBox.Show($"Userul {_currentActiveUser} s-a deconectat!");
                                _currentActiveUser = null;

                                Invoke((Action)(() =>
                                {
                                    activeUsersControl.resetSelectedActiveUser();
                                }));

                            }
                        }
                    }));
                }
                else
                if (msg.Type == MessageType.ChatMsg)
                {

                    Invoke((Action)(() =>
                    {
                        chatControl.ReceiveMessage(msg.Sender, msg.Receiver, msg.Body["message"]);
                        activeUsersControl.messageNotification(msg.Sender);
                    }));

                }
                await Task.Delay(100);
            }
            Console.WriteLine("Pooling messages finished");
        }

        /// <summary>
        /// Loads chat history from a file and updates the chat control.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void LoadFromFile(object sender, EventArgs e)
        {
            Console.WriteLine($"{_name}-{_currentActiveUser}.txt");
            Invoke((Action)(() =>
                    chatControl.LoadListBoxFromFile(_name, _currentActiveUser)));
        }

        /// <summary>
        /// Removes the message notification for the active user.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveNotif(object sender, EventArgs e)
        {
            Invoke((Action)(() =>
                    activeUsersControl.removeMessageNotification()));
        }
    }
}
