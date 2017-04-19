﻿using System;
using Microsoft.Web.Administration;

/// <summary>
/// Setup and control IIS HTTP transfer server
/// </summary>
class TransferServer
{
    private const string SITE_NAME = "NFT_Transfer_Server";

    public string url { get; private set; }
    public string address { get; set; }
    public string port { get; set; }

    private ServerManager iisManager = new ServerManager();

    public TransferServer() { }

    public void start()
    {
        if (!checkIfSiteExists())
            // Create site instance if it doesn't exist
            iisManager.Sites.Add(SITE_NAME, "http", ":8000:", Settings.WorkingDirectory);

        iisManager.CommitChanges();
    }
    public void stop()
    {
        if (checkIfSiteExists())
        {
            Site s = iisManager.Sites[SITE_NAME];
            iisManager.Sites[SITE_NAME].Stop();
            iisManager.Sites.Remove(s);
            iisManager.CommitChanges();
            Log.info("[" + url + "] IIS server stopped");
        }
    }

    private bool checkIfSiteExists()
    {
        SiteCollection sites = iisManager.Sites;
        // Loop through each active site
        foreach (Site site in sites)
            if (site.Name == SITE_NAME)
                return true;
        return false;
    }
    private void getSiteRequests() { }

}
