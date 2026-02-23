using Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public class Application
    {
        public Window Window { get; set; }

        public Application()
        {
            this.SwitchWindow(new ConfigWindow());

            this.DrawBorder();
        }

        public void Run()
        {
            while (true)
            {
                this.Window.Draw();

                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key == ConsoleKey.Escape)
                {
                    return;
                }

                this.Window.HandleKey(info);
            }
        }

        public void SwitchWindow(Window window)
        {
            window.Application = this;
            this.Window = window;
        }

        public void DrawBorder()
        {
            Console.Write("┌");
            for (int i = 1; i < Console.WindowWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("╥");
            for (int i = Console.WindowWidth / 2; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2, i);
                Console.Write("║");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.Write("└");
            for (int i = 1; i < Console.WindowWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("╨");
            for (int i = Console.WindowWidth / 2; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");
        }
    }
}
