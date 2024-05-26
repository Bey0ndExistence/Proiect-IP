using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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

namespace ServerNamespace
{
    public class Server
    {
        private ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        private AutoResetEvent messageEvent = new AutoResetEvent(false);
        private Socket serverSocket;
        private Dictionary<string, Socket> users = new Dictionary<string, Socket>();


        private MessageRequestHandler chatHandler;
        private LogInRequestHandler loginHandler;
        private RegisterRequestHandler registerHandler;
        private LogOutRequestHandler logoutHandler;

        public Server(int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(10);
        }

        public void ServerStart()
        {
            chatHandler = new MessageRequestHandler();
            loginHandler = new LogInRequestHandler();
            registerHandler = new RegisterRequestHandler();
            logoutHandler = new LogOutRequestHandler();

            chatHandler.SetNext(loginHandler);
            loginHandler.SetNext(registerHandler);
            registerHandler.SetNext(logoutHandler);

            Console.WriteLine("Server started...");

            // Start a background thread to process messages
            Thread processingThread = new Thread(() => ProcessMessages());
            processingThread.Start();

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(() => HandleClient(clientSocket));
                clientThread.Start();
            }
        }

        private void HandleClient(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int receivedBytes = clientSocket.Receive(buffer);
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Message message = Message.FromJson(receivedText);

                    lock (users)
                    {
                        if (!users.ContainsKey(message.Sender))
                        {
                            users.Add(message.Sender, clientSocket);
                        }
                    }

                    messageQueue.Enqueue(message);
                    messageEvent.Set(); // Signal that a new message is available
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        private void ProcessMessages()
        {
            while (true)
            {
                messageEvent.WaitOne(); // Wait for a signal that a new message is available

                while (messageQueue.TryDequeue(out Message message))
                {
                    Task.Run(() => chatHandler.Handle(message, users));
                }
            }
        }
    }
}
