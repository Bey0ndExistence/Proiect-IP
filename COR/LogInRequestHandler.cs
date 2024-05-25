using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;

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
            }
            else
            {
                _next.Handle(message, users);
            }
        }
    }
}
