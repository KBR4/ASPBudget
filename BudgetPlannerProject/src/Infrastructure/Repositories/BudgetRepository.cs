using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private List<Budget> budgets = new List<Budget>();
        public BudgetRepository()
        {

        }
        public Task Create(Budget budget)
        {
            budgets.Add(budget);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (budgets.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            budgets.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<Budget>> ReadAll()
        {
            return Task.FromResult(budgets);
        }

        public Task<Budget?> ReadById(int id)
        {
            var budget = budgets.Find(x => x.Id == id);
            return Task.FromResult(budget);
        }

        public Task<bool> Update(Budget budget)
        {
            var budgetToUpdate = budgets.Find(x => x.Id == budget.Id);
            if (budgetToUpdate == null)
            {
                return Task.FromResult(false);
            }
            budgetToUpdate.Name = budget.Name;
            budgetToUpdate.StartDate = budget.StartDate;
            budgetToUpdate.FinishDate = budget.FinishDate;
            budgetToUpdate.BudgetRecords = budget.BudgetRecords;
            budgetToUpdate.Description = budget.Description;
            budgetToUpdate.Creator = budget.Creator;

            return Task.FromResult(true);
        }
    }
}
