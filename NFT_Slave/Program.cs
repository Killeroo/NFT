using System;
using System.Reflection;
using System.Net;

using NFT.Net;
using NFT.Logger;
using NFT.Core;

using Console = Colorful.Console;

namespace NFT_Slave
{
    class Program
    {
        private static MasterListener listener = new MasterListener();

        static void Main(string[] args)
        {
            // Add control c hanlder
            Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

            // Get version info
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = Assembly.GetExecutingAssembly().GetName().Name;// + " V" + v.Major + "." + v.Minor;// + "." + v.Build;// + " (r" + v.Revision + ")";

            // Setup log
            Log.Identifier = "";//Environment.MachineName;
            //Log.Info(version);
            Console.WriteAscii("NFT_SLAVE");//version);
            Console.WriteAscii("   V" + v.Major + "." + v.Minor + "." + v.Build);//, Colorful.Figlet);

            if (args.Length < 1)
            {
                listener.Start();
                //Log.Fatal("Please enter a debug argument and try again");
                return;
            }

            // Process debug arguments
            switch (args[0])
            {
                case @"/scan":

                    // Start listener
                    listener.Start();

                    break;

                case @"/error":

                    if (string.IsNullOrEmpty(args[1]))
                        Log.Fatal("Missing args");

                    // Send an error to NFT master
                    ErrorReporter.SendError(new Error(new Exception()), new IPEndPoint(IPAddress.Parse(args[1]), 0));

                    Console.ReadLine();

                    break;

                case @"/transfer":

                    if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]))
                        Log.Fatal("Missing args");

                    FileOps.FetchFile(args[1], args[2]);

                    Console.ReadLine();

                    break;

                case @"/signature":

                    // listen (for signature command)
                    listener.Start();

                    break;

                default:

                    Log.Fatal("Arg not recognised");

                    break;
            }
            
        }

        protected static void exitHandler(object sender, ConsoleCancelEventArgs args)
        {
            // Cancel termination
            args.Cancel = true;

            // Stop commandlistener
            listener.Stop();

            Console.WriteLine("Slave stopped. Press any key to exit...");
            Console.ReadLine();

            args.Cancel = false;
            Environment.Exit(0);
        }
    }
}
