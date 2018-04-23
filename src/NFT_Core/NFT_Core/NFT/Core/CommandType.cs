namespace NFT.Core
{
    public enum CommandType
    {
        /// <summary>
        /// Initial command between master and slave
        /// </summary>
        Initial = 0,
        /// <summary>
        /// Synchronize all files on the slave device
        /// so they are the same as those on the master
        /// </summary>
        Synchronize = 1,
        /// <summary>
        /// Transfer all files in Command.Files
        /// </summary>
        Transfer = 2,
        /// <summary>
        /// Symbolises that command is being used
        /// to transfer RsyncStream
        /// </summary>
        RsyncStream = 3,
        /// <summary>
        /// Sends signal to abort current operation
        /// </summary>
        Abort = 4,
        /// <summary>
        /// Signals operation successful
        /// </summary>
        Success = 5,
        /// <summary>
        /// Signals operation successful
        /// </summary>
        Failure = 6,
        /// <summary>
        /// Info message
        /// </summary>
        Info = 7,
        /// <summary>
        /// Quit Message
        /// </summary>
        Quit = 8,
        // Fetch (for when a file fails)
    };
}