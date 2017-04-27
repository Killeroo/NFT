using System;
using Microsoft.Web.Administration;
using System.Configuration;
//TODO: Move to core when done

/// <summary>
/// Setup and control IIS HTTP transfer server
/// </summary>
public class TransferServer
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
            //iisManager.Sites.Add(SITE_NAME, "http", ":8000:", Properties.Settings.Default.WorkingDirectory);

        iisManager.CommitChanges();

        iisManager.Sites[SITE_NAME].Start();
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
    public void displaySites()
    {
        foreach (var site in iisManager.Sites)
            Log.info("Site " + site.Name + " active");
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
