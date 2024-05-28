using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageNamespace;

namespace Message
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageNamespace.Message message = new MessageNamespace.Message(MessageType.ChatMsg, "Alice", "Bob", "Hellor Bob!");

            string json = MessageNamespace.Message.ToJson(message);
            Console.WriteLine("Serialiced JSON:");
            Console.WriteLine(json);

            MessageNamespace.Message deserializedMessage = MessageNamespace.Message.FromJson(json);
            Console.WriteLine("\nDeserialized Message:");
            Console.WriteLine($"Type: {deserializedMessage.Type}");

        }
    }
}
