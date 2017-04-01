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
        try
        {
            while (running)
            {
                using (var master = listener.AcceptSocket())
                {
                    Log.info(master.LocalEndPoint + " connected");
                    while (master.Connected)
                    {
                        using (var stream = new NetworkStream(master))
                        {
                            //stream.Read();
                            c = Command.deserialize(stream);
                            Log.command(c);
                        }
                    }
                }
            }

            // THIS SHOULD WORK FINE (GOOD LOGIC LAYOUT)
            //while (running)
            //{
            //    using (var master = listener.AcceptTcpClient())
            //    {
            //        Log.info(master.Client.RemoteEndPoint.ToString() + " connected");
            //        while (master.Connected)
            //        {
            //            using (var stream = master.GetStream())
            //            {
            //                //stream.Read();
            //                c = Command.deserialize(stream);
            //                Log.command(c);
            //            }
            //        }
            //    }
            //}
        }
        catch (SocketException)
        {
            Log.fatal("Connection error occured (SocketException)");
        }
        catch (SerializationException)
        {
            Log.fatal("Error decoding client stream (SerializationException)");
        }
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
