/**************************************************************************
 *                                                                        *
 *  File:        IDatabaseAccess.cs                                       *
 *  Copyright:   (c) 2024, Moloman Laurentiu-Ionut                        *
 *  E-mail:      laurentiu-ionut.moloman@student.tuiasi.ro                *
 *  Website:                                                              *
 *  Description: Interface for the DatabaseAccess class.                  *
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

namespace Persistance
{
    /// <summary>
    /// Interface for database access operations.
    /// </summary>
    public interface IDatabaseAccess
    {
        /// <summary>
        /// Registers a new user in the database.
        /// </summary>
        /// <param name="userDict">A dictionary containing user details.</param>
        /// <returns>True if registration is successful, otherwise false.</returns>
        bool RegisterUser(Dictionary<string, string> userDict);

        /// <summary>
        /// Retrieves user information from the database.
        /// </summary>
        /// <param name="username">The username of the user to retrieve information for.</param>
        /// <param name="fields">A list of fields to retrieve for the user.</param>
        /// <returns>A dictionary containing the requested user information.</returns>
        Dictionary<string, string> GetUserInfo(string username, List<string> fields);

        /// <summary>
        /// Retrieves conversation logs between two users.
        /// </summary>
        /// <param name="username1">The first username in the conversation.</param>
        /// <param name="username2">The second username in the conversation.</param>
        /// <returns>The conversation logs as a string.</returns>
        string GetLogs(string username1, string username2);

        /// <summary>
        /// Saves conversation logs between two users.
        /// </summary>
        /// <param name="username1">The first username in the conversation.</param>
        /// <param name="username2">The second username in the conversation.</param>
        /// <param name="conversation">The conversation logs to save.</param>
        /// <returns>True if saving is successful, otherwise false.</returns>
        bool SaveLogs(string username1, string username2, string conversation);

        /// <summary>
        /// Checks user login credentials.
        /// </summary>
        /// <param name="credentials">A dictionary containing the login credentials.</param>
        /// <returns>True if login is successful, otherwise false.</returns>
        bool LoginCheck(Dictionary<string, string> credentials);
    }
}
