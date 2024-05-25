using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Persistance.Exceptions
{
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message) { }

    }

    public class DatabaseDisconnectionException : Exception
    {
        public DatabaseDisconnectionException(string message) : base(message) { }
    }

    public class CreateTableException : Exception
    {
        public CreateTableException(string message)
            : base(message)
        {
        }
    }

    public class UserRegisterException : Exception
    {
        public UserRegisterException(string message)
            : base(message)
        {
        }
    }
    public class UserUpdateException: Exception
    {
        public UserUpdateException(string message): base(message)
        {

        }
    }
    public class UserReadInformationException : Exception
    {
        public string Username { get; }
        public UserReadInformationException(string message, string username)
            : base(message)
        {
            Username = username;
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) 
            : base(message) { }
    }

    public class UserDeletionException : Exception
    {
        public string Username { get; }
        public UserDeletionException(string message, string username)
            : base(message)
        {
            Username = username;
        }
    }

    public class LoginException : Exception
    {
        public LoginException(string message)
            : base(message)
        {
        }
    }
}

