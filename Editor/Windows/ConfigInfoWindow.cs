using Editor.Components;
using Editor.Models;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Editor.Windows
{
    public class ConfigInfoWindow : Window
    {
        private BackupJob backupJob = new BackupJob();

        private BackupType backupType = new BackupType();

        public event Action<BackupJob> UpdateJobs;

        public ConfigInfoWindow(BackupJob backupJob)
        {
            this.backupJob = backupJob;

            this.ComponentOffset = Console.BufferWidth / 2 + 3;

            this.IsOnLeft = false;

            Textbox textbox = new Textbox("Název", this.backupJob.Name, 2);
            textbox.TextChanged += this.ChangeText;
            this.Components.Add(textbox);

            Button buttonMethod = new Button("Metoda", 2) { Text = this.backupJob.BackupType.ToString() };
            buttonMethod.Clicked += this.ButtonMethod;
            this.Components.Add(buttonMethod);

            Button buttonTiming = new Button("Časování", 2) { Text = this.backupJob.Timing.ToString() };
            this.Components.Add(buttonTiming);

            Button buttonRetention = new Button("Retence", 2) { Text = $"Počet záloh: {this.backupJob.BackupRetention.Count.ToString()} o velikosti {this.backupJob.BackupRetention.Size.ToString()}" };
            this.Components.Add(buttonRetention);

            Button buttonSources = new Button("Zdroje", 2); //{ Text = string.Join(",", this.backupJob.Sources).Substring(0, 20) + "..." };
            this.Components.Add(buttonSources);

            Button buttonTargets = new Button("Cíle", 2); //{ Text = string.Join(",", this.backupJob.Targets).Substring(0, 20) + "..." };
            this.Components.Add(buttonTargets);

            Button buttonOK = new Button("OK", 1);
            buttonOK.Clicked += this.ButtonOK;
            this.Components.Add(buttonOK);

            Button buttonCancel = new Button("Storno", 1);
            buttonCancel.Clicked += this.ButtonCancel;
            this.Components.Add(buttonCancel);

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

        public void ButtonMethod()
        {
            List<string> methods = Enum.GetValues(typeof(BackupType)).Cast<BackupType>().Select(v => v.ToString()).ToList();
            List<Component> components = new List<Component>();

            foreach (string method in methods)
            {
                components.Add(new Button(method, 1));               
            }

            this.Application.SwitchWindowForward(new MethodWindow(components));
        }
    }
}
