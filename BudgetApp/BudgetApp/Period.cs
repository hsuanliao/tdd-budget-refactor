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
            if (EndDate < budget.FirstDay())
            {
                return 0;
            }

            var overlappingStart = StartDate > budget.FirstDay()
                ? StartDate
                : budget.FirstDay();

            var overlappingEnd = EndDate < budget.LastDay()
                ? EndDate
                : budget.LastDay();

            return DayCount(overlappingStart, overlappingEnd);
        }
    }
}