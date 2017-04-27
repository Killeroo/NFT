using System;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// For sending and listening for errors using UDP
/// </summary>
class ErrorReporter
{
    private const int UDP_LISTEN_PORT = 12300;
    private const int UDP_SEND_PORT = 12301;

    // Add error handling
    public static void listen()
    {
        UdpClient listener = new UdpClient(UDP_LISTEN_PORT);
        Log.info("Listening for slave errors on UDP port " + UDP_LISTEN_PORT + "...");

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, UDP_SEND_PORT);
            byte[] data = listener.Receive(ref remoteEP); // Listen for message
            if (data != null)
            {
                Error err = Helper.FromByteArray<Error>(data); // Serialize data to Error object
                Log.error(err);
            }
        }
    }
    public static void sendError(Error e, IPEndPoint destEP)
    {
        Log.info("Sending error to master at " + destEP.Address + ":" + UDP_LISTEN_PORT);
        destEP.Port = UDP_LISTEN_PORT;

        using (UdpClient client = new UdpClient(UDP_SEND_PORT))
        {
            byte[] data = Helper.ToByteArray<Error>(e);
            client.Send(data, data.Length + 1, (IPEndPoint)destEP);
        }
    }
}
