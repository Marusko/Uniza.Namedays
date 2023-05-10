namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleViewer cv = new ConsoleViewer(new FileInfo(args[0]));
            //ConsoleViewer cv = new ConsoleViewer();
            cv.Show();
        }
    }
}