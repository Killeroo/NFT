using System;
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
        object syncLock = new Object(); // Locking object (stops two threads using same code)
        List<Task> pendingConns = new List<Task>(); // list of connections to be established
        List<TcpClient> conns = new List<TcpClient>(); // List of connected clients

        //private async Task StartListener()
        //{
        //    var tcpListener = TcpListener.Create(13000); // Start listening for clients on port 13000
        //    tcpListener.Start();
        //    Log.Info("Server started on port 13000");

        //    while (true) // Core Listening loop
        //    {
        //        var tcpClient = await tcpListener.AcceptTcpClientAsync(); // suspend while looking for client, resume from method caller
        //        Log.Info("A client has connected, retrieving info...");
        //        var task = StartHandleConnectionAsync(tcpClient); // Handle connected client in new task(thread)
        //        if (task.IsFaulted)
        //            task.Wait(); // If handling connection exceptioned wait for task to finish
        //    }
        //} // Core async server method

        private async Task StartHandleConnectionAsync(TcpClient tcpClient)
        {
            var connectionTask = HandleConnectionAsync(tcpClient);

            lock (syncLock) // Dont access list if its being accessed by other thread
                pendingConns.Add(connectionTask); // Add connection to list of pending client connections

            try
            {
                await connectionTask;
            }
            catch (Exception e)
            {
                Log.Warning(e.Message.ToString() + "\n" + e.StackTrace.ToString());
            }
            finally
            {
                lock (syncLock)
                    pendingConns.Remove(connectionTask); // Connected to client remove from pending list
            }
        } // Handle newly connected clients

        private async Task HandleConnectionAsync(TcpClient tcpClient)
        {
            await Task.Yield(); // Resume other threads here

            bool connected = true; // Is client connected
            string username = null;

            try
            {
                using (var networkStream = tcpClient.GetStream()) // Using stream from/to client
                {
                    // Add client to active connections 
                    lock (syncLock)
                        conns.Add(tcpClient);

                    /* Get Initial Client Data */


                    /* Message Listening Loop */
                    while (connected)
                    {

                    }

                    Log.Info("Disconnected from " + username + ".");
                }
            }
            catch (IOException)
            {
                Log.Warning("Client [" + username + "] unexpectedly disconnected.");
            }
            finally
            {
                tcpClient.Close(); // Close connection

                lock (syncLock)
                    conns.Remove(tcpClient); // Remove client from active connections
            }

            await Broadcast(username + " has disconnected."); // Tell all clients someone has disconnected

        } // Handle client connection


        private async Task Broadcast(string message, string username = "")
        {
            Byte[] buffer = new Byte[4096];
            StringBuilder builder = new StringBuilder();

            if (username != "")
                builder.Append("[" + username + "] " + message);
            else
                builder.Append(message);

            buffer = System.Text.Encoding.ASCII.GetBytes(builder.ToString()); // convert string to bytes to send

            foreach (var client in conns) // Send message to all clients
            {
                var stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        } // Broadcasts messages to all connected clients

    }
}
