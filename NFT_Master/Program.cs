﻿using System;
using System.Reflection;
using System.Threading;

using NFT.Core;
using NFT.Comms;
using NFT.Rsync;
using NFT.Logger;

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

            // Setup log
            Log.identifier = Environment.MachineName;
            Log.showTimestamp = true;
            Log.Info(version);

            // Setup error listener
            Thread errorThread = new Thread(new ThreadStart(ErrorReporter.Listen));
            //errorThread.Start();

            //Slave.Scan(args[0]);

            // Start listening to each connected slave
            foreach (var slave in Slave.slaves)
            {
                SlaveListener sl = new SlaveListener(slave);
                Thread listeningThread = new Thread(new ThreadStart(sl.Start));
                listeningThread.Start();
            }

            FileOps.FetchFile("http://localhost/kali.iso", @"C:\");

            //c.type = CommandType.Info;
            //c.message = "Hello World!";
            //Slave.SendAll(new Command(CommandType.Info));

            // Generate and store sig
            //var stream = RsyncOps.GenerateSignature(@"C:\Projects\NFT\NFT_Master\bin\Debug\NFT_Core.dll");
            //RsyncStream rs = new RsyncStream(StreamType.Signature, stream, @"C:\NFT\NFT_Master\bin\Debug\NFT_Core.dll");
            //Log.Stream(rs);
            //c.addStream(rs);
            //Slave.SendAll(c);

            //var peam = RsyncOps.GenerateSignature(@"C:\Projects\kali.iso");
            //RsyncStream ds = new RsyncStream(StreamType.Signature, stream, @"C:\Projects\kali.iso");
            //Log.Stream(ds);
            //c.addStream(rs);
            //Slave.SendAll(c);

            Console.ReadLine();

            foreach (var slave in Slave.slaves)
                slave.Disconnect();

            Console.ReadLine();

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
