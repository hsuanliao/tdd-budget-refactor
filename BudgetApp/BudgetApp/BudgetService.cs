using System;
using System.Linq;

namespace BudgetApp
{
    public class BudgetService
    {
        private readonly IBudgetRepository _repo;

        public BudgetService(IBudgetRepository repo)
        {
            _repo = repo;
        }

        public decimal Query(DateTime startDate, DateTime endDate)
        {
            var budgets = this._repo.GetAll();
            if (startDate > endDate)
            {
                return 0;
            }

            if (IsSameMonth(startDate, endDate))
            {
                var budget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
                if (budget == null)
                {
                    return 0;
                }

                return budget.DailyAmount() * EffectiveDays(startDate, endDate);
            }
            else
            {
                var firstMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
                int firstMonthAmount = 0;
                if (firstMonthBudget != null)
                {
                    firstMonthAmount = firstMonthBudget.DailyAmount() * EffectiveDays(startDate, firstMonthBudget.LastDay());
                }

                var lastMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == endDate.ToString("yyyyMM"));
                int lastMonthAmount = 0;
                if (lastMonthBudget != null)
                {
                    lastMonthAmount = lastMonthBudget.DailyAmount() * EffectiveDays(lastMonthBudget.FirstDay(), endDate);
                }
                var totalAmount = firstMonthAmount + lastMonthAmount;
                var allStartMonth = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1);
                var allEndMonth = new DateTime(endDate.Year, endDate.Month, 1);
                while (allEndMonth > allStartMonth)
                {
                    var searchMonth = allStartMonth.ToString("yyyyMM");
                    var middleBudget = budgets.FirstOrDefault(x => x.YearMonth == searchMonth);
                    if (middleBudget != null)
                    {
                        totalAmount += middleBudget.DailyAmount() * EffectiveDays(middleBudget.FirstDay(), middleBudget.LastDay());
                    }

                    allStartMonth = allStartMonth.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private static int EffectiveDays(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        private static bool IsSameMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.ToString("yyyyMM") == endDate.ToString("yyyyMM");
        }
    }
}