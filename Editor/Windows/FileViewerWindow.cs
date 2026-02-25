using Editor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Windows
{
    public class FileViewerWindow : Window
    {
        public FileViewerWindow()
        {
            this.ComponentOffset = Console.BufferWidth / 2 + 3;

            this.IsOnLeft = false;

            Button buttonOK = new Button("OK", 1);
            buttonOK.Clicked += this.ButtonOK;
            this.Components.Add(buttonOK);

            Button buttonCancel = new Button("Cancel", 1);
            buttonCancel.Clicked += this.ButtonCancel;
            this.Components.Add(buttonCancel);

            this.ComponentPositionsVertical(this.ComponentOffset);
        }

        public override void Draw()
        {
            Console.ResetColor();
            this.Clear(IsOnLeft);

            int i = 0;
            foreach (Component component in this.Components)
            {
                if (i++ == this.SelectedIndex)
                {
                    this.HiglightRow(component.Location, component.Height, ConsoleColor.Blue);
                }
                component.Draw();
                Console.ResetColor();
            }
        }

        public override void HandleKey(ConsoleKeyInfo info)
        {
            
        }

        public void DirectoryAdded(string path)
        {
            this.Components.Insert(this.Components.Count() - 2, new Button(path, 1));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.Draw();
        }

        private void ButtonOK()
        {
            
        }

        private void ButtonCancel()
        {

        }
    }
}
