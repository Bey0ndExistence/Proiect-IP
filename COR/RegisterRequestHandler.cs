using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                lock(users)
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

                        // removing the client after we temporarely stored him and his socket in usersList
                        users.Remove(message.Sender);
                    }
                    catch (DatabaseConnectionException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (DatabaseDisconnectionException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (CreateTableException e)
                    {
                        Console.WriteLine(e.Message);
                        SendErrorResponse(MessageType.ServerError, message.Sender, "An unexpected error occurred. Please try again later.", users);
                    }
                    catch (UserRegisterException e)
                    {
                        Console.WriteLine(e.Message);
                        string pattern = @"userdata\.(\w+)";
                        Match match = Regex.Match(e.Message, pattern);
                        SendErrorResponse(MessageType.RegisterResponse, message.Sender, $"{match.Groups[1].Value} already in use", users);
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
