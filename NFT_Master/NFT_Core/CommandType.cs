public enum CommandType
{
    /// <summary>
    /// Initial command between master and slave
    /// </summary>
    Initial = 0,
    /// <summary>
    /// Download a given file
    /// </summary>
    Fetch = 1,
    /// <summary>
    /// Delete a local file
    /// </summary>
    Delete = 2,
    /// <summary>
    /// Generate a signature for a file
    /// </summary>
    Signature = 3,
    /// <summary>
    /// Create a delta for a file
    /// </summary>
    Delta = 4, 
    /// <summary>
    /// Apply a patch to a file using a given delta
    /// </summary>
    Patch = 5,
    /// <summary>
    /// Info message
    /// </summary>
    Info = 6, 
    /// <summary>
    /// Error Message
    /// </summary>
    Error = 7,
};