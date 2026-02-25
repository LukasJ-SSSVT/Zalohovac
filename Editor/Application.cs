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
        public Stack<Window> Windows = new Stack<Window>();

        public Application()
        {   
            this.Windows.Push(new ConfigWindow(this));

            this.DrawBorder();
        }

        public void Run()
        {
            while (true)
            {
                this.Windows.Peek().Draw();

                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key == ConsoleKey.Escape)
                {
                    return;
                }

                this.Windows.Peek().HandleKey(info);
            }
        }

        public void SwitchWindowBack()
        {
            this.Windows.Pop();
        }

        public void SwitchWindowForward(Window window)
        {
            window.Application = this;
            this.Windows.Push(window);
        }

        public void DrawBorder()
        {
            Console.ResetColor();

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
