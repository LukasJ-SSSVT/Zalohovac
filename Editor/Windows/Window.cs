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

        public int ComponentOffset { get; set; }

        public abstract void HandleKey(ConsoleKeyInfo info);

        public List<Component> Components = new List<Component>();

        public int SelectedIndex = 0;

        public bool IsOnLeft;

        public abstract void Draw();       

        public void HiglightRow(Point location, int height, ConsoleColor color)
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

        public void ComponentPositionsVertical(int offset)
        {
            int i = 2;
            foreach (Component component in this.Components)
            {
                component.Location = new Point(offset, i);
                i = i + component.Height + 2;
            }
        }

        public void ComponentPositionsHorizontal(int windowWidth)
        {
            for (int i = 0; i < this.Components.Count; i++)
            {
                this.Components[i].Location = new Point((Console.WindowWidth / 2 - windowWidth / 2 + windowWidth / this.Components.Count * i) + windowWidth / this.Components.Count / 2 - this.Components[i].Label.Length / 2, Console.WindowHeight / 2 + 1);
            }
        }
    }
}
