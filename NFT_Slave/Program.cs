using System;
using System.Reflection;

namespace NFT_Slave
{
    class Program
    {
        private static CommandListener listener = new CommandListener(11000);


        static void Main(string[] args)
        {
            // Add control c hanlder
            Console.CancelKeyPress += new ConsoleCancelEventHandler(exitHandler);

            // Get version info
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            string version = Assembly.GetExecutingAssembly().GetName().Name + " Version " + v.Major + "." + v.Minor + "." + v.Build + " (r" + v.Revision + ")";

            // Setup log
            Log.identifier = "slave";
            Log.info(version);

            listener.start();

            Console.ReadLine();

            // Add control C or exit event handler
        }

        protected static void exitHandler(object sender, ConsoleCancelEventArgs args)
        {
            // Cancel termination
            args.Cancel = true;

            // Stop commandlistenr
            listener.stop();

            Console.WriteLine("Slave stopped. Press any key to exit...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
