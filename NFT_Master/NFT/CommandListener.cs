using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

/// <summary>
/// Async class for recieving Command messages, using TCPListening loop
/// </summary>
public class CommandListener
{
    public bool running;
    private TcpListener listener;
    private int listeningPort;

    public CommandListener(int port = 11000)
    {
        listeningPort = port;
    }

    public void start()
    {
        running = true;
        listeningLoop();
    }
    public void stop()
    {

    }
    public void reset()
    {

    }
    public bool isRunning()
    {
        // Change
        if (listener == null)
            return false;
        else
            return true;
    }

    private void listeningLoop()
    {
        Command c = new Command();
        string masterAddress = null;

        // Start TcpListener
        listener = new TcpListener(IPAddress.Parse(Helper.getLocalIPAddress()), listeningPort);
        listener.Start();
        Log.info("CommandListener started on " + Helper.getLocalIPAddress() + ":" + listeningPort + "...");

        // Listening loop
        while (running)
        {
            using (var master = listener.AcceptSocket())
            {
                try
                {
                    // Store NFT master ip address
                    masterAddress = master.LocalEndPoint.ToString();
                    Log.info(masterAddress + " connected");

                    // Command recieving loop
                    while (master.Connected)
                    {
                        using (var stream = new NetworkStream(master))
                        {
                            // Only process when stream has something on it
                            if (stream.DataAvailable)
                            {
                                // Deserialize and display command
                                c = Command.deserialize(stream);
                                Log.command(c);

                                // Disconnect on quit command
                                if (c.type == CommandType.Quit)
                                    break;
                            }
                        }

                        // Throw exception if master unexpectedly disconnects
                        if (!master.IsConnected())
                            throw new SocketException();
                    }
                }
                catch (SocketException)
                {
                    Log.error("Connection error occured (SocketException)");
                }
                catch (SerializationException)
                {
                    Log.error("Error decoding master stream (SerializationException)");
                }
                finally
                {
                    Log.info(masterAddress + " disconnected");
                }
            }
        }
    }
    private void handleCommand(Command c)
    {
        switch (c.type)
        {
            case CommandType.Initial:
                break;
            case CommandType.SynchronizeAll:
                break;
            case CommandType.CleanTransfer:
                break;
            case CommandType.Abort:
                break;
            case CommandType.Info:
                break;
            case CommandType.Error:
                break;
            case CommandType.Quit:
                break;
            default:
                Log.error("(" + c.sender + ") Command type not recognised");
                break;
        }
    }
}
