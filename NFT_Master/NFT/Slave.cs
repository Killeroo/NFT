using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

class Slave
{
    public static Slave[] slaveList;

    public IPEndPoint endPoint;
    public TcpClient client { get; private set; }
    public NetworkStream stream { get; private set; }
    public bool isConnected { get; private set; }

    private int curSeqNum = 0; // Current command sequence number

    public Slave(IPEndPoint ep)
    {
        client = new TcpClient();
        endPoint = ep;
        connect();
    }

    public void sendCommand(Command c)
    {
        try
        {
            byte[] buffer = new byte[4096];
            curSeqNum++; // Increment sequence number
            c.seq = curSeqNum;
            buffer = Command.serialize(c);
            Log.command(c);
            stream.Write(buffer, 0, buffer.Length);
            Log.info("Command sent");
        }
        catch (IOException)
        {
            Log.error("Failed to send command (IOException)");
        }
        catch (ObjectDisposedException)
        {
            Log.error("Object failure (ObjectDisposedException)");
        }
        catch (Exception e)
        {
            Log.error("General exception occured (Exception)");
            Log.info("---Stacktrace---");
            Log.info(e.ToString());
        }
    }
    public void connect()
    {
        try
        {
            Log.info("Connecting to " + endPoint.Address + ":" + endPoint.Port + "...");
            client.Connect(endPoint);
            Log.info("Connection established");
            stream = client.GetStream();
        }
        catch (SocketException)
        {
            Log.error("Failed to connect to slave (SocketException)");
        }
        catch (ObjectDisposedException)
        {
            Log.error("Object failure (ObjectDisposedException)");
        }
    }
    public void disconnect()
    {
        // Construct command
        Command c = new Command();
        c.type = CommandType.Quit;

        // Send quit command to slave
        sendCommand(c);

        // Cleanup
        client.Close();
        stream.Close();
    }

    public static void findSlaves(string range)//Slave[] findSlaves()
    {
        Socket scanSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint scanEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        List<Slave> foundSlaves = new List<Slave>();
        scanEP.Port = 11000; // Default NFT Slave listening port
        scanSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 200); // Set timeout on scan socket
        scanSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 200);

        // Split ip address into segments
        string[] addressSegs = range.Split('.');

        // Holds the ranges for each IP segment
        int[] segLower = new int[4];
        int[] segUpper = new int[4];

        // Work out upper and lower ranges for each segment
        for (int x = 0; x < 4; x++)
        {
            string[] ranges = addressSegs[x].Split('-');
            segLower[x] = Convert.ToInt16(ranges[0]);
            segUpper[x] = (ranges.Length == 1) ? segLower[x] : Convert.ToInt16(ranges[1]);
        }

        // Loop through each possible address
        Log.info("Starting slave scan on range [" + range + "]");
        for (int seg1 = segLower[0]; seg1 <= segUpper[0]; seg1++)
        {
            for (int seg2 = segLower[1]; seg2 <= segUpper[1]; seg2++)
            {
                for (int seg3 = segLower[2]; seg3 <= segUpper[2]; seg3++)
                {
                    for (int seg4 = segLower[3]; seg4 <= segUpper[3]; seg4++)
                    {
                        // Update with current address to try and connect to
                        scanEP.Address = new IPAddress(new byte[] { (byte)seg1, (byte)seg2, (byte)seg3, (byte)seg4 });

                        try
                        {
                            // Try to connect
                            Log.info("Attempting to connect to " + scanEP.Address.ToString() + ":" + scanEP.Port.ToString() + "...");
                            scanSock.Connect(scanEP);
                            Log.info("Connection established to " + scanEP.Address.ToString() + ":" + scanEP.Port.ToString() + " [NFT Slave]");
                        }
                        catch (SocketException) { }
                        catch (InvalidOperationException) { }
                    }
                }
            }
        }
    }
    public static void sendToAllSlaves(Command c) { }

    private bool checkConnected() { return true; }
}
