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
            var firstDay = another.StartDate;
            var lastDay = another.EndDate;

            if (EndDate < firstDay || StartDate > lastDay)
            {
                return 0;
            }

            var overlappingStart = StartDate > firstDay
                ? StartDate
                : firstDay;

            var overlappingEnd = EndDate < lastDay
                ? EndDate
                : lastDay;

            return DayCount(overlappingStart, overlappingEnd);
        }
    }
}