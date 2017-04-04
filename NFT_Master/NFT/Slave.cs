using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

class Slave
{
    public static Slave[] slaveList;

    public IPEndPoint endPoint;
    public Socket socket; // DELETE
    public TcpClient client { get; private set; } // SWITCH TO USE THIS
    public bool isReady;

    public Slave(IPEndPoint ep, Socket sock)
    {
        endPoint = ep;
        socket = sock;
    }

    public void sendCommand(Command c) { }
    public void reconnect() { }
    public void disconnect() { }

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
    public static void sendCommandToAll()
    {

    }
}
