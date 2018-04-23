using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using NFT.Rsync;

namespace NFT.Core
{
    /// <summary>
    /// Command class for communicating instructions between NFT Master and Slave applications
    /// </summary>
    [Serializable()]
    public class Command
    {
        public CommandType Type { get; set; } // type of command
        public List<string> Files { get; set; } = new List<string>(); // Relative file paths (eg \TestDir\filename.txt)
        public IPAddress Source { get; set; } // IP address of sender
        public IPAddress Destination { get; set; } // IP address of recipient
        public string Message { get; set; } = ""; // (Optional) text to be displayed if INFO type command
        public int Seq { get; set; } = 0; // Sequence number
        public RsyncStream Stream { get; private set; } = null; // (Optional) stores Rsync data

        // Constructors
        public Command()
        {
            Source = IPAddress.Parse(Helper.GetLocalIPAddress());
        }
        public Command(CommandType ct)
        {
            Type = ct;
            Source = IPAddress.Parse(Helper.GetLocalIPAddress());
        }
        public Command(CommandType ct, string destinationIP)
        {
            Type = ct;
            Destination = IPAddress.Parse(destinationIP);
            Source = IPAddress.Parse(Helper.GetLocalIPAddress());
        }

        /// <summary>
        /// Add files paths from specified path (pass path removed)
        /// </summary>
        public void AddFiles(string pathToFiles)
        {
            List<FileInfo> fileList = FileOps.DiscoverFiles(pathToFiles);

            if (fileList == null)
                return; // Exit if path does not exist

            // Add file paths to list
            foreach (var file in fileList)
                Files.Add(file.FullName.ToString().Replace(pathToFiles, "")); // Remove inputted path to get relative path

        }
        /// <summary>
        /// shuffle file list (to avoid overloading server)
        /// </summary>
        public void ShuffleFiles()
        {
            // Shuffle list based on Fisher-Yates shuffle
            Random rng = new Random();
            int x = Files.Count;

            // Decrement through list
            while (x > 1)
            {
                x--;
                int r = rng.Next(x + 1);
                // Swap round values at random position and x
                string value = Files[r];
                Files[r] = Files[x];
                Files[x] = value;
            }
        }

        public void AddStream(RsyncStream rs)
        {
            Stream = rs;
            Type = CommandType.RsyncStream;
        }

    }
}