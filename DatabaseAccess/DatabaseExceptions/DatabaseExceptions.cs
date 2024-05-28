
/**************************************************************************
 *                                                                        *
 *  File:        DatabaseExceptions.cs                                    *
 *  Copyright:   (c) 2024, Moloman Laurentiu-Ionut                        *
 *  E-mail:      laurentiu-ionut.moloman@student.tuiasi.ro                *
 *  Website:                                                              *
 *  Description: Exception class for the Persistance Layer                *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Exceptions
{
    /// <summary>
    /// Exception thrown when there is a database connection error.
    /// </summary>
    public class DatabaseConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DatabaseConnectionException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when there is a database disconnection error.
    /// </summary>
    public class DatabaseDisconnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDisconnectionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DatabaseDisconnectionException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception thrown when there is an error creating a table.
    /// </summary>
    public class CreateTableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTableException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CreateTableException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Exception thrown when there is an error registering a user.
    /// </summary>
    public class UserRegisterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegisterException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserRegisterException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Exception thrown when there is an error updating user information.
    /// </summary>
    public class UserUpdateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserUpdateException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Exception thrown when there is an error reading user information.
    /// </summary>
    public class UserReadInformationException : Exception
    {
        /// <summary>
        /// Gets the username associated with the error.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserReadInformationException"/> class with a specified error message and username.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="username">The username associated with the error.</param>
        public UserReadInformationException(string message, string username)
            : base(message)
        {
            Username = username;
        }
    }

    /// <summary>
    /// Exception thrown when a user is not found.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserNotFoundException(string message)
            : base(message) { }
    }

    /// <summary>
    /// Exception thrown when there is an error deleting a user.
    /// </summary>
    public class UserDeletionException : Exception
    {
        /// <summary>
        /// Gets the username associated with the error.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeletionException"/> class with a specified error message and username.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="username">The username associated with the error.</param>
        public UserDeletionException(string message, string username)
            : base(message)
        {
            Username = username;
        }
    }

    /// <summary>
    /// Exception thrown when there is an error during user login.
    /// </summary>
    public class LoginException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LoginException(string message)
            : base(message)
        {
        }
    }
}
