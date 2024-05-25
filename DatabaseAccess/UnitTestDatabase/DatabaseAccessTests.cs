using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Persistance;
using Persistance.Exceptions;
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
            DatabaseAccess.Instance.CreateUserTable();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        
        [TestMethod]
        public void TestRegisterUser()
        {
            try
            {
                var userDict = new Dictionary<string, string>
            {
                { "username", "mircea3" },
                { "password", "mircea3" },
                { "email", "mircea3@example.com" },
                { "firstname", "mircea3" },
                { "lastname", "mircea3" },
                { "phone_number", "mircea3" }
            };

                var result = DatabaseAccess.Instance.RegisterUser(userDict);
                Assert.IsTrue(result);
            }
            catch (DatabaseConnectionException ex)
            {
                Console.Write(ex.ToString());
            }
            catch(UserRegisterException ex)
            {
                Console.WriteLine(ex.ToString());
            }

          

            var userInfo = DatabaseAccess.Instance.GetUserInfo("mircea3", new List<string> { "username", "email" });
            Assert.IsNotNull(userInfo);
            Assert.AreEqual("mircea3", userInfo["username"]);
            Assert.AreEqual("mircea3@example.com", userInfo["email"]);
            

        }

        [TestMethod]
        public void TestLoginCheck()
        { 

            var credentials = new Dictionary<string, string>
            {
                { "username", "userToUpdate" },
                { "password", "password" }
            };

            var isAuthenticated = DatabaseAccess.Instance.LoginCheck(credentials);
            Assert.IsTrue(isAuthenticated);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            try
            { 
                    var deleteResult = DatabaseAccess.Instance.DeleteUserInfo("mircea6");
                    Assert.IsTrue(deleteResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [TestMethod]
        public void TestUpdateUserInfo()
        {
            try
            {
                // First, register a user to update
                var userDict = new Dictionary<string, string>
        {
            { "username", "userToUpdate" },
            { "password", "password" },
            { "email", "user@example.com" },
            { "firstname", "User" },
            { "lastname", "ToUpdate" },
            { "phone_number", "1234567890" }
        };

                var registerResult = DatabaseAccess.Instance.RegisterUser(userDict);
                Assert.IsTrue(registerResult);

                // Now update the user information
                var updatedFields = new Dictionary<string, string>
        {
            { "email", "updated@example.com" },
            { "firstname", "Updated" },
            { "lastname", "User" }
        };
                var updateResult = DatabaseAccess.Instance.UpdateUserInfo("userToUpdate", updatedFields);
                Assert.IsTrue(updateResult);

                // Retrieve the updated user information to verify
                var userInfo = DatabaseAccess.Instance.GetUserInfo("userToUpdate", new List<string> { "email", "firstname", "lastname" });
                Assert.IsNotNull(userInfo);
                Assert.AreEqual("updated@example.com", userInfo["email"]);
                Assert.AreEqual("Updated", userInfo["firstname"]);
                Assert.AreEqual("User", userInfo["lastname"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
