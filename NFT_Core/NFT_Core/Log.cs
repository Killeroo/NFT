using System;

/// <summary>
/// Constructs and displays log messages to standard output
/// </summary>
public class Log
{
    public static bool logToFile = false;
    public static bool showTimestamp = true;
    public static bool longTimestamp = false;
    public static string identifier = "";

    public static void info(string message)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[Info] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }
    public static void error(Error err)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Error] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        if (err.message != "")
            Console.Write("msg=\"{0}\" ", err.message); // Show message if set
        Console.WriteLine("type={0}", err.type);
        //if (err.ex is Exception)
            //Console.WriteLine(err.ex.ToString()); // Print stacktrace if general exception 
    }
    public static void remoteError(Error err)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("[SlaveError] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("Sender=\"{0}\" ", err.senderAddr);
        if (err.message != "")
            Console.Write("msg=\"{0}\" ", err.message); // Show message if set
        Console.Write("type={0} ", err.type);
        Console.WriteLine("fatal={0}", err.fatal);
    }
    public static void fatal(string message)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("[Fatal] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
        Console.WriteLine("Press any key to exit...");
        Console.Read();

        // Exit program
        System.Environment.Exit(1);
    }
    public static void command(Command c)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[Command] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("seq={2} type={0} sender={1} destination={3}", c.type, c.sender, c.seq, c.reciever);

        // Display message if exists
        if (c.message != "" && c.type == CommandType.Info)
            Console.WriteLine(" msg=\"{0}\"", c.message);
        else
            Console.WriteLine();
    }

    private static void title()
    {
        if (identifier != "")
            Console.Write("[{0}]", identifier);
    }
    private static void timestamp()
    {
        if (showTimestamp)
        {
            if (longTimestamp)
                Console.Write("[{0}]", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
            else
                Console.Write("[{0}]", DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}