public enum CommandType
{
    /// <summary>
    /// Initial command between master and slave
    /// </summary>
    Initial = 0,
    /// <summary>
    /// Synchronize all files between master and slave directories
    /// </summary>
    SynchronizeAll = 1,
    /// <summary>
    /// Transfer all files present on master to slave
    /// </summary>
    TranferAll = 2,
    /// <summary>
    /// Download a given file
    /// </summary>
    Fetch = 3,
    /// <summary>
    /// Delete a local file
    /// </summary>
    Delete = 4,
    /// <summary>
    /// Generate a signature for a file
    /// </summary>
    Signature = 5,
    /// <summary>
    /// Create a delta for a file
    /// </summary>
    Delta = 6, 
    /// <summary>
    /// Apply a patch to a file using a given delta
    /// </summary>
    Patch = 7,
    /// <summary>
    /// Info message
    /// </summary>
    Info = 8, 
    /// <summary>
    /// Error Message
    /// </summary>
    Error = 9,
    /// <summary>
    /// Quit Message
    /// </summary>
    Quit = 10,
};