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

            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
            {
                var searchMonth = startDate.ToString("yyyyMM");
                if (budgets.All(x => x.YearMonth != searchMonth))
                {
                    return 0;
                }

                var budget = budgets.FirstOrDefault(x => x.YearMonth == searchMonth);

                return budget.Amount / DateTime.DaysInMonth(startDate.Year, startDate.Month) * (endDate.Day - startDate.Day + 1);
            }
            else
            {
                var firstMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == startDate.ToString("yyyyMM"));
                int firstMonthAmount = 0;
                if (firstMonthBudget != null)
                {
                    firstMonthAmount = firstMonthBudget.Amount /
                                 DateTime.DaysInMonth(startDate.Year, startDate.Month) *
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
    }
}