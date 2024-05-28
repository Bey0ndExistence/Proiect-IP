/**************************************************************************
 *                                                                        *
 *  File:        RegisterRequestHandler.cs                                *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description: This file contains the implementation of the 
 *  RegisterRequestHandler class, which handles user registration requests.*
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

using MessageNamespace;
using Persistance;
using Persistance.Exceptions;

namespace ServerRequestHandler
{
    /// <summary>
    /// Handles user registration requests.
    /// </summary>
    public class RegisterRequestHandler : RequestHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequestHandler"/> class.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        public RegisterRequestHandler(RequestHandler next = null) : base(next) { }

        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="users">The dictionary of connected users.</param>
        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Register)
            {
                lock(users)
                {
                    Console.WriteLine($"Register Handler: Handling register of {message.Sender}.\n client register data: ");
                    Console.WriteLine(string.Join(", ", message.Body.Select(kv => $"{kv.Key}: {kv.Value}")));
                    // resolve the request
                    try
                    {
                        DatabaseAccess.Instance.CreateUserTable();
                        Console.WriteLine(DatabaseAccess.Instance.RegisterUser(message.Body));

                        string messageJson = Message.ToJson(message);
                        byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                        // Send the message
                        users[message.Sender].Send(messageBuffer);
                        Console.WriteLine($"Message sent to {message.Sender}");

                        // removing the client after we temporarely stored him and his socket in usersList
                        users.Remove(message.Sender);
                        Console.WriteLine(users.Count);
                        Console.WriteLine(users.Keys);
                    }
                    catch (DatabaseConnectionException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (DatabaseDisconnectionException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (CreateTableException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (UserRegisterException e)
                    {
                        Console.WriteLine(e.Message);
                        string pattern = @"userdata\.(\w+)";
                        Match match = Regex.Match(e.Message, pattern);
                        SendErrorResponse(MessageType.RegisterResponse, message.Sender, $"{match.Groups[1].Value} already in use", users);
                        users.Remove(message.Sender);
                        Console.WriteLine(users.Count);
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
