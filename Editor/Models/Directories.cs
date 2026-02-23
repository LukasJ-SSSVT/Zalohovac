using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Models
{
    public class Directories
    {
        public string Path { get; set; }

        public Directories(string path)
        {
            this.Path = path;
        }

        public IEnumerator<DirectoryInfo> GetEnumerator()
        {
            Stack<DirectoryInfo> stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(this.Path));

            while (stack.Count > 0)
            {
                DirectoryInfo current = stack.Pop();
                yield return current;

                foreach (DirectoryInfo subdir in current.GetDirectories())
                {
                    stack.Push(subdir);
                }
            }
        }
    }
}
