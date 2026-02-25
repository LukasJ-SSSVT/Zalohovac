using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Editor.Components
{
    public class Textbox : Component
    {
        public event Action<string> TextChanged;

        public event Action Clicked;

        private Point offset;

        public Textbox(string label, string value, int height, Point offset)
        {
            this.offset = offset;
            this.Label = label;
            this.Text = value;
            this.Height = height;
        }

        public override void Draw()
        {
            Console.SetCursorPosition(Location.X, Location.Y);
            Console.Write(Label);
            Console.SetCursorPosition(Location.X + offset.X, Location.Y + offset.Y);
            Console.Write(Text);
        }

        public override void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Backspace)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    this.Text = this.Text.Substring(0, this.Text.Length - 1);
                }
            }
            else if (info.Key == ConsoleKey.Enter)
            {
                this.Clicked?.Invoke();
            }
            else if (!char.IsControl(info.KeyChar))
            {
                this.Text += info.KeyChar;
            }

            this.TextChanged?.Invoke(this.Text);
        }
    }
}
