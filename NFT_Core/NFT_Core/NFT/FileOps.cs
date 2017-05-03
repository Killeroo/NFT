using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Octodiff.CommandLine;
using Octodiff.CommandLine.Support;
using Octodiff.Core;

/// <summary>
/// Stores static helper methods for file operations required by NFT
/// </summary>
public class FileOps
{
    private static List<FileInfo> files = new List<FileInfo>();

    /// <summary>
    /// Transfer file from a remote web server
    /// </summary>
    /// <param name="url"></param>
    /// <param name="destPath"></param>
    public static void fetchFile(string url, string destPath)
    {
        // TODO: Switch to ASync

        string filename = Path.GetFileName(url);

        try
        {
            // Request a file from the local server
            WebRequest request = WebRequest.Create(url);
            Log.info("Requesting file (" + request.RequestUri.ToString() + ")");
            // Retrieve the response
            WebResponse response = request.GetResponse();
            // Display status
            Log.info("Response recieved. Status=\"" + ((HttpWebResponse)response).StatusDescription + "\"");
            // Get response stream (containing file data)
            Stream fileStream = response.GetResponseStream();
            Log.info("Transferring...");

            // Write file locally
            using (FileStream fs = File.Create(Path.Combine(destPath, filename)))
                fileStream.CopyTo(fs);
            Log.info("File created \"" + Path.Combine(destPath, filename + "\""));
        }
        catch (WebException e)
        {
            Log.error(new Error(e, "Request to \"" + url + "\" timed out"));
        }
        catch (UnauthorizedAccessException e)
        {
            Log.error(new Error(e, "Cannot create file \"" + filename + "\""));
        }
        catch (PathTooLongException e)
        {
            Log.error(new Error(e, "Cannot create file \"" + filename + "\""));
        }
        catch (DirectoryNotFoundException e)
        {
            Log.error(new Error(e, "Cannot create file \"" + filename + "\""));
        }
        catch (IOException e)
        {
            Log.error(new Error(e, "Cannot create file \"" + filename + "\""));
        }
        catch (Exception e)
        {
            Log.error(new Error(e, "Cannot transfer file \"" + filename + "\\"));
            Log.info("---Stacktrace---\n" + e.ToString());
            Log.info(e.ToString());
        }
    }
    /// <summary>
    /// Generate rsync signature for a given file
    /// </summary>
    /// <param name="filePath"></param>
    public static MemoryStream generateSignature(string filePath)
    {
        // Setup Octodiff command
        var cl = new CommandLocator();
        var command = cl.Find("signature");
        string[] args = { filePath , "", "--progress"};
        MemoryStream sigStream = null;

        try
        {
            Log.info("Octodiff --signature \"" + filePath + "\" --progress");
            sigStream = cl.Create(command).Execute(args);

            return sigStream;
        }
        catch (OptionException ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }
        catch (UsageException ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }
        catch (FileNotFoundException ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }
        catch (CorruptFileFormatException ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }
        catch (IOException ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }
        catch (Exception ex)
        {
            Log.error(new Error(ex, "Octodiff exception"));
        }

        return null;
    }
    /// <summary>
    /// Generate rsync delta based on signature files
    /// </summary>
    /// <param name="originalSignaturePath"></param>
    /// <param name="newSignaturePath"></param>
    public static void generateDelta(string originalSignaturePath, string newSignaturePath)
    {

    }
    /// <summary>
    /// Patch an existing file using a delta file
    /// </summary>
    /// <param name="path"></param>
    /// <param name="deltaPath"></param>
    public static void patchFile(string path, string deltaPath)
    {

    }
    /// <summary>
    /// Discover all files at a path put in list
    /// </summary>
    public static List<FileInfo> discoverFiles(string path, bool recursing = false)
    {
        if (!recursing)
        {
            // Check path exits
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                Log.warning("Could not discover files at [" + path + "]");
                return null;
            }

            // Clear temp list
            files.Clear();

            // Start recursive discovery
            Log.info("Starting file discovery at \"" + path + "\"...");
            discoverFiles(path, true);
            Log.info(files.Count.ToString() + " files found");

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
                Log.error(new Error(e, "Directory not found \"" + path + "\""));
            }

            try
            {
                // Recurse through each sub directory to find files
                foreach (DirectoryInfo di in dir.GetDirectories())
                    discoverFiles(di.FullName, true);
            }
            catch (DirectoryNotFoundException e)
            {
                Log.error(new Error(e, "Directory not found \"" + path + "\""));
            }
            catch (UnauthorizedAccessException e)
            {
                Log.error(new Error(e, "Access denied \"" + path + "\""));
            }

            return null;
        }
    }
}

