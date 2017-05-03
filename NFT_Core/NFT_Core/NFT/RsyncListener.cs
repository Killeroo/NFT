using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Listens for and handles Rsync synchronization commands 
/// </summary>
class SyncListener
{
    private const int UDP_LISTEN_PORT = 12450;
    private const int UDP_SEND_PORT = 12451;

    public static void listen()
    {
        UdpClient listener = new UdpClient(UDP_LISTEN_PORT);
        Log.info("Listening for rsync streams on " + Helper.GetLocalIPAddress() + ":" + UDP_LISTEN_PORT + "...");

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, UDP_SEND_PORT);
            byte[] data = listener.Receive(ref remoteEP); // Listen for message
            if (data != null)
            {
                RsyncStream rs = Helper.FromByteArray<RsyncStream>(data); // Serialize data to Error object
                Log.stream(rs);

                handleStream(rs); // **START IN NEW THREAD**
            }
        }
    }
    public static void send(RsyncStream rs, IPEndPoint destEP)
    {
        Log.info("Sending command to " + destEP.Address + ":" + UDP_LISTEN_PORT);
        destEP.Port = UDP_LISTEN_PORT;

        using (UdpClient client = new UdpClient(UDP_SEND_PORT))
        {
            byte[] data = Helper.ToByteArray<RsyncStream>(rs);
            Log.stream(rs);
            client.Send(data, data.Length, (IPEndPoint)destEP);
        }
    }
    private static void handleStream(RsyncStream rs)
    {
        switch (rs.type)
        {
            case StreamType.Signature:
                break;
            case StreamType.Delta:
                break;
            default:
                Log.error(new Error(new Exception(), "Could not recognise stream type"));
                break;
        }
    }
}
