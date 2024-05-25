using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using ServerRequestHandler;
using MessageNamespace;

namespace CORModuleTest
{
    [TestClass]
    public class UnitTest1
    {
        private RegisterRequestHandler _registerHandler;
        private LogInRequestHandler _loginHandler;
        private MessageRequestHandler _chatMsgHandler;
        private LogOutRequestHandler _logoutHandler;

        [TestInitialize]
        public void Setup()
        {
            // Create handlers
            _chatMsgHandler = new MessageRequestHandler();
            _loginHandler = new LogInRequestHandler();
            _registerHandler = new RegisterRequestHandler();
            _logoutHandler = new LogOutRequestHandler();

            // Set up the chain of responsibility
            _chatMsgHandler.SetNext(_loginHandler);
            _loginHandler.SetNext(_registerHandler);
            _registerHandler.SetNext(_logoutHandler);
        }

        [TestMethod]
        public void Handle_RegisterMessage_ShouldHandleByRegisterHandler()
        {
            // Arrange
            var registerMessage = new Message(MessageType.Register, "User1", null,
                new Dictionary<string, string> { { "username", "User1" }, { "password", "password123" } });

            // Act
            var consoleOutput = CaptureConsoleOutput(() => _chatMsgHandler.Handle(registerMessage));

            // Assert
            Assert.IsTrue(consoleOutput.Contains("Handling register of User1.\n client register data:"));
        }

        [TestMethod]
        public void Handle_LoginMessage_ShouldHandleByLoginHandler()
        {
            // Arrange
            var loginMessage = new Message(MessageType.Login, "User1", "User2",
                new Dictionary<string, string> { { "username", "User1" }, { "password", "password123" } });

            // Act
            var consoleOutput = CaptureConsoleOutput(() => _chatMsgHandler.Handle(loginMessage));
            Console.WriteLine(consoleOutput);

            // Assert
            Assert.IsTrue(consoleOutput.Contains($"Login Request Handle: Handling log in of {loginMessage.Sender} client to {loginMessage.Receiver}"));
        }

        [TestMethod]
        public void Handle_ChatMessage_ShouldHandleByChatHandler()
        {
            // Arrange
            var chatMessage = new Message(MessageType.ChatMsg, "User1", "User2",
                new Dictionary<string, string> { { "message", "Hello, User2!" } });

            // Act
            var consoleOutput = CaptureConsoleOutput(() => _chatMsgHandler.Handle(chatMessage));
            Console.WriteLine(consoleOutput);

            // Assert
            Assert.IsTrue(consoleOutput.Contains($"Chat Message Handler: Handling chat msg from {chatMessage.Sender} to {chatMessage.Receiver}: {chatMessage.Body}"));
        }

        [TestMethod]
        public void Handle_Logout_ShouldHandleByLogoutHandler()
        {
            // Arrange
            var logoutMessage = new Message(MessageType.Logout, "User1", "server", null);

            // Act
            var consoleOutput = CaptureConsoleOutput(() => _chatMsgHandler.Handle(logoutMessage));
            Console.WriteLine(consoleOutput);

            // Assert
            Assert.IsTrue(consoleOutput.Contains($"Logout Request handler: Handling logout of {logoutMessage.Sender}"));
        }

        // Helper method to capture console output
        private string CaptureConsoleOutput(Action action)
        {
            var originalConsoleOut = Console.Out;
            using (var consoleOutput = new System.IO.StringWriter())
            {
                Console.SetOut(consoleOutput);
                action();
                Console.SetOut(originalConsoleOut);
                Console.WriteLine(consoleOutput.ToString());
                return consoleOutput.ToString();
            }
        }
    }
}
