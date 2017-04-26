using System;
using System.Net;

/// <summary>
/// Stores exception information (for error reporting)
/// </summary>
[Serializable()]
public class Error
{
    public Exception e { get; set; }
    public EndPoint sender { get; private set; }
    public string type { get; private set; }
    public bool fatal { get; set; } = false;

    public Error(Exception ex, EndPoint localEP)
    {
        e = ex;
        type = e.GetType().ToString();
        sender = localEP;
    }

    // Serialization functions
    public static byte[] serialize(Error err)
    {

    }
    public static Error deserialize(byte[] data)
    {

    }
}
