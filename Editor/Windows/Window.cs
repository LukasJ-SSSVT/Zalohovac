using Editor.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Windows
{
    public abstract class Window
    {
        public Application Application { get; set; }

        public abstract void HandleKey(ConsoleKeyInfo info);

        public List<Component> Components = new List<Component>();

        public int SelectedIndex = 0;

        public bool IsOnLeft;

        public void Draw()
        {
            this.Clear(IsOnLeft);

            int i = 0;
            foreach (Component component in this.Components)
            {
                if (i++ == this.SelectedIndex)
                {
                    this.ChangeBackgroundColor(component.Location, component.Height, ConsoleColor.Blue);
                }
                component.Draw();
                Console.ResetColor();
            }
        }

        public void ChangeBackgroundColor(Point location, int height, ConsoleColor color)
        {
            Console.BackgroundColor = color;

            for (int j = -1; j < height + 1; j++)
            {
                Console.SetCursorPosition(location.X - 1, location.Y + j);

                for (int i = 0; i < Console.WindowWidth / 2 - 3; i++)
                {
                    Console.Write(" ");
                }
            }
        }

        public void Clear(bool leftSide)
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (int j = 1; j < Console.WindowHeight - 1; j++)
            {
                if (leftSide)
                {
                    for (int i = 1; i < Console.WindowWidth / 2; i++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(" ");
                    }
                }
                else
                {
                    for (int i = Console.WindowWidth / 2 + 1; i < Console.WindowWidth - 1; i++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(" ");
                    }
                }
            }
        }
    }
}
