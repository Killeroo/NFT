using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using NFT.Core;
using NFT.Logger;

namespace NFT.Net
{
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
        }
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
        }
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
        public void Reconnect() { }

        private bool TestConnection()
        {
            if (!Stream.DataAvailable)
                return false;
            else
                return true;
        }
    }
}