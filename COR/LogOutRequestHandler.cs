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
                Console.WriteLine($"Logout Request handler: Handling logout of {message.Sender}");
                // resolve the request
            }
            else
            {
                Console.WriteLine("Not a valid request!");
            }
        }
    }
}
