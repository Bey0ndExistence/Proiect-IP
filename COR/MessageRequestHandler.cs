/**************************************************************************
 *                                                                        *
 *  File:        MessageRequestHandler.cs                                 *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description: This file contains the implementation of the 
 *  MessageRequestHandler class, which handles chat messages between users.*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;

namespace ServerRequestHandler
{
    /// <summary>
    /// Handles chat messages between users.
    /// </summary>
    public class MessageRequestHandler : RequestHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestHandler"/> class.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        public MessageRequestHandler(RequestHandler next = null) : base(next) { }

        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="users">The dictionary of connected users.</param>
        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if ( message.Type == MessageType.ChatMsg )
            {
                lock(users)
                {
                    Console.WriteLine($"Chat Message Handler: Handling chat msg from {message.Sender} to {message.Receiver}:");
                    Console.WriteLine(string.Join(", ", message.Body.Select(kv => $"{kv.Key}: {kv.Value}")));
                    // resolve the request
                    if (!users.ContainsKey(message.Receiver) || users[message.Receiver] == null)
                    {
                        Console.WriteLine($"User {message.Receiver} not found!");
                        SendErrorResponse(MessageType.UserNotFound, message.Sender, $"User {message.Receiver} was not found!", users);
                        return;
                    }

                    try
                    {
                        string messageJson = Message.ToJson(message);
                        byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                        Socket receiverSocket = users[message.Receiver];
                        receiverSocket.Send(messageBuffer);

                        Console.WriteLine($"Message sent to {message.Receiver}");
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"SocketException: {ex.Message}");
                        SendErrorResponse(MessageType.ServerError, message.Sender, "Server is not responding... Try again later...", users);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception: {e.Message}");
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                }
            }
            else
            {
                try
                {
                    _next.Handle(message, users);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
