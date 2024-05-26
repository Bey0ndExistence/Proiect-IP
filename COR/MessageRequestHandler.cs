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
                lock(users)
                {
                    Console.WriteLine($"Chat Message Handler: Handling chat msg from {message.Sender} to {message.Receiver}:");
                    Console.WriteLine(string.Join(", ", message.Body.Select(kv => $"{kv.Key}: {kv.Value}")));
                    // resolve the request
                    if (!users.ContainsKey(message.Receiver) || users[message.Receiver] == null)
                    {
                        Console.WriteLine($"User {message.Receiver} not found!");
                        SendErrorResponse(MessageType.UserNotFound, message.Sender, $"User {message.Receiver} was not found!", users);
                        return;
                    }

                    try
                    {
                        string messageJson = Message.ToJson(message);
                        byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                        Socket receiverSocket = users[message.Receiver];
                        receiverSocket.Send(messageBuffer);

                        Console.WriteLine($"Message sent to {message.Receiver}");
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"SocketException: {ex.Message}");
                        SendErrorResponse(MessageType.ServerError, message.Sender, "Server is not responding... Try again later...", users);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Exception: {e.Message}");
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
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
