using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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
                            c = deserializeCommand(stream);//Command.deserialize(stream);
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

            // Display command
            Log.command(c);
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



        /// <summary>
        /// TEMP - Add to command.cs
        /// </summary>

        static NetworkStream serializeCommand(Command fi, Socket sock)
        {
            // Create a stream for storing our serialized object
            NetworkStream netStream = new NetworkStream(sock);

            // Serialize the object onto the stream transport medium
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(netStream, fi);

            // Clean up
            netStream.Close();

            // Return the network stream containing our serialized object
            return netStream;
        }

        static Command deserializeCommand(NetworkStream netStream)
        {
            // Deseralize our object from the stream
            IFormatter formatter = new BinaryFormatter();
            Command fi = (Command)formatter.Deserialize(netStream);

            // Clean up
            netStream.Close();

            // Return the deserialized object
            return fi;
        }
    }
}
