using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using MessageNamespace;

namespace MessageModuleTest
{
    /// <summary>
    /// Unit tests for the Message class.
    /// </summary>
    [TestClass]
    public class MessageTest
    {
        /// <summary>
        /// Tests serialization and deserialization of a message with ChatMsg type.
        /// </summary>
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

        /// <summary>
        /// Tests serialization and deserialization of a message with Register type.
        /// </summary>
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

        /// <summary>
        /// Tests serialization and deserialization of a message with Logout type.
        /// </summary>
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
