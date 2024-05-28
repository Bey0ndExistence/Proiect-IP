/**************************************************************************
 *                                                                        *
 *  File:        IServer.cs                                               *
 *  Copyright:   (c) 2024, Andrei Zacordoneț                              *
 *  Description: This file contains the definition of the IServer interface, 
 *  which defines the methods that a server implementation will provide.  *
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
