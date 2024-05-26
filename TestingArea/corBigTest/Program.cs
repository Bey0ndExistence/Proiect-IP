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
        }

        static void HandleClient(Socket clientSocket, RequestHandler handler, Dictionary<string, Socket> users)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int receivedBytes = clientSocket.Receive(buffer);
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Message message = Message.FromJson(receivedText);

                    if (!users.ContainsKey(message.Sender))
                    {
                        users.Add(message.Sender, clientSocket);
                    }

                    Task.Run(() => handler.Handle(message, users));
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
            
        }
    }
}
