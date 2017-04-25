using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

class Slave
{
    public static List<Slave> slaves = new List<Slave>();

    public IPEndPoint endPoint;
    public TcpClient client { get; private set; }
    public NetworkStream stream { get; private set; }
    public bool isConnected { get; private set; } = false;

    private int curSeqNum = 0; // Current command sequence number (slave independent)

    public Slave(IPEndPoint ep)
    {
        client = new TcpClient();
        endPoint = ep;
        //connect();
    }

    /// <summary>
    /// Send a command to connected slave
    /// </summary>
    public void send(Command c)
    {
        try
        {
            byte[] buffer = new byte[4096];
            curSeqNum++; // Increment sequence number
            c.seq = curSeqNum;
            buffer = Command.serialize(c);
            Log.command(c);
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (IOException)
        {
            Log.error("Failed to send command ");
        }
        catch (ObjectDisposedException)
        {
            Log.error("Object failure");
        }
        catch (Exception e)
        {
            Log.error("An exception occured ");
            Log.info("---Stacktrace---");
            Log.info(e.ToString());
        }
    }
    public void connect()
    {
        try
        {
            Log.info("Connecting to " + endPoint.Address + ":" + endPoint.Port + "...");

            // Async connection attempt
            //client.Connect(endPoint);
            var result = client.BeginConnect(endPoint.Address.ToString(), endPoint.Port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1)); // Set timeout 
            if (!success)
                throw new SocketException();

            // Connected
            client.EndConnect(result);
            Log.info("Connection established");

            stream = client.GetStream();
            isConnected = true;
        }
        catch (SocketException)
        {
            Log.error("Failed to connect to slave");
        }
        catch (ObjectDisposedException)
        {
            Log.error("Object failure");
        }
    }
    public void disconnect()
    {
        // Construct command
        Command c = new Command();
        c.type = CommandType.Quit;

        // Send quit command to slave
        send(c);

        // Cleanup
        client.Close();
        stream.Close();
        isConnected = false;
    }

    /// <summary>
    /// Finds NFT slaves on specified address range
    /// </summary>
    public static void scan(string range)//Slave[] findSlaves()
    {
        IPEndPoint scanEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        List<Slave> foundSlaves = new List<Slave>();
        scanEP.Port = 11000; // Default NFT Slave listening port

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
                        Slave s = new Slave(scanEP);

                        s.connect();

                        if (s.isConnected)
                            Slave.slaves.Add(s);
                    }
                }
            }
        }
    }
    public static void sendToAll(Command c) { }

    private bool checkConnected() { return true; }
}
