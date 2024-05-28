/// <file>
/// <author>Andrei Zacordoneț</author>
/// <summary>
/// This file contains the entry point for the server application.
/// It initializes and starts the server.
/// </summary>
/// </file>

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
