using System;
using System.Collections.Generic;
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
                if (budget == null) return 0;

                return budget.DailyAmount() * EffectiveDayCount(startDate, endDate);
            }
            else
            {
                decimal totalAmount = 0;
                var firstMonth = startDate.ToString("yyyyMM");
                var firstMonthBudget = FindBudget(budgets, firstMonth);

                if (firstMonthBudget != null)
                {
                    totalAmount += firstMonthBudget.DailyAmount() * EffectiveDayCount(startDate, firstMonthBudget.LastDay());
                }

                var lastMonth = endDate.ToString("yyyyMM");
                var lastMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == lastMonth);

                var lastMonthAmount = lastMonthBudget == null
                    ? 0
                    : lastMonthBudget.DailyAmount() * EffectiveDayCount(lastMonthBudget.FirstDay(), endDate);

                totalAmount += lastMonthAmount;

                var allStartMonth = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1);
                var allEndMonth = new DateTime(endDate.Year, endDate.Month, 1);

                string currentMonth = "";
                while (allEndMonth > allStartMonth)
                {
                    currentMonth = allStartMonth.ToString("yyyyMM");
                    var currentBudget = budgets.FirstOrDefault(x => x.YearMonth == currentMonth);

                    var currentBudgetAmount = currentBudget == null
                        ? 0
                        : currentBudget.DailyAmount() *
                        EffectiveDayCount(currentBudget.FirstDay(), currentBudget.LastDay());

                    totalAmount += currentBudgetAmount;

                    allStartMonth = allStartMonth.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private static int EffectiveDayCount(DateTime startDate, DateTime end)
        {
            return ((end - startDate).Days + 1);
        }

        private static Budget FindBudget(List<Budget> budgets, string firstMonth)
        {
            var firstMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == firstMonth);
            return firstMonthBudget;
        }

        private static bool IsSameMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.ToString("yyyyMM") == endDate.ToString("yyyyMM");
        }

        private static decimal QuerySingleMonth(DateTime startDate, DateTime endDate, List<Budget> budgets)
        {
            var budget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
            if (budget == null) return 0;

            return budget.DailyAmount() * (endDate.Day - startDate.Day + 1);
        }
    }
}