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
    /// Sends signal to abort current operation
    /// </summary>
    Abort = 3,
    /// <summary>
    /// Signals operation successful
    /// </summary>
    Success = 4,
    /// <summary>
    /// Info message
    /// </summary>
    Info = 5, 
    /// <summary>
    /// Quit Message
    /// </summary>
    Quit = 6,
    // Fetch (for when a file fails)
};