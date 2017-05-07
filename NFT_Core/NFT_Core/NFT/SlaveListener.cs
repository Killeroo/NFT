using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;

namespace NFT
{
    /// <summary>
    /// Listens for Command messages for an NFT_Slave application
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
                byte[] buffer = new byte[4096];
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
                    }
                    catch (IOException e)
                    {
                        Log.Error(new Error(e, "Connection failure"));
                    }
                    catch (ObjectDisposedException e)
                    {
                        Log.Error(new Error(e, "Object failure"));
                        running = false; // Exit when object becomes disposed (must be DC)
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
