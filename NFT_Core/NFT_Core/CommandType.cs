﻿public enum CommandType
{
    /// <summary>
    /// Initial command between master and slave
    /// </summary>
    Initial = 0,
    /// <summary>
    /// Synchronize all files on the slave device
    /// so they are the same as those on the master
    /// </summary>
    SynchronizeAll = 1,
    /// <summary>
    /// Re-Transfer all files present on master to slave
    /// reguardless of what files are present on slave device
    /// </summary>
    CleanTransfer = 2,
    /// <summary>
    /// Sends signal to abort current operation
    /// </summary>
    Abort = 3,
    /// <summary>
    /// Info message
    /// </summary>
    Info = 4, 
    /// <summary>
    /// Error Message
    /// </summary>
    Error = 5,
    /// <summary>
    /// Quit Message
    /// </summary>
    Quit = 6,
    // Fetch (for when a file fails)
};