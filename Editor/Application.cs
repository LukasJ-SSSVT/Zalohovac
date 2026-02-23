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

                Console.Clear();
            }
        }

        public void SwitchWindow(Window window)
        {
            window.Application = this;
            this.Window = window;
        }
    }
}
