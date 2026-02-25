using Editor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component = Editor.Components.Component;

namespace Editor.Windows
{
    public class EditWindow : Window
    {
        private string label;

        private int windowWidth;

        private int windowHeight;

        public EditWindow(string label, List<Component> components, int windowWidth, int windowHeight)
        {
            this.label = label;
            this.Components = components;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            this.ComponentPositionsHorizontal(this.windowWidth, this.windowHeight);
        }

        public override void Draw()
        {
            this.ComponentPositionsHorizontal(this.windowWidth, this.windowHeight);
            this.MakeBackground();

            Console.SetCursorPosition(Console.WindowWidth / 2 - this.label.Length / 2, Console.WindowHeight / 2 - this.windowHeight / 2 + 1);
            Console.Write(this.label);

            int i = 0;
            foreach (Component component in this.Components)
            {
                if (i++ == this.SelectedIndex)
                {
                    this.HighlightButton(ConsoleColor.Blue);
                }
                component.Draw();
                Console.BackgroundColor = ConsoleColor.White;
            }
        }

        public void HighlightButton(ConsoleColor color)
        {
            Console.BackgroundColor = color;

            for (int j = -1; j <= 1; j++)
            {
                for (int i = Console.WindowWidth / 2 - this.windowWidth / 2 + (this.SelectedIndex * this.windowWidth / this.Components.Count) + 1; i < Console.WindowWidth / 2 - this.windowWidth / 2 + (this.SelectedIndex * this.windowWidth / this.Components.Count) + this.windowWidth / this.Components.Count - 1; i++)
                {
                    Console.SetCursorPosition(i, this.Components[this.SelectedIndex].Location.Y + j);
                    Console.Write(" ");
                }
            }
        }

        public void MakeBackground()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = Console.WindowHeight / 2 - this.windowHeight / 2; j < Console.WindowHeight / 2 + this.windowHeight / 2; j++)
            {
                for (int i = Console.WindowWidth / 2 - this.windowWidth / 2; i < Console.WindowWidth / 2 + this.windowWidth / 2; i++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write(" ");
                }
            }
        }

        public override void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.RightArrow)
            {
                this.KeyLeft();
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                this.KeyRight();
            }
            else
            {
                this.Components[this.SelectedIndex].HandleKey(info);
            }
        }

        private void KeyRight()
        {
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyLeft()
        {
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }
    }
}
