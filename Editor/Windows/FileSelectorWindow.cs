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

        private event Action keyPressed;

        public FileSelectorWindow(string path, Application app)
        {
            this.keyPressed += this.Clear;

            this.Application = app;
            this.path = path;
            this.directories = new Directories(this.path);
            this.ComponentOffset = 3;

            this.AddComponents();
        }

        public override void Draw()
        {
            Console.ResetColor();
            if (this.SelectedIndex >= 0) { this.Clear(); }

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
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.keyPressed?.Invoke();
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        private void Select()
        {
            this.DirectoryAdded?.Invoke(this.Components[this.SelectedIndex].Label);
        }

        private void KeyRight()
        {
            this.Clear();
            this.SelectedIndex = -1;
            this.Draw();
            this.Application.SwitchWindowBack();
        }

        private void ButtonPressed()
        {
            this.path = this.Components[this.SelectedIndex].Label;
            this.directories = new Directories(this.path);
            this.AddComponents();
        }

        private void AddComponents()
        {
            this.Components.Clear();

            //Button buttonBack = new Button("..", 1);
            //buttonBack.Clicked += this.ButtonBack;
            //this.Components.Add(buttonBack);

            this.Components.Add(this.ButtonBuilder("..", 1, this.ButtonBack, () => { }, ""));

            foreach (DirectoryInfo item in directories)
            {
                //Button button = new Button(item.FullName, 1);
                //button.Clicked += this.ButtonPressed;
                //this.Components.Add(button);

                this.Components.Add(this.ButtonBuilder(item.FullName, 1, this.ButtonPressed, () => { }, ""));
            }

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.SelectedIndex = 0;
        }

        private void ButtonBack()
        {
            this.path = this.path.Substring(0, this.path.Length - this.path.Split('\\')[this.path.Split('\\').Count() - 1].Length - 1);
            this.directories = new Directories(this.path);
            this.AddComponents();
        }
    }
}
