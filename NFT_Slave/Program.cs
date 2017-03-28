using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

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
