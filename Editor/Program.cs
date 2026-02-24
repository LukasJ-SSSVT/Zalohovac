namespace Editor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // use stack for windows
            // don't use console.writeline but cursore position
            Console.CursorVisible = false;

            Application app = new Application();
            app.Run();
        }
    }
}