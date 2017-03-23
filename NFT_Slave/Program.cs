using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace NFT_Slave
{
    class Program
    {
        static void Main(string[] args)
        {
            Command c = new Command();

            // Create listener
            TcpListener listener = new TcpListener(IPAddress.Parse(getLocalIPAddress()), 11000);
            listener.Start();
            Log.info("TcpListener started on " + getLocalIPAddress() + ":11000...");

            // Listening loop
            try
            {
                while (true)
                {
                    using (var client = listener.AcceptTcpClient())
                    {
                        Log.info(client.Client.RemoteEndPoint.ToString() + " connected");
                        using (var stream = client.GetStream())
                        {
                            c = Command.deserialize(stream);
                            Log.command(c); // Display command
                            break;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                Log.fatal("Connection error occured (SocketException)");
            }
            catch (SerializationException)
            {
                Log.fatal("Error parsing client stream (SerializationException)");
            }
            finally
            {
                Log.info("Master disconnected");
            }

            Console.Read();
        }

        static String getLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            return "";
        }
    }
}
