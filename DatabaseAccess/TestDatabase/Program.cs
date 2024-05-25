using System;
using System.Collections.Generic;
using Persistance;

/*
namespace DatabaseAccessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseAccess dbAccess = new DatabaseAccess();

            // Register a new user
            Dictionary<string, string> newUser = new Dictionary<string, string>
            {
                { "username", "john_doe" },
                { "password", "password123" },
                { "email", "john@example.com" },
                { "firstname", "John" },
                { "lastname", "Doe" },
                { "phone_number", "1234567890" }
            };

            bool isRegistered = dbAccess.RegisterUser(newUser);
            Console.WriteLine("Register User: " + (isRegistered ? "Success" : "Failed"));

            // Get user info
            List<string> fields = new List<string> { "username", "email", "firstname", "lastname" };
            Dictionary<string, string> userInfo = dbAccess.GetUserInfo("john_doe", fields);

            Console.WriteLine("User Info:");
            foreach (var field in userInfo)
            {
                Console.WriteLine($"{field.Key}: {field.Value}");
            }

            // Check login credentials
            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                { "username", "john_doe" },
                { "password", "password123" }
            };

            bool isAuthenticated = dbAccess.LoginCheck(credentials);
            Console.WriteLine("Login Check: " + (isAuthenticated ? "Success" : "Failed"));


            // Save logs
             string conversation = "Hello John! How are you?";
             bool isLogsSaved = dbAccess.SaveLogs("john_doe", "jane_doe", conversation);
             Console.WriteLine("Save Logs: " + (isLogsSaved ? "Success" : "Failed"));

             // Get logs
             string logs = dbAccess.GetLogs("john_doe", "jane_doe");
             Console.WriteLine("Logs: " + logs);
           
        }
    }
} */