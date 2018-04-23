using System;

using System.Drawing;
//using Colorful;
//using Console = Colorful.Console;

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

        //public static Color DefaultColour = Color.GhostWhite;
        //private static Formatter[] flags = new Formatter[]
        //{
        //    // Our flag colour rules
        //    new Formatter("[Info]", Color.NavajoWhite),
        //    new Formatter("[Warning]", Color.LightGoldenrodYellow),
        //    new Formatter("[Error]", Color.IndianRed),
        //    new Formatter("[SlaveError]", Color.DeepPink),
        //    new Formatter("[Fatal]", Color.HotPink),
        //    new Formatter("[Command]", Color.LawnGreen),
        //    new Formatter("[RsyncStream]", Color.Lavender)
        //};

        public static void Info(string message)
        {
            Console.WriteLine(Title() + Timestamp() + "[Info] " + message);//, DefaultColour, flags);
        }
        public static void Warning(string message)
        {
            Console.WriteLine(Title() + Timestamp() + "[Warning] " + message);//, DefaultColour, flags);
        }
        public static void Error(Error err)
        {
            Console.WriteLine(Title() + Timestamp() + "[Error] " + (err.message != "" ? "msg=\"" + err.message + "\" " : "") + "type=" + err.type);//, DefaultColour, flags);
        }
        public static void RemoteError(Error err)
        {
            Console.WriteLine(Title() + Timestamp() + "[SlaveError] " + (err.message != "" ? "msg=\"" + err.message + "\"" : "") + " type=" + err.type + " fatal=" + err.fatal);//, DefaultColour, flags);
        }
        public static void Fatal(string message)
        {
            Console.WriteLine(Title() + Timestamp() + "[Fatal] " + message);//, DefaultColour, flags);
            Console.WriteLine("Press any key to exit...");
            Console.Read();

            // Exit program
            System.Environment.Exit(1);
        }
        public static void Command(Command c)
        {
            Console.WriteLine(Title() + Timestamp() + "[Cmd] seq=" + c.Seq + " type=" + c.Type + " src=" + (c.Source.ToString() == Helper.GetLocalIPAddress() ? "[Me]" : c.Source.ToString()) + " dest=" + (c.Destination.ToString() == Helper.GetLocalIPAddress() ? "[Me]" : c.Destination.ToString()) + (c.Message != "" ? " msg=\"" + c.Message + "\"" : ""));//, DefaultColour, flags);
        }
        public static void Stream(NFT.Rsync.RsyncStream rs)
        {
            Console.WriteLine(Title() + Timestamp() + "[RsyncStream] type=" + rs.type + " file=" + rs.filename);//, DefaultColour, flags);
        }

        private static string Title()
        {
            if (Identifier != "")
                return "[" + Identifier + "]";
            else
                return "";
        }
        private static string Timestamp()
        {
            if (ShowTimestamp)
            {
                if (LongTimestamp)
                    return "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff") + "] ";
                else
                    return "[" + DateTime.Now.ToString("HH:mm:ss") + "] ";
            }
            else
            {
                return "";
            }
        }
    }
}