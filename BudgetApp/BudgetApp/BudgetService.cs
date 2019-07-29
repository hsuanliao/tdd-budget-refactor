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
                int totalAmount = 0;

                var currentDate = startDate;
                var loopEndDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1);
                while (currentDate < loopEndDate)
                {
                    if (IsSameMonth(currentDate, startDate))
                    {
                        var firstMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == currentDate.ToString("yyyyMM"));
                        if (firstMonthBudget != null)
                        {
                            totalAmount += firstMonthBudget.DailyAmount() * EffectiveDays(startDate, firstMonthBudget.LastDay());
                        }
                    }
                    else if (IsSameMonth(currentDate, endDate))
                    {
                        var lastMonthBudget = budgets.FirstOrDefault(x => x.YearMonth == currentDate.ToString("yyyyMM"));
                        if (lastMonthBudget != null)
                        {
                            totalAmount += lastMonthBudget.DailyAmount() * EffectiveDays(lastMonthBudget.FirstDay(), endDate);
                        }
                    }
                    else
                    {
                        var middleBudget = budgets.FirstOrDefault(x => x.YearMonth == currentDate.ToString("yyyyMM"));
                        if (middleBudget != null)
                        {
                            totalAmount += middleBudget.DailyAmount() * EffectiveDays(middleBudget.FirstDay(), middleBudget.LastDay());
                        }
                    }

                    currentDate = currentDate.AddMonths(1);
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