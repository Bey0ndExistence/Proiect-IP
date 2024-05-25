using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageNamespace;

namespace ServerRequestHandler
{
    public class RegisterRequestHandler : RequestHandler
    {
        public RegisterRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message)
        {
            if (message.Type == MessageType.Register)
            {
                Console.WriteLine($"Register Handler: Handling register of {message.Sender}.\n client register data: {message.Body}");
                // resolve the request
            }
            else
            {
                _next.Handle(message);
            }
        }
    }
}
