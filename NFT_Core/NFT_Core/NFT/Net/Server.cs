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
using NFT.Net;

namespace NFT_Core.NFT.Net
{
    class Server
    {
        object syncLock = new Object(); // Locking object (prevents two threads from using the same code)
        List<Task> pendingConnections = new List<Task>(); // List of connections to be established
        List<Client> connections = new List<Client>(); // List of connected clients
        
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
        private async Task StartHandleConnectionAsync(NFT_Core.Net.Client client)
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
                // Remove from pending connections once we are done
                lock (syncLock)
                    pendingConnections.Remove(connectionTask);
            }
        } // Change to use NFT.Comms.Client?
        private async Task HandleConnectionAsync(NFT.Net.Client client) { }
        private async Task HandleDisconnectAsync() { }

        private async Task Scan(string range)
        {
            IPEndPoint scanEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Constants.COMMAND_LISTEN_PORT);
            List<Client> foundClients = new List<Client>();
            bool rangeFound = false;
            int hostCount = 0;

            // Split ip address into segments
            string[] addressSegs = range.Split('.');

            // Check range is specified
            foreach (var seg in addressSegs)
                if (seg.Contains("-"))
                    rangeFound = true;

            // Exit if we havent found a range
            if (!rangeFound)
            {
                Log.Error(new Error(new Exception(), "Scan range not specified"));
                return;
            }

            // Holds the ranges for each IP segment
            int[] segLower = new int[4];
            int[] segUpper = new int[4];

            // Work out upper and lower ranges for each segment
            for (int x = 0; x < 4; x++)
            {
                string[] ranges = addressSegs[x].Split('-');
                segLower[x] = Convert.ToInt16(ranges[0]);
                segUpper[x] = (ranges.Length == 1) ? segLower[x] : Convert.ToInt16(ranges[1]);
            }

            // Loop through each possible address
            Log.Info("Scanning for NFT Clients on range [" + range + "]");
            for (int seg1 = segLower[0]; seg1 <= segUpper[0]; seg1++)
            {
                for (int seg2 = segLower[1]; seg2 <= segUpper[1]; seg2++)
                {
                    for (int seg3 = segLower[2]; seg3 <= segUpper[2]; seg3++)
                    {
                        for (int seg4 = segLower[3]; seg4 <= segUpper[3]; seg4++)
                        {
                            // Update with current address to try and connect to
                            scanEP.Address = new IPAddress(new byte[] { (byte)seg1, (byte)seg2, (byte)seg3, (byte)seg4 });
                            NFT.Net.Client c = new Client(scanEP);

                            // Attempt to connect
                            if (c.Connect())
                                // Handle connection if connection is established
                                HandleConnectionAsync(c);
                            else 
                                continue;

                            // Add to list of connected Clients
                            if (s.IsConnected)
                            {
                                //ConnectedClients.Add(s);
                                //hostCount++;
                            }
                        }
                    }
                }
            }

            Log.Info("Scan complete. " + hostCount + " host(s) found");
        }
        private async Task Send(Command c) { }
        private async Task Broadcast() { }

        private async Task DisconnectAll() { }

        public void Start() { }
        public void Stop() { }

    }
}
