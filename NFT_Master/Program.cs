using System;
using System.Net;
using Microsoft.Web.Administration;

namespace NFT_Master
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arguments check
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
                Log.fatal("Please enter IP of NFT Slave");

            TransferServer ts = new TransferServer();

            ts.displaySites();
            ts.start();

            Console.ReadLine();

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
    }
}
