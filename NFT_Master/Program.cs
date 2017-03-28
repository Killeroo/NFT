using System;
using System.Net;
using System.Net.Sockets;

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
            c.sender = Helper.getLocalIPAddress();
            sock.Send(Command.serialize(c));

            Console.Read();

            sock.Send(Command.serialize(c));

            Console.Read();

            c.type = CommandType.Quit;

            // Cleanup
            Log.info("Command sent, exiting...");
            Console.Read();
            sock.Close();
        }
    }
}
