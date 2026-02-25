using Editor.Components;
using Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Windows
{
    public class FileSelectorWindow : Window
    {
        private string path;

        private Directories directories;

        public event Action<string> DirectoryAdded;

        private FileViewerWindow fileViewer;

        public FileSelectorWindow(string path, Application app)
        {
            this.Application = app;
            this.path = path;
            this.directories = new Directories(this.path);
            this.ComponentOffset = 3;

            this.AddComponents();

            this.IsOnLeft = true;

            FileViewerWindow viewerWindow = new FileViewerWindow() { SelectedIndex = - 1};
            this.DirectoryAdded += viewerWindow.DirectoryAdded;
            this.fileViewer = viewerWindow;
            this.fileViewer.Draw();
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
            if (info.Key == ConsoleKey.DownArrow)
            {
                this.KeyDown();
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                this.KeyUp();
            }
            else if (info.Key == ConsoleKey.Spacebar)
            {
                this.Select();
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                this.SelectedIndex = -1;
                this.fileViewer.SelectedIndex = 0;
                this.Draw();
                this.Application.SwitchWindowForward(this.fileViewer);
            }
            else
            {
                this.Components[this.SelectedIndex].HandleKey(info);
            }
        }

        private void KeyUp()
        {
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        private void Select()
        {
            this.DirectoryAdded?.Invoke(this.Components[this.SelectedIndex].Label);
        }

        private void ButtonPressed()
        {
            this.path = this.Components[this.SelectedIndex + 1].Label;
            this.directories = new Directories(this.path);
            this.AddComponents();
        }

        private void AddComponents()
        {
            this.Components.Clear();

            Button buttonBack = new Button("..", 1);
            buttonBack.Clicked += this.ButtonBack;
            this.Components.Add(buttonBack);

            foreach (DirectoryInfo item in directories)
            {
                Button button = new Button(item.FullName, 1);
                button.Clicked += this.ButtonPressed;
                this.Components.Add(button);
            }

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.SelectedIndex = 0;
        }

        private void ButtonBack()
        {
            this.directories = new Directories(this.path.Substring(0, this.path.Length - this.path.Split('\\')[this.path.Split('\\').Count() - 1].Length - 1));
            this.AddComponents();
        }
    }
}
