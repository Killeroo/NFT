using System;

namespace NFT_Slave
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup log
            Log.identifier = "slave";

            CommandListener listener = new CommandListener(11000);

            listener.start();

            Console.ReadLine();
        }
    }
}
