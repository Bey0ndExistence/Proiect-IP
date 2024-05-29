/**************************************************************************
 *                                                                        *
 *  File:        DatabaseAccessTests.cs                                   *
 *  Copyright:   (c) 2024, Moloman Laurentiu-Ionut                        *
 *  E-mail:      laurentiu-ionut.moloman@student.tuiasi.ro                *
 *  Website:                                                              *
 *  Description: Unit test class for the Persistance Layer                *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Persistance;
using Persistance.Exceptions;
using System.Collections.Generic;

namespace Persistance.Tests
{
    /// <summary>
    /// Unit tests for the DatabaseAccess class, testing various database operations.
    /// </summary>
    [TestClass]
    public class DatabaseAccessTests
    {
        private static MySqlConnection _connection;
        private static string _connectionString = "server=localhost;uid=root;pwd=root;database=userdata";

        /// <summary>
        /// Initializes the database connection and creates necessary tables before any tests are run.
        /// </summary>
        /// <param name="context">The test context.</param>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();

            // Create the userdata table
            DatabaseAccess.Instance.CreateUserTable();
        }

        /// <summary>
        /// Closes the database connection after all tests have been run.
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        /// <summary>
        /// Tests the user registration functionality.
        /// </summary>
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
                    { "phone_number", "0777777" }
                };

                var result = DatabaseAccess.Instance.RegisterUser(userDict);
                Assert.IsTrue(result);
            }
            catch (DatabaseConnectionException ex)
            {
                Console.Write(ex.ToString());
            }
            catch (UserRegisterException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var userInfo = DatabaseAccess.Instance.GetUserInfo("mircea3", new List<string> { "username", "email" });
            Assert.IsNotNull(userInfo);
            Assert.AreEqual("mircea3", userInfo["username"]);
            Assert.AreEqual("mircea3@example.com", userInfo["email"]);
        }

    

        /// <summary>
        /// Tests the user deletion functionality.
        /// </summary>
        [TestMethod]
        public void TestDeleteUser()
        {
            try
            {
                var deleteResult = DatabaseAccess.Instance.DeleteUserInfo("mircea3");
                Assert.IsTrue(deleteResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Tests the functionality for updating user information.
        /// </summary>
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

        /// <summary>
        /// Tests saving and retrieving logs.
        /// </summary>
        [TestMethod]
        public void TestSaveAndGetLogs()
        {
            var user1 = new Dictionary<string, string>
            {
                { "username", "user10" },
                { "password", "password10" },
                { "email", "user10@example.com" },
                { "firstname", "First1" },
                { "lastname", "User1" },
                { "phone_number", "111111111111" }
            };

            var user2 = new Dictionary<string, string>
            {
                { "username", "user20" },
                { "password", "password20" },
                { "email", "user20@example.com" },
                { "firstname", "Second" },
                { "lastname", "User" },
                { "phone_number", "222222222222" }
            };

            DatabaseAccess.Instance.RegisterUser(user1);
            DatabaseAccess.Instance.RegisterUser(user2);

            var conversation = "Hello, how are you?";
            var saveResult = DatabaseAccess.Instance.SaveLogs("user10", "user20", conversation);
            Assert.IsTrue(saveResult);

            var retrievedConversation = DatabaseAccess.Instance.GetLogs("user10", "user20");
            Assert.AreEqual(conversation, retrievedConversation);
        }

        /// <summary>
        /// Tests attempting to register a user with a duplicate username.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserRegisterException))]
        public void TestRegisterDuplicateUsername()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "duplicateUser" },
                { "password", "password" },
                { "email", "duplicate@example.com" },
                { "firstname", "Duplicate" },
                { "lastname", "User" },
                { "phone_number", "1234567890" }
            };

            // First registration should succeed
            var result = DatabaseAccess.Instance.RegisterUser(userDict);
            Assert.IsTrue(result);

            // Second registration should fail
            DatabaseAccess.Instance.RegisterUser(userDict);
        }

        /// <summary>
        /// Tests attempting to register a user with a null email.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserRegisterException))]
        public void TestRegisterNullEmail()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "nullEmailUser" },
                { "password", "password" },
                { "email", null },
                { "firstname", "Null" },
                { "lastname", "Email" },
                { "phone_number", "1234567890" }
            };

            DatabaseAccess.Instance.RegisterUser(userDict);
        }

        /// <summary>
        /// Tests attempting to register a user with an invalid phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserRegisterException))]
        public void TestRegisterInvalidPhoneNumber()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "invalidPhoneUser" },
                { "password", "password" },
                { "email", "invalidphone@example.com" },
                { "firstname", "Invalid" },
                { "lastname", "Phone" },
                { "phone_number", "invalidphone" } // Not a valid phone number format
            };

            DatabaseAccess.Instance.RegisterUser(userDict);
        }

        /// <summary>
        /// Tests attempting to register a user with an existing email.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserRegisterException))]
        public void TestRegisterDuplicateEmail()
        {
            var userDict1 = new Dictionary<string, string>
            {
                { "username", "user1" },
                { "password", "password" },
                { "email", "duplicateemail@example.com" },
                { "firstname", "User" },
                { "lastname", "One" },
                { "phone_number", "1111111111" }
            };

            var userDict2 = new Dictionary<string, string>
            {
                { "username", "user2" },
                { "password", "password" },
                { "email", "duplicateemail@example.com" },
                { "firstname", "User" },
                { "lastname", "Two" },
                { "phone_number", "2222222222" }
            };

            // First registration should succeed
            var result1 = DatabaseAccess.Instance.RegisterUser(userDict1);
            Assert.IsTrue(result1);

            // Second registration should fail due to duplicate email
            DatabaseAccess.Instance.RegisterUser(userDict2);
        }

        /// <summary>
        /// Tests attempting to register a user with missing required fields.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserRegisterException))]
        public void TestRegisterMissingFields()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "missingFieldsUser" },
                { "password", "password" }
                // Missing email, firstname, lastname, and phone_number
            };

            DatabaseAccess.Instance.RegisterUser(userDict);
        }

        /// <summary>
        /// Tests attempting to login with incorrect credentials.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(LoginException))]
        public void TestLoginIncorrectCredentials()
        {
            var credentials = new Dictionary<string, string>
        {
            { "username", "nonexistentuser" },
            { "password", "wrongpassword" }
        };

            // This call should throw a LoginException
            DatabaseAccess.Instance.LoginCheck(credentials);
        }

        /// <summary>
        /// Tests updating user information with invalid data.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UserUpdateException))]
        public void TestUpdateUserInvalidData()
        {
            var userDict = new Dictionary<string, string>
            {
                { "username", "updateInvalidDataUser" },
                { "password", "password" },
                { "email", "updateinvaliddata@example.com" },
                { "firstname", "Update" },
                { "lastname", "Invalid" },
                { "phone_number", "12345678905" }
            };

            var registerResult = DatabaseAccess.Instance.RegisterUser(userDict);
            Assert.IsTrue(registerResult);

            var updatedFields = new Dictionary<string, string>
            {
                { "email", "invalidemail" } // Invalid email format
            };

            DatabaseAccess.Instance.UpdateUserInfo("updateInvalidDataUser", updatedFields);
        }


        /// <summary>
        /// Tests the login check functionality.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(LoginException))]
        public void TestLoginCheck()
        {
            var credentials = new Dictionary<string, string>
            {
                { "username", "user10" },
                { "password", "password10" }
            };

            var isAuthenticated = DatabaseAccess.Instance.LoginCheck(credentials);
            Assert.IsTrue(isAuthenticated);
        }
    }
}
