using System;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Helper class containing misc static methods
/// </summary>
public class Helper
{
    /// <summary>
    /// Gets internetwork IPv4 address of computer
    /// </summary>
    /// <returns></returns>
    public static String getLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        return "";
    }
}

/// <summary>
/// Socket class extension to poll if a socket is actively connected
/// </summary>
static class SocketExtensions
{
    public static bool IsConnected(this Socket socket)
    {
        try
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }
        catch (SocketException) { return false; }
    }
}