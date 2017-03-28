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