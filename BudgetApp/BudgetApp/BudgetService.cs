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

            int totalAmount = 0;

            var currentDate = startDate;
            var loopEndDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1);
            while (currentDate < loopEndDate)
            {
                var currentBudget = budgets.FirstOrDefault(x => x.YearMonth == currentDate.ToString("yyyyMM"));
                var period = new Period(startDate, endDate);
                if (currentBudget != null)
                {
                    totalAmount += currentBudget.DailyAmount() * period.OverlappingDayCount(currentBudget);
                }

                currentDate = currentDate.AddMonths(1);
            }

            return totalAmount;
        }
    }
}