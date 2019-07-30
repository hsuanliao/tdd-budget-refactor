using System;

namespace BudgetApp
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay().Year, FirstDay().Month);
        }

        public DateTime FirstDay()
        {
            return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }

        public DateTime LastDay()
        {
            return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null);
        }

        public Period GetPeriod()
        {
            return new Period(FirstDay(), LastDay());
        }

        public int OverlappingAmount(Period period)
        {
            return DailyAmount() * period.OverlappingDayCount(GetPeriod());
        }
    }
}