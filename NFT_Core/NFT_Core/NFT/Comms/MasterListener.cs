using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    /// <summary>
    /// Class for recieving Command messages from NFT master application
    /// </summary>
    public class MasterListener
    {
        public bool isListening { get; private set; }
        public bool isConnected { get; private set; }
        public TcpClient master { get; private set; }

        private TcpListener listener;
        private IPEndPoint masterEP;
        private NetworkStream stream;
        private bool running;

        public MasterListener()
        {
            listener = new TcpListener(IPAddress.Parse(Helper.GetLocalIPAddress()), NFT.Core.Constants.COMMAND_LISTEN_PORT);
        }

        public void Start()
        {
            running = true;

            while (running)
            {
                ListeningLoop();
                CommandLoop();
            }
        }
        public void Stop()
        {
            Log.Info("Stopping CommandListener...");
            running = false;

            // Cleanup
            try
            {
                master.Close();
                stream.Close();
            }
            catch (NullReferenceException e)
            {
                Log.Error(new Error(e));
            }
        }

        /// <summary>
        /// Listens for TCP connection for NFT master
        /// </summary>
        private void ListeningLoop()
        {
            // Start Tcplistener
            listener.Start();
            Log.Info("Listening for NFT Master on " + Helper.GetLocalIPAddress() + ":" + NFT.Core.Constants.COMMAND_LISTEN_PORT + "...");
            isListening = true;

            // Listening loop
            while (running)
            {
                try
                {
                    master = listener.AcceptTcpClient();

                    if (master != null)
                    {
                        // Store NFT Master endpoint
                        masterEP = (IPEndPoint)master.Client.RemoteEndPoint;
                        stream = master.GetStream();
                        Log.Info(masterEP.Address.ToString() + ":" + masterEP.Port.ToString() + " [NFT Master] connected");
                        break; // Exit listening loop
                    }
                }
                catch (SocketException e)
                {
                    Log.Error(new Error(e, "Master connection attempt failed"));
                }
                catch (ObjectDisposedException e)
                {
                    Log.Error(new Error(e, "Object failure"));
                }
                catch (Exception e)
                {
                    Log.Error(new Error(e, "Master connection attempt failed"));
                }
            }

            // Cleanup
            listener.Stop();
            isListening = false;
        }
        /// <summary>
        /// Listens for Commands from NFT master
        /// </summary>
        private void CommandLoop()
        {
            Command c = new Command();
            isConnected = true;
            Log.Info("Listening to " + masterEP.Address.ToString() + "...");

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
                        Task.Run(() => CommandHandler.Handle(c, master));
                    }
                    catch (SerializationException e)
                    {
                        Log.Error(new Error(e, "Cannot parse master stream"));
                        isConnected = false;
                    }
                    catch (IOException e)
                    {
                        Log.Error(new Error(e, "Connection failure"));
                        isConnected = false;
                    }
                    catch (ObjectDisposedException e)
                    {
                        Log.Error(new Error(e, "Object failure"));
                        isConnected = false;
                    }
                    catch (Exception e)
                    {
                        Log.Error(new Error(e));
                        isConnected = false;
                    }

                    // Disconnect on quit flags
                    if (c.type == CommandType.Quit || isConnected == false)
                        break;
                }
            }

            // Clean up
            Log.Info(masterEP.Address + ":" + masterEP.Port + " disconnected");
            isConnected = false;
        }
        private void TestConnection() { }
    }
}
