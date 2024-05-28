/// <file>
/// <author>Andrei Zacordoneț</author>
/// <summary>
/// This file contains the definition of the IServer interface, which defines the methods that a server implementation will provide.
/// </summary>
/// </file>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ServerNamespace
{
    /// <summary>
    /// Interface for the server.
    /// </summary>
    /// <remarks>
    /// This interface defines the methods that a server implementation will provide.
    /// </remarks>
    interface IServer
    {
        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <remarks>
        /// This method initializes the server and starts listening for client connections.
        /// </remarks>
        void ServerStart();
    }
}
