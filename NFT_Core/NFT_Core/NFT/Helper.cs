using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NFT
{

    /// <summary>
    /// Helper class containing misc static methods
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Gets IPv4 address of computer
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            return "";
        }

        /// <summary>
        /// Conversion methods
        /// </summary>
        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        public static T FromMemoryStream<T>(MemoryStream ms)
        {
            if (ms == null)
                return default(T);

            BinaryFormatter bf = new BinaryFormatter();
            ms.Seek(0, SeekOrigin.Begin);
            Object obj = bf.Deserialize(ms);
            ms.Close();
            return (T)obj;
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
}