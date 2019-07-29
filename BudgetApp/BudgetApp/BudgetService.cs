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
                    var currentBudget = budgets.FirstOrDefault(x => x.YearMonth == currentDate.ToString("yyyyMM"));
                    if (currentBudget != null)
                    {
                        int effectiveDays;
                        if (IsSameMonth(currentDate, startDate))
                        {
                            effectiveDays = EffectiveDays(startDate, currentBudget.LastDay());
                        }
                        else if (IsSameMonth(currentDate, endDate))
                        {
                            effectiveDays = EffectiveDays(currentBudget.FirstDay(), endDate);
                        }
                        else
                        {
                            effectiveDays = EffectiveDays(currentBudget.FirstDay(), currentBudget.LastDay());
                        }
                        totalAmount += currentBudget.DailyAmount() * effectiveDays;
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