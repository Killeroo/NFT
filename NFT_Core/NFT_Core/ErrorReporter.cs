using System;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// For sending and listening for errors using UDP
/// </summary>
class ErrorReporter
{
    private const int UDP_LISTEN_PORT = 12000;
    private const int UDP_SEND_PORT = 12001;

    public static void listen()
    {
        UdpClient listener = new UdpClient(UDP_LISTEN_PORT);

        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, UDP_SEND_PORT);
            byte[] data = listener.Receive(ref remoteEP);
        }
    }
    public static void sendError(Error e, EndPoint destEP)
    {
        using (UdpClient client = new UdpClient(UDP_SEND_PORT))
        {
            byte[] data = Helper.ToByteArray<Error>(e);
            client.Send(data, data.Length + 1, (IPEndPoint)destEP);
        }
    }
}
