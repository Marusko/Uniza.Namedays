namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class ConsoleViewer
    {
        private NamedayCalendar _calendar;
        private int _choice;

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
            Console.WriteLine($"Dnes {DateTime.Now.ToShortDateString()} " + (_calendar[DateTime.Now].Equals("0") ? "nemá nikto meniny" : $"má meniny: {string.Join(", ", _calendar[DateTime.Now])}"));
            Console.WriteLine("Zajtra " + (_calendar[DateTime.Now.AddDays(1)].Equals("0") ? "nemá nikto meniny" : $"má meniny: {string.Join(", ", _calendar[DateTime.Now.AddDays(1)])}"));
            Console.WriteLine();
            Console.Write("Menu\n1 - načítať kalendár" +
                              "\n2 - zobraziť štatistiku" +
                              "\n3 - vyhľadať mená" +
                              "\n4 - vyhľadať mená podľa dátumu" +
                              "\n5 - zobraziť kalendár mien v mesiaci" +
                              "\n6 | Escape - koniec" +
                              "\nVaša voľba: "); 
            
            
            if (int.TryParse(Console.ReadLine(), out _choice))
            {
                Console.Clear();
                switch (_choice)
                {
                    case 1: Load();
                        break;
                    case 2: ShowStatistics();
                        break;
                    case 3: SearchNames();
                            break;
                    case 4: SearchNamesByDate();
                            break;
                    case 5: ShowNamedaysInMonth();
                            break;
                    default: Environment.Exit(0);
                            break;
                }
            }
            else
            {
                Console.Clear();
                Show();
            }
        }

        private void Load()
        {
            Console.WriteLine("NAČÍTANIE");
            Console.WriteLine("Zadajte cestu ku súboru: ");
            FileInfo cesta = new FileInfo(Console.ReadLine());
            if (!cesta.Exists)
            {
                Console.WriteLine("Zadaný súbor neexistuje!");
                Console.WriteLine("Zadajte cestu ku súboru: ");
                cesta = new FileInfo(Console.ReadLine());
            }
            else
            {
                if (!cesta.Extension.ToLower().Equals(".csv"))
                {
                    Console.WriteLine($"Zadaný súbor {cesta.Name} nie je typu CSV!");
                    Console.WriteLine("Zadajte cestu ku súboru: ");
                    cesta = new FileInfo(Console.ReadLine());
                }
                else
                {
                    _calendar.Clear();
                    _calendar.Load(cesta);
                    Console.WriteLine("Kalendár bol načítaný.\nPre pokračovanie stlačte Enter");
                    /*if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        
                    }*/

                    while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                    {
                        
                    }
                    Console.Clear();
                    Show();
                }
            }
        }

        private void ShowStatistics()
        {
            Console.WriteLine("ŠTATISTIKA");
        }

        private void SearchNames()
        {
            Console.WriteLine("VYHĽADÁVENIE MIEN");
        }

        private void SearchNamesByDate()
        {
            Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU");
        }

        private void ShowNamedaysInMonth()
        {
            Console.WriteLine("KALENDÁR MENÍN");
        }
    }
}
