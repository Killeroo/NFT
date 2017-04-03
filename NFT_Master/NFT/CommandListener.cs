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
    //public bool isConnected()
    //{
    //    try
    //    {
    //        return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
    //    }
    //    catch (SocketException) { return false; }
    //}
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
                    masterAddress = master.LocalEndPoint.ToString();
                    Log.info(masterAddress + " connected");
                    while (master.Connected)
                    {
                        using (var stream = new NetworkStream(master))
                        {
                            if (stream.DataAvailable)
                            {
                                c = Command.deserialize(stream);
                                Log.command(c);

                                if (c.type == CommandType.Quit)
                                    break;
                            }
                        }

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

        Console.ReadLine();
        
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
