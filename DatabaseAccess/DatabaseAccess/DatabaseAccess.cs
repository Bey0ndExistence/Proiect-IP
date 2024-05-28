/**************************************************************************
 *                                                                        *
 *  File:        DatabaseAccess.cs                                        *
 *  Copyright:   (c) 2024, Moloman Laurentiu-Ionut                        *
 *  E-mail:      laurentiu-ionut.moloman@student.tuiasi.ro                *
 *  Website:                                                              *
 *  Description: Persistance Layer that manages the database.             *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/




using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Persistance.Exceptions;
using System.Security.Cryptography;
using System.Text;
using ZstdSharp;

namespace Persistance
{
    public class DatabaseAccess : IDatabaseAccess
    {
        private static readonly Lazy<DatabaseAccess> _instance = new Lazy<DatabaseAccess>(() => new DatabaseAccess());
        private string _connectionString = "server=localhost;uid=root;pwd=root;database=userdata";

        // Private constructor to prevent instantiation
        private DatabaseAccess() { }

        // Static method to get the singleton instance
        public static DatabaseAccess Instance => _instance.Value;

        /// <summary>
        /// Creates the userdata table if it does not exist.
        /// </summary>
        /// <returns>True if the table is created successfully, otherwise throws an exception.</returns>
        public bool CreateUserTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS userdata (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    username CHAR(50) NOT NULL UNIQUE,
                    password VARCHAR(100) NOT NULL,
                    email CHAR(50) NOT NULL UNIQUE,
                    firstname CHAR(50),
                    lastname CHAR(50),
                    phone_number CHAR(50) UNIQUE
                )";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(createTableQuery, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    return true;
                }
                catch (MySqlException)
                {
                    throw new CreateTableException("An error occurred while creating the userdata table");
                }
            }
        }

        /// <summary>
        /// Creates the userslogs table if it does not exist.
        /// </summary>
        /// <returns>True if the table is created successfully, otherwise throws an exception.</returns>
        public bool CreateUsersLogs()
        {
            string connectionString = "server=localhost;uid=root;pwd=root;database=userdata";
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS userslogs (
                    users_IDs CHAR(50) PRIMARY KEY,
                    conversation TEXT
                )";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(createTableQuery, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    return true;
                }
                catch (MySqlException)
                {
                    throw new CreateTableException("An error occurred while creating the userslogs table");
                }
            }
        }

        /// <summary>
        /// Connects to the database.
        /// </summary>
        /// <param name="connection">The MySQL connection object.</param>
        /// <returns>True if connection is successful, otherwise throws an exception.</returns>
        private bool ConnectDB(out MySqlConnection connection)
        {
            connection = new MySqlConnection(_connectionString);
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException)
            {
                throw new CreateTableException("An error occurred while connecting to userdata database");
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        /// <param name="connection">The MySQL connection object.</param>
        /// <returns>True if disconnection is successful, otherwise throws an exception.</returns>
        private bool DisconnectDB(MySqlConnection connection)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                throw new CreateTableException("An error occurred while disconnecting from userdata database");
            }
        }

        /// <summary>
        /// Hashes the password using SHA-256.
        /// </summary>
        /// <param name="password">The plain text password.</param>
        /// <returns>The hashed password as a hexadecimal string.</returns>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Registers a new user in the database.
        /// </summary>
        /// <param name="userDict">Dictionary containing user details.</param>
        /// <returns>True if the user is registered successfully, otherwise throws an exception.</returns>
        public bool RegisterUser(Dictionary<string, string> userDict)
        {
            User user = new User
            {
                Username = userDict["username"],
                Password = HashPassword(userDict["password"]),
                Email = userDict["email"],
                Firstname = userDict["firstname"],
                Lastname = userDict["lastname"],
                PhoneNumber = userDict["phone_number"]
            };

            MySqlConnection connection;

            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            MySqlTransaction transaction = connection.BeginTransaction();

            string query = "INSERT INTO userdata (username, password, email, firstname, lastname, phone_number) " +
                           "VALUES (@username, @password, @Email, @Firstname, @Lastname, @PhoneNumber)";

            using (MySqlCommand cmd = new MySqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Firstname", user.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", user.Lastname);
                cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

                try
                {
                    cmd.ExecuteNonQuery();
                    transaction.Commit(); // Commit the transaction if the query is successful
                }
                catch (MySqlException ex)
                {
                    transaction.Rollback(); // Rollback the transaction if an error occurs
                    throw new UserRegisterException("An error occurred while registering the user: " + ex.Message);
                }
                finally
                {
                    DisconnectDB(connection);
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes a user's information from the database.
        /// </summary>
        /// <param name="username">The username of the user to delete.</param>
        /// <returns>True if the user is deleted successfully, otherwise throws an exception.</returns>
        public bool DeleteUserInfo(string username)
        {
            string query = "DELETE FROM userdata WHERE username = @username";

            MySqlConnection connection;
            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new UserDeletionException("No rows were deleted for the user.", username);
                    }
                }
                catch (MySqlException)
                {
                    DisconnectDB(connection);
                    throw new UserDeletionException("An error occurred while deleting user information.", username);
                }
            }

            DisconnectDB(connection);
            return true;
        }

        /// <summary>
        /// Updates a user's information in the database.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="updatedFields">Dictionary containing the fields to update and their new values.</param>
        /// <returns>True if the user information is updated successfully, otherwise throws an exception.</returns>
        public bool UpdateUserInfo(string username, Dictionary<string, string> updatedFields)
        {
            StringBuilder setFields = new StringBuilder();
            foreach (var field in updatedFields)
            {
                setFields.Append($"{field.Key} = @{field.Key}, ");
            }
            setFields.Length -= 2; // Remove the last comma and space

            string query = $"UPDATE userdata SET {setFields} WHERE username = @username";

            MySqlConnection connection;
            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                foreach (var field in updatedFields)
                {
                    cmd.Parameters.AddWithValue($"@{field.Key}", field.Value);
                }

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new UserUpdateException("No rows were updated for the user " + username);
                    }
                }
                catch (MySqlException)
                {
                    DisconnectDB(connection);
                    throw new UserUpdateException("An error occurred while updating user information for the user " + username);
                }
            }

            DisconnectDB(connection);
            return true;
        }

        /// <summary>
        /// Retrieves user information from the database.
        /// </summary>
        /// <param name="username">The username of the user to retrieve information for.</param>
        /// <param name="fields">List of fields to retrieve.</param>
        /// <returns>A dictionary containing the user information.</returns>
        public Dictionary<string, string> GetUserInfo(string username, List<string> fields)
        {
            string selectFields = string.Join(", ", fields);
            string query = $"SELECT {selectFields} FROM userdata WHERE username = @username";

            MySqlConnection connection;
            ConnectDB(out connection);

            Dictionary<string, string> userInfo = new Dictionary<string, string>();

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foreach (var field in fields)
                            {
                                userInfo[field] = reader[field]?.ToString();
                            }
                        }
                        else
                        {
                            throw new UserNotFoundException($"No information found for the user '{username}'.");
                        }
                    }
                }
                catch (MySqlException)
                {
                    throw new UserReadInformationException("An error occurred while retrieving user information.", username);
                }
            }

            DisconnectDB(connection);
            return userInfo;
        }

        /// <summary>
        /// Retrieves conversation logs between two users.
        /// </summary>
        /// <param name="username1">The first username.</param>
        /// <param name="username2">The second username.</param>
        /// <returns>The conversation logs as a string.</returns>
        public string GetLogs(string username1, string username2)
        {
            MySqlConnection connection;
            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            int id1 = GetUserId(username1, connection);
            int id2 = GetUserId(username2, connection);

            if (id1 == -1 || id2 == -1)
            {
                DisconnectDB(connection);
                return null;
            }

            string userIds = id1 < id2 ? $"{id1}{id2}" : $"{id2}{id1}";
            string query = "SELECT conversation FROM userslogs WHERE users_IDs = @userIds";

            string conversation = null;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userIds", userIds);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        conversation = reader["conversation"].ToString();
                    }
                    else
                    {
                        reader.Close();
                        query = "INSERT INTO userslogs (users_IDs, conversation) VALUES (@userIds, '')";
                        using (MySqlCommand insertCmd = new MySqlCommand(query, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@userIds", userIds);
                            insertCmd.ExecuteNonQuery();
                        }
                        conversation = "";
                    }
                }
            }

            DisconnectDB(connection);

            return conversation;
        }

        /// <summary>
        /// Saves conversation logs between two users.
        /// </summary>
        /// <param name="username1">The first username.</param>
        /// <param name="username2">The second username.</param>
        /// <param name="conversation">The conversation logs to save.</param>
        /// <returns>True if the logs are saved successfully, otherwise returns false.</returns>
        public bool SaveLogs(string username1, string username2, string conversation)
        {
            MySqlConnection connection;
            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            int id1 = GetUserId(username1, connection);
            int id2 = GetUserId(username2, connection);

            if (id1 == -1 || id2 == -1)
            {
                DisconnectDB(connection);
                return false;
            }

            string userIds = id1 < id2 ? $"{id1},{id2}" : $"{id2},{id1}";
            string query = "INSERT INTO userslogs (users_IDs, conversation) VALUES (@userIds, @conversation) " +
                           "ON DUPLICATE KEY UPDATE conversation = @conversation";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userIds", userIds);
                cmd.Parameters.AddWithValue("@conversation", conversation);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException)
                {
                    DisconnectDB(connection);
                    return false;
                }
            }

            DisconnectDB(connection);

            return true;
        }

        /// <summary>
        /// Checks the login credentials of a user.
        /// </summary>
        /// <param name="credentials">Dictionary containing username and password.</param>
        /// <returns>True if the credentials are correct, otherwise throws an exception.</returns>
        public bool LoginCheck(Dictionary<string, string> credentials)
        {
            string username = credentials["username"];
            string password = credentials["password"];

            MySqlConnection connection;
            try
            {
                ConnectDB(out connection);
            }
            catch (DatabaseConnectionException ex)
            {
                throw new DatabaseConnectionException(ex.Message);
            }

            string query = "SELECT COUNT(*) FROM userdata WHERE username = @username AND password = @password";
            bool isAuthenticated = false;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", HashPassword(password));

                isAuthenticated = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }

            DisconnectDB(connection);

            if (isAuthenticated)
                return true;
            else
                throw new LoginException("Wrong credentials");
        }

        /// <summary>
        /// Retrieves the user ID for a given username.
        /// </summary>
        /// <param name="username">The username to retrieve the ID for.</param>
        /// <param name="connection">The MySQL connection object.</param>
        /// <returns>The user ID if found, otherwise returns -1.</returns>
        private int GetUserId(string username, MySqlConnection connection)
        {
            string query = "SELECT ID FROM userdata WHERE username = @username";
            int userId = -1;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["ID"]);
                    }
                }
            }

            return userId;
        }
    }

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
