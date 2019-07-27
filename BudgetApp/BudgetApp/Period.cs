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

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public static int DayCount(DateTime startDate, DateTime end)
        {
            return ((end - startDate).Days + 1);
        }

        public int OverlappingDayCount(Budget budget)
        {
            var another = new Period(budget.FirstDay(), budget.LastDay());

            if (EndDate < another.StartDate || StartDate > another.EndDate)
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
    }
}