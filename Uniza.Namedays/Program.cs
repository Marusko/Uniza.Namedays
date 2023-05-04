﻿namespace Uniza.Namedays
{
    internal class Program
    {
    }

    struct DayMonth
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
}
