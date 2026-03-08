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

        private event Action keyPressed;

        private int startIndex = 0; 
        private int displayedCount = Console.WindowHeight / 3 - 2;
        private bool hasScrolled = false;


        public ConfigWindow(Application app)
        {
            this.keyPressed += this.ClearBackground;

            this.Application = app;

            this.backupJobs = this.service.GetAllBackupJobs();

            this.ComponentOffset = 3;

            foreach (BackupJob backupJob in this.backupJobs)
            {
                this.Components.Add(this.ButtonBuilder(backupJob.Name, 1, this.ButtonClicked, this.DeleteBackup, ""));
            }
            this.Components.Add(this.ButtonBuilder("Create backup", 1, this.CreateBackup, () => { }, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);
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
                this.Components[this.SelectedIndex].HandleKey(info);
            }
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

        private void DeleteBackup()
        {
            if (this.SelectedIndex == this.Components.Count - 1) { return; }
            this.Components.RemoveAt(this.SelectedIndex);
            this.backupJobs.RemoveAt(this.SelectedIndex);

            this.ClearAll();

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.service.WirteJobs(this.backupJobs);
        }

        private void ButtonClicked()
        {
            ConfigInfoWindow configInfoWindow = new ConfigInfoWindow(this.backupJobs[this.SelectedIndex].Clone());
            configInfoWindow.UpdateJobs += this.Update;
            configInfoWindow.RedrawTable += this.RedrawTable;
            this.Application.SwitchWindowForward(configInfoWindow);
        }

        private void CreateBackup()
        {
            this.backupJobs.Insert(this.SelectedIndex, new BackupJob()
            {                
                Id = this.backupJobs[this.SelectedIndex - 1].Id + 1,
            });

            this.Components.Insert(this.SelectedIndex, this.ButtonBuilder("New backup", 1, this.ButtonClicked, this.DeleteBackup, ""));

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.service.WirteJobs(this.backupJobs);
        }

        private void Update(BackupJob backupJob)
        {
            this.backupJobs[this.SelectedIndex] = backupJob;
            this.Components[this.SelectedIndex].Label = this.backupJobs[this.SelectedIndex].Name.ToString();

            this.service.WirteJobs(this.backupJobs);
        }

        private void RedrawTable()
        {
            this.Application.DrawBorder();
            this.ClearBackground();
            this.Draw();
        }
    }
}
