using Editor.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component = Editor.Components.Component;

namespace Editor.Windows
{
    public class MethodWindow : Window
    {
        public MethodWindow(List<Component> components)
        {
            this.Components = components;

            this.ComponentPositionsHorizontal(50);
        }

        public override void Draw()
        {
            foreach (Component component in this.Components)
            {
                component.Draw();
            }
        }

        public override void HandleKey(ConsoleKeyInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
