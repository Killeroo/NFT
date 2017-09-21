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
    public class Client
    {
        public IPEndPoint EndPoint;
        public TcpClient ClientObj { get; private set; }
        public NetworkStream Stream { get; private set; }
        public bool IsConnected { get; private set; } = false;

        private int _curSeqNum = 0; // Current command sequence number (Client independent)
        private TcpListener _listener;
        private IPEndPoint _masterEP;
        private bool _running;
        private bool _listening;

        public Client()
        {
            _listener = new TcpListener(IPAddress.Parse(Helper.GetLocalIPAddress()), Constants.COMMAND_LISTEN_PORT);
        }

        public Client(IPEndPoint ep)
            : this()
        {
            ClientObj = new TcpClient();
            EndPoint = ep;
            //connect();
        }

        /// <summary>
        /// Send a command to connected Client
        /// </summary>
        public void Send(Command c)
        {
            // Check if connected first
            if (ClientObj == null || !IsConnected)
                throw new Exception();

            _curSeqNum++; // Increment sequence number

            // Send command
            CommandHandler.Send(c, ClientObj, _curSeqNum);
        }
        public void Connect()
        {
            try
            {
                // Async connection attempt
                var result = ClientObj.BeginConnect(EndPoint.Address.ToString(), EndPoint.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));//FromSeconds(1)); // Set timeout 
                if (!success)
                    throw new SocketException();

                // Connected
                ClientObj.EndConnect(result);
                Log.Info("Connected to " + EndPoint.Address + " [NFT_Client]");

                Stream = ClientObj.GetStream();
                IsConnected = true;
            }
            catch (SocketException)
            {
                // For cleaner scanning output
                //Log.error(new Error(e, "Could not connect to Client"));
            }
            catch (ObjectDisposedException)
            {
                //Log.error(new Error(e, "Object failure"));
            }
            catch (Exception)
            {
                //Log.error(new Error(e, "Could not connect to Client"));
            }
        }
        public void Disconnect()
        {
            // Construct command
            Command c = new Command();
            c.Type = CommandType.Quit;

            // Send quit command to Client
            Send(c);

            // Cleanup
            ClientObj.Close();
            Stream.Close();
            IsConnected = false;
        }
        public void Reconnect() { }

        private bool TestConnection()
        {
            if (!Stream.DataAvailable)
                return false;
            else
                return true;
        }
        public void Start()
        {
            _running = true;

            while (_running)
            {
                ListeningLoop();
                CommandLoop();
            }
        }

        public void Stop()
        {
            Log.Info("Stopping Command_listener...");
            _running = false;
            _listening = false;
        }

        /// <summary>
        /// Listens for TCP connection for NFT master
        /// </summary>
        private void ListeningLoop()
        {
            // Start Tcp_listener
            _listener.Start();
            Log.Info("Listening for NFT Master on " + Helper.GetLocalIPAddress() + ":" + NFT.Core.Constants.COMMAND_LISTEN_PORT + "...");
            _listening = true;

            // Listening loop
            while (_running)
            {
                try
                {
                    ClientObj = _listener.AcceptTcpClient();

                    if (ClientObj != null)
                    {
                        // Store NFT Master endpoint
                        _masterEP = (IPEndPoint)ClientObj.Client.RemoteEndPoint;
                        Stream = ClientObj.GetStream();
                        Log.Info(_masterEP.Address.ToString() + ":" + _masterEP.Port.ToString() + " [NFT Master] connected");
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
            _listener.Stop();
            _listening = false;
        }

        /// <summary>
        /// Listens for Commands from NFT master
        /// </summary>
        private void CommandLoop()
        {
            Command c = new Command();
            IsConnected = true;
            Log.Info("Listening to " + _masterEP.Address.ToString() + "...");

            // Command recieving loop
            while (_running)
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
                            bytesRead = Stream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, bytesRead);
                        }
                        while (Stream.DataAvailable);

                        // Deserialize command 
                        c = Helper.FromMemoryStream<Command>(ms);

                        // Handle command
                        Task.Run(() => CommandHandler.Handle(c, ClientObj));
                    }
                    catch (SerializationException e)
                    {
                        Log.Error(new Error(e, "Cannot parse master stream"));
                        IsConnected = false;
                    }
                    catch (IOException e)
                    {
                        Log.Error(new Error(e, "Connection failure"));
                        IsConnected = false;
                    }
                    catch (ObjectDisposedException e)
                    {
                        Log.Error(new Error(e, "Object failure"));
                        IsConnected = false;
                    }
                    catch (Exception e)
                    {
                        Log.Error(new Error(e));
                        IsConnected = false;
                    }

                    // Disconnect on quit flags
                    if (c.Type == CommandType.Quit || IsConnected == false)
                        break;
                }
            }

            // Clean up
            Log.Info(_masterEP.Address + ":" + _masterEP.Port + " disconnected");
            IsConnected = false;
        }
    }
}