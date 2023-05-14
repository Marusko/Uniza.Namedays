﻿using System.Globalization;

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
                    case 2: Console.WriteLine("ŠTATISTIKA");
                            ShowStatistics();
                            break;
                    case 3: Console.WriteLine("VYHĽADÁVENIE MIEN");
                            SearchNames();
                            break;
                    case 4: Console.WriteLine("VYHĽADÁVANIE MIEN PODĽA DÁTUMU\nPre ukončenie stlačte Enter");
                            SearchNamesByDate();
                            break;
                    case 5: Console.WriteLine("KALENDÁR MENÍN");
                            ShowNamedaysInMonth();
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
            //TODO dlzka
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
            
        }
    }
}
