using Editor.Models;
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

        private Frame frame = new Frame();

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

                ConsoleKeyInfo info = Console.ReadKey(intercept: true);

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
            this.frame.Draw();
        }
    }
}
