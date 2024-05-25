using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

using ServerRequestHandler;
using MessageNamespace;
using Persistance;
using MySql.Data.MySqlClient;

namespace corBigTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 5678));
            serverSocket.Listen(10);

            var users = new Dictionary<string, Socket>();

            MessageRequestHandler chatHandler = new MessageRequestHandler();
            LogInRequestHandler loginHandler = new LogInRequestHandler();
            RegisterRequestHandler registerHandler = new RegisterRequestHandler();
            LogOutRequestHandler logoutHandler = new LogOutRequestHandler();

            chatHandler.SetNext(loginHandler);
            loginHandler.SetNext(registerHandler);
            registerHandler.SetNext(logoutHandler);

            Console.WriteLine("Server started on port 5678...");

            while(true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(() => HandleClient(clientSocket, chatHandler, users));
                clientThread.Start();
            }

            /*
            Dictionary<string, Socket> users = new Dictionary<string, Socket> { { "user1", null }, { "user2", null } };

            // Request Handlers
            RegisterRequestHandler registerHandler = new RegisterRequestHandler();
            MessageRequestHandler messageHandler = new MessageRequestHandler();

            // request init
            var userData = new Dictionary<string, string>
              {
                  { "username", "testuser7" },
                  { "password", "testpassword" },
                  { "email", "testuser7@example.com" },
                  { "firstname", "Test" },
                  { "lastname", "User" },
                  { "phone_number", "12345678907" }
              };
            Message request = new Message(MessageType.Register, "testuser2", "server", userData);

            // Request Handle
            // registerHandler.Handle(request, users);
            Console.Read();
            */
        }

        static void HandleClient(Socket clientSocket, RequestHandler handler, Dictionary<string, Socket> users)
        {
            byte[] buffer = new byte[1024];
            int receivedBytes = clientSocket.Receive(buffer);
            string receivedText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
            Message message = Message.FromJson(receivedText);

            if (!users.ContainsKey(message.Sender))
            {
                users.Add(message.Sender, clientSocket);
            }

            handler.Handle(message, users);
        }
    }
}
