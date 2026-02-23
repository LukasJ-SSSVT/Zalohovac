using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Components
{
    public class Textbox : IComponent
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public event Action<string> TextChanged;

        public Point Location { get; set; }

        public Textbox(Point location)
        {
            this.Location = location;
        }

        public void Draw()
        {
            Console.WriteLine(Label);
            Console.WriteLine($"_{Value.PadRight(20, '_')}_");
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Backspace)
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    this.Value = this.Value.Substring(0, this.Value.Length - 1);
                }
            }
            else if (info.Key == ConsoleKey.Enter)
            {

            }
            else if (!char.IsControl(info.KeyChar))
            {
                this.Value += info.KeyChar;
            }

            this.TextChanged?.Invoke(this.Value);
        }
    }
}
