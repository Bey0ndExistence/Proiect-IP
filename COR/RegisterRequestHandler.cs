using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.MySqlClient;

using MessageNamespace;
using Persistance;
using Persistance.Exceptions;

namespace ServerRequestHandler
{
    public class RegisterRequestHandler : RequestHandler
    {
        public RegisterRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Register)
            {
                Console.WriteLine($"Register Handler: Handling register of {message.Sender}.\n client register data: ");
                Console.WriteLine(string.Join(", ", message.Body.Select(kv => $"{kv.Key}: {kv.Value}")));
                // resolve the request
                try
                {
                    DatabaseAccess.Instance.CreateUserTable();
                    Console.WriteLine(DatabaseAccess.Instance.RegisterUser(message.Body));

                    string messageJson = Message.ToJson(message);
                    byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                    // Send the message
                    users[message.Sender].Send(messageBuffer);
                    Console.WriteLine($"Message sent to {message.Receiver}");
                    //Console.WriteLine(RegisterUser2(message.Body));
                }
                catch (DatabaseConnectionException e)
                {
                    Console.WriteLine(e.Message);
                    // send serverError to client
                }
                catch (DatabaseDisconnectionException e)
                {
                    Console.WriteLine(e.Message);
                    // send serverError to client
                }
                catch (CreateTableException e)
                {
                    Console.WriteLine(e.Message);
                    // send serverError to client
                }
                catch (UserRegisterException e)
                {
                    Console.WriteLine(e.Message);
                    // send serverError to client
                }
            }
            else
            {
                _next.Handle(message, users);
            }
        }
    }
}
