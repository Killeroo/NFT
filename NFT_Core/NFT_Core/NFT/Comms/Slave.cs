using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    public class Slave
    {
        public static List<Slave> ConnectedSlaves = new List<Slave>();

        public IPEndPoint EndPoint;
        public TcpClient Client { get; private set; }
        public NetworkStream Stream { get; private set; }
        public bool IsConnected { get; private set; } = false;

        private int curSeqNum = 0; // Current command sequence number (slave independent)

        public Slave(IPEndPoint ep)
        {
            Client = new TcpClient();
            EndPoint = ep;
            //connect();
        }

        /// <summary>
        /// Send a command to connected slave
        /// </summary>
        public void Send(Command c)
        {
            // Check if connected first
            if (Client == null || !IsConnected)
                throw new Exception();

            curSeqNum++; // Increment sequence number

            // Send command
            CommandHandler.Send(c, Client, curSeqNum);
        }
        public void Connect()
        {
            try
            {
                // Async connection attempt
                var result = Client.BeginConnect(EndPoint.Address.ToString(), EndPoint.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));//FromSeconds(1)); // Set timeout 
                if (!success)
                    throw new SocketException();

                // Connected
                Client.EndConnect(result);
                Log.Info("Connected to " + EndPoint.Address + " [NFT_Slave]");

                Stream = Client.GetStream();
                IsConnected = true;
            }
            catch (SocketException)
            {
                // For cleaner scanning output
                //Log.error(new Error(e, "Could not connect to slave"));
            }
            catch (ObjectDisposedException)
            {
                //Log.error(new Error(e, "Object failure"));
            }
            catch (Exception)
            {
                //Log.error(new Error(e, "Could not connect to slave"));
            }
        }
        public void Disconnect()
        {
            // Construct command
            Command c = new Command();
            c.Type = CommandType.Quit;

            // Send quit command to slave
            Send(c);

            // Cleanup
            Client.Close();
            Stream.Close();
            IsConnected = false;
        }
        public void Reconnect() { }

        /// <summary>
        /// Finds NFT slaves on specified address range
        /// </summary>
        public static void Scan(string range)
        {
            IPEndPoint scanEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), NFT.Core.Constants.COMMAND_LISTEN_PORT);
            List<Slave> foundSlaves = new List<Slave>();
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
            Log.Info("Scanning for NFT Slaves on range [" + range + "]");
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
                            Slave s = new Slave(scanEP);

                            // Attempt to connect
                            s.Connect();

                            // Add to list of connected slaves
                            if (s.IsConnected)
                            {
                                Slave.ConnectedSlaves.Add(s);
                                hostCount++;
                            }
                        }
                    }
                }
            }

            // Start listening to each connected slave in new thread
            foreach (var slave in Slave.ConnectedSlaves)
            {
                SlaveListener sl = new SlaveListener(slave);
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
                    Log.Error(new Error(e, "Cannot create listening thread for [" + slave.EndPoint.Address.ToString() + "]"));
                }
            }

            Log.Info("Scan complete. " + hostCount + " host(s) found");
        }
        /// <summary>
        /// Send command to all connected slaves
        /// </summary>
        public static void SendAll(Command c)
        {
            if (c == null)
                return;

            // Loop through connected slave list
            foreach (Slave slave in Slave.ConnectedSlaves)
                slave.Send(c);
        }
        /// <summary>
        /// Disconnect from all currently connected slaves
        /// </summary>
        public static void DisconnectAll()
        {
            foreach (Slave slave in Slave.ConnectedSlaves)
                slave.Disconnect();
        }

        private bool TestConnection()
        {
            if (!Stream.DataAvailable)
                return false;
            else
                return true;
        }
    }
}