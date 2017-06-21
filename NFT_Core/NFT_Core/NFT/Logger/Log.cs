using System;

using NFT.Core;

namespace NFT.Logger
{
    /// <summary>
    /// Constructs and displays log messages to standard output
    /// </summary>
    public class Log
    {
        public static bool LogToFile = false;
        public static bool ShowTimestamp = true;
        public static bool LongTimestamp = false;
        public static string Identifier = "";

        public static void Info(string message)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[Info] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
        }
        public static void Warning(string message)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warning] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
        }
        public static void Error(Error err)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (err.message != "")
                Console.Write("msg=\"{0}\" ", err.message); // Show message if set
            Console.WriteLine("type={0}", err.type);
        }
        public static void RemoteError(Error err)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[SlaveError] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Sender=\"{0}\" ", err.senderAddr);
            if (err.message != "")
                Console.Write("msg=\"{0}\" ", err.message); // Show message if set
            Console.Write("type={0} ", err.type);
            Console.WriteLine("fatal={0}", err.fatal);
        }
        public static void Fatal(string message)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("[Fatal] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.WriteLine("Press any key to exit...");
            Console.Read();

            // Exit program
            System.Environment.Exit(1);
        }
        public static void Command(Command c)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[Command] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("seq={2} type={0} src={1} dest={3}", c.type, c.source.ToString(), c.seq, c.destination.ToString());

            // Display message if exists
            if (c.message != "" && c.type == CommandType.Info)
                Console.WriteLine(" msg=\"{0}\"", c.message);
            else
                Console.WriteLine();
        }
        public static void Stream(NFT.Rsync.RsyncStream rs)
        {
            Title();
            Timestamp();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("[RsyncStream] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("type={0} file={1}", rs.type, rs.filename);
        }

        private static void Title()
        {
            if (Identifier != "")
                Console.Write("[{0}]", Identifier);
        }
        private static void Timestamp()
        {
            if (ShowTimestamp)
            {
                if (LongTimestamp)
                    Console.Write("[{0}]", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                else
                    Console.Write("[{0}]", DateTime.Now.ToString("HH:mm:ss"));
            }
        }
    }
}