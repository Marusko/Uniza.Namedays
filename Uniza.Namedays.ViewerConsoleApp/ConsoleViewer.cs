using System.Globalization;

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
           
            var key = Console.ReadKey().Key;
            Console.Clear();
            switch (key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.WriteLine("NAČÍTANIE\nZadajte cestu k súboru kalendára mien alebo stlačte enter pre ukončenie");
                    Load();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.WriteLine("ŠTATISTIKA");
                    ShowStatistics();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.WriteLine("VYHĽADÁVENIE MIEN\nPre ukončenie stlačte Enter");
                    SearchNames();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU\nPre ukončenie stlačte Enter");
                    SearchNamesByDate();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    ShowNamedaysInMonth(DateTime.Now);
                    break;
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    Show();
                    break;
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
            List<string> pismena = new List<string>();
            List<int> pocty = new List<int>();
            foreach (var nameday in _calendar.GetNamedays())
            {
                if (!pismena.Contains(nameday.Name.Substring(0, 1)) && !nameday.Name.Substring(0, 1).Equals("0"))
                {
                    pismena.Add(nameday.Name.Substring(0, 1));
                }

                if (!pocty.Contains(nameday.Name.Length) && !nameday.Name.Equals("0"))
                {
                    pocty.Add(nameday.Name.Length);
                }
            }
            pismena.Sort();
            pocty.Sort();
            Console.WriteLine($"Celkový počet mien v kalendári: {_calendar.NameCount}");
            Console.WriteLine($"Celkový počet dní obsahujúci mená v kalendári: {_calendar.DayCount}");
            Console.WriteLine("Celkový počet mien v jednotlivých mesiacoch:");
            for (int i = 1; i <= 12; i++)
            {
                int pocet = _calendar.GetNamedays(i).ToList().Count;
                Console.WriteLine($"  {DateTimeFormatInfo.CurrentInfo.GetMonthName(i)}: {pocet}");
            }

            Console.WriteLine("Počet mien podľa začiatočných písmen");
            foreach (var pismeno in pismena)
            {
                Console.WriteLine($"  {pismeno}: {_calendar.GetNamedays(pismeno).Count()}");
            }

            Console.WriteLine("Počet mien podľa dĺžky znakov");
            
            foreach (var i in pocty)
            {
                int p = _calendar.GetNamedays($@"\b.{{{i}}}\b").Count();
                Console.WriteLine($"  {i}: {p}");
            }
            Console.WriteLine("Pre ukončenie stlačte Enter");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {

            }
            Console.Clear();
            Show();
        }

        private void SearchNames()
        {
            string? input;
            Console.Write("Zadajte meno(regulárny výraz): ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.Clear();
                Show();
            }

            var pocet = _calendar.GetNamedays(input).ToList().Count;
            if (pocet > 0)
            {
                for (int i = 0; i < pocet; i++)
                {
                    var nameday = _calendar.GetNamedays(input).ToArray()[i];
                    Console.WriteLine($"  {i + 1}. {nameday.Name} ({nameday.DayMonth.Day}.{nameday.DayMonth.Month})");
                }
            }
            else
            {
                Console.WriteLine("Neboli nájdené žiadne mená!");
            }
            SearchNames();
        }

        private void SearchNamesByDate()
        {
            string? input;
            Console.Write("Zadajte deň a mesiac: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.Clear();
                Show();
            }

            if (!string.IsNullOrWhiteSpace(input))
            {
                var splitted = input?.Split(".");
                int day;
                int month;
                int.TryParse(splitted?[0], out day);
                int.TryParse(splitted?[1], out month);
                if (_calendar[day, month].Length == 0)
                {
                    Console.WriteLine("Neboli nájdené žiadne mená!");
                    SearchNamesByDate();
                }

                for (int i = 0; i < _calendar[day, month].Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {_calendar[day, month][i]}");
                }
            }
            else
            {
                Console.WriteLine("Neplatný formát dátumu!");
            }

            SearchNamesByDate();
        }

        private void ShowNamedaysInMonth(DateTime date)
        {
            Console.WriteLine("KALENDÁR MENÍN");
            var datum = new DateTime(date.Year, date.Month, 1);
            var pocetDni = DateTime.DaysInMonth(datum.Year, datum.Month);
            Console.WriteLine($"{DateTimeFormatInfo.CurrentInfo.GetMonthName(datum.Month)} {datum.Year}:");
            for (int i = 1; i <= pocetDni; i++)
            {
                if (datum == DateTime.Now.Date)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i}.{datum.Month} {datum:ddd} {string.Join(", ", _calendar[datum])}");
                    Console.ResetColor();
                }
                else if (datum.DayOfWeek == DayOfWeek.Saturday || datum.DayOfWeek == DayOfWeek.Sunday)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{i}.{datum.Month} {datum:ddd} {string.Join(", ", _calendar[datum])}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{i}.{datum.Month} {datum:ddd} {string.Join(", ", _calendar[datum])}");
                }
                datum = datum.AddDays(1);
            }

            Console.WriteLine("\nŠípka doľava / doprava - mesiac dozadu / dopredu\n" +
                              "Šípka dole / hore - rok dozadu / dopredu\n" +
                              "Kláves Home alebo D - aktuálny deň\n" +
                              "Pre ukončenie stlačte Enter");
            var key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    ShowNamedaysInMonth(datum.AddMonths(-2));
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                    ShowNamedaysInMonth(datum);
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear(); 
                    ShowNamedaysInMonth(datum.AddYears(1).AddMonths(-1));
                    break;
                case ConsoleKey.DownArrow: 
                    Console.Clear();
                    ShowNamedaysInMonth(datum.AddYears(-1).AddMonths(-1));
                    break;
                case ConsoleKey.Home:
                case ConsoleKey.D: 
                    Console.Clear();
                    ShowNamedaysInMonth(DateTime.Now);
                    break;
                case ConsoleKey.Enter: 
                    Console.Clear();
                    Show(); 
                    break;
                default:
                    Console.Clear();
                    ShowNamedaysInMonth(datum.AddMonths(-1));
                    break;
            }
        }
    }
}
