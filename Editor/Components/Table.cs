using Editor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Components
{
    public class Table : IComponent
    {
        public List<BackupJob> BackupJons { get; set; }

        public int SelectedIndex { get; set; } = 0;

        public event Action<int> Selected;

        public Table(List<BackupJob> backupJons)
        {
            this.BackupJons = backupJons;
        }

        public void HandleKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.DownArrow)
            {
                this.KeyDown();
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                this.KeyUp();
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {

            }
            else if (info.Key == ConsoleKey.RightArrow)
            {

            }
            else if (info.Key == ConsoleKey.Enter)
            {
                this.Select();
            }
        }

        private void KeyUp()
        {
            this.SelectedIndex = Math.Max(--this.SelectedIndex, 0);
        }

        private void KeyDown()
        {
            this.SelectedIndex = Math.Min(++this.SelectedIndex, this.BackupJons.Count - 1);
        }

        private void Select()
        {
            this.Selected?.Invoke(this.SelectedIndex);
        }

        public void Draw()
        {
            this.DrawBorder();
        }

        public void DrawBorder()
        {
            Console.Write("┌");
            for (int i = 1; i < Console.WindowWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("╥");
            for (int i = Console.WindowWidth / 2; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");

            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(0 , i);
                Console.Write("│");
                Console.SetCursorPosition(Console.WindowWidth / 2, i);
                Console.Write("║");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("│");
            }

            Console.Write("└");
            for (int i = 1; i < Console.WindowWidth / 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("╨");
            for (int i = Console.WindowWidth / 2; i < Console.WindowWidth - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");
        }
    }
}
