﻿using System;
using System.Collections.Generic;
using System.IO;
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
        /// Generate rsync delta based on signature files and the new file
        /// </summary>
        /// <param name="originalSignaturePath"></param>
        /// <param name="newSignaturePath"></param>
        public static MemoryStream GenerateDelta(MemoryStream sigStream, string newFilePath)
        {
            // Setup Ocotodiff command
            string[] args = { newFilePath, "", "--progress" };

            // Construct and execute command
            return ConstructOctodiffCmd("delta", args, sigStream);
        }
        /// <summary>
        /// Patch an existing file using a delta file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="deltaPath"></param>
        public static void PatchFile(string path, string deltaPath)
        {

        }

        private static MemoryStream ConstructOctodiffCmd(string cmdName, string[] args, MemoryStream ms = null)
        {
            // Setup Octodiff command
            var sb = new StringBuilder();
            var cl = new CommandLocator();
            var command = cl.Find(cmdName);
            MemoryStream sigStream = null;

            // Parse commands
            foreach (string arg in args)
                sb.Append(arg != "" ? arg + " " : "");

            try
            {
                // Execute octodiff command 
                Log.Info("Octodiff --" + cmdName  + " " + sb.ToString());
                sigStream = cl.Create(command).Execute(args, ms);

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