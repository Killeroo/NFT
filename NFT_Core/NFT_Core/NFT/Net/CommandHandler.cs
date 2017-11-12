using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using NFT.Core;
using NFT.Logger;
using NFT.Rsync;

namespace NFT.Net
{
    /// <summary>
    /// Static class for sending and handling Command messages
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Handle & execute a command
        /// </summary>
        /// <param name="c"></param>
        /// <param name="client"></param>
        public static void Handle(Command c, TcpClient client)
        {
            switch (c.Type)
            {
                case CommandType.Transfer:
                    Log.Command(c);

                    foreach (string path in c.Files)
                        Console.WriteLine(path);

                    break;

                case CommandType.Synchronize:


                    break;

                case CommandType.RsyncStream:

                    if (c.Stream.type == StreamType.Signature)
                    {
                        // Display
                        Log.Command(c);
                        Log.Stream(c.Stream);

                        // Generate delta
                        MemoryStream deltaStream = RsyncOps.GenerateDelta(c.Stream.stream, c.Stream.relativePath);
                        RsyncStream rs = new RsyncStream(StreamType.Delta, deltaStream, c.Stream.relativePath);
                        Command replyCommand = new Command(CommandType.RsyncStream, c.Source.ToString());
                        replyCommand.AddStream(rs);

                        // Send delta back to source
                        CommandHandler.Send(replyCommand, client, c.Seq++);
                    }
                    else if (c.Stream.type == StreamType.Delta)
                    {
                        // Display
                        Log.Command(c);
                        Log.Stream(c.Stream);

                        // Patch delta to file

                    }
                    else
                    {
                        Log.Error(new Error(new Exception(), "Recieved unknown rsync stream type"));
                    }

                    break;

                default:
                    Log.Command(c);
                    break;
            }
        }
        /// <summary>
        /// Send command to host
        /// </summary>
        /// <param name="c"></param>
        /// <param name="client"></param>
        /// <param name="seqnum"></param>
        public static void Send(Command c, TcpClient client, int seqnum)
        {
            try
            {
                // Check if connected first
                if (client == null)
                    throw new Exception();

                byte[] buffer = new byte[NFT.Core.Constants.COMMAND_BUFFER_SIZE];
                c.Seq = seqnum;
                c.Destination = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]); // Set destination of command
                buffer = Helper.ToByteArray<Command>(c);
                Log.Command(c);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
            catch (IOException e)
            {
                Log.Error(new Error(e, "Failed to send command"));
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(new Error(e, "Object failure"));
            }
            catch (Exception e)
            {
                Log.Error(new Error(e, "Could not send command"));
            }
        }
    }
}