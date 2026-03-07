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
        public event Action<List<string>> Save;

        public event Action End;

        private event Action keyPressed;

        public FileViewerWindow(List<string> paths, Application app)
        {
            this.Application = app;

            this.keyPressed += this.Clear;

            this.ComponentOffset = Console.BufferWidth / 2 + 2;

            foreach (string path in paths)
            {
                //Button button = new Button(path, 1);
                //button.Deleted += this.DeletePath;
                //this.Components.Add(button);

                this.Components.Add(this.ButtonBuilder(path, 1, () => { }, this.DeletePath, ""));
            }


            //Button buttonOK = new Button("OK", 1);
            //buttonOK.Clicked += this.ButtonOK;
            //this.Components.Add(buttonOK);
            this.Components.Add(this.ButtonBuilder("OK", 1, this.ButtonOK, () => { }, ""));

            //Button buttonCancel = new Button("Cancel", 1);
            //buttonCancel.Clicked += this.ButtonCancel;
            //this.Components.Add(buttonCancel);
            this.Components.Add(this.ButtonBuilder("Cancel", 1, this.ButtonCancel, () => { }, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.Application.DrawBorder();
            this.Draw();
        }

        public override void Draw()
        {
            Console.ResetColor();
            this.Clear();

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
            if (info.Key == ConsoleKey.DownArrow)
            {
                this.KeyDown();
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                this.KeyUp();
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                this.KeyLeft();
            }
            else
            {
                this.Components[this.SelectedIndex].HandleKey(info);
            }
        }

        private void KeyUp()
        {
            this.keyPressed?.Invoke();
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.keyPressed?.Invoke();
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        private void KeyLeft()
        {
            FileSelectorWindow fileSelectorWindow = new FileSelectorWindow(this.Components[0].Label, this.Application);
            fileSelectorWindow.DirectoryAdded += this.DirectoryAdded;
            this.Application.SwitchWindowForward(fileSelectorWindow);
        }

        public void DirectoryAdded(string path)
        {
            //Button button = new Button(path, 1);
            //button.Deleted += this.DeletePath;
            this.Components.Insert(this.Components.Count() - 2, this.ButtonBuilder(path, 1, () => { }, this.DeletePath, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.Draw();
        }

        private void ButtonOK()
        {
            List<string> paths = new List<string>();

            for (int i = 0; i < this.Components.Count - 2; i++)
            {
                paths.Add(this.Components[i].Label);
            }

            this.Save?.Invoke(paths);

            this.End?.Invoke();
        }

        private void ButtonCancel()
        {
            this.End?.Invoke();
        }

        private void DeletePath()
        {
            this.Components.RemoveAt(this.SelectedIndex);
            this.ComponentPositionsVertical(this.ComponentOffset);
        }
    }
}
