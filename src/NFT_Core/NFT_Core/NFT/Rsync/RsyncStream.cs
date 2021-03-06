﻿using System;
using System.IO;

using NFT.Core;
using NFT.Logger;

namespace NFT.Rsync
{
    /// <summary>
    /// Class used to store Rsync signature and delta streams
    /// </summary>
    [Serializable()]
    public class RsyncStream
    {
        public StreamType type { get; set; }
        public MemoryStream stream { get; set; }
        public string filename { get; private set; }
        public string relativePath { get; private set; }

        public RsyncStream(StreamType st, MemoryStream ms, string relativeFilePath)
        {
            if (String.IsNullOrEmpty(relativeFilePath))
            {
                Log.Error(new Error(new Exception(), "Relative file path empty"));
                return;
            }

            // Setup attributes 
            filename = Path.GetFileName(relativeFilePath);
            relativePath = Path.GetFullPath(relativeFilePath); // Check
            type = st;
            stream = ms;
        }
    }
}