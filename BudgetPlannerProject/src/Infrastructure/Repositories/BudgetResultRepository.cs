using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BudgetResultRepository : IBudgetResultRepository
    {
        private List<BudgetResult> budgetResults = new List<BudgetResult>();
        public BudgetResultRepository()
        {

        }
        public Task Create(BudgetResult budgetResult)
        {
            budgetResults.Add(budgetResult);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (budgetResults.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            budgetResults.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<BudgetResult>> ReadAll()
        {
            return Task.FromResult(budgetResults);
        }

        public Task<BudgetResult?> ReadById(int id)
        {
            var budgetResult = budgetResults.Find(x => x.Id == id);
            return Task.FromResult(budgetResult);
        }

        public Task<bool> Update(BudgetResult budgetResult)
        {
            var budgetResultToUpdate = budgetResults.Find(x => x.Id == budgetResult.Id);
            if (budgetResultToUpdate == null)
            {
                return Task.FromResult(false);
            }
            budgetResultToUpdate.BudgetId = budgetResult.BudgetId;
            budgetResultToUpdate.Budget = budgetResult.Budget;
            budgetResultToUpdate.TotalProfit = budgetResult.TotalProfit;

            return Task.FromResult(true);
        }
    }
}
