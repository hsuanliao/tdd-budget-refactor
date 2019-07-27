using System;

namespace BudgetApp
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public decimal OverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDayCount(GetPeriod());
        }

        private decimal DailyAmount()
        {
            return Amount / (decimal) Days();
        }

        private int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        private DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        private Period GetPeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        private DateTime LastDay()
        {
            return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null);
        }
    }
}