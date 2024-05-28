/// <file>
/// <author>Andrei Zacordoneț</author>
/// <summary>
/// This file contains the implementation of the LogOutRequestHandler class,
/// which handles logout requests from users.
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
    /// Handles logout requests from users.
    /// </summary>
    public class LogOutRequestHandler : RequestHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogOutRequestHandler"/> class.
        /// </summary>
        /// <param name="next">The next handler in the chain.</param>
        public LogOutRequestHandler(RequestHandler next = null) : base(next) { }

        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <param name="users">The dictionary of connected users.</param>
        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Logout)
            {
                Console.WriteLine($"Logout Request Handler: Handling logout of {message.Sender}");
                lock (users)
                {
                    //Console.WriteLine(string.Join(", ", users.Select(kv => $"{kv.Key}")));
                    if (users.ContainsKey(message.Sender))
                    {
                        try
                        {
                            // users[message.Sender].Close();
                            //users[message.Sender].Shutdown(SocketShutdown.Both);
                            HandleClientDisconnection(users[message.Sender]);
                            users.Remove(message.Sender);
                            Console.WriteLine($"User {message.Sender} has been logged out and removed from the users list.");

                            if (users.Count > 0)
                            {
                                BroadcastUpdatedUserList(users);
                            }
                            else
                            {
                                Console.WriteLine("No users online!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during logout process: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User {message.Sender} not found in the users list.");
                        //Console.WriteLine(string.Join(", ", users.Select(kv => $"{kv.Key}")));
                        SendErrorResponse(MessageType.UserNotFound, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not a valid request!");
            }
        }

        /// <summary>
        /// Broadcasts the updated user list to all connected users.
        /// </summary>
        /// <param name="users">The dictionary of connected users.</param>
        private void BroadcastUpdatedUserList(Dictionary<string, Socket> users)
        {
            var userList = string.Join(", ", users.Keys.ToList());
            var response = new Message(MessageType.UpdateOnlineUsers, "", "", new Dictionary<string, string> { { "userList", userList } });
            var responseJson = Message.ToJson(response);
            var responseBuffer = Encoding.UTF8.GetBytes(responseJson);

            foreach (var userSocket in users.Values)
            {
                try
                {
                    userSocket.Send(responseBuffer);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Error sending updated user list to {userSocket.RemoteEndPoint}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Handles client disconnection by shutting down and closing the socket.
        /// </summary>
        /// <param name="clientSocket">The socket of the client to disconnect.</param>
        public void HandleClientDisconnection(Socket clientSocket)
        {
            try
            {
                // Shut down the socket to signal that no more data will be sent or received
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException during shutdown: {ex.Message}");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"ObjectDisposedException during shutdown: {ex.Message}");
            }
            finally
            {
                // Close the socket and release all resources
                clientSocket.Close();
                Console.WriteLine("Socket closed successfully.");
            }
        }
    }
}
