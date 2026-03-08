using Editor.Components;
using Editor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        private event Action keyPressed;

        private int startIndex = 0;
        private int displayedCount = Console.WindowHeight / 3 - 2;
        private bool hasScrolled = false;

        public FileSelectorWindow(string path, Application app)
        {
            this.keyPressed += this.ClearBackground;

            this.Application = app;
            this.path = path;
            this.directories = new Directories(this.path);
            this.ComponentOffset = 3;

            this.AddComponents();

            this.ClearAll();
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
                component.Location = new Point(component.Location.X, (i - startIndex) * (component.Height + 2) + this.ComponentOffset);

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
            else if (info.Key == ConsoleKey.Spacebar)
            {
                this.Select();
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                this.KeyRight();
            }
            else
            {
                this.Components[this.SelectedIndex].HandleKey(info);
            }
        }

        private void KeyUp()
        {
            this.keyPressed?.Invoke();
            if (this.SelectedIndex > 1)
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

        private void Select()
        {
            if (this.SelectedIndex >= 2) { this.DirectoryAdded?.Invoke(this.Components[0].Label + '\\' + this.Components[this.SelectedIndex].Label); }
        }

        private void KeyRight()
        {
            this.ClearAll();
            this.ClearBackground();
            this.SelectedIndex = -1;
            this.Draw();
            this.Application.SwitchWindowBack();
        }

        private void ButtonPressed()
        {
            this.ClearAll();

            char character = Convert.ToChar(this.path.Substring(this.path.Length - 1));
            if (character != '\\') { this.path = this.Components[0].Label + '\\' + this.Components[this.SelectedIndex].Label; }
            else { this.path = this.Components[0].Label + this.Components[this.SelectedIndex].Label; }
            this.directories = new Directories(this.path);
            this.AddComponents();
        }

        private void AddComponents()
        {
            this.Components.Clear();

            this.Components.Add(this.ButtonBuilder(this.path, 1, () => { }, () => { }, ""));
            this.Components.Add(this.ButtonBuilder("..", 1, this.ButtonBack, () => { }, ""));

            foreach (DirectoryInfo item in directories)
            {
                this.Components.Add(this.ButtonBuilder(item.FullName.Split('\\')[item.FullName.Split('\\').Count() - 1], 1, this.ButtonPressed, () => { }, ""));
            }

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.SelectedIndex = 1;
        }

        private void ButtonBack()
        {
            this.ClearAll();
            this.path = this.path.Substring(0, this.path.Length - this.path.Split('\\')[this.path.Split('\\').Count() - 1].Length - 1);
            if (!this.path.Contains('\\')) { this.path += '\\'; }

            this.directories = new Directories(this.path);
            this.AddComponents();
        }
    }
}
