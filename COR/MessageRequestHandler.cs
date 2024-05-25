using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageNamespace;

namespace ServerRequestHandler
{
    public class MessageRequestHandler : RequestHandler
    {
        public MessageRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message)
        {
            if ( message.Type == MessageType.ChatMsg )
            {
                Console.WriteLine($"Chat Message Handler: Handling chat msg from {message.Sender} to {message.Receiver}: {message.Body}");
                // resolve the request
            }
            else
            {
                _next.Handle(message);
            }
        }
    }
}
