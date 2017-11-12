using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using NFT.Logger;
using NFT.Core;

namespace NFT_Core.NFT.Net
{
    class Server
    {
        object syncLock = new Object(); // Locking object (prevents two threads from using the same code)
        List<Task> pendingConnections = new List<Task>(); // List of connections to be established
        List<TcpClient> connections = new List<TcpClient>(); // List of connected clients
        
        /// <summary>
        /// Listen for clients
        /// </summary>
        private async Task StartListener()
        {
            var listener = TcpListener.Create(Constants.COMMAND_LISTEN_PORT);
            listener.Start();

            // Listen for clients 
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var task = StartHandleConnectionAsync(client);

                if (task.IsFaulted)
                    task.Wait();
            }
        }
        /// <summary>
        /// Handle newly connected clients
        /// </summary>
        private async Task StartHandleConnectionAsync(TcpClient client)
        {
            // Handle client connection
            var connectionTask = HandleConnectionAsync(client);

            // Only add to other pending connection when list is free
            lock (syncLock)
                pendingConnections.Add(connectionTask);

            try
            {
                // Wait for task to finish
                await connectionTask;
            }
            catch (Exception ex)
            {
                Log.Error(new Error(ex, "Error handling client connection"));
            }
            finally
            {
                lock (syncLock)
                    pendingConnections.Remove(connectionTask);
            }
        } // Change to use NFT.Comms.Client?
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
