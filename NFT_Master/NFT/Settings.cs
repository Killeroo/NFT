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
    public static int SimultaneousTransfers
    {
        get { return Convert.ToInt32(Registry.GetValue(settingsRegPath, "SimultaneousTransfers", "5")); }
        set { if (value < 5) { Registry.SetValue(settingsRegPath, "SimultaneousTransfers", value, RegistryValueKind.DWord); } }
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
            catch (IOException) { Log.fatal("Cannot create working directory [" + WorkingDirectory + "]"); }
            catch (UnauthorizedAccessException) { Log.fatal("Access Denied, cannot create working directory [" + WorkingDirectory + "]"); }
            catch (Exception) { Log.fatal("Cannot create working directory [" + WorkingDirectory + "]"); }
        }
    }

    private void createDefaultSettings()
    {
        Log.info("Creating default registry settings...");
        Registry.SetValue(settingsRegPath, "WorkingDirectory", @"C:\NFT", RegistryValueKind.String);
        Registry.SetValue(settingsRegPath, "NumberOfThreads", 1, RegistryValueKind.DWord);
        Registry.SetValue(settingsRegPath, "SimultaneousTransfers", 5, RegistryValueKind.DWord);
    }
    private bool exists()
    {
        RegistryKey r = null;

        try
        {
            r = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, @"Software\NFT");
        }
        catch (IOException)
        {
            Log.info("Cannot find registry settings");
        }
        catch (Exception e)
        {
            Log.error("Could not load registry settings");
            Log.info("---Stacktrace---\n" + e.ToString());
            Log.info(e.ToString());
        }

        if (r != null)
            return true;
        else
            return false;
    }
}
