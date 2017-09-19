using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

        private int curSeqNum = 0; // Current command sequence number (Client independent)

        public Client(IPEndPoint ep)
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

            curSeqNum++; // Increment sequence number

            // Send command
            CommandHandler.Send(c, ClientObj, curSeqNum);
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
    }
}