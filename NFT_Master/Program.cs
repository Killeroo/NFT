using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NFT_Master
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arguments check
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
                Log.fatal("Please enter IP of NFT Slave");

            // Network setup
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(args[0]);
            IPEndPoint ep = new IPEndPoint(ip, 11000);
            Command c = new Command(CommandType.Info, "Aimee smells <3");

            // Connect to slave
            try
            {
                Log.info("Connecting to " + args[0] + "...");
                sock.Connect(ep);
                Log.info("Connection established");
            }
            catch (SocketException)
            {
                Log.fatal("Failed to connect (SocketException)");
            }

            // Send command
            c.sender = getLocalIPAddress();
            sock.Send(Command.serialize(c));

            // Cleanup
            Log.info("Command sent, exiting...");
            Console.Read();
            sock.Close();
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
        /// TEMP - Old Serialization methods
        /// </summary>

        //static NetworkStream serializeCommand(Command fi, Socket sock)
        //{
        //    // Create a stream for storing our serialized object
        //    NetworkStream netStream = new NetworkStream(sock);

        //    // Serialize the object onto the stream transport medium
        //    IFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(netStream, fi);

        //    // Clean up
        //    netStream.Close();

        //    // Return the network stream containing our serialized object
        //    return netStream;
        //}

        //static Command deserializeCommand(NetworkStream netStream)
        //{
        //    // Deseralize our object from the stream
        //    IFormatter formatter = new BinaryFormatter();
        //    Command fi = (Command)formatter.Deserialize(netStream);

        //    // Clean up
        //    netStream.Close();

        //    // Return the deserialized object
        //    return fi;
        //}
    }
}
