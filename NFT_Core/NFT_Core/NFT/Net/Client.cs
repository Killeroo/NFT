using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using NFT.Core;
using NFT.Logger;

namespace NFT.Net
{
    // Stores all client operations and functions
    public class Client
    {
        public IPEndPoint EndPoint;
        public TcpClient Connection { get; private set; }
        public NetworkStream Stream { get; private set; }
        public bool IsConnected { get; private set; } = false;

        private int curSeqNum = 0; // Current command sequence number (Client independent)

        public Client(IPEndPoint ep)
        {
            Connection = new TcpClient();
            EndPoint = ep;
        }

        /// <summary>
        /// Send a command to connected Client
        /// </summary>
        public bool Send(Command c)
        {
            // Check if connected first
            if (Connection == null || !IsConnected)
                throw new Exception();

            curSeqNum++; // Increment sequence number

            // Send command
            CommandHandler.Send(c, Connection, curSeqNum);
        } // REMOVE
        public void Connect()
        {
            try
            {
                // Async connection attempt
                var result = Connection.BeginConnect(EndPoint.Address.ToString(), EndPoint.Port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));//FromSeconds(1)); // Set timeout 
                if (!success)
                    throw new SocketException();

                // Connected
                Connection.EndConnect(result);
                Log.Info("Connected to " + EndPoint.Address + " [NFT_Client]");

                Stream = Connection.GetStream();
                IsConnected = true;
                
                return true;
            }
            catch (SocketException)
            {
                // For cleaner scanning output
                //Log.error(new Error(e, "Could not connect to Client"));
            }
            catch (ObjectDisposedException)
            {
                Log.error(new Error(e, "Object failure"));
            }
            catch (Exception)
            {
                //Log.error(new Error(e, "Could not connect to Client"));
            }

            return false;
        } // REMOVE
        public void Disconnect()
        {
            // Construct command
            Command c = new Command();
            c.Type = CommandType.Quit;

            // Send quit command to Client
            Send(c);

            // Cleanup
            Connection.Close();
            Stream.Close();
            IsConnected = false;
        }
        public void Reconnect() { } // REMOVE

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
            Log.Info(ep.Address + ":" + ep.Port + " disconnected");
        }

        private bool TestConnection()
        {
            if (!Stream.DataAvailable)
                return false;
            else
                return true;
        } // MODIFY
    }
}