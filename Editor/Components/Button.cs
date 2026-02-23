using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Components
{
    public class Button : IComponent
    {
        public string Label { get; set; }

        public event Action Clicked;

        public Point Location { get; set; }

        public Button(Point location, string label)
        {
            this.Location = location;
            this.Label = label;
        }

        public void Draw()
        {
            Console.SetCursorPosition(Location.X, Location.Y);
            Console.WriteLine(Label);
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
            {
                this.Clicked?.Invoke();
            }
        }
    }
}
