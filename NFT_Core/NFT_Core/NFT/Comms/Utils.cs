using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    public static class Utils
    {
        public static List<Client> ConnectedClients { get; set; }
        /// <summary>
        /// Finds NFT Clients on specified address range
        /// </summary>
        public static void Scan(string range)
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
                            Client s = new Client(scanEP);

                            // Attempt to connect
                            s.Connect();

                            // Add to list of connected Clients
                            if (s.IsConnected)
                            {
                                ConnectedClients.Add(s);
                                hostCount++;
                            }
                        }
                    }
                }
            }

            // Start listening to each connected Client in new thread
            foreach (var client in ConnectedClients)
            {
                SlaveListener sl = new SlaveListener(client);
                Thread listeningThread = new Thread(new ThreadStart(sl.Start));
                listeningThread.IsBackground = true; // Stop threading hanging program

                try
                {
                    // Attempt to start listening thread
                    listeningThread.Start();
                }
                catch (OutOfMemoryException)
                {
                    // Exit program if we are out of memory
                    Log.Fatal("Ran out of memory");
                }
                catch (Exception e)
                {
                    // Otherwise throw error
                    Log.Error(new Error(e, "Cannot create listening thread for [" + client.EndPoint.Address.ToString() + "]"));
                }
            }

            Log.Info("Scan complete. " + hostCount + " host(s) found");
        }

        /// <summary>
        /// Send command to all connected Clients
        /// </summary>
        public static void SendAll(Command c)
        {
            if (c == null)
                return;

            // Loop through connected Client list
            foreach (Client Client in ConnectedClients)
                Client.Send(c);
        }
        /// <summary>
        /// Disconnect from all currently connected Clients
        /// </summary>
        public static void DisconnectAll()
        {
            foreach (Client Client in ConnectedClients)
                Client.Disconnect();
        }
    }
}
