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

        public ConfigInfoWindow(BackupJob backupJob)
        {
            this.backupJob = backupJob;

            this.ComponentOffset = Console.BufferWidth / 2 + 3;

            this.IsOnLeft = false;

            Textbox textbox = new Textbox(this.backupJob.GetPropertyNames()[0], this.backupJob.Name, 2, new Point(3, 1));
            textbox.TextChanged += this.ChangeText;
            this.Components.Add(textbox);

            Button buttonMethod = new Button(this.backupJob.GetPropertyNames()[1], 2) { Text = this.backupJob.Method.ToString() };
            buttonMethod.Clicked += this.ButtonMethod;
            this.Components.Add(buttonMethod);

            Button buttonTiming = new Button(this.backupJob.GetPropertyNames()[2], 2) { Text = this.backupJob.Timing.ToString() };
            buttonTiming.Clicked += this.ButtonTiming;
            this.Components.Add(buttonTiming);

            Button buttonRetention = new Button(this.backupJob.GetPropertyNames()[3], 2) { Text = $"Počet záloh: {this.backupJob.Retention.Count.ToString()} o velikosti: {this.backupJob.Retention.Size.ToString()}" };
            buttonRetention.Clicked += this.ButtonRetention;
            this.Components.Add(buttonRetention);

            Button buttonSources = new Button(this.backupJob.GetPropertyNames()[4], 2); //{ Text = string.Join(",", this.backupJob.Sources).Substring(0, 20) + "..." };
            buttonSources.Clicked += this.ButtonSources;
            this.Components.Add(buttonSources);

            Button buttonTargets = new Button(this.backupJob.GetPropertyNames()[5], 2); //{ Text = string.Join(",", this.backupJob.Targets).Substring(0, 20) + "..." };
            buttonTargets.Clicked += this.ButtonTargets;
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

        private void ButtonMethod()
        {
            List<string> methods = Enum.GetValues(typeof(BackupType)).Cast<BackupType>().Select(v => v.ToString()).ToList();
            List<Component> components = new List<Component>();

            foreach (string method in methods)
            {
                Button button = new Button(method, 1);
                button.Clicked += this.EditWindowClick;
                components.Add(button);
            }

            this.Application.SwitchWindowForward(new EditWindow("Choose a method", components, 60, 10));
        }

        private void ButtonTiming()
        {
            List<Component> components = new List<Component>();
            Textbox textbox = new Textbox("", this.Components[2].Text, 1, new Point(0, 0));
            textbox.Clicked += this.EditWindowClick;
            components.Add(textbox);

            this.Application.SwitchWindowForward(new EditWindow("Create cron", components, 40, 10));
        }

        private void ButtonRetention()
        {
            List<Component> components = new List<Component>();
            Textbox textboxCount = new Textbox("", this.backupJob.Retention.Count.ToString(), 1, new Point(0, 0));
            components.Add(textboxCount);
            Textbox textboxSize = new Textbox("", this.backupJob.Retention.Size.ToString(), 1, new Point(0, 0));
            components.Add(textboxSize);
            Button button = new Button("OK", 1);
            button.Clicked += this.RetentionClick;
            components.Add(button);

            this.Application.SwitchWindowForward(new EditWindow("Imput amount of backups and their size", components, 60, 10));
        }

        private void ButtonSources()
        {
            FileSelectorWindow selectorWindow = new FileSelectorWindow(this.backupJob.Sources, this.Application, true);
            selectorWindow.SaveSource += this.SaveSource;
            selectorWindow.End += this.RedrawTablePuhUp;
            this.Application.SwitchWindowForward(selectorWindow);
        }

        private void ButtonTargets()
        {
            FileSelectorWindow selectorWindow = new FileSelectorWindow(this.backupJob.Targets, this.Application, false);
            selectorWindow.SaveTarget += this.SaveTarget;
            selectorWindow.End += this.RedrawTablePuhUp;
            this.Application.SwitchWindowForward(selectorWindow);
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
            this.Application.SwitchWindowBack();
        }
    }
}