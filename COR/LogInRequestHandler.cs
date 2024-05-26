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
    public class LogInRequestHandler : RequestHandler
    {
        public LogInRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Login)
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
                            userSocket.Send(responseBuffer);
                        }
                    }
                }
                catch (LoginException e)
                {
                    Console.WriteLine(e.Message);
                    // send request to 
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
