using System;
using System.Net;
using System.Net.Sockets;

using NFT.Core;
using NFT.Logger;

namespace NFT.Comms
{

    /// <summary>
    /// For sending and listening for errors using UDP
    /// </summary>
    public class ErrorReporter
    {
        private const int UDP_LISTEN_PORT = 12300;
        private const int UDP_SEND_PORT = 12301;

        public static void Listen()
        {
            UdpClient listener = new UdpClient(UDP_LISTEN_PORT);
            Log.Info("Listening for slave errors on " + Helper.GetLocalIPAddress() + ":" + UDP_LISTEN_PORT + "...");

            while (true)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, UDP_SEND_PORT);
                    byte[] data = listener.Receive(ref remoteEP); // Listen for message
                    if (data != null)
                    {
                        Error err = Helper.FromByteArray<Error>(data); // Serialize data to Error object
                        Log.RemoteError(err);
                    }
                }
                catch (SocketException ex)
                {
                    Log.Error(new Error(ex, "Could not recieve UDP error message"));
                }
                catch (Exception ex)
                {
                    Log.Error(new Error(ex, "Could not recieve UDP error message"));
                }
            }
        }
        public static void SendError(Error e, IPEndPoint destEP)
        {
            Log.Info("Sending error to master at " + destEP.Address + ":" + UDP_LISTEN_PORT);
            destEP.Port = UDP_LISTEN_PORT;

            using (UdpClient client = new UdpClient(UDP_SEND_PORT))
            {
                try
                {
                    byte[] data = Helper.ToByteArray<Error>(e);
                    client.Send(data, data.Length, (IPEndPoint)destEP);
                }
                catch (SocketException ex)
                {
                    Log.Error(new Error(ex, "Could not send UDP error message"));
                }
                catch (Exception ex)
                {
                    Log.Error(new Error(ex, "Could not send UDP error message"));
                }
            }
        }
    }
}