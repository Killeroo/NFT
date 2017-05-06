using System;
using System.Net;
using System.Linq;

namespace NFT
{

    /// <summary>
    /// Stores exception information (for error reporting)
    /// </summary>
    [Serializable()]
    public class Error
    {
        public Exception ex { get; set; }
        public IPAddress senderAddr { get; private set; }
        public string type { get; private set; }
        public string message { get; set; } = "";
        public bool fatal { get; set; } = false;

        public Error(Exception e)
        {
            ex = e;
            type = e.GetType().ToString().Split('.').Last();
            senderAddr = IPAddress.Parse(Helper.GetLocalIPAddress());
        }
        public Error(Exception e, string msg)
        {
            ex = e;
            type = e.GetType().ToString().Split('.').Last();
            message = msg;
            senderAddr = IPAddress.Parse(Helper.GetLocalIPAddress());
        }
    }
}
