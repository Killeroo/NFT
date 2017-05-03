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
    public List<string> files { get; set; } = new List<string>(); // Relative file paths (eg \TestDir\filename.txt)
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
    
    /// <summary>
    /// Add files paths from specified path (pass path removed)
    /// </summary>
    public void addFiles(string pathToFiles)
    {
        List<FileInfo> fileList = FileOps.discoverFiles(pathToFiles);

        if (fileList == null)
            return; // Exit if path does not exist

        // Add file paths to list
        foreach (var file in fileList)
            files.Add(file.FullName.ToString().Replace(pathToFiles, "")); // Remove inputted path to get relative path

    }
    /// <summary>
    /// shuffle file list (to avoid overloading server)
    /// </summary>
    public void shuffleFiles()
    {
        // Shuffle list based on Fisher-Yates shuffle
        Random rng = new Random();
        int x = files.Count;

        // Decrement through list
        while (x > 1)
        {
            x--;
            int r = rng.Next(x + 1);
            // Swap round values at random position and x
            string value = files[r];
            files[r] = files[x];
            files[x] = value;
        }
    }

}
    