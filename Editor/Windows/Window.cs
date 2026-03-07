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

        public void Clear()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            for (int y = this.Components[this.SelectedIndex].Location.Y - 1; y < this.Components[this.SelectedIndex].Location.Y + this.Components[this.SelectedIndex].Height + 1; y++)
            {
                for (int x = this.Components[this.SelectedIndex].Location.X - 2; x < Console.WindowWidth / 2 + this.Components[this.SelectedIndex].Location.X - 4; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
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

        public void ComponentPositionsHorizontal(int windowWidth, int windowHeight)
        {
            for (int i = 0; i < this.Components.Count; i++)
            {
                int textLenght = this.Components[i].Label.Length;
                if (this.Components[i] is Textbox)
                {
                    textLenght = this.Components[i].Text.Length;
                }

                this.Components[i].Location = new Point((Console.WindowWidth / 2 - windowWidth / 2 + windowWidth / this.Components.Count * i) + windowWidth / this.Components.Count / 2 - textLenght / 2, Console.WindowHeight / 2 + windowHeight / 2 - 3);
            }
        }

        public Button ButtonBuilder(string label, int height, Action clicked, Action deleted, string text)
        {
            Button button = new Button(label, height) { Text = text };
            button.Clicked += clicked;
            button.Deleted += deleted;

            return button;
        }

        public Textbox TextboxBuilder(string label, int height, Action<string> textChanged, Action clicked, string text, Point offset)
        {
            Textbox textbox = new Textbox(label, text, height, offset);
            textbox.TextChanged += textChanged;
            textbox.Clicked += clicked;

            return textbox;
        }
    }
}
