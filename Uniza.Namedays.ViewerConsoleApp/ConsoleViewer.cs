namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class ConsoleViewer
    {
        private NamedayCalendar _calendar;

        public ConsoleViewer(FileInfo cesta)
        {
            _calendar = new();
            if (!string.IsNullOrEmpty(cesta.ToString()))
            {
                _calendar.Load(cesta);
            }
        }

        public ConsoleViewer()
        {
            _calendar = new();
        }

        public void Show()
        {
            Console.WriteLine("KALENDÁR MIEN");
            Console.WriteLine($"Dnes {DateTime.Now.ToShortDateString()} " + (_calendar[DateTime.Now].Length == 0 ? "nemá nikto meniny" : $"má meniny: {string.Join(", ", _calendar[DateTime.Now])}"));
            Console.WriteLine("Zajtra " + (_calendar[DateTime.Now.AddDays(1)].Length == 0 ? "nemá nikto meniny" : $"má meniny: {string.Join(", ", _calendar[DateTime.Now.AddDays(1)])}"));
            Console.WriteLine();
            Console.Write("Menu\n1 - načítať kalendár" +
                              "\n2 - zobraziť štatistiku" +
                              "\n3 - vyhľadať mená" +
                              "\n4 - vyhľadať mená podľa dátumu" +
                              "\n5 - zobraziť kalendár mien v mesiaci" +
                              "\n6 | Escape - koniec" +
                              "\nVaša voľba: ");
        }
    }
}
