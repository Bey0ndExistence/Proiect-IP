/**************************************************************************
 *                                                                        *
 *  File:        LogInRequestHandler.cs                                   *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description:  This file contains the implementation of the
 *  LogInRequestHandler class,which handles login requests from users.    *
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
using Persistance;
using Persistance.Exceptions;

namespace ServerRequestHandler
{
    /// <summary>
    /// Handles login requests from users.
    /// </summary>
    public class LogInRequestHandler : RequestHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogInRequestHandler"/> class.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        public LogInRequestHandler(RequestHandler next = null) : base(next) { }

        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="users">The dictionary of connected users.</param>
        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Login)
            {
                lock(users)
                {
                    Console.WriteLine($"Login Request Handle: Handling log in of {message.Sender} client to {message.Receiver}");
                    // resolve the request
                    try
                    {
                        if (DatabaseAccess.Instance.LoginCheck(message.Body))
                        {
                            Message response = new Message(MessageType.UpdateOnlineUsers, "", "", new Dictionary<string, string> { { "userList", string.Join(", ", users.Keys.ToList()) } });
                            string responseJson = Message.ToJson(response);
                            byte[] responseBuffer = Encoding.UTF8.GetBytes(responseJson);
                            foreach (var userSocket in users.Values)
                            {
                                try
                                {
                                    userSocket.Send(responseBuffer);
                                }
                                catch (SocketException ex)
                                {
                                    Console.WriteLine($"SocketException while sending online users update: {ex.Message}");
                                    SendErrorResponse(MessageType.ServerError, message.Sender, "Server is not responding... Try again later...", users);
                                }
                            }
                        }
                    }
                    catch (LoginException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ErrorLogin, message.Sender, "Invalid login credentials", users);
                        if (users.ContainsKey(message.Sender))
                            users.Remove(message.Sender);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception: {e.Message}");
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                        if (users.ContainsKey(message.Sender))
                            users.Remove(message.Sender);
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
