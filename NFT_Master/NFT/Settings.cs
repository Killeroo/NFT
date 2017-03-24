﻿using System;
using System.IO;
using Microsoft.Win32;

/// <summary>
/// Allows modification of NFT registry setting
/// </summary>
public class Settings
{
    // Reg settings
    public string WorkingDirectory
    {
        get { return Registry.GetValue(settingsRegPath, "WorkingDirectory", @"C:\NFT").ToString(); }
        set { if (Directory.Exists(value)) { Registry.SetValue(settingsRegPath, "WorkingDirectory", value, RegistryValueKind.String); } }
    }
    public int NumThreads
    {
        get { return Convert.ToInt32(Registry.GetValue(settingsRegPath, "NumberOfThreads", "1")); }
        set { if (value < 5) { Registry.SetValue(settingsRegPath, "NumberOfThreads", value, RegistryValueKind.DWord); } }
    }

    private string settingsRegPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\NFT\";

    public Settings()
    {
        // Check if settings exist
        if (!exists())
            createDefaultSettings();
    }

    private void createDefaultSettings()
    {
        Registry.SetValue(settingsRegPath, "WorkingDirectory", @"C:\NFT", RegistryValueKind.String);
        Registry.SetValue(settingsRegPath, "WorkingDirectory", 1, RegistryValueKind.DWord);
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
