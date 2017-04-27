using System;
using System.IO;
using System.Net;

/// <summary>
/// Stores static helper methods for file operations required by NFT
/// </summary>
public class FileOps
{
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
    public static void createSignature(string filePath)
    {

    }
    /// <summary>
    /// Generate rsync delta based on signature files
    /// </summary>
    /// <param name="originalSignaturePath"></param>
    /// <param name="newSignaturePath"></param>
    public static void createDelta(string originalSignaturePath, string newSignaturePath)
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
}

