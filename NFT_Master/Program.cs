using System;
using System.Reflection;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;

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
            Log.identifier = Environment.MachineName;
            Log.showTimestamp = true;
            Log.info(version);

            //List<FileInfo> files = FileOps.discoverFiles(@"C:\Projects\NFT");

            //foreach (var file in files)
            //    Log.info(file.FullName);


            c.addFiles(@"C:\Projects\NFT");

            foreach (var file in c.files)
                Log.info(file);

            Log.warning("Hip hop de stoppy");

            c.shuffleFiles();

            foreach (var file in c.files)
                Log.info(file);

            Console.ReadLine();

            ////ErrorReporter.listen();

            //Slave.scan("10.40.39.130-140");//"192.168.0.1-100");

            //foreach (var slave in Slave.slaves)
            //    slave.send(c);

            //foreach (var slave in Slave.slaves)
            //    slave.disconnect();

            ////Console.ReadLine();

            //ErrorReporter.listen();

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
