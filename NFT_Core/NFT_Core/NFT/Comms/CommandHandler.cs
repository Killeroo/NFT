using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using NFT.Core;
using NFT.Logger;
using NFT.Rsync;

namespace NFT.Comms
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
            switch (c.type)
            {
                case CommandType.Synchronize:


                    break;

                case CommandType.RsyncStream:

                    if (c.stream.type == StreamType.Signature)
                    {
                        // Display
                        Log.Command(c);
                        Log.Stream(c.stream);

                        // Generate delta
                        MemoryStream deltaStream = RsyncOps.GenerateDelta(c.stream.stream, c.stream.relativePath);
                        RsyncStream rs = new RsyncStream(StreamType.Delta, deltaStream, c.stream.relativePath);
                        Command replyCommand = new Command(CommandType.RsyncStream, c.source.ToString());
                        replyCommand.AddStream(rs);

                        // Send delta back to source
                        CommandHandler.Send(replyCommand, client, c.seq++);
                    }
                    else if (c.stream.type == StreamType.Delta)
                    {
                        // Display
                        Log.Command(c);
                        Log.Stream(c.stream);

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
                c.seq = seqnum;
                c.destination = IPAddress.Parse(client.Client.RemoteEndPoint.ToString().Split(':')[0]); // Set destination of command
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