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

            decimal totalAmount = 0;
            var period = new Period(startDate, endDate);

            foreach (var currentBudget in budgets)
            {
                totalAmount += currentBudget.DailyAmount() *
                    period.OverlappingDayCount(GetPeriod(currentBudget));
            }

            return totalAmount;
        }

        private static Period GetPeriod(Budget currentBudget)
        {
            return new Period(currentBudget.FirstDay(), currentBudget.LastDay());
        }

        private static Budget FindBudget(DateTime startDate, List<Budget> budgets)
        {
            var firstMonth = startDate.ToString("yyyyMM");
            var firstMonthBudget = FindBudget(budgets, firstMonth);
            return firstMonthBudget;
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