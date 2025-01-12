using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon
{
    public static class Logger
    {
        public static void Log(string? str, ConsoleColor color)
        {
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[CSharp] {str}");
            //Natives.CSharpPrint(str);
            Console.ForegroundColor = orig;
        }
        public static void Log(string? str) => Log(str, ConsoleColor.DarkGreen); 
        public static void Log(object str) => Log(str.ToString(), ConsoleColor.DarkGreen); 
        public static void Warn(string? str) => Log($"[Warn] {str}", ConsoleColor.DarkYellow);
        public static void Warn(object str) => Log($"[Warn] {str.ToString()}", ConsoleColor.DarkYellow);
        public static void Error(string? str) => Log($"[Error] {str}", ConsoleColor.DarkRed);
        public static void Error(object str) => Log($"[Error] {str.ToString()}", ConsoleColor.DarkRed);

    }
}
