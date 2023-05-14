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
            //TODO esc
            
            if (int.TryParse(Console.ReadLine(), out _choice))
            {
                Console.Clear();
                switch (_choice)
                {
                    case 1: Console.WriteLine("NAČÍTANIE\nZadajte cestu k súboru kalendára mien alebo stlačte enter pre ukončenie");
                            Load();
                            break;
                    case 2: ShowStatistics();
                            break;
                    case 3: SearchNames();
                            break;
                    case 4: Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU\nPre ukončenie stlačte Enter");
                            SearchNamesByDate();
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
            Console.WriteLine("Zadajte cestu ku súboru: ");
            FileInfo? cesta = null;
            try
            {
                cesta = new FileInfo(Console.ReadLine() ?? throw new InvalidOperationException());
            }
            catch (Exception)
            {
                Console.Clear();
                Show();
            }
            if (cesta != null && !cesta.Exists)
            {
                Console.WriteLine("Zadaný súbor neexistuje!");
                Load();
            }
            else
            {
                if (cesta != null && !cesta.Extension.ToLower().Equals(".csv"))
                {
                    Console.WriteLine($"Zadaný súbor {cesta.Name} nie je typu CSV!");
                    Load();
                }
                else
                {
                    _calendar.Clear();
                    if (cesta != null) _calendar.Load(cesta);
                    Console.WriteLine("Kalendár bol načítaný.\nPre pokračovanie stlačte Enter");
                    
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
            //TODO ukoncenie na jeden riadok
            string? input;
            Console.Write("Zadajte deň a mesiac: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Show();
            }
            var splitted = input?.Split(".");
            int day;
            int month;
            int.TryParse(splitted?[0], out day);
            int.TryParse(splitted?[1], out month);
            if (_calendar[day, month][0].Equals("0")) 
            {
                Console.WriteLine("Neboli nájdené žiadne mená!");
                SearchNamesByDate();
            }

            for (int i = 0; i < _calendar[day, month].Length; i++)
            {
                Console.WriteLine($"  {i+1}. {_calendar[day, month][i]}");
            }
            SearchNamesByDate();
        }

        private void ShowNamedaysInMonth()
        {
            Console.WriteLine("KALENDÁR MENÍN");
        }
    }
}
