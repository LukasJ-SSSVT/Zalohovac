using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Models
{
    public class Frame
    {
        public void Draw()
        {
            Console.ResetColor();
            Console.Clear();

            Console.Write("┌");
            for (int i = 1; i < Console.WindowWidth / 2 - 1; i++)
            {
                Console.Write("─");
            }
            Console.Write("╥");
            for (int i = Console.WindowWidth / 2 - 1; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 1, i);
                Console.Write("║");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.Write("└");
            for (int i = 1; i < Console.WindowWidth / 2 - 1; i++)
            {
                Console.Write("─");
            }
            Console.Write("╨");
            for (int i = Console.WindowWidth / 2 - 1; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");
        }
    }
}
