using System.Collections;
using System.Text.RegularExpressions;

namespace Uniza.Namedays
{
    record NamedayCalendar : IEnumerable<Nameday>
    {
        public int NameCount { get; }
        public int DayCount { get; }

        public DayMonth? this[string name]
        {
            get
            {
                if (Contains(name))
                {
                    return GetEnumerator().Current.DayMonth;
                }
                return null;
            }
        }

        public string[] this[DayMonth dayMonth] => 
            (from meno in _kalendar 
                where meno.DayMonth.Day == dayMonth.Day && meno.DayMonth.Month == dayMonth.Month 
                select meno.Name).ToArray();

        public string[] this[DateOnly date] => 
            (from meno in _kalendar 
                where meno.DayMonth.Day == date.Day && meno.DayMonth.Month == date.Month 
                select meno.Name).ToArray();

        public string[] this[DateTime date] =>
            (from meno in _kalendar
                where meno.DayMonth.Day == date.Day && meno.DayMonth.Month == date.Month
                select meno.Name).ToArray();

        public string[] this[int day, int month] =>
            (from meno in _kalendar 
                where meno.DayMonth.Day == day && meno.DayMonth.Month == month 
                select meno.Name).ToArray();

        private readonly List<Nameday> _kalendar = new();

        public IEnumerator<Nameday> GetEnumerator()
        {
            return _kalendar.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Nameday> GetNamedays()
        {
            return _kalendar;
        }

        public IEnumerable<Nameday> GetNamedays(int month)
        {
            return from nameday in _kalendar where nameday.DayMonth.Month == month select nameday;
        }

        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            return from nameday in _kalendar where Regex.IsMatch(nameday.Name, pattern) select nameday;
        }

        public void Add(Nameday nameday)
        {
            _kalendar.Add(nameday);
        }

        public void Add(int day, int month, params string[] names)
        {
            foreach (var name in names)
            {
                _kalendar.Add(new Nameday(name, new DayMonth(day, month)));
            }
        }

        public void Add(DayMonth dayMonth, params string[] names)
        {
            foreach (var name in names)
            {
                _kalendar.Add(new Nameday(name, dayMonth));
            }
        }

        public bool Remove(string name)
        {
            return Contains(name) && _kalendar.Remove(GetEnumerator().Current);
        }

        public bool Contains(string name)
        {
            GetEnumerator().Reset();
            while (GetEnumerator().MoveNext())
            {
                if (GetEnumerator().Current.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            _kalendar.Clear();
        }

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
                    if (!string.IsNullOrEmpty(tmp) || !tmp.Equals("-"))
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

        public void Save(FileInfo csvFile)
        {
            //TODO nefunguje tak ako ma
            if (!csvFile.Extension.Equals(".csv"))
            {
                return;
            }
            var stream = csvFile.Open(FileMode.Open);
            var writer = new StreamWriter(stream);
            foreach (var nameday in _kalendar)
            {
                writer.WriteLine($"{nameday.DayMonth.Day}. {nameday.DayMonth.Month}.;{nameday.Name}");
            }
            writer.Close();
            stream.Close();
        }
    }

    record struct DayMonth(int Day, int Month)
    {
        public int Day { get; init; } = Day;
        public int Month { get; init; } = Month;

        public DayMonth() : this(DateTime.Now.Day, DateTime.Now.Month)
        {
        }

        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }
    }

    record struct Nameday
    {
        public string Name { get; init; }
        public DayMonth DayMonth { get; init; }

        public Nameday() { }

        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }
    }
}
