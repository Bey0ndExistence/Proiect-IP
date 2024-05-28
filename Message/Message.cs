/**************************************************************************
 *                                                                        *
 *  File:        Message.cs                                               *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description: This file contains classes and enums for handling messages 
 *  in a chat application.                                                *
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
using System.Text.Json;

namespace MessageNamespace
{
    /// <summary>
    /// Enumeration representing different types of messages in the chat application.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Message type for user registration.
        /// </summary>
        Register = 0,

        /// <summary>
        /// Message type for the response to a user registration.
        /// </summary>
        RegisterResponse,

        /// <summary>
        /// Message type for user login.
        /// </summary>
        Login,

        /// <summary>
        /// Message type for updating the list of online users.
        /// </summary>
        UpdateOnlineUsers,

        /// <summary>
        /// Message type indicating an error during login.
        /// </summary>
        ErrorLogin,

        /// <summary>
        /// Message type for sending a chat message.
        /// </summary>
        ChatMsg,

        /// <summary>
        /// Message type for user logout.
        /// </summary>
        Logout,

        /// <summary>
        /// Message type indicating a server error.
        /// </summary>
        ServerError,

        /// <summary>
        /// Message type indicating that a user was not found.
        /// </summary>
        UserNotFound
    }

    /// <summary>
    /// Class representing a message in the chat application.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Gets or sets the sender of the message.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the receiver of the message.
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// Gets or sets the body of the message as a dictionary.
        /// </summary>
        public Dictionary<string, string> Body { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="receiver">The receiver of the message.</param>
        /// <param name="body">The body of the message.</param>
        public Message(MessageType type, string sender, string receiver, Dictionary<string, string> body)
        {
            Type = type;
            Sender = sender;
            Receiver = receiver;
            Body = body;
        }

        /// <summary>
        /// Converts a <see cref="Message"/> object to its JSON string representation.
        /// </summary>
        /// <param name="message">The message object to convert.</param>
        /// <returns>The JSON string representation of the message.</returns>
        public static string ToJson(Message message)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(message, options);
        }

        /// <summary>
        /// Converts a JSON string to a <see cref="Message"/> object.
        /// </summary>
        /// <param name="json">The JSON string representation of a message.</param>
        /// <returns>The <see cref="Message"/> object.</returns>
        public static Message FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            try
            {
                return JsonSerializer.Deserialize<Message>(json, options);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return new Message(MessageType.ServerError, "", "", new Dictionary<string, string> { { "", "" } });
            }
        }
    }
}
