using System.Collections;

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
                       
                }
                return null;
            }
        }

        public string[] this[DayMonth dayMonth] { get; }
        public string[] this[DateOnly date] { get; }
        public string[] this[DateTime date] { get; }
        public string[] this[int day, int month] { get; }

        private readonly List<Nameday> _kalendar = new();

        public IEnumerator<Nameday> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Nameday> GetNamedays()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Nameday> GetNamedays(int month)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            throw new NotImplementedException();
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

        }

        public bool Contains(string name)
        {

        }

        public void Clear()
        {
            _kalendar.Clear();
        }

        public void Load(FileInfo csvFile)
        {

        }

        public void Save(FileInfo csvFile)
        {

        }
    }

    record struct DayMonth
    {
        public int Day { get; init; }
        public int Month { get; init; }

        public DayMonth()
        {
            Day = DateTime.Now.Day;
            Month = DateTime.Now.Month;
        }

        public DayMonth(int day, int month)
        {
            Day = day;
            Month = month;
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
