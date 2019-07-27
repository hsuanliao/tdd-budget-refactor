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

        public int OverlappingDayCount(Budget currentBudget)
        {
            var effectiveStart = StartDate > currentBudget.FirstDay()
                ? StartDate
                : currentBudget.FirstDay();

            var effectiveEnd = EndDate < currentBudget.LastDay()
                ? EndDate
                : currentBudget.LastDay();

            if (currentBudget.YearMonth == StartDate.ToString("yyyyMM"))
            {
                //effectiveStart = StartDate;
                //effectiveEnd = currentBudget.LastDay();
            }
            else if (currentBudget.YearMonth == EndDate.ToString("yyyyMM"))
            {
                //effectiveStart = currentBudget.FirstDay();
                //effectiveEnd = EndDate;
            }
            else
            {
                //effectiveStart = currentBudget.FirstDay();
                //effectiveEnd = currentBudget.LastDay();
            }

            var effectiveDayCount = DayCount(effectiveStart, effectiveEnd);
            return effectiveDayCount;
        }
    }
}