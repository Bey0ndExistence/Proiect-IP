/**************************************************************************
 *                                                                        *
 *  File:        Server.cs                                                *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description:  This file contains the implementation of the Server class, 
 *  which handles client connections and processes messages.              *
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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using ServerRequestHandler;
using MessageNamespace;
using Persistance;
using MySql.Data.MySqlClient;

namespace ServerNamespace
{
    /// <summary>
    /// Represents a server that handles client connections and processes messages.
    /// </summary>
    public class Server : IServer
    {
        private ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        private AutoResetEvent messageEvent = new AutoResetEvent(false);
        private Socket serverSocket;
        private int portNumber;
        private Dictionary<string, Socket> users = new Dictionary<string, Socket>();


        private MessageRequestHandler chatHandler;
        private LogInRequestHandler loginHandler;
        private RegisterRequestHandler registerHandler;
        private LogOutRequestHandler logoutHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class with the specified port number.
        /// </summary>
        /// <param name="port">The port number on which the server will listen for client connections.</param>
        public Server(int port)
        {
            portNumber = port;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, portNumber));
            serverSocket.Listen(10);
        }

        /// <summary>
        /// Starts the server and begins listening for client connections.
        /// </summary>
        public void ServerStart()
        {
            chatHandler = new MessageRequestHandler();
            loginHandler = new LogInRequestHandler();
            registerHandler = new RegisterRequestHandler();
            logoutHandler = new LogOutRequestHandler();

            chatHandler.SetNext(loginHandler);
            loginHandler.SetNext(registerHandler);
            registerHandler.SetNext(logoutHandler);

            Console.WriteLine($"Server started at port {portNumber}");

            // Start a background thread to process messages
            Thread processingThread = new Thread(() => ProcessMessages());
            processingThread.Start();

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(() => HandleClient(clientSocket));
                clientThread.Start();
            }
        }

        /// <summary>
        /// Handles client connections and processes incoming messages.
        /// </summary>
        /// <param name="clientSocket">The socket associated with the connected client.</param>
        private void HandleClient(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int receivedBytes = clientSocket.Receive(buffer);
                    if (receivedBytes == 0)
                    {
                        Console.WriteLine($"received 0 bytes");
                        break; // Client disconnected
                    }

                    string receivedText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Message message;
                    try
                    {
                        message = Message.FromJson(receivedText);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid JSON: {ex.Message}");
                        continue;
                    }

                    lock (users)
                    {
                        if (!users.ContainsKey(message.Sender))
                        {
                            //users[message.Sender] = clientSocket;
                            users.Add(message.Sender, clientSocket);
                            Console.WriteLine(string.Join(", ", users.Select(kv => $"{kv.Key}")));
                        }
                        else
                        {
                            if (message.Type == MessageType.Login)
                            {

                                Message msg = new Message(MessageType.ErrorLogin, "", message.Sender, new Dictionary<string, string> { { "Error message", "User deja autentificat!" } });
                                clientSocket.Send(Encoding.UTF8.GetBytes(Message.ToJson(msg)));
                            }
                        }
                    }

                    messageQueue.Enqueue(message);
                    messageEvent.Set(); // Signal that a new message is available
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"ia de aici tata");
                    Console.WriteLine($"SocketException: {e.Message}");
                    break;
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine($"ObjectDisposedException: {e.Message}");
                    break;
                }
            }
        }

        /// <summary>
        /// Processes messages in the message queue.
        /// </summary>
        private void ProcessMessages()
        {
            while (true)
            {
                messageEvent.WaitOne(); // Wait for a signal that a new message is available

                while (messageQueue.TryDequeue(out Message message))
                {
                    Task.Run(() => chatHandler.Handle(message, users));
                }
            }
        }
    }
}
