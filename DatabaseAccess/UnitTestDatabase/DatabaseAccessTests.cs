using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Persistance;
using System.Collections.Generic;

namespace Persistance.Tests
{
    [TestClass]
    public class DatabaseAccessTests
    {
        private static MySqlConnection _connection;
        private static string _connectionString = "server=localhost;uid=root;pwd=root;database=userdata";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();

            // Create the userdata table
            DatabaseAccess.CreateUserTable();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            using (var cmd = new MySqlCommand("DELETE FROM userdata", _connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void TestRegisterUser()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "testuser" },
                { "password", "testpassword" },
                { "email", "testuser@example.com" },
                { "firstname", "Test" },
                { "lastname", "User" },
                { "phone_number", "1234567890" }
            };

            var result = DatabaseAccess.Instance.RegisterUser(userDict);
            Assert.IsTrue(result);

            var userInfo = DatabaseAccess.Instance.GetUserInfo("testuser", new List<string> { "username", "email" });
            Assert.IsNotNull(userInfo);
            Assert.AreEqual("testuser", userInfo["username"]);
            Assert.AreEqual("testuser@example.com", userInfo["email"]);
        }

        [TestMethod]
        public void TestLoginCheck()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "loginuser" },
                { "password", "loginpassword" },
                { "email", "loginuser@example.com" },
                { "firstname", "Login" },
                { "lastname", "User" },
                { "phone_number", "0987654321" }
            };

            DatabaseAccess.Instance.RegisterUser(userDict);

            var credentials = new Dictionary<string, string>
            {
                { "username", "loginuser" },
                { "password", "loginpassword" }
            };

            var isAuthenticated = DatabaseAccess.Instance.LoginCheck(credentials);
            Assert.IsTrue(isAuthenticated);

            credentials["password"] = "wrongpassword";
            isAuthenticated = DatabaseAccess.Instance.LoginCheck(credentials);
            Assert.IsFalse(isAuthenticated);
        }
        /*
        [TestMethod]
        public void TestSaveAndGetLogs()
        {
            var user1 = new Dictionary<string, string>
            {
                { "username", "user1" },
                { "password", "password1" },
                { "email", "user1@example.com" },
                { "firstname", "First" },
                { "lastname", "User" },
                { "phone_number", "1111111111" }
            };

            var user2 = new Dictionary<string, string>
            {
                { "username", "user2" },
                { "password", "password2" },
                { "email", "user2@example.com" },
                { "firstname", "Second" },
                { "lastname", "User" },
                { "phone_number", "2222222222" }
            };

            DatabaseAccess.Instance.RegisterUser(user1);
            DatabaseAccess.Instance.RegisterUser(user2);

            var conversation = "Hello, how are you?";
            var saveResult = DatabaseAccess.Instance.SaveLogs("user1", "user2", conversation);
            Assert.IsTrue(saveResult);

            var retrievedConversation = DatabaseAccess.Instance.GetLogs("user1", "user2");
            Assert.AreEqual(conversation, retrievedConversation);
        }
        */
    }
}
