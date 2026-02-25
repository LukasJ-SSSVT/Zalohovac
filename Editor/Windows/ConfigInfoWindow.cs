using Editor.Components;
using Editor.Models;
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Editor.Windows
{
    public class ConfigInfoWindow : Window
    {
        private BackupJob backupJob = new BackupJob();

        private BackupType backupType = new BackupType();

        public event Action<BackupJob> UpdateJobs;

        public event Action RedrawTable;

        public ConfigInfoWindow(BackupJob backupJob)
        {
            this.backupJob = backupJob;

            this.ComponentOffset = Console.BufferWidth / 2 + 3;

            this.IsOnLeft = false;

            Textbox textbox = new Textbox("Name", this.backupJob.Name, 2);
            textbox.TextChanged += this.ChangeText;
            this.Components.Add(textbox);

            Button buttonMethod = new Button("Method", 2) { Text = this.backupJob.Method.ToString() };
            buttonMethod.Clicked += this.ButtonMethod;
            this.Components.Add(buttonMethod);

            Button buttonTiming = new Button("Timing", 2) { Text = this.backupJob.Timing.ToString() };
            this.Components.Add(buttonTiming);

            Button buttonRetention = new Button("Retention", 2) { Text = $"Počet záloh: {this.backupJob.Retention.Count.ToString()} o velikosti {this.backupJob.Retention.Size.ToString()}" };
            this.Components.Add(buttonRetention);

            Button buttonSources = new Button("Sources", 2); //{ Text = string.Join(",", this.backupJob.Sources).Substring(0, 20) + "..." };
            this.Components.Add(buttonSources);

            Button buttonTargets = new Button("Targets", 2); //{ Text = string.Join(",", this.backupJob.Targets).Substring(0, 20) + "..." };
            this.Components.Add(buttonTargets);

            Button buttonOK = new Button("OK", 1);
            buttonOK.Clicked += this.ButtonOK;
            this.Components.Add(buttonOK);

            Button buttonCancel = new Button("Cancel", 1);
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
            Console.ResetColor();
            this.Clear(this.IsOnLeft);

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
                Button button = new Button(method, 1);
                button.Clicked += this.EditWindowClick;
                components.Add(button);
            }

            this.Application.SwitchWindowForward(new MethodWindow("Choose a method", components, 60, 10));
        }

        private void EditWindowClick()
        {
            this.Components[this.SelectedIndex].Text = this.Application.Windows.Peek().Components[this.Application.Windows.Peek().SelectedIndex].Label;

            PropertyInfo property = this.backupJob.GetType().GetProperty(this.Components[this.SelectedIndex].Label);
            string stringProperty = this.Application.Windows.Peek().Components[this.Application.Windows.Peek().SelectedIndex].Label;

            object convertedProperty = stringProperty;
            if (property.PropertyType.IsEnum)
            {
                convertedProperty = Enum.Parse(property.PropertyType, stringProperty);
            }

            property.SetValue(this.backupJob, convertedProperty);

            this.RedrawTable?.Invoke();
            this.Application.SwitchWindowBack();
        }
    }
}
