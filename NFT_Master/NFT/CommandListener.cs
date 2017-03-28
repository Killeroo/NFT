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
    public void stop() { }
    public void reset() { }
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

        // Start TcpListener
        listener = new TcpListener(IPAddress.Parse(Helper.getLocalIPAddress()), listeningPort);
        listener.Start();
        Log.info("CommandListener started on " + Helper.getLocalIPAddress() + ":" + listeningPort + "...");

        // Listening loop
        //while (running)
        //{
        try
        {
            using (var client = listener.AcceptTcpClient())
            {
                Log.info(client.Client.RemoteEndPoint.ToString() + " connected");
                using (var stream = client.GetStream())
                {
                    while (c.type != CommandType.Quit)
                    {
                        if (stream.CanRead)
                        {
                            c = Command.deserialize(stream); // Deserialize incomming command
                            Log.command(c); // Display
                                            //handleCommand(c); // Process the command

                            //if (c.type == CommandType.Quit)
                            //    break;
                        }
                    }
                }
            }
        }
        catch (SocketException)
        {
            Log.fatal("Connection error occured (SocketException)");
        }
        catch (SerializationException)
        {
            Log.fatal("Error decoding client stream (SerializationException)");
        }
        //}
    }
    private void handleCommand(Command c)
    {
        switch (c.type)
        {
            case CommandType.SynchronizeAll:
                break;
            default:
                Log.error("(" + c.sender + ") Command type not recognised");
                break;
        }
    }
}
