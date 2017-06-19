using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using NFT.Core;
using NFT.Comms;
using NFT.Rsync;
using NFT.Logger;

namespace NFT_Master
{
    class Program
    {
        [STAThread]
        static void Main()//string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //// Add control c hanlder
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

            //// Get version info
            //Version v = Assembly.GetExecutingAssembly().GetName().Version;
            //string version = Assembly.GetExecutingAssembly().GetName().Name + " Version " + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

            //// Setup log
            //Log.identifier = Environment.MachineName;
            //Log.showTimestamp = true;
            //Log.Info(version);

            //if (string.IsNullOrEmpty(args[0]))
            //    Log.Fatal("Please enter a debug argument and try again");

            //// Process debug arguments
            //switch (args[0])
            //{
            //    case @"/scan":

            //        // Args check
            //        if (string.IsNullOrEmpty(args[1]))
            //            Log.Fatal("Scan - Missing scan range (format 192.168.0.1-10)");

            //        // Scan for slaves
            //        Slave.Scan(args[1]);

            //        // Start listening to each connected slave
            //        foreach (var slave in Slave.slaves)
            //        {
            //            SlaveListener sl = new SlaveListener(slave);
            //            Thread listeningThread = new Thread(new ThreadStart(sl.Start));
            //            listeningThread.Start();
            //        }

            //        // Send message to all connect slaves
            //        Command c = new Command(CommandType.Info);
            //        c.message = "Hello World! =^-^=";
            //        Slave.SendAll(c);

            //        Console.ReadLine();

            //        // Disconnect
            //        foreach (var slave in Slave.slaves)
            //            slave.Disconnect();

            //        Console.ReadLine();

            //        break;

            //    case @"/error":

            //        // Setup error listener
            //        Thread errorThread = new Thread(new ThreadStart(ErrorReporter.Listen));
            //        errorThread.Start();

            //        Console.ReadLine();

            //        break;

            //    case @"/transfer":

            //        if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]))
            //            Log.Fatal("Missing args");

            //        FileOps.FetchFile(args[1], args[2]);

            //        Console.ReadLine();

            //        break;

            //    case @"/signature":

            //        // Args check
            //        if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]))
            //            Log.Fatal("Missing args");

            //        // Scan for slaves
            //        Slave.Scan(args[1]);

            //        // Start listening to each connected slave
            //        foreach (var slave in Slave.slaves)
            //        {
            //            SlaveListener sl = new SlaveListener(slave);
            //            Thread listeningThread = new Thread(new ThreadStart(sl.Start));
            //            listeningThread.Start();
            //        }

            //        // Generate signature
            //        RsyncStream rs = new RsyncStream(StreamType.Signature, RsyncOps.GenerateSignature(args[2]), args[2]);

            //        // Pack into command
            //        Command com = new Command(CommandType.RsyncStream);
            //        com.AddStream(rs);

            //        // Send to all slaves
            //        Slave.SendAll(com);

            //        Console.ReadLine();

            //        // Disconnect
            //        foreach (var slave in Slave.slaves)
            //            slave.Disconnect();

            //        break;

            //    default:

            //        Log.Fatal("Arg not recognised");

            //        break;
            //}
 
        }

        //protected static void exitHandler(object sender, ConsoleCancelEventArgs args)
        //{
        //    // Cancel termination
        //    args.Cancel = true;

        //    Console.WriteLine("Master stopped. Press any key to exit...");
        //    Console.ReadLine();
        //    Environment.Exit(0);
        //}
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
