using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Command class for communicating instructions between NFT Master and Slave applications
/// </summary>
[Serializable()]
public class Command
{
    public string sender { get; set; } // IP address of sender
    public CommandType type { get; set; } // type of command
    public List<string> files { get; set; }

    // Constructors
    public Command() { }
    public Command(CommandType ct)
    {
        type = ct;
    }
    public Command(CommandType ct, string pathToFiles)
    {
        // Check file exists
        if (string.IsNullOrWhiteSpace(pathToFiles) || !Directory.Exists(pathToFiles))
            Log.error("Path \"" + pathToFiles + "\" could not be found");
        else
            //file = new FileInfo(pathToFile);
            //Implement backup reursive file searh feature
        type = ct;
    }

    // Serialization functions
    public static byte[] serialize(Command c)
    {
        if (c == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, c);
            return ms.ToArray();
        }
    }
    public static Command deserialize(Stream ns)//NetworkStream ns)
    {
        IFormatter formatter = new BinaryFormatter();
        Command c = (Command)formatter.Deserialize(ns);
        ns.Close();
        return c;
    }

}
