/**************************************************************************
 *                                                                        *
 *  File:        ClientHandle.cs                                          *
 *  Copyright:   (c) 2024, Rares-Gabriel Petrisor                         *
 *  E-mail:      rares-gabriel.petrisor@student.tuiasi.ro                 *
 *  Description: Creates a socket connection for client side message      *
 *		         processing						                          *
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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using MessageNamespace;
using System.Collections.Concurrent;

namespace ClientHandle
{

    /// <summary>
    /// Handles client-side communication with a server.
    /// </summary>
    public class ClientHandle
    {


        private Socket _socket;
        private string _username;
        private bool _running;
        private ConcurrentQueue<string> _receivedMessages;

        /// <summary>
        /// Initializes a new instance of the ClientHandle class.
        /// </summary>
        /// <param name="username">The username of the client.</param>
        public ClientHandle(string username)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _username = username;
            _receivedMessages = new ConcurrentQueue<string>();
        }
        /// <summary>
        /// Starts the client and connects to the server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port number of the server.</param>
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
                //_socket.Close();
                Console.WriteLine($"END");
            }
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
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
        /// <summary>
        /// Listens for responses from the server.
        /// </summary>
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

                        // maybe for msg recived handler
                        /*var message = Message.FromJson(responseText);
                        MessageReceived?.Invoke(this, message);*/

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
        /// <summary>
        /// Closes the client connection.
        /// </summary>
        public void Close()
        {
            _running = false;
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    
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
                    _socket.Close();
                    // _socket = null;
                }
            }
        }
        /// <summary>
        /// Retrieves the next message from the server.
        /// </summary>
        /// <returns>The next message received from the server.</returns>
        public async Task<Message> GetNextMessage()
        {
            string msg;
            while (!_receivedMessages.TryDequeue(out msg))
            {
                await Task.Delay(100); // Wait for a short period before checking again
            }

            return Message.FromJson(msg);
        }
        /// <summary>
        /// Checks if the server response indicates success.
        /// </summary>
        /// <returns>True if the response indicates success; otherwise, false.</returns>
        public async Task<bool> IsOKResponse()
        {
            Message message = await GetNextMessage();
            Console.WriteLine(message.Type);
            return message.Type != MessageType.ErrorLogin;
        }

        /// <summary>
        /// Retrieves the next message from the server.
        /// </summary>
        /// <returns>The next message received from the server.</returns>
        public async Task<Message> GetServerResponse()
        {
            Message message = await GetNextMessage();
            Console.WriteLine(message.Type);
            return message;
        }

        /// <summary>
        /// Registers a message with the server.
        /// </summary>
        /// <param name="data">The data to be included in the message.</param>
        public void RegisterMessage(Dictionary<string, string> data)
        {
            Message registerMessage = new Message(MessageType.Register, _username, "Server", data);
            SendMessage(registerMessage);
        }

        /// <summary>
        /// Sends a logout message to the server.
        /// </summary>
        public void LogOutMessage()
        {
            Message logOutMessage = new Message(MessageType.Logout, _username, "", new Dictionary<string, string> { { "", ""} });
            SendMessage(logOutMessage);
            // Close();
        }
        /// <summary>
        /// Sends a login message to the server.
        /// </summary>
        /// <param name="username">The username for login.</param>
        /// <param name="password">The password for login.</param>
        public void LogInMessage(string username, string password)
        {
            Message logInMessage = new Message(MessageType.Login, _username, "Server", new Dictionary<string, string> { 
                { "username", username }, 
                { "password", password } 
            });
            SendMessage(logInMessage);
        }

        /// <summary>
        /// Sends a chat message to the specified recipient.
        /// </summary>
        /// <param name="to">The recipient of the message.</param>
        /// <param name="messageContent">The content of the message.</param>
        public void ChatMessage(string to, string messageContent)
        {
            Message chatMessage = new Message(MessageType.ChatMsg, _username, to, new Dictionary<string, string> { { "message", messageContent } });
            SendMessage(chatMessage);
        }

       
    }
}
