using System;
using System.Reflection;

using NFT.Comms;
using NFT.Logger;

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
            string version = Assembly.GetExecutingAssembly().GetName().Name + " Version " + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

            // Setup log
            Log.identifier = Environment.MachineName;//"slave";
            Log.Info(version);

            listener.Start();
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
