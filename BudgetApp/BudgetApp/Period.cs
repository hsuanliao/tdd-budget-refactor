using System;

namespace BudgetApp
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime EndDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public int OverlappingDayCount(Period another)
        {
            if (HasNoOverlapping(another))
            {
                return 0;
            }

            var overlappingStart = StartDate > another.StartDate
                ? StartDate
                : another.StartDate;

            var overlappingEnd = EndDate < another.EndDate
                ? EndDate
                : another.EndDate;

            return DayCount(overlappingStart, overlappingEnd);
        }

        private static int DayCount(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        private bool HasNoOverlapping(Period another)
        {
            return EndDate < another.StartDate || StartDate > another.EndDate;
        }
    }
}