using System;

namespace BudgetApp
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int Days()
        {
            var firstDay = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }
    }
}