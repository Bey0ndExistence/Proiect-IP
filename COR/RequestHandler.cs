using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;

namespace ServerRequestHandler
{
    public abstract class RequestHandler
    {
        protected RequestHandler _next;

        protected RequestHandler(RequestHandler next = null)
        {
            _next = next;
        }

        public void SetNext(RequestHandler next)
        {
            _next = next;
        }

        public abstract void Handle(Message msg, Dictionary<string, Socket> users);

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
