﻿using System;
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

                return budget.DailyAmount() * DayCount(startDate, endDate);
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
                        var effectiveDayCount = OverlappingDayCount(startDate, endDate, currentBudget);
                        totalAmount += currentBudget.DailyAmount() * effectiveDayCount;
                    }

                    currentDate = currentDate.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private static int OverlappingDayCount(DateTime startDate, DateTime endDate, Budget currentBudget)
        {
            DateTime effectiveStartDate;
            DateTime effectiveEndDate;
            if (currentBudget.YearMonth == startDate.ToString("yyyyMM"))
            {
                effectiveStartDate = startDate;
                effectiveEndDate = currentBudget.LastDay();
            }
            else if (currentBudget.YearMonth == endDate.ToString("yyyyMM"))
            {
                effectiveStartDate = currentBudget.FirstDay();
                effectiveEndDate = endDate;
            }
            else
            {
                effectiveStartDate = currentBudget.FirstDay();
                effectiveEndDate = currentBudget.LastDay();
            }

            var effectiveDayCount = DayCount(effectiveStartDate, effectiveEndDate);
            return effectiveDayCount;
        }

        private static int DayCount(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        private static bool IsSameMonth(DateTime startDate, DateTime endDate)
        {
            return startDate.ToString("yyyyMM") == endDate.ToString("yyyyMM");
        }
    }
}