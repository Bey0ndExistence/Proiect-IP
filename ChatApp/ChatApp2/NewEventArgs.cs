using System;
using System.Collections.Generic;

namespace ChatApp
{
    public class UserCredentialsEventArgs : EventArgs
    {
        public string Username { get; }
        public string Password { get; }

        public UserCredentialsEventArgs(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
    public class UserRegisterDataEventArgs : EventArgs
    {
        public Dictionary<string, string> Data { get; }

        public UserRegisterDataEventArgs(string user, string pass, string email,
                                        string firstName, string lastName, string phone)
        {
            Data = new Dictionary<string, string> {
                  { "username", user },
                  { "password", pass },
                  { "email", email },
                  { "firstname", firstName },
                  { "lastname", lastName },
                  { "phone_number", phone } 
            };
        }
    }
}
