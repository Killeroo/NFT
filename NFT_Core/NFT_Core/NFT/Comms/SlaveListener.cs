﻿using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    /// <summary>
    /// Listens for Command messages from NFT_Slave application
    /// </summary>
    public class SlaveListener
    {
        private NetworkStream stream; // Need to be a reference?
        private IPEndPoint ep;
        private bool running = false;

        public SlaveListener(Slave s)
        {
            // Load required slave data
            stream = s.stream;
            ep = s.endPoint;
        }

        public void Start()
        {
            running = true;
            CommandLoop();
        }
        public void Stop()
        {
            running = false;
        }

        /// <summary>
        /// Listens for Commands from NFT master
        /// </summary>
        private void CommandLoop()
        {
            Command c = new Command();
            Log.Info("Listening to " + ep.Address.ToString() + "...");

            // Command recieving loop
            while (running)
            {
                byte[] buffer = new byte[NFT.Core.Constants.COMMAND_BUFFER_SIZE];
                using (MemoryStream ms = new MemoryStream())
                {
                    int bytesRead = 0;

                    try
                    {
                        do
                        {
                            // Read data from client stream
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, bytesRead);
                        }
                        while (stream.DataAvailable);

                        // Deserialize command 
                        c = Helper.FromMemoryStream<Command>(ms);

                        // Handle command
                        CommandHandler.Handle(c);
                    }
                    catch (SerializationException e)
                    {
                        Log.Error(new Error(e, "Cannot parse master stream"));
                        running = false;
                    }
                    catch (IOException e)
                    {
                        Log.Error(new Error(e, "Connection failure"));
                        running = false;
                    }
                    catch (ObjectDisposedException e)
                    {
                        Log.Error(new Error(e, "Object failure"));
                        running = false; 
                    }
                    catch (Exception e)
                    {
                        Log.Error(new Error(e));
                    }

                    // Disconnect on quit flags
                    if (c.type == CommandType.Quit)
                        break;
                }
            }

            // Clean up
            Log.Info(ep.Address + ":" + ep.Port + " disconnected");
        }
    }
}