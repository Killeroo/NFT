using System;
using System.Reflection;
using System.Configuration;
using System.Net;
using System.Net.Sockets;

namespace NFT_Master
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add control c hanlder
            Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

            // Get version info
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = Assembly.GetExecutingAssembly().GetName().Name + " Version " + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

            // Local variable setup
            Command c = new Command();
            c.type = CommandType.Info;

            // Setup log
            Log.identifier = "master";
            Log.info(version);

            Slave.scan("192.168.0.1-100", Properties.Settings.Default.COMMAND_LISTEN_PORT);

            foreach (var slave in Slave.slaves)
                slave.send(c);

            foreach (var slave in Slave.slaves)
                slave.disconnect();

            Console.ReadLine();

            //TransferServer ts = new TransferServer();

            ////ts.displaySites();
            //ts.start();

            //Console.ReadLine();

            //ts.stop();

            //Console.ReadLine();

            //ServerManager iisManager = new ServerManager();
            //iisManager.Sites["NewSite"].Stop();
            ////iisManager.Sites.Add("NewSite", "E:\\", 80);
            ////iisManager.CommitChanges();

            //foreach (var site in iisManager.Sites)
            //{
            //    Console.WriteLine(site.Name);
            //    //Console.WriteLine(site.State);
            //}

            //Console.ReadLine();

            //Command c = new Command();
            //c.type = CommandType.Abort;
            //IPAddress ip = IPAddress.Parse(args[0]);
            //IPEndPoint ep = new IPEndPoint(ip, 11000);
            //Slave s = new Slave(ep);

            //Console.ReadLine();

            //s.sendCommand(c);

            //Console.ReadLine();

            //s.disconnect();

            //Console.ReadLine();

        }

        protected static void exitHandler(object sender, ConsoleCancelEventArgs args)
        {
            // Cancel termination
            args.Cancel = true;

            Console.WriteLine("Master stopped. Press any key to exit...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
