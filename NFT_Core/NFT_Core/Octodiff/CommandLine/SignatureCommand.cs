﻿using System;
using System.Collections.Generic;
using System.IO;
using Octodiff.CommandLine.Support;
using Octodiff.Core;
using Octodiff.Diagnostics;

namespace Octodiff.CommandLine
{
    [Command("signature", "sig", Description = "Given a basis file, creates a signature file", Usage = "<basis-file> [<signature-file>]")]
    class SignatureCommand : ICommand
    {
        private readonly List<Action<SignatureBuilder>> configuration = new List<Action<SignatureBuilder>>();
        private readonly OptionSet options;
        private string basisFilePath;
        private string signatureFilePath;

        public SignatureCommand()
        {
            options = new OptionSet();
            options.Positional("basis-file", "The file to read and create a signature from.", v => basisFilePath = v);
            options.Positional("signature-file", "The file to write the signature to.", v => signatureFilePath = v);
            options.Add("chunk-size=", string.Format("Maximum bytes per chunk. Defaults to {0}. Min of {1}, max of {2}.", SignatureBuilder.DefaultChunkSize, SignatureBuilder.MinimumChunkSize, SignatureBuilder.MaximumChunkSize), v => configuration.Add(builder => builder.ChunkSize = short.Parse(v)));
            options.Add("progress", "Whether progress should be written to stdout", v => configuration.Add(builder => builder.ProgressReporter = new ConsoleProgressReporter()));
        }

        public void GetHelp(TextWriter writer)
        {
            options.WriteOptionDescriptions(writer);
        }

        public MemoryStream Execute(string[] commandLineArguments, MemoryStream ms = null)
        {
            // Parse through arguments
            options.Parse(commandLineArguments);

            // Check if the file path contains a string
            if (string.IsNullOrWhiteSpace(basisFilePath))
                throw new OptionException("No basis file was specified", "basis-file");

            // Specifiy new files path
            basisFilePath = Path.GetFullPath(basisFilePath);

            // Set up the signature generator (load options)
            var signatureBuilder = new SignatureBuilder();
            foreach (var config in configuration) config(signatureBuilder);

            // Check if the file exists
            if (!File.Exists(basisFilePath))
            {
                throw new FileNotFoundException("File not found: " + basisFilePath, basisFilePath);
            }

            // Generate signature from file
            using (var basisStream = new FileStream(basisFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var signatureStream = new MemoryStream())//FileStream(signatureFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                signatureBuilder.Build(basisStream, new SignatureWriter(signatureStream));

                // Return stream containing signature
                return signatureStream;
            }
        }
    }
}
