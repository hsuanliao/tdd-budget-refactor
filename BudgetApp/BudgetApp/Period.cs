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

        public int OverlappingDayCount(Budget currentBudget)
        {
            var effectiveStartDate = StartDate > currentBudget.FirstDay()
                ? StartDate
                : currentBudget.FirstDay();

            var effectiveEndDate = EndDate < currentBudget.LastDay()
                ? EndDate
                : currentBudget.LastDay();

            return DayCount(effectiveStartDate, effectiveEndDate);
        }

        public static int DayCount(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }
    }
}