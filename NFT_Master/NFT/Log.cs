﻿using System;

/// <summary>
/// Constructs and displays log messages to standard output
/// </summary>
public class Log
{
    public static bool logToFile = false;
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
    public static void error(string message)
    {
        title();
        timestamp();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[Error] ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
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
        Console.WriteLine("seq={2} type={0} sender={1} destination={2}", c.type, c.sender, c.seq, c.reciever);
    }

    private static void title()
    {
        if (identifier != "")
            Console.Write("[{0}] ", identifier);
    }
    private static void timestamp()
    {
        if (longTimestamp)
            Console.Write("[{0}] ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
        else
            Console.Write("[{0}] ", DateTime.Now.ToString("HH:mm:ss.fff"));
    }
}