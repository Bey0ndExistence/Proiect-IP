using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class DatabaseAccess
{
    private string _connectionString = "Server=localhost;Database=userdata;User ID=root;Password=root;";

    // Private method to connect to the database
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

    // Private method to disconnect from the database
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

    // Method to register a new user
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

    // Method to get user info
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

    // Method to get logs between two users
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
                    // If no entry exists, create one with an empty conversation
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

    // Method to save logs between two users
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

        string userIds = id1 < id2 ? $"{id1}{id2}" : $"{id2}{id1}";
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

    // Method to check login credentials
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

    // Helper method to get user ID by username
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

// User class
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string PhoneNumber { get; set; }
}
