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
    private bool running;
    private int listeningPort;

    public CommandListener(int port)
    {
        listeningPort = port;
        listener = new TcpListener(IPAddress.Parse(Helper.getLocalIPAddress()), listeningPort);
    }

    public void start()
    {
        running = true;

        while (running)
        {
            listeningLoop();
            commandLoop();
        }
    }
    public void stop()
    {
        running = false;
    }

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
        while (running)
        {
            try
            {
                master = listener.AcceptTcpClient();

                if (master != null)
                {
                    // Store NFT Master endpoint
                    masterEP = (IPEndPoint)master.Client.RemoteEndPoint;
                    stream = master.GetStream();
                    Log.info(masterEP.Address.ToString() + ":" + masterEP.Port.ToString() + " [NFT Master] connected");
                    break; // Exit listening loop
                }
            }
            catch (SocketException)
            {
                Log.error("Error connecting to master client");
            }
            catch (ObjectDisposedException)
            {
                Log.error("Client object failure");
            }
            catch (Exception e)
            {
                Log.error("An exception occured");
                Log.info("---Stacktrace---");
                Log.info(e.ToString());
            }
        }

        // Cleanup
        listener.Stop();
        isListening = false;
    }
    /// <summary>
    /// Listens for Commands from NFT master
    /// </summary>
    private void commandLoop()
    {
        Command c = new Command();
        isConnected = true;
        Log.info("Listening to " + masterEP.Address.ToString() + "...");

        // Command recieving loop
        while (running)
        {
            byte[] buffer = new byte[4096];
            using (MemoryStream ms = new MemoryStream())
            {
                int bytesRead = 0;

                try
                {
                    do
                    {
                        // Read data from client stream
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, bytesRead);
                    }
                    while (stream.DataAvailable);

                    // Deserialize command 
                    c = Command.deserialize(ms);
                    Log.command(c);
                }
                catch (SerializationException e)
                {
                    Log.error("Cannot parse client stream - " + e.Message);
                    isConnected = false;
                }
                catch (IOException)
                {
                    Log.error("Connection failure (IOException)");
                    isConnected = false;
                }
                catch (ObjectDisposedException)
                {
                    Log.error("Client object failure");
                    isConnected = false;
                }
                catch (Exception e)
                {
                    Log.error("An exception occured");
                    Log.info("---Stacktrace---");
                    Log.info(e.ToString());
                    isConnected = false;
                }

                // Disconnect on quit flags
                if (c.type == CommandType.Quit || isConnected == false)
                    break;
            }
        }

        // Clean up
        Log.info(masterEP.Address + ":" + masterEP.Port + " disconnected");
        isConnected = false;
    }
    /// <summary>
    /// Handles recieved commands from NFT master
    /// </summary>
    private void handleCommand() { }
}
