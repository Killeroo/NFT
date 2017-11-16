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
using System.Runtime.Serialization;

namespace NFT_Core.NFT.Net
{
    // Stores all server operations and functions
    public class Server
    {
        //https://stackoverflow.com/questions/21013751/what-is-the-async-await-equivalent-of-a-threadpool-server

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

            // Add to pending connections
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
        }
        private async Task HandleConnectionAsync(TcpClient client) 
        { 
            // Resume threads here
            await Task.Yield();

            // Client is connected
            NetworkStream stream = client.GetStream();
            string slaveName = GenerateSlaveName();
            bool connected = true;
            int seqNum = 1;
            
            // Add to active connections
            lock (syncLock)
                connections.Add(client);

            // Command listening loop
            while (connected)
            {
                Command c = new Command();
                int bytesRead = 0;
                var buffer = new byte[Constants.COMMAND_BUFFER_SIZE];

                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        // Method
                        //// Wait for data from client
                        //var buffer = new byte[Constants.COMMAND_BUFFER_SIZE];
                        //var bytesRecv = await stream.ReadAsync(buffer, 0, buffer.Length);

                        //// Convert stream to command
                        //c = Helper.FromByteArray<Command>(buffer);

                        do
                        {
                            // Recieve message/command from client
                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, bytesRead);

                        }
                        while (stream.DataAvailable);

                        // Deserialise & execute command in new thread
                        c = Helper.FromMemoryStream<Command>(ms);
                        Task.Run(() => CommandHandler.Handle(c, client));
                        
                    }
                    catch (SerializationException e)
                    {
                        Log.Error(new Error(e, "Cannot parse master stream"));
                        connected = false;
                    }
                    catch (IOException e)
                    {
                        Log.Error(new Error(e, "Connection failure"));
                        connected = false;
                    }
                    catch (ObjectDisposedException e)
                    {
                        Log.Error(new Error(e, "Object failure"));
                    }
                    catch (Exception e)
                    {
                        Log.Error(new Error(e));
                    }
                }

                if (c.Type == CommandType.Quit)
                    connected = false;


            }


        }
        private async Task HandleDisconnectAsync() { }

        // Scans for clients on a network range
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
                            //NFT.Net.Client c = new Client(scanEP);

                            //// Attempt to connect
                            //if c.Connect())
                            //    // Handle connection if connection is established
                            //    //HandleConnectionAsync(c);
                            //else 
                            //    continue;

                            //// Add to list of connected Clients
                            //if s.IsConnected)
                            //{
                            //    //ConnectedClients.Add(s);
                            //    //hostCount++;
                            //}
                        }
                    }
                }
            }

            Log.Info("Scan complete. " + hostCount + " host(s) found");
        }
        // Sends a command to a client
        private async Task Send(TcpClient client, Command c) 
        { 
            
        }
        // Attempts to connect to a client
        private async Task Connect(IPEndPoint ep)
        {
            TcpClient client = new TcpClient();
            try
            {
                // Async connection attempt
                var result = client.BeginConnect(ep.Address.ToString(), ep.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));//FromSeconds(1)); // Set timeout 
                if (!success)
                    throw new SocketException();

                // Connected
                Log.Info("Found NFT_Slave @ " + ep.Address);
            }
            catch (SocketException)
            {
                // For cleaner scanning output
                //Log.error(new Error(e, "Could not connect to Client"));
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(new Error(e, "Object failure"));
            }
            catch (Exception e)
            {
                Log.Error(new Error(e, "Could not connect to Client"));
            }
        }
        // Broadcasts a message to all connectect clients
        private async Task Broadcast() { }
        private async Task DisconnectAll() { }
        // Generate a unique id for client
        public string GenerateSlaveName()
        {
            string[] actions = new string[] {
                               "Raving", "Praying", "Cautious", "Fast", "Light",
                                "Slow", "Tired", "Warm", "Snazzy", "Soft", "Sharp",
                                "Sweet", "Strong", "Hard", "Dull", "Noisy", "Mute",
                                "Faint", "Light", "Shaggy", "Slippery", "Icy", "Young",
                                "Frantic", "Invisible", "Friendly", "Silly", "Tense",
                                "Cross", "Glow", "Gold", "Red", "Green", "Shiny"};

            string[] names = new string[] {
                               "Mako", "Turtle", "Alpha", "Beaver", "Pigglet",
                                "Axy", "Squid", "Troll", "Seal", "Ray", "Eel",
                                "Otter", "Bear", "Wolf", "Weasal", "Rabbit", "Snail",
                                "Cheater", "Lion", "Frog", "Cloud", "Nimbus", "Linx",
                                "Eagle", "Whale", "Bioid", "Pika", "Ozzy", "Slave", "Daemon",
                                "Cat", "Rhino", "Anteater", "Crab"};

            Random random = new Random();
            List<int> indices = new List<int>();
            // Get an array of 2 random ints 
            while (indices.Count < 2)
            {
                int index = random.Next(0, names.Length);
                if (indices.Count == 0 || !indices.Contains(index))
                    indices.Add(index);
            }
            // Use to get 2 random elements in name array
            string[] slaveName = new string[2];
            slaveName[0] = actions[indices[0]];
            slaveName[1] = names[indices[1]];

            // Return name
            return string.Concat(slaveName);
        }

        public void Start() { }
        public void Stop() { }

    }
}
