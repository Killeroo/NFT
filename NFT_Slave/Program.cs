using System;

namespace NFT_Slave
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandListener listener = new CommandListener();

            listener.start();
        }
    }
}
