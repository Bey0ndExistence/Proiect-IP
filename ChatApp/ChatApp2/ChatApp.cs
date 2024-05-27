using System;
using System.Windows.Forms;

namespace ChatApp
{
    using ClientHandle;
    using MessageNamespace;
    using System.Threading.Tasks;

    public partial class ChatApp : Form
    {
        string _ip;
        int _port;
        string _name;
        ClientHandle _client;
        string _currentActiveUser;
        bool _loggedIn;

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

            _ip = "127.0.0.1";
            _port = 5678;
            _name = "Client 1";
            /*_client = new ClientHandle(_name);
            _client.Start(_ip, _port);
*/
            _currentActiveUser = null;
            _loggedIn = false;
        }

        private void GetActiveUser(object sender, string e)
        {
            _currentActiveUser = e;
            MessageBox.Show("Select user to chat with");
        }

        private async void SendMessageClicked(object sender, string e)
        {
            if (_currentActiveUser != null)
            {
                _client.ChatMessage(_currentActiveUser, e);
            }
            MessageBox.Show("Send Message Request");
        }

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
            //MessageBox.Show($"------\n{msg.Type} \n {msg.Body.Values}\n------");

            /*bool isOK = await _client.IsOKResponse();
            if (isOK)
            {
                Console.WriteLine("Eroare: NU");
                loginControl.Visible = false;
                activeUsersControl.Visible = true;
                registerControl.Visible = false;
                chatControl.Visible = false;

                _currentActiveUser = null;

            }
            else
            {
                Console.WriteLine("Eroare: DA");
                MessageBox.Show("Login failed. Please check your credentials.");
            }*/
        }

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

        private async void LogOutDataSendClicked(object sender, EventArgs e)
        {
            _client.LogOutMessage();
            MessageBox.Show("Send Log Out Request");
            _loggedIn = false;
            _client.Close();
        }

        /*private async void LogOutClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Logging out");
            _client.LogOutMessage();
        }*/

        private void SwitchToActiveUsers(object sender, EventArgs e)
        {
            // Show the active users control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = true;
            registerControl.Visible = false;
            chatControl.Visible = false;

            _currentActiveUser = null;
        }

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

        private void SwitchToChat(object sender, EventArgs e)
        {
            // Show the chat control and hide all the other controls
            loginControl.Visible = false;
            activeUsersControl.Visible = false;
            registerControl.Visible = false;
            chatControl.Visible = true;
        }

        // Method II
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
                    }));
                }
                else
                if (msg.Type == MessageType.ChatMsg)
                {
                    Invoke((Action)(() =>
                    chatControl.ReceiveMessage(msg.Sender, msg.Body["message"])));
                    //MessageBox.Show($"{msg.Body["message"]}");
                }
                await Task.Delay(100);
            }
            Console.WriteLine("Pooling messages finished");
        }
    }
}
