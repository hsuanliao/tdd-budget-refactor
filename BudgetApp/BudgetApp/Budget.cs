using System;

namespace BudgetApp
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public decimal DailyAmount()
        {
            return Amount / (decimal) Days();
        }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime LastDay()
        {
            return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null);
        }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }
    }
}