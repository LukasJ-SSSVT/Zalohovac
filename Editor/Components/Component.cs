using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Components
{
    public abstract class Component
    {
        public abstract void HandleKey(ConsoleKeyInfo info);

        public abstract void Draw();

        public Point Location;

        public int Height;

        public string Label { get; set; }

        public string Text { get; set; }
    }
}
