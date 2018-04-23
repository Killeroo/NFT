using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using NFT.Core;
using NFT.Logger;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NFT.Net
{
    // Stores all client operations and functions
    public class Client
    {

        public bool IsListening { get; private set; }
        public bool IsConnected { get; private set; }
        public TcpClient Master { get; private set; }

        private TcpListener listener;
        private IPEndPoint masterEP;
        private NetworkStream stream;
        private bool running;

        public Client() { }

        // Listens for a NFT Server/Master
        public void Listen()
        {
            // Start Tcplistener
            listener.Start();
            Log.Info("Listening for NFT Master on " + Helper.GetLocalIPAddress() + ":" + NFT.Core.Constants.COMMAND_LISTEN_PORT + "...");
            IsListening = true;

            // Listening loop
            while (running)
            {
                try
                {
                    Master = listener.AcceptTcpClient();

                    if (Master != null)
                    {
                        // Store NFT Master endpoint
                        masterEP = (IPEndPoint)Master.Client.RemoteEndPoint;
                        stream = Master.GetStream();
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
            IsListening = false;
        }
        // Recieves and executes commands from a server
        public void RecvCommands()
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
            //Log.Info(ep.Address + ":" + ep.Port + " disconnected");
        }

        private void TestConnection()
        {

        } // MODIFY
    }
}