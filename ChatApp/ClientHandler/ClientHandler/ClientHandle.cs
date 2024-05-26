using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MessageNamespace;
using System.Collections.Concurrent;

namespace ClientHandle
{
    public class ClientHandle
    {
        private Socket _socket;
        private string _username;
        private bool _running;
        private ConcurrentQueue<string> _receivedMessages;

        public ClientHandle(string username)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _username = username;
            _receivedMessages = new ConcurrentQueue<string>();
        }
        public async void Start(string ip, int port)
        {
            _running = true;
            try
            {
                _socket.Connect(ip, port);
                Task listenTask = Task.Run(() => ListenForResponse());
                Console.WriteLine("Listening...");

                await listenTask;
            }
            finally
            {
                _socket.Close();
                Console.WriteLine($"END");
            }
        }

        private void SendMessage(Message message)
        {
            if (_socket == null || !_socket.Connected)
            {
                Console.WriteLine("Socket is not connected or has been closed.");
                return;
            }

            string messageJson = Message.ToJson(message);
            byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

            try
            {
                Console.WriteLine($"Sent: {messageJson}");
                _socket.Send(messageBuffer);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine($"ObjectDisposedException in SendMessage: {e.Message}");
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException in SendMessage: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in SendMessage: {e.Message}");
            }
        }

        private void ListenForResponse()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (_running)
                {
                    int receivedBytes = _socket.Receive(buffer);
                    if (receivedBytes > 0)
                    {
                        string responseText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                        Console.WriteLine($"Received: {responseText}");
                        
                        _receivedMessages.Enqueue(responseText);
                    }
                }
            }
            catch (SocketException e)
            {
                if (_running)
                {
                    Console.WriteLine($"SocketException: {e.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }

        public void Close()
        {
            _running = false;
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                    Console.WriteLine("Connection closed.");
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine($"ObjectDisposedException in Close: {e.Message}");
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"SocketException in Close: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception in Close: {e.Message}");
                }
                finally
                {
                    _socket = null;
                }
            }
        }

        public async Task<Message> GetNextMessage()
        {
            string msg;
            while (!_receivedMessages.TryDequeue(out msg))
            {
                await Task.Delay(100); // Wait for a short period before checking again
            }

            return Message.FromJson(msg);
        }

        public async Task<bool> IsOKResponse()
        {
            Message message = await GetNextMessage();
            Console.WriteLine(message.Type);
            return message.Type != MessageType.ErrorLogin;
        }

        public void RegisterMessage(Dictionary<string, string> data)
        {
            Message registerMessage = new Message(MessageType.Register, _username, "Server", data);
            SendMessage(registerMessage);
        }
        public void LogOutMessage()
        {
            Message logOutMessage = new Message(MessageType.Logout, _username, "", new Dictionary<string, string> { { "", ""} });
            SendMessage(logOutMessage);
            Close();
        }
        public void LogInMessage(string username, string password)
        {
            Message logInMessage = new Message(MessageType.Login, _username, "Server", new Dictionary<string, string> { 
                { "username", username }, 
                { "password", password } 
            });
            SendMessage(logInMessage);
        }
        public void ChatMessage(string to, string messageContent)
        {
            Message chatMessage = new Message(MessageType.ChatMsg, _username, to, new Dictionary<string, string> { { "message", messageContent } });
            SendMessage(chatMessage);
        }
    }
}
