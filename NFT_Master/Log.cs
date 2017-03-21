using System;

class Log
{
    public static bool logToFile = false;

    public static void info(string message)
    {
        Console.Write("[{0}] ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[Info] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }

    public static void error(string message)
    {
        Console.Write("[{0}] ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Error] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
    }

    public static void fatal(string message)
    {
        Console.Write("[{0}] ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.Write("[Fatal] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
        Console.WriteLine("Press any key to exit...");
        Console.Read();

        System.Environment.Exit(1);
    }

    public static void command(Command c)
    {
        Console.Write("[{0}] ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[Command] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("type={0} message={1} sender={2}", c.type, c.message, c.sender);
    }
}