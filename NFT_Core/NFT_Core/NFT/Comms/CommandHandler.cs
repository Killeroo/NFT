using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{
    /// <summary>
    /// Static class for sending and handling Command messages
    /// </summary>
    public class CommandHandler
    {
        public static void Handle(Command c)
        {
            switch (c.type)
            {
                case CommandType.RsyncStream:

                    // Display
                    Log.Command(c);
                    Log.Stream(c.stream);
                    
                    // Generate delta

                    // Send delta back to source


                    break;

                default:
                    Log.Command(c);
                    break;
            }
        }
        public static void Send(Command c, TcpClient client, int seqnum)
        {
            try
            {
                // Check if connected first
                if (client == null)
                    throw new Exception();

                byte[] buffer = new byte[4096];
                c.seq = seqnum;
                c.destination = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]); // Set destination of command
                buffer = Helper.ToByteArray<Command>(c);
                Log.Command(c);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
            catch (IOException e)
            {
                Log.Error(new Error(e, "Failed to send command"));
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(new Error(e, "Object failure"));
            }
            catch (Exception e)
            {
                Log.Error(new Error(e, "Could not send command"));
            }
        }
    }
}