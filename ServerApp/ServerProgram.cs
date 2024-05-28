/**************************************************************************
 *                                                                        *
 *  File:        ServerProgram.cs                                         *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description: It initializes and starts the server.                    *
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

using ServerNamespace;

namespace ServerApp
{
    /// <summary>
    /// The main class for the server application.
    /// </summary>
    class ServerProgram
    {
        /// <summary>
        /// The entry point of the server application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main(string[] args)
        {
            Server server = new Server(5678);
            server.ServerStart();
        }
    }
}
