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

                return budget.DailyAmount() * (endDate.Day - startDate.Day + 1);
            }
            else
            {
                var firstMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
                int firstMonthAmount = 0;
                if (firstMonthBudget != null)
                {
                    firstMonthAmount = firstMonthBudget.DailyAmount() *
                                 (DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);
                }

                var lastMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == endDate.ToString("yyyyMM"));
                int lastMonthAmount = 0;
                if (lastMonthBudget != null)
                {
                    lastMonthAmount = lastMonthBudget.Amount /
                                  DateTime.DaysInMonth(endDate.Year, endDate.Month) * (endDate.Day);
                }
                var totalAmount = firstMonthAmount + lastMonthAmount;
                var allStartMonth = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1);
                var allEndMonth = new DateTime(endDate.Year, endDate.Month, 1);
                while (allEndMonth > allStartMonth)
                {
                    var searchMonth = allStartMonth.ToString("yyyyMM");
                    if (budgets.Any(x => x.YearMonth == searchMonth))
                        totalAmount += budgets.FirstOrDefault(x => x.YearMonth == searchMonth).Amount;

                    allStartMonth = allStartMonth.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private static bool IsSameMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.ToString("yyyyMM") == endDate.ToString("yyyyMM");
        }
    }
}