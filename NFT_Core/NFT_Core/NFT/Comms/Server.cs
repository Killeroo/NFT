﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using NFT.Logger;

namespace NFT_Core.NFT.Comms
{
    class Server
    {
        object syncLock = new Object(); // Locking object (prevents two threads from using the same code)
        List<Task> pendingConnections = new List<Task>(); // List of connections to be established
        List<Task> connections = new List<TcpClient>(); // List of connected clients

        private async Task StartHandleConnectionAsync(TcpClient client) { } // Change to use NFT.Comms.Client?
        private async Task HandleConnectionAsync(TcpClient client) { }
        private async Task HandleDisconnectAsync() { }

        private async Task Scan() { }
        private async Task Send(Command c) { }
        private async Task Broadcast() { }

        private async Task DisconnectAll() { }

        public void Start() { }
        public void Stop() { }

    }
}
