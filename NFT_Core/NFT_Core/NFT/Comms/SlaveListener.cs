using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    /// <summary>
    /// Listens for Command messages from NFT_Slave application
    /// </summary>
    public class SlaveListener
    {
        private NetworkStream stream; 
        private IPEndPoint ep;
        private TcpClient client; // The specific TcpClient object we are listening too
        private bool running = false;

        public SlaveListener(Client c)
        {
            // Load required slave data
            client = c.ClientObj;
            stream = c.Stream;
            ep = c.EndPoint;
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
                        Task.Run(() => CommandHandler.Handle(c, client));
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
                    if (c.Type == CommandType.Quit)
                        break;
                }
            }

            // Clean up
            Log.Info(ep.Address + ":" + ep.Port + " disconnected");
        }
    }
}
