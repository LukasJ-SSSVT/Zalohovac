using Editor.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        private int startIndex = 0;
        private int displayedCount = Console.WindowHeight / 3 - 2;
        private bool hasScrolled = false;

        public FileViewerWindow(List<string> paths, Application app)
        {
            this.Application = app;

            this.keyPressed += this.ClearBackground;

            this.ComponentOffset = Console.BufferWidth / 2 + 2;

            foreach (string path in paths)
            {
                this.Components.Add(this.ButtonBuilder(path, 1, () => { }, this.DeletePath, ""));
            }

            this.Components.Add(this.ButtonBuilder("OK", 1, this.ButtonOK, () => { }, ""));
            this.Components.Add(this.ButtonBuilder("Cancel", 1, this.ButtonCancel, () => { }, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.Application.DrawBorder();
            this.Draw();
        }

        public override void Draw()
        {
            Console.ResetColor();
            if (this.hasScrolled)
            {
                this.ClearAll();
                this.hasScrolled = false;
            }

            int endIndex = Math.Min(this.startIndex + this.displayedCount, this.Components.Count);

            for (int i = this.startIndex; i < endIndex; i++)
            {
                Component component = this.Components[i];
                component.Location = new Point(component.Location.X, (i - startIndex) * (component.Height + 2) + 3);

                if (i == this.SelectedIndex)
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
            if (this.SelectedIndex > 0)
            {
                this.SelectedIndex--;

                if (this.SelectedIndex < this.startIndex)
                {
                    this.startIndex--;
                    this.hasScrolled = true;
                }
            }
        }

        private void KeyDown()
        {
            this.keyPressed?.Invoke();
            if (this.SelectedIndex < this.Components.Count - 1)
            {
                this.SelectedIndex++;

                if (this.SelectedIndex >= this.startIndex + this.displayedCount)
                {
                    this.startIndex++;
                    this.hasScrolled = true;
                }
            }
        }

        private void KeyLeft()
        {
            FileSelectorWindow fileSelectorWindow = new FileSelectorWindow(this.Application);
            fileSelectorWindow.DirectoryAdded += this.DirectoryAdded;
            this.Application.SwitchWindowForward(fileSelectorWindow);
        }

        public void DirectoryAdded(string path)
        {
            this.Components.Insert(this.Components.Count() - 2, this.ButtonBuilder(path, 1, () => { }, this.DeletePath, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.ClearAll();

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
            this.ClearAll();

            this.Components.RemoveAt(this.SelectedIndex);
            this.ComponentPositionsVertical(this.ComponentOffset);
        }
    }
}
