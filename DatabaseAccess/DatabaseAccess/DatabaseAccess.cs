using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;

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

        // Static method to create the userdata table
        public static bool CreateUserTable()
        {
            string connectionString = "server=localhost;uid=root;pwd=root;database=userdata";
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS userdata (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    username CHAR(50) NOT NULL UNIQUE,
                    password CHAR(50) NOT NULL,
                    email CHAR(50) NOT NULL UNIQUE,
                    firstname CHAR(50) NOT NULL,
                    lastname CHAR(50) NOT NULL,
                    phone_number CHAR(50) NOT NULL UNIQUE
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
                catch (MySqlException ex)
                {
                    Console.WriteLine("An error occurred while creating the table: " + ex.Message);
                    return false;
                }
            }
        }

        // Existing methods and private helper methods...
        public static bool CreateUsersLogs()
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
                catch (MySqlException ex)
                {
                    Console.WriteLine("An error occurred while creating the table: " + ex.Message);
                    return false;
                }
            }
        }
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
                return false;
            }
        }

        private bool DisconnectDB(MySqlConnection connection)
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public bool RegisterUser(Dictionary<string, string> userDict)
        {
            User user = new User
            {
                Username = userDict["username"],
                Password = userDict["password"],
                Email = userDict["email"],
                Firstname = userDict["firstname"],
                Lastname = userDict["lastname"],
                PhoneNumber = userDict["phone_number"]
            };

            MySqlConnection connection;
            if (!ConnectDB(out connection))
            {
                return false;
            }

            string query = "INSERT INTO userdata (username, password, email, firstname, lastname, phone_number) " +
                           "VALUES (@username, @password, @Email, @Firstname, @Lastname, @PhoneNumber)";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
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

        public Dictionary<string, string> GetUserInfo(string username, List<string> fields)
        {
            string selectFields = string.Join(", ", fields);
            string query = $"SELECT {selectFields} FROM userdata WHERE username = @username";

            MySqlConnection connection;
            if (!ConnectDB(out connection))
            {
                return null;
            }

            Dictionary<string, string> userInfo = new Dictionary<string, string>();

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        foreach (var field in fields)
                        {
                            userInfo[field] = reader[field]?.ToString();
                        }
                    }
                }
            }

            DisconnectDB(connection);

            return userInfo;
        }

        public string GetLogs(string username1, string username2)
        {
            MySqlConnection connection;
            if (!ConnectDB(out connection))
            {
                return null;
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

        public bool SaveLogs(string username1, string username2, string conversation)
        {
            MySqlConnection connection;
            if (!ConnectDB(out connection))
            {
                return false;
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

        public bool LoginCheck(Dictionary<string, string> credentials)
        {
            string username = credentials["username"];
            string password = credentials["password"];

            MySqlConnection connection;
            if (!ConnectDB(out connection))
            {
                return false;
            }

            string query = "SELECT COUNT(*) FROM userdata WHERE username = @username AND password = @password";
            bool isAuthenticated = false;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                isAuthenticated = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }

            DisconnectDB(connection);

            return isAuthenticated;
        }

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
