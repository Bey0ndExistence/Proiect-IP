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

namespace corBigTest
{
    class Program
    {
        static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        static AutoResetEvent messageEvent = new AutoResetEvent(false);
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

            // Start a background thread to process messages
            Thread processingThread = new Thread(() => ProcessMessages(chatHandler, users));
            processingThread.Start();

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread clientThread = new Thread(() => HandleClient(clientSocket, users));
                clientThread.Start();
            }
        }

        static void HandleClient(Socket clientSocket, Dictionary<string, Socket> users)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int receivedBytes = clientSocket.Receive(buffer);
                    if (receivedBytes == 0) break; // Client disconnected

                    string receivedText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Message message;
                    try
                    {
                        message = Message.FromJson(receivedText);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid JSON: {ex.Message}");
                        continue;
                    }

                    lock (users)
                    {
                        if (!users.ContainsKey(message.Sender))
                        {
                            //users[message.Sender] = clientSocket;
                            users.Add(message.Sender, clientSocket);
                            Console.WriteLine(string.Join(", ", users.Select(kv => $"{kv.Key}")));
                        }
                    }

                    messageQueue.Enqueue(message);
                    messageEvent.Set(); // Signal that a new message is available
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"SocketException: {e.Message}");
                    break;
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine($"ObjectDisposedException: {e.Message}");
                    break;
                }
            }

            // Client has disconnected
            /*lock (users)
            {
                foreach (var user in users.Where(kvp => kvp.Value == clientSocket).ToList())
                {
                    users.Remove(user.Key);
                }
            }

            clientSocket.Close();*/

        }

        static void ProcessMessages(RequestHandler handler, Dictionary<string, Socket> users)
        {
            while (true)
            {
                messageEvent.WaitOne(); // Wait for a signal that a new message is available

                while (messageQueue.TryDequeue(out Message message))
                {
                    Task.Run(() => handler.Handle(message, users));
                }
            }
        }
    }
}
