using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Command class for communicating instructions to NFT Slave applications
/// </summary>
[Serializable]
class Command
{
    public string message { get; set; }
    public string sender { get; set; }
    public CommandType type { get; set; }
    public List<string> files = new List<string>();

    public Command() { }
    public Command(CommandType ct, string[] fileArgs)
    {
        type = ct;
        foreach (string file in files)
        {
            files.Add(file);
        }
    }
    public Command(CommandType ct, string mesg)
    {
        type = ct;
        message = mesg;
    }

    public void addFile(string filename)
    {
        files.Add(filename);
    }

    private void fetchFile(string addr)
    {

    }
    private void generateSignature(string path)
    {

    }
    private void generateDelta(string originalSignature, string newSignature)
    {

    }
    private void patchFile(string path, string delta)
    {

    }

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
    public static Command deserialize(NetworkStream ns)
    {
        IFormatter formatter = new BinaryFormatter();
        Command c = (Command)formatter.Deserialize(ns);
        ns.Close();
        return c;
    }
}
