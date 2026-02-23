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

        public Button(Point location)
        {
            this.Location = location;
        }

        public void Draw()
        {
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
