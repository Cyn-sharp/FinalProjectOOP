using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem2
{
    public static class ConsoleHelper
    {
        public static void WriteCentered(string text)
        {
            int screenWidth = Console.WindowWidth;
            int textWidth = text.Length;
            int padding = (screenWidth - textWidth) / 2;
            Console.WriteLine(new string(' ', padding) + text);
        }

        public static void WriteCentered(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            WriteCentered(text);
            Console.ResetColor();
        }
    }
}
