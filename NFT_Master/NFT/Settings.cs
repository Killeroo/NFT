using System;
using System.IO;
using Microsoft.Win32;

/// <summary>
/// Allows modification of NFT registry setting
/// </summary>
public class Settings
{
    // Reg settings
    public static string WorkingDirectory
    {
        get { return Registry.GetValue(settingsRegPath, "WorkingDirectory", @"C:\NFT").ToString(); }
        set { if (Directory.Exists(value)) { Registry.SetValue(settingsRegPath, "WorkingDirectory", value, RegistryValueKind.String); } }
    }
    public static int NumThreads
    {
        get { return Convert.ToInt32(Registry.GetValue(settingsRegPath, "NumberOfThreads", "1")); }
        set { if (value < 5) { Registry.SetValue(settingsRegPath, "NumberOfThreads", value, RegistryValueKind.DWord); } }
    }

    private static string settingsRegPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\NFT\";

    public Settings()
    {
        // Check if settings exist
        if (!exists())
            createDefaultSettings();

        // Check that WorkingDirectory exists
        if (!Directory.Exists(WorkingDirectory))
        {
            try
            {
                Directory.CreateDirectory(WorkingDirectory);
            }
            catch (IOException) { Log.fatal("Cannot create working directory (IOException)"); }
            catch (UnauthorizedAccessException) { Log.fatal("Cannot create working directory, Access Denied (UnauthAccessException)"); }
            catch (Exception) { Log.fatal("Cannot create working directory (Exception)"); }
        }
    }

    private void createDefaultSettings()
    {
        Registry.SetValue(settingsRegPath, "WorkingDirectory", @"C:\NFT", RegistryValueKind.String);
        Registry.SetValue(settingsRegPath, "NumberOfThreads", 1, RegistryValueKind.DWord);
    }
    private bool exists()
    {
        RegistryKey r = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, @"Software\NFT");

        if (r != null)
            return true;
        else
            return false;
    }
}
