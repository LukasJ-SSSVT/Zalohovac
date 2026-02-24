using Editor.Components;
using Editor.Models;
using System.Drawing;

namespace Editor.Windows
{
    public class ConfigInfoWindow : Window
    {
        private BackupJob backupJob = new BackupJob();

        public event Action<BackupJob> UpdateJobs;

        public ConfigInfoWindow(BackupJob backupJob)
        {
            this.backupJob = backupJob;

            this.ComponentOffset = Console.BufferWidth / 2 + 3;

            this.IsOnLeft = false;

            Textbox textbox = new Textbox(new Point(0,0), "Název", this.backupJob.Name, 2);
            textbox.TextChanged += this.ChangeText;
            this.Components.Add(textbox);

            Button buttonMethod = new Button(new Point(0,0), "Metoda", 2) { Text = this.backupJob.BackupType.ToString() };
            this.Components.Add(buttonMethod);

            Button buttonTiming = new Button(new Point(0, 0), "Časování", 2) { Text = this.backupJob.Timing.ToString() };
            this.Components.Add(buttonTiming);

            Button buttonRetention = new Button(new Point(0, 0), "Retence", 2) { Text = $"Počet záloh: {this.backupJob.BackupRetention.Count.ToString()} o velikosti {this.backupJob.BackupRetention.Size.ToString()}" };
            this.Components.Add(buttonRetention);

            Button buttonSources = new Button(new Point(0, 0), "Zdroje", 2); //{ Text = string.Join(",", this.backupJob.Sources).Substring(0, 20) + "..." };
            this.Components.Add(buttonSources);

            Button buttonTargets = new Button(new Point(0, 0), "Cíle", 2); //{ Text = string.Join(",", this.backupJob.Targets).Substring(0, 20) + "..." };
            this.Components.Add(buttonTargets);

            Button buttonOK = new Button(new Point(0, 0), "OK", 1);
            buttonOK.Clicked += this.ButtonOK;
            this.Components.Add(buttonOK);

            Button buttonCancel = new Button(new Point(0, 0), "Storno", 1);
            buttonCancel.Clicked += this.ButtonCancel;
            this.Components.Add(buttonCancel);

            this.ComponentPositions(this.ComponentOffset);
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

        private void KeyUp()
        {
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        public void ChangeText(string text)
        {
            this.backupJob.Name = text;
        }

        public void ButtonCancel()
        {
            this.Clear(this.IsOnLeft);
            this.Application.SwitchWindowBack();
        }

        public void ButtonOK()
        {
            this.Clear(this.IsOnLeft);
            this.UpdateJobs?.Invoke(this.backupJob);
            this.Application.SwitchWindowBack();
        }
    }
}
