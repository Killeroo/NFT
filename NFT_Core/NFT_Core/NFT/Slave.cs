using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Configuration;

public class Slave
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
            // Check if connected first
            if (client == null || !isConnected)
                throw new Exception();

            byte[] buffer = new byte[4096];
            curSeqNum++; // Increment sequence number
            c.seq = curSeqNum;
            c.destination = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]); // Set destination of command
            buffer = Helper.ToByteArray<Command>(c);//Command.serialize(c);
            Log.command(c);
            stream.Write(buffer, 0, buffer.Length);
        }
        catch (IOException e)
        {
            Log.error(new Error(e, "Failed to send command"));
        }
        catch (ObjectDisposedException e)
        {
            Log.error(new Error(e, "Object failure"));
        }
        catch (Exception e)
        {
            Log.error(new Error(e, "Could not send command"));
        }
    }
    public void connect()
    {
        try
        {
            // Async connection attempt
            var result = client.BeginConnect(endPoint.Address.ToString(), endPoint.Port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(150));//FromSeconds(1)); // Set timeout 
            if (!success)
                throw new SocketException();

            // Connected
            client.EndConnect(result);
            Log.info("Connected to " + endPoint.Address + " [NFT_Slave]");

            stream = client.GetStream();
            isConnected = true;
        }
        catch (SocketException)
        {
            // For cleaner scanning output
            //Log.error(new Error(e, "Could not connect to slave"));
        }
        catch (ObjectDisposedException)
        {
            //Log.error(new Error(e, "Object failure"));
        }
        catch (Exception)
        {
            //Log.error(new Error(e, "Could not connect to slave"));
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
    public void reconnect() { }

    /// <summary>
    /// Finds NFT slaves on specified address range
    /// </summary>
    public static void scan(string range)
    {
        IPEndPoint scanEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), MasterListener.COMMAND_LISTEN_PORT);
        List<Slave> foundSlaves = new List<Slave>();
        int hostCount = 0;

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
        Log.info("Scanning for NFT_Slave programs on range [" + range + "]...");
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

                        // Attempt to connect
                        s.connect();

                        // Add to list of connected slaves
                        if (s.isConnected)
                        {
                            Slave.slaves.Add(s);
                            hostCount++;
                        }
                    }
                }
            }
        }
        Log.info("Scan complete. " + hostCount + " host(s) found");
    }
    /// <summary>
    /// Send command to all connected slaves
    /// </summary>
    public static void sendToAll(Command c)
    {
        if (c == null)
            return;

        // Loop through connected slave list
        foreach (Slave slave in Slave.slaves)
            slave.send(c);
    }

    private bool testConnection()
    {
        if (!stream.DataAvailable)
            return false;
        else
            return true;
    }
}
