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

        public ConfigWindow(Application app)
        {
            this.Application = app;

            this.backupJobs = this.service.GetAllBackupJobs();

            this.ComponentOffset = 3;

            foreach (BackupJob backupJob in this.backupJobs)
            {
                Button button = new Button(backupJob.Name, 1);
                button.Clicked += this.ButtonClicked;
                button.Deleted += this.DeleteBackup;
                this.Components.Add(button);
            }

            Button buttonAdd = new Button("Create backup", 1);
            buttonAdd.Clicked += this.CreateBackup;
            this.Components.Add(buttonAdd);

            this.ComponentPositionsVertical(this.ComponentOffset);

            this.IsOnLeft = true;
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

        private void KeyUp()
        {
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        private void DeleteBackup()
        {
            if (this.SelectedIndex == this.Components.Count - 1) { return; }
            this.Components.RemoveAt(this.SelectedIndex);
            this.backupJobs.RemoveAt(this.SelectedIndex);

            this.ComponentPositionsVertical(this.ComponentOffset);
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

            Button button = new Button("New backup", 1);
            button.Clicked += this.ButtonClicked;
            this.Components.Insert(this.SelectedIndex, button);

            this.ComponentPositionsVertical(this.ComponentOffset);
        }

        private void Update(BackupJob backupJob)
        {
            this.backupJobs[this.SelectedIndex] = backupJob;
            this.Components[this.SelectedIndex].Label = this.backupJobs[this.SelectedIndex].Name.ToString();

            //this.service
        }

        private void RedrawTable()
        {
            this.Application.DrawBorder();
            this.Clear(this.IsOnLeft);
            this.Draw();
        }
    }
}
