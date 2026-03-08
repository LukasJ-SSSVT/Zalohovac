using Editor.Components;
using Editor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Editor.Windows
{
    public class ConfigInfoWindow : Window
    {
        private BackupJob backupJob = new BackupJob();

        public event Action<BackupJob> UpdateJobs;

        public event Action RedrawTable;

        private event Action keyPressed;

        public ConfigInfoWindow(BackupJob backupJob)
        {
            this.backupJob = backupJob;
            this.keyPressed += this.ClearBackground;

            this.ComponentOffset = Console.BufferWidth / 2 + 2;

            this.Components.Add(this.TextboxBuilder(this.backupJob.GetPropertyNames()[0], 2, this.ChangeText, () => { }, this.backupJob.Name, new Point(3, 1)));
            this.Components.Add(this.ButtonBuilder(this.backupJob.GetPropertyNames()[1], 2, this.ButtonMethod, () => { },
                this.backupJob.Method.ToString()
                ));
            this.Components.Add(this.ButtonBuilder(this.backupJob.GetPropertyNames()[2], 2, this.ButtonTiming, () => { },
                this.backupJob.Timing.ToString()
                ));
            this.Components.Add(this.ButtonBuilder(this.backupJob.GetPropertyNames()[3], 2, this.ButtonRetention, () => { },
                $"Počet záloh: {this.backupJob.Retention.Count.ToString()} o velikosti: {this.backupJob.Retention.Size.ToString()}"
                ));
            this.Components.Add(this.ButtonBuilder(this.backupJob.GetPropertyNames()[4], 2, this.ButtonSources, () => { },
                ""
                ));
            this.Components.Add(this.ButtonBuilder(this.backupJob.GetPropertyNames()[5], 2, this.ButtonTargets, () => { },
                ""
                ));
            this.Components.Add(this.ButtonBuilder("OK", 1, this.ButtonOK, () => { }, ""));
            this.Components.Add(this.ButtonBuilder("Cancel", 1, this.ButtonCancel, () => { }, ""));

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
            this.keyPressed?.Invoke();
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.keyPressed?.Invoke();
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.Components.Count - 1);
        }

        public void ChangeText(string text)
        {
            this.backupJob.Name = text;
        }

        public void ButtonCancel()
        {
            this.Application.DrawBorder();
            this.Application.SwitchWindowBack();
        }

        public void ButtonOK()
        {
            this.Application.DrawBorder();
            this.UpdateJobs?.Invoke(this.backupJob);
            this.Application.SwitchWindowBack();
        }

        private void ButtonMethod()
        {
            List<string> methods = Enum.GetValues(typeof(BackupType)).Cast<BackupType>().Select(v => v.ToString()).ToList();
            List<Component> components = new List<Component>();
            foreach (string method in methods)
            {
                components.Add(this.ButtonBuilder(method, 1, this.EditWindowClick, () => { }, ""));
            }

            this.Application.SwitchWindowForward(new EditWindow("Choose a method", components, 60, 10));
        }

        private void ButtonTiming()
        {
            List<Component> components = new List<Component>();
            components.Add(this.TextboxBuilder("", 1, (str) => { }, this.EditWindowClick, this.Components[2].Text, new Point(0, 0)));

            this.Application.SwitchWindowForward(new EditWindow("Create cron", components, 40, 10));
        }

        private void ButtonRetention()
        {
            List<Component> components = new List<Component>();
            components.Add(this.TextboxBuilder("", 1, (str) => { }, () => { }, this.backupJob.Retention.Count.ToString(), new Point(0, 0)));
            components.Add(this.TextboxBuilder("", 1, (str) => { }, () => { }, this.backupJob.Retention.Size.ToString(), new Point(0, 0)));
            components.Add(this.ButtonBuilder("OK", 1, this.RetentionClick, () => { }, ""));
            
            this.Application.SwitchWindowForward(new EditWindow("Imput amount of backups and their size", components, 60, 10));
        }

        private void ButtonSources()
        {
            this.RedrawTable?.Invoke();
            FileViewerWindow viewerWindow = new FileViewerWindow(this.backupJob.Sources, this.Application);
            viewerWindow.Save += this.SaveSource;
            viewerWindow.End += this.RedrawTablePuhUp;

            FileSelectorWindow fileSelectorWindow = new FileSelectorWindow(this.backupJob.Sources[0], this.Application);
            fileSelectorWindow.DirectoryAdded += viewerWindow.DirectoryAdded;

            this.Application.SwitchWindowForward(viewerWindow);
            this.Application.SwitchWindowForward(fileSelectorWindow);
        }

        private void ButtonTargets()
        {
            this.RedrawTable?.Invoke();
            FileViewerWindow viewerWindow = new FileViewerWindow(this.backupJob.Targets, this.Application);
            viewerWindow.Save += this.SaveTarget;
            viewerWindow.End += this.RedrawTablePuhUp;

            FileSelectorWindow fileSelectorWindow = new FileSelectorWindow(this.backupJob.Targets[0], this.Application);
            fileSelectorWindow.DirectoryAdded += viewerWindow.DirectoryAdded;

            this.Application.SwitchWindowForward(viewerWindow);
            this.Application.SwitchWindowForward(fileSelectorWindow);
        }

        private void EditWindowClick()
        {
            string value = this.Application.Windows.Peek().Components[this.Application.Windows.Peek().SelectedIndex].Label;
            if (this.Application.Windows.Peek().Components[this.Application.Windows.Peek().SelectedIndex] is Textbox) { value = this.Application.Windows.Peek().Components[this.Application.Windows.Peek().SelectedIndex].Text; }

            this.Components[this.SelectedIndex].Text = value;

            PropertyInfo property = this.backupJob.GetType().GetProperty(this.backupJob.GetPropertyNames()[this.SelectedIndex]);

            object convertedValue = value;
            if (property.PropertyType.IsEnum)
            {
                convertedValue = Enum.Parse(property.PropertyType, value);
            }

            property.SetValue(this.backupJob, convertedValue);

            this.RedrawTable?.Invoke();
            this.Application.SwitchWindowBack();
        }

        private void RetentionClick()
        {
            this.backupJob.Retention.Count = Convert.ToInt32(this.Application.Windows.Peek().Components[0].Text);
            this.backupJob.Retention.Size = Convert.ToInt32(this.Application.Windows.Peek().Components[1].Text);

            List<string> list = new List<string>();
            int j = 0;
            foreach (PropertyInfo propertyInfo in this.backupJob.Retention.GetType().GetProperties())
            {
                list.Add(this.Application.Windows.Peek().Components[j++].Text);
            }

            this.Components[this.SelectedIndex].Text = this.SetComponentText(list);

            this.RedrawTable?.Invoke();
            this.Application.SwitchWindowBack();
        }

        private string SetComponentText(List<string> list)
        {
            StringBuilder result = new StringBuilder();

            string[] str = this.Components[this.SelectedIndex].Text.Split(':');
            result.Append(str[0]);

            for (int i = 1; i < str.Length - 1; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(str[i].Substring(1));

                sb.Remove(0, str[i].Substring(1).IndexOf(' '));
                sb.Insert(0, list[i - 1]);

                result.Append(": " + sb);
            }

            return result.Append(": " + list[list.Count - 1]).ToString();
        }

        private void SaveSource(List<string> paths)
        {
            this.backupJob.Sources = paths;
        }

        private void SaveTarget(List<string> paths)
        {
            this.backupJob.Targets = paths;
        }

        private void RedrawTablePuhUp()
        {
            this.Application.SwitchWindowBack();
            this.RedrawTable?.Invoke();
        }
    }
}