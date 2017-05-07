using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

using Octodiff.CommandLine;
using Octodiff.CommandLine.Support;
using Octodiff.Core;

using NFT.Core;
using NFT.Logger;

namespace NFT.Rsync
{
    /// <summary>
    /// Stores static helper methods for rsync file operations 
    /// </summary>
    public class RsyncOps
    {
        private static List<FileInfo> files = new List<FileInfo>();

        ///// <summary>
        ///// Transfer file from a remote web server
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="destPath"></param>
        //public static void FetchFile(string url, string destPath)
        //{
        //    string filename = Path.GetFileName(url);

        //    try
        //    {
        //        // Request a file from the local server
        //        WebRequest request = WebRequest.Create(url);
        //        Log.Info("Requesting file (" + request.RequestUri.ToString() + ")");
        //        // Retrieve the response
        //        WebResponse response = request.GetResponse();
        //        // Display status
        //        Log.Info("Response recieved. Status=\"" + ((HttpWebResponse)response).StatusDescription + "\"");
        //        // Get response stream (containing file data)
        //        Stream fileStream = response.GetResponseStream();
        //        Log.Info("Transferring...");

        //        // Write file locally
        //        using (FileStream fs = File.Create(Path.Combine(destPath, filename)))
        //            fileStream.CopyTo(fs);
        //        Log.Info("File created \"" + Path.Combine(destPath, filename + "\""));
        //    }
        //    catch (WebException e)
        //    {
        //        Log.Error(new Error(e, "Request to \"" + url + "\" timed out"));
        //    }
        //    catch (UnauthorizedAccessException e)
        //    {
        //        Log.Error(new Error(e, "Cannot create file \"" + filename + "\""));
        //    }
        //    catch (PathTooLongException e)
        //    {
        //        Log.Error(new Error(e, "Cannot create file \"" + filename + "\""));
        //    }
        //    catch (DirectoryNotFoundException e)
        //    {
        //        Log.Error(new Error(e, "Cannot create file \"" + filename + "\""));
        //    }
        //    catch (IOException e)
        //    {
        //        Log.Error(new Error(e, "Cannot create file \"" + filename + "\""));
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(new Error(e, "Cannot transfer file \"" + filename + "\\"));
        //        Log.Info("---Stacktrace---\n" + e.ToString());
        //        Log.Info(e.ToString());
        //    }
        //}
        /// <summary>
        /// Generate rsync signature for a given file
        /// </summary>
        /// <param name="filePath"></param>
        public static MemoryStream GenerateSignature(string filePath)
        {
            // Setup Octodiff command
            string[] args = { filePath, "", "--progress" };

            // Construct and execute command
            return ConstructOctodiffCmd("signature", args);
        }
        /// <summary>
        /// Generate rsync delta based on signature files
        /// </summary>
        /// <param name="originalSignaturePath"></param>
        /// <param name="newSignaturePath"></param>
        public static void GenerateDelta(MemoryStream sigStream, string newSignaturePath)
        {
            // Setup Ocotodiff command
            //string[] args = { }
        }
        /// <summary>
        /// Patch an existing file using a delta file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="deltaPath"></param>
        public static void PatchFile(string path, string deltaPath)
        {

        }
        /// <summary>
        /// Discover all files at a path put in list
        /// </summary>
        public static List<FileInfo> DiscoverFiles(string path, bool recursing = false)
        {
            if (!recursing)
            {
                // Check path exits
                if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                {
                    Log.Warning("Could not discover files at [" + path + "]");
                    return null;
                }

                // Clear temp list
                files.Clear();

                // Start recursive discovery
                Log.Info("Starting file discovery at \"" + path + "\"...");
                DiscoverFiles(path, true);
                Log.Info(files.Count.ToString() + " files found");

                return files;
            }
            else
            {
                // Recursive file discovery //
                DirectoryInfo dir = new DirectoryInfo(path);

                try
                {
                    // Get all files at path
                    foreach (FileInfo file in dir.GetFiles())
                        files.Add(file);
                }
                catch (DirectoryNotFoundException e)
                {
                    Log.Error(new Error(e, "Directory not found \"" + path + "\""));
                }

                try
                {
                    // Recurse through each sub directory to find files
                    foreach (DirectoryInfo di in dir.GetDirectories())
                        DiscoverFiles(di.FullName, true);
                }
                catch (DirectoryNotFoundException e)
                {
                    Log.Error(new Error(e, "Directory not found \"" + path + "\""));
                }
                catch (UnauthorizedAccessException e)
                {
                    Log.Error(new Error(e, "Access denied \"" + path + "\""));
                }

                return null;
            }
        }

        private static MemoryStream ConstructOctodiffCmd(string cmdName, string[] args)
        {
            // Setup Octodiff command
            var sb = new StringBuilder();
            var cl = new CommandLocator();
            var command = cl.Find("signature");
            MemoryStream sigStream = null;

            // Parse commands
            foreach (string arg in args)
                sb.Append(arg != "" ? arg + " " : "");

            try
            {
                // Execute octodiff command 
                Log.Info("Octodiff --" + cmdName  + " " + sb.ToString() + " --progress");
                sigStream = cl.Create(command).Execute(args);

                return sigStream;
            }
            catch (OptionException ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }
            catch (UsageException ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }
            catch (CorruptFileFormatException ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }
            catch (IOException ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }
            catch (Exception ex)
            {
                Log.Error(new Error(ex, "Octodiff exception"));
            }

            return null;
        }
    }
}