using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

/// <summary>
/// Static class for handling Command messages
/// </summary>
public class CommandHandler
{
    public static void handle(Command c)
    {
        switch (c.type)
        {
            case CommandType.RsyncStream:
                Log.command(c);
                Log.stream(c.stream);

                foreach (var data in c.stream.stream.ToArray())
                    Console.Write(data);

                ErrorReporter.sendError(new Error(new Exception(), "test"), new IPEndPoint(c.source, 0));

                break;

            default:
                Log.command(c);
                break;
        }
    }
    public static void send(Command c, TcpClient client, int seqnum)
    {
        try
        {
            // Check if connected first
            if (client == null)
                throw new Exception();

            byte[] buffer = new byte[4096];
            c.seq = seqnum;
            c.destination = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]); // Set destination of command
            buffer = Helper.ToByteArray<Command>(c);
            Log.command(c);
            client.GetStream().Write(buffer, 0, buffer.Length);
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
}
