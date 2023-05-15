using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Uniza.Namedays
{
    /// <summary>
    /// Trieda reprezentujúca kalendár menín
    /// </summary>
    public record NamedayCalendar : IEnumerable<Nameday>
    {
        /// <summary>
        /// Vráti celkový počet mien v kalendári
        /// </summary>
        public int NameCount => _nameCount;

        /// <summary>
        /// Vráti celkový počet dní, v ktorých má niekto meniny
        /// </summary>
        public int DayCount => _dayCount;

        private int _nameCount;
        private int _dayCount;
        private List<Nameday>.Enumerator _enumerator;

        /// <summary>
        /// Indexer vráti deň a mesiac oslavy zadaného mena
        /// </summary>
        /// <param name="name">Meno, ktorého dátum menín hľadáme</param>
        /// <returns>Vráti deň a mesiac oslavy zadaného mena, ak meno v kalendári nie je vráti null</returns>
        public DayMonth? this[string name]
        {
            get
            {
                if (Contains(name))
                {
                    return _enumerator.Current.DayMonth;
                }
                return null;
            }
        }

        /// <summary>
        /// Vráti pole mien, ktoré oslavujú meniny v zadaný deň a mesiac
        /// </summary>
        /// <param name="dayMonth">Štruktúra DayMonth s hľadaným dátumom</param>
        /// <returns>Vráti pole mien, ktoré oslavujú meniny v zadaný dátum</returns>
        public string[] this[DayMonth dayMonth] => 
            (from meno in _kalendar 
                where meno.DayMonth.Day == dayMonth.Day && meno.DayMonth.Month == dayMonth.Month && !meno.Name.Equals("-")
             select meno.Name).ToArray();

        /// <summary>
        /// Vráti pole mien, ktoré oslavujú meniny v zadaný deň a mesiac
        /// </summary>
        /// <param name="date">Štruktúra DateOnly s hľadaným dátumom</param>
        /// <returns>Vráti pole mien, ktoré oslavujú meniny v zadaný dátum</returns>
        public string[] this[DateOnly date] => 
            (from meno in _kalendar 
                where meno.DayMonth.Day == date.Day && meno.DayMonth.Month == date.Month && !meno.Name.Equals("-")
             select meno.Name).ToArray();

        /// <summary>
        /// Vráti pole mien, ktoré oslavujú meniny v zadaný deň a mesiac
        /// </summary>
        /// <param name="date">Štruktúra DateTime s hľadaným dátumom</param>
        /// <returns>Vráti pole mien, ktoré oslavujú meniny v zadaný dátum</returns>
        public string[] this[DateTime date] =>
            (from meno in _kalendar
                where meno.DayMonth.Day == date.Day && meno.DayMonth.Month == date.Month && !meno.Name.Equals("-")
             select meno.Name).ToArray();

        /// <summary>
        /// Vráti pole mien, ktoré oslavujú meniny v zadaný deň a mesiac
        /// </summary>
        /// <param name="day">Číslo hľadaného dňa</param>
        /// <param name="month">Číslo hľadaného mesiaca</param>
        /// <returns>Vráti pole mien, ktoré oslavujú meniny v zadaný dátum</returns>
        public string[] this[int day, int month] =>
            (from meno in _kalendar 
                where meno.DayMonth.Day == day && meno.DayMonth.Month == month && !meno.Name.Equals("-")
             select meno.Name).ToArray();

        private readonly List<Nameday> _kalendar = new();

        /// <summary>
        /// Vráti objekt, ktorý vracia všetky meniny v kalendári
        /// </summary>
        /// <returns>Vráti objekt implementujúci IEnumerator, ktorý vracia všetky meniny v kalendári</returns>
        public IEnumerator<Nameday> GetEnumerator()
        {
            return _kalendar.GetEnumerator();
        }
        /// <summary>
        /// Vráti objekt, ktorý vracia všetky meniny v kalendári
        /// </summary>
        /// <returns>Vráti objekt implementujúci IEnumerator, ktorý vracia všetky meniny v kalendári</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Vráti všetky meniny v kalendári
        /// </summary>
        /// <returns>Vráti zoznam všetkých menín v kalendári</returns>
        public IEnumerable<Nameday> GetNamedays()
        {
            _kalendar.RemoveAll(nameday =>
            {
                if (nameday.Name.Equals("-"))
                {
                    return true;
                }
                return false;
            });
            return _kalendar;
        }

        /// <summary>
        /// Vráti všetky meniny v zadanom mesiaci
        /// </summary>
        /// <returns>Vráti zoznam všetkých menín v zadanom mesiaci</returns>
        public IEnumerable<Nameday> GetNamedays(int month)
        {
            return from nameday in _kalendar where nameday.DayMonth.Month == month && !nameday.Name.Equals("-") select nameday;
        }

        /// <summary>
        /// Vráti všetky meniny, ktoré zodpovedajú zadanému filtru mien
        /// </summary>
        /// <returns>Vráti zoznam všetkých menín, ktoré zodpovedajú zadanému filtru mien</returns>
        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            return from nameday in _kalendar where Regex.IsMatch(nameday.Name, pattern) select nameday;
        }

        /// <summary>
        /// Pridá meniny do kalendára
        /// </summary>
        /// <param name="nameday">Štruktúra Nameday so zadaným menom a dátumom</param>
        public void Add(Nameday nameday)
        {
            if (!nameday.Name.Equals("-"))
            {
                _nameCount++;
            }

            if (!nameday.Name.Equals("-") && this[nameday.DayMonth].Length == 0)
            {
                _dayCount++;
            }
            _kalendar.Add(nameday);
            
        }

        /// <summary>
        /// Pridá jedno alebo viac mien so zadaným dňom a mesiacom do kalendára
        /// </summary>
        /// <param name="day">Deň, na ktorý sa uloží/uložia meno/mená</param>
        /// <param name="month">Mesiac, na ktorý sa uloží/uložia meno/mená</param>
        /// <param name="names">Meno/mená, ktoré sa uloží/uložia do kalendára</param>
        public void Add(int day, int month, params string[] names)
        {
            if (!names[0].Equals("-") && this[day, month].Length == 0)
            {
                _dayCount++;
            }
            foreach (var name in names)
            {
                _kalendar.Add(new Nameday(name, new DayMonth(day, month)));
                if (!name.Equals("-"))
                {
                    _nameCount++;
                }
            }
        }

        /// <summary>
        /// Pridá jedno alebo viac mien so zadaným dňom a mesiacom do kalendára
        /// </summary>
        /// <param name="dayMonth">Štruktúra DayMonth s nastavneným dátumom</param>
        /// <param name="names">Meno/mená, ktoré sa uloží/uložia do kalendára</param>
        public void Add(DayMonth dayMonth, params string[] names)
        {
            if (!names[0].Equals("-") && this[dayMonth].Length == 0)
            {
                _dayCount++;
            }
            foreach (var name in names)
            {
                _kalendar.Add(new Nameday(name, dayMonth));
                if (!name.Equals("-"))
                {
                    _nameCount++;
                }
            }
        }

        /// <summary>
        /// Odstráni meno z kalendára
        /// </summary>
        /// <param name="name">Meno, ktoré sa odstráni</param>
        /// <returns>Vráti true ak sa podarí meno odstrániť, inak vráti false</returns>
        public bool Remove(string name)
        {
            if (Contains(name) && _kalendar.Remove(_enumerator.Current))
            {
                _nameCount--;
                if (GetNamedays(name).ToList().Count == 0)
                {
                    _dayCount--;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Zistí či sa meno nachádza v kalendári
        /// </summary>
        /// <param name="name">Meno, ktoré zisťujeme či sa nachádza v kalendári</param>
        /// <returns>Vráti true ak sa meno v kalendári nachádza, inak vráti false</returns>
        public bool Contains(string name)
        {
            _enumerator = (List<Nameday>.Enumerator)GetEnumerator();
            while (_enumerator.MoveNext())
            {
                if (_enumerator.Current.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Vymaže všetky údaje z kalendára
        /// </summary>
        public void Clear()
        {
            _kalendar.Clear();
        }

        /// <summary>
        /// Načíta kalendár mien zo súboru CSV
        /// </summary>
        /// <param name="csvFile">Cesta k súboru v tvare FileInfo</param>
        public void Load(FileInfo csvFile)
        {
            if (!csvFile.Extension.Equals(".csv"))
            {
                return;
            }
            var stream = csvFile.Open(FileMode.Open);
            var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                var splitted = line.Split(";");
                var names = new List<string>();
                for (var i = 1; i < splitted.Length; i++)
                {
                    var tmp = splitted[i].Trim();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        names.Add(tmp);
                    }
                }

                var datum = splitted[0];
                var dotIndex = datum.IndexOf('.');
                var dotLength = datum.Length - datum.Substring(dotIndex).Length;
                var lastDotLength = datum.Length - dotLength - 2;
                var day = datum.Substring(0, dotLength).Trim();
                var month = datum.Substring(dotIndex + 1, lastDotLength).Trim();

                Add(int.Parse(day), int.Parse(month), names.ToArray());
            }
            reader.Close();
            stream.Close();
        }

        /// <summary>
        /// Zapíše kalendár mien do súboru CSV
        /// </summary>
        /// <param name="csvFile">Cesta k súboru v tvare FileInfo</param>
        public void Save(FileInfo csvFile)
        {
            if (!csvFile.Extension.Equals(".csv"))
            {
                return;
            }
            
            var stream = csvFile.Open(FileMode.Open);
            stream.SetLength(0);
            var writer = new StreamWriter(stream);
            var multiple = 0;
            foreach (var nameday in _kalendar)
            {
                if (multiple == 0)
                {
                    writer.Write($"{nameday.DayMonth.Day}. {nameday.DayMonth.Month}.;{string.Join(";", this[nameday.DayMonth])}");
                    if (this[nameday.DayMonth].Length == 0)
                    {
                        writer.Write("-;;");
                    }
                    else
                    {
                        for (int i = 0; i < (3 - this[nameday.DayMonth].Length); i++)
                        {
                            writer.Write(";");
                        }
                        multiple = this[nameday.DayMonth].Length - 1;
                    }
                    writer.WriteLine();
                }
                else
                {
                    multiple--;
                }
            }
            writer.Close();
            stream.Close();
        }
    }

    /// <summary>
    /// Štruktúra reprezentujúca deň a mesiac
    /// </summary>
    /// <param name="Day">Uložený deň</param>
    /// <param name="Month">Uložený mesiac</param>
    public record struct DayMonth(int Day, int Month)
    {
        /// <summary>
        /// Vlastnosť vracajúca deň
        /// </summary>
        public int Day { get; init; } = Day;
        /// <summary>
        /// Vlastnosť vracajúca mesiac
        /// </summary>
        public int Month { get; init; } = Month;

        /// <summary>
        /// Bezparametrický konštruktor, nastaví vlastnosti na aktuálny dátum
        /// </summary>
        public DayMonth() : this(DateTime.Now.Day, DateTime.Now.Month)
        {
        }
        /// <summary>
        /// Metóda vráti štruktúru DateTime
        /// </summary>
        /// <returns>Štruktúra má nastavený aktuálny rok, mesiac a deň sú nastavené podľa vlastností</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }
    }

    /// <summary>
    /// Štruktúra reprezentujúca oslavu menín(meno a dátum oslavy)
    /// </summary>
    public record struct Nameday
    {
        /// <summary>
        /// Vlastnosť vracajúca meno, ktoré oslavuje meniny
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Vlastnosť vracajúca deň a mesiac menín
        /// </summary>
        public DayMonth DayMonth { get; set; }

        /// <summary>
        /// Bezparametrický konštruktor
        /// </summary>
        public Nameday()
        {
            Name = "0";
        }

        /// <summary>
        /// Parametrický konštruktor na inicializáciu
        /// </summary>
        /// <param name="name">Meno oslavujúce meniny</param>
        /// <param name="dayMonth">Deň a mesiac menín</param>
        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }

        override 
        public string ToString()
        {
            return Name + " (" + DayMonth.Day + "." + DayMonth.Month + ")";
        }
    }
}
