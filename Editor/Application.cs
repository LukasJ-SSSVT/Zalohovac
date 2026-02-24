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
        public Stack<Window> Window = new Stack<Window>();

        public Application()
        {
            //this.SwitchWindow(new ConfigWindow());          
            this.Window.Push(new ConfigWindow(this));

            this.DrawBorder();
        }

        public void Run()
        {
            while (true)
            {
                this.Window.Peek().Draw();

                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key == ConsoleKey.Escape)
                {
                    return;
                }

                this.Window.Peek().HandleKey(info);
            }
        }

        public void SwitchWindowBack()
        {
            this.Window.Pop();
        }

        public void SwitchWindowForward(Window window)
        {
            window.Application = this;
            this.Window.Push(window);
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
