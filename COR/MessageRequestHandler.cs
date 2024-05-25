using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;

namespace ServerRequestHandler
{
    public class MessageRequestHandler : RequestHandler
    {
        public MessageRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if ( message.Type == MessageType.ChatMsg )
            {
                Console.WriteLine($"Chat Message Handler: Handling chat msg from {message.Sender} to {message.Receiver}:");
                Console.WriteLine(string.Join(", ", message.Body.Select(kv => $"{kv.Key}: {kv.Value}")));
                // resolve the request
                if (!users.ContainsKey(message.Receiver))
                {
                    // throw user list empty
                }
                string messageJson = Message.ToJson(message);
                Socket receiverSocket = users[message.Receiver];

                if (receiverSocket == null)
                {
                    // throw receiver socket null
                }
                try
                {
                    byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                    // Send the message
                    receiverSocket.Send(messageBuffer);
                    Console.WriteLine($"Message sent to {message.Receiver}");
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"SocketException: {ex.Message}");
                    // Handle socket exception
                }
            }
            else
            {
                _next.Handle(message, users);
            }
        }
    }
}
