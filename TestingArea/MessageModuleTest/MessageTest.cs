using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using MessageNamespace;

namespace MessageModuleTest
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void TestSerialization()
        {
            // Arrange
            var body = new Dictionary<string, string> { { "body", "Hello Dl Polaru!" } };
            var message = new Message(MessageType.ChatMsg, "Andrei", "Dl Polaru", body);

            // Act
            string json = Message.ToJson(message);
            Message deserializedMessage = Message.FromJson(json);

            // Assert
            Assert.AreEqual(message.Type, deserializedMessage.Type);
            Assert.AreEqual(message.Sender, deserializedMessage.Sender);
            Assert.AreEqual(message.Receiver, deserializedMessage.Receiver);
            CollectionAssert.AreEqual(message.Body, deserializedMessage.Body);
        }

        [TestMethod]
        public void TestSerializationWithRegisterType()
        {
            // Arrange
            var body = new Dictionary<string, string> { { "username", "Polaru" }, { "password", "123Dl1223Polaru!" } };
            var message = new Message(MessageType.Register, "Eusebiu", "Server", body);

            // Act
            string json = Message.ToJson(message);
            var deserializedMessage = Message.FromJson(json);

            // Assert
            Assert.AreEqual(message.Type, deserializedMessage.Type);
            Assert.AreEqual(message.Sender, deserializedMessage.Sender);
            Assert.AreEqual(message.Receiver, deserializedMessage.Receiver);
            CollectionAssert.AreEqual(message.Body, deserializedMessage.Body);
        }

        [TestMethod]
        public void TestSerializationWithLogoutType()
        {
            // Arrange
            var body = new Dictionary<string, string> { { "body", "Hello Dl Polaru!" } };
            var message = new Message(MessageType.Logout, "DaveSapun", "Server", body);

            // Act
            string json = Message.ToJson(message);
            var deserializedMessage = Message.FromJson(json);

            // Assert
            Assert.AreEqual(message.Type, deserializedMessage.Type);
            Assert.AreEqual(message.Sender, deserializedMessage.Sender);
            Assert.AreEqual(message.Receiver, deserializedMessage.Receiver);
            CollectionAssert.AreEqual(message.Body, deserializedMessage.Body);
        }
    }
}
