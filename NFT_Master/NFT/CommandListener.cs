using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

/// <summary>
/// Class for recieving Command messages from NFT master application
/// </summary>
public class CommandListener
{
    public bool isListening { get; private set; }
    public bool isConnected { get; private set; }

    private TcpClient master;
    private TcpListener listener;
    private IPEndPoint masterEP;
    private NetworkStream stream;
    private int listeningPort;

    public CommandListener(int port)
    {
        listeningPort = port;
        listener = new TcpListener(IPAddress.Parse(Helper.getLocalIPAddress()), listeningPort);
    }

    public void start()
    {
        listeningLoop();
        Console.ReadLine();
    }
    public void stop() { }

    /// <summary>
    /// Listens for TCP connection for NFT master
    /// </summary>
    private void listeningLoop()
    {
        // Start Tcplistener
        listener.Start();
        Log.info("Listening for NFT Master on " + Helper.getLocalIPAddress() + ":" + listeningPort + "...");
        isListening = true;

        // Listening loop
        while (true)
        {
            master = listener.AcceptTcpClient();

            if (master != null)
            {
                // Store NFT Master endpoint
                masterEP = (IPEndPoint)master.Client.RemoteEndPoint;
                Log.info(masterEP.Address.ToString() + ":" + masterEP.Port.ToString() + " [NFT Master] connected");
                break; // Exit listening loop
            }
        }

        // Cleanup
        listener.Stop();
        isListening = false;
    }
    private void commandLoop() { }
    private void handleCommand() { }
}
