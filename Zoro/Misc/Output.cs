using System;

namespace Zoro.Misc
{
    internal class Output
    {
        public static void Success(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SUCCESS] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n");
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n");
        }

        public static void Information(string text)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n");
        }

        public static void Basic(string text)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[CONSOLE] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text + "\n");
        }
    }
}