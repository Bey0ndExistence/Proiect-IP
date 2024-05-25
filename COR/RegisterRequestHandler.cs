using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using MessageNamespace;
using Persistance;
using MySql.Data.MySqlClient;

namespace ServerRequestHandler
{
    public class RegisterRequestHandler : RequestHandler
    {
        public RegisterRequestHandler(RequestHandler next = null) : base(next) { }

        public override void Handle(Message message, Dictionary<string, Socket> users)
        {
            if (message.Type == MessageType.Register)
            {
                Console.WriteLine($"Register Handler: Handling register of {message.Sender}.\n client register data: {message.Body}");
                // resolve the request
                DatabaseAccess.CreateUserTable();
                Console.WriteLine(DatabaseAccess.Instance.RegisterUser(message.Body));
                //Console.WriteLine(RegisterUser2(message.Body));
            }
            else
            {
                _next.Handle(message, users);
            }
        }
        /*
        public bool RegisterUser2(Dictionary<string, string> userDict)
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
                catch (MySqlException e)
                {
                    Console.WriteLine(e.Message);
                    DisconnectDB(connection);
                    return false;
                }
            }

            DisconnectDB(connection);
            return true;
        }
        private bool ConnectDB(out MySqlConnection connection)
        {
            connection = new MySqlConnection("server=localhost;uid=root;pwd=root;database=userdata");
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
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
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        */
    }
}
