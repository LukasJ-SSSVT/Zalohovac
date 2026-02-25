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
            DirectoryInfo current = new DirectoryInfo(this.Path);
            DirectoryInfo[] subDirs;

            subDirs = current.GetDirectories();

            foreach (DirectoryInfo dir in subDirs)
            {
                yield return dir;
            }
        }
    }
}
