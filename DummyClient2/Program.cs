using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using MessageNamespace;

namespace DummyClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("127.0.0.1", 5678);

            Task listenTask = Task.Run(() => ListenForResponse(clientSocket));

            var msgList = new List<Message>
            {
                new Message(MessageType.ChatMsg, "Client2", "Client1", new Dictionary<string, string> { { "message", "Hello, CLient1!" } }),
                new Message(MessageType.ChatMsg, "Client2", "Client1", new Dictionary<string, string> { { "message", "Hello, Client1!" } }),
                /*new Message(MessageType.Register, "Client2", "server", new Dictionary<string, string> {{ "username", "username7" },
                  { "password", "pass" },
                  { "email", "username7" },
                  { "firstname", "Test" },
                  { "lastname", "User" },
                  { "phone_number", "username7" } }),
                new Message(MessageType.ChatMsg, "Client2", "Client2", new Dictionary<string, string> { { "message", "Hello, Server! Message 1" } }),
                new Message(MessageType.Login, "Client1", "server", new Dictionary<string, string>{ { "username", "Client1" }, { "password", "pass" } }),
                new Message(MessageType.Logout, "Client1", "sever", new Dictionary<string, string>{ })*/
            };

            foreach (var message in msgList)
            {
                Thread.Sleep(5000);
                string messageJson = Message.ToJson(message);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(messageJson);

                clientSocket.Send(messageBuffer);
                Console.WriteLine($"Sent: {messageJson}");

                Thread.Sleep(5000);
            }

            await listenTask;

            clientSocket.Close();
        }

        private static void ListenForResponse(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            try
            {
                while(true)
                {
                    int receivedBytes = clientSocket.Receive(buffer);
                    if (receivedBytes > 0)
                    {
                        string responseText = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                        Console.WriteLine($"Received: {responseText}");
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
