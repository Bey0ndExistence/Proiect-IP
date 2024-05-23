using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MessageNamespace
{
    public enum MessageType
    {
        Register = 0,
        RegisterResponse,
        Login,
        UpdateOnlineUsers,
        ErrorLogin,
        ChatMsg,
        Logout
    }

    public class Message
    {
        public MessageType Type { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public Dictionary<string, string> Body { get; set; }

        public Message(MessageType type, string sender, string receiver, Dictionary<string, string> body)
        {
            Type = type;
            Sender = sender;
            Receiver = receiver;
            Body = body;
        }

        public static string ToJson(Message message)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(message, options);
        }

        public static Message FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            return JsonSerializer.Deserialize<Message>(json, options);
        }
    }
}
