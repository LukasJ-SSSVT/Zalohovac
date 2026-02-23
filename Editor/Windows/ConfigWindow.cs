using Editor.Components;
using Editor.Models;
using Editor.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Windows
{
    public class ConfigWindow : Window
    {
        private List<BackupJob> backupJobs = new List<BackupJob>();

        private DataService service = new DataService();

        private int selectedIndex = 0;

        private List<IComponent> components = new List<IComponent>();

        public ConfigWindow()
        {
            this.backupJobs = this.service.GetAllBackupJobs();

            int i = 2;
            foreach (BackupJob backupJob in this.backupJobs)
            {
                Button button = new Button(new Point(3, i), backupJob.Id.ToString());
                button.Clicked += this.ButtonClicked;
                this.components.Add(button);
                i = i + 3;
            }
        }

        public override void Draw()
        {
            int i = 0;
            foreach (Button button in this.components)
            {
                this.ChangeBackgroundColor(button.Location, ConsoleColor.Black);

                if (i++ == this.selectedIndex)
                {
                    this.ChangeBackgroundColor(button.Location, ConsoleColor.Blue);
                }
                button.Draw();
                Console.ResetColor();
            }
        }

        private void ChangeBackgroundColor(Point location, ConsoleColor color)
        {
            Console.BackgroundColor = color;

            for (int j = -1; j < 2; j++)
            {
                Console.SetCursorPosition(location.X - 1, location.Y + j);
                for (int i = 0; i < Console.WindowWidth / 2 - 3; i++)
                {
                    Console.Write(" ");
                }
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
            else
            {
                this.components[this.selectedIndex].HandleKey(info);
            }
        }
       
        private void KeyUp()
        {
            this.selectedIndex = Math.Max(--this.selectedIndex, 0);
        }

        private void KeyDown()
        {
            this.selectedIndex = Math.Min(++this.selectedIndex, this.backupJobs.Count - 1);
        }

        private void ButtonClicked()
        {
            this.Application.SwitchWindow(new ConfigInfoWindow());
        }
    }
}
