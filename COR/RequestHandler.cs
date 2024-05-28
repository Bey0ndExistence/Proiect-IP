/// <file>
/// <author>Andrei Zacordoneț</author>
/// <summary>
/// This file contains the definition of the abstract RequestHandler class,
/// which provides the base for handling different types of requests in the server.
/// </summary>
/// </file>

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
    /// Abstract base class for handling requests.
    /// </summary>
    public abstract class RequestHandler
    {
        /// <summary>
        /// The next handler in the chain of responsibility.
        /// </summary>
        protected RequestHandler _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler"/> class.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        protected RequestHandler(RequestHandler next = null)
        {
            _next = next;
        }

        /// <summary>
        /// Sets the next handler in the chain of responsibility.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        public void SetNext(RequestHandler next)
        {
            _next = next;
        }

        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="msg">The message to handle.</param>
        /// <param name="users">The dictionary of connected users.</param>
        public abstract void Handle(Message msg, Dictionary<string, Socket> users);

        /// <summary>
        /// Sends an error response to the sender of the message.
        /// </summary>
        /// <param name="msgType">The type of the message.</param>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="errorMessage">The error message to send.</param>
        /// <param name="users">The dictionary of connected users.</param>
        protected static void SendErrorResponse(MessageType msgType, string sender, string errorMessage, Dictionary<string, Socket> users)
        {
            Message errorResponse = new Message(msgType, null, sender, new Dictionary<string, string> { { "Error message", errorMessage } });

            if (users.ContainsKey(sender))
            {
                try
                {
                    users[sender].Send(Encoding.UTF8.GetBytes(Message.ToJson(errorResponse)));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception while sending error response: {e.Message}");
                }
            }
        }
    }
}
