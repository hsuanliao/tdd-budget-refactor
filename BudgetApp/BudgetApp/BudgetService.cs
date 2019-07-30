using System;

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

            var totalAmount = 0;
            var period = new Period(startDate, endDate);
            foreach (var currentBudget in budgets)
            {
                totalAmount += currentBudget.DailyAmount() * period.OverlappingDayCount(currentBudget.GetPeriod());
            }

            return totalAmount;
        }
    }
}