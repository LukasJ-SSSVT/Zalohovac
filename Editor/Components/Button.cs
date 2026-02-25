using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Components
{
    public class Button : Component
    {
        public event Action Clicked;

        public event Action Deleted;

        public Button(string label, int height)
        {
            this.Label = label;
            this.Height = height;
        }

        public override void Draw()
        {
            Console.SetCursorPosition(Location.X, Location.Y);
            Console.Write(Label);
            Console.SetCursorPosition(Location.X + 3, Location.Y + 1);
            Console.Write(Text);
        }

        public override void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
            {
                this.Clicked?.Invoke();
            }
            if (info.Key == ConsoleKey.Delete)
            {
                this.Deleted?.Invoke();
            }
        }
    }
}
