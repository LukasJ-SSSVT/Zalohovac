using System.Text;

namespace Editor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            Application app = new Application();
            app.Run();
        }
    }
}