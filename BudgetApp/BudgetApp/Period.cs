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
            DateTime effectiveStartDate;
            DateTime effectiveEndDate;
            if (currentBudget.YearMonth == StartDate.ToString("yyyyMM"))
            {
                effectiveStartDate = StartDate;
                effectiveEndDate = currentBudget.LastDay();
            }
            else if (currentBudget.YearMonth == EndDate.ToString("yyyyMM"))
            {
                effectiveStartDate = currentBudget.FirstDay();
                effectiveEndDate = EndDate;
            }
            else
            {
                effectiveStartDate = currentBudget.FirstDay();
                effectiveEndDate = currentBudget.LastDay();
            }

            var effectiveDayCount = DayCount(effectiveStartDate, effectiveEndDate);
            return effectiveDayCount;
        }

        public static int DayCount(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }
    }
}