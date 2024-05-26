using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;

namespace ServerRequestHandler
{
    public class LogOutRequestHandler : RequestHandler
    {
        public LogOutRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Logout)
            {
                Console.WriteLine($"Logout Request Handler: Handling logout of {message.Sender}");
                lock (users)
                {
                    if (users.ContainsKey(message.Sender))
                    {
                        try
                        {
                            users[message.Sender].Close();
                            users.Remove(message.Sender);
                            Console.WriteLine($"User {message.Sender} has been logged out and removed from the users list.");

                            if (users.Count > 0)
                            {
                                BroadcastUpdatedUserList(users);
                            }
                            else
                            {
                                Console.WriteLine("No users online!");
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
                    }
                }
            }
            else
            {
                Console.WriteLine("Not a valid request!");
            }
        }

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
    }
}
