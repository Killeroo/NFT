using System;
using System.IO;

/// <summary>
/// Classed used to store Rsync signature and delta streams
/// </summary>
[Serializable()]
public class RsyncStream
{
    public StreamType type { get; set; }
    public MemoryStream stream { get; set; }
    public string sender { get; set; }
    public string reciever { get; set; }
    public int seq { get; set; } = 0;

    public RsyncStream(StreamType st, MemoryStream ms, string destIP)
    {
        sender = Helper.GetLocalIPAddress();
        reciever = destIP;
        stream = ms;
    }
}
