using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Command class for communicating instructions between NFT Master and Slave applications
/// </summary>
[Serializable()]
public class Command
{
    public CommandType type { get; set; } // type of command
    public List<string> files { get; set; }
    public string sender { get; set; } // IP address of sender
    public string reciever { get; set; } = ""; // IP address of recipient
    public string message { get; set; } = ""; // (Optional) text to be displayed if INFO type command
    public int seq { get; set; } = 0;// Sequence number

    // Constructors
    public Command()
    {
        sender = Helper.GetLocalIPAddress();
    }
    public Command(CommandType ct)
    {
        type = ct;
        sender = Helper.GetLocalIPAddress();
    }
    public Command(CommandType ct, string destinationIP)
    {
        type = ct;
        reciever = destinationIP;
        sender = Helper.GetLocalIPAddress();
    }
    
    public void addFiles(string pathToFiles)
    {
        // Check path exists
        if (string.IsNullOrWhiteSpace(pathToFiles) || !Directory.Exists(pathToFiles))
            Log.error(new Error(new Exception(), "Path \"" + pathToFiles + "\" could not be found"));
        //else
            //file = new FileInfo(pathToFile);
            //Implement backup reursive file searh feature  
    }

    // Randomise file lists to avoid overloading server
    private void randomiseFiles() { }

}
    