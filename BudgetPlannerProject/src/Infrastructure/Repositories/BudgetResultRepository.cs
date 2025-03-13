using Bogus;
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
        private List<BudgetResult> _budgetResults = new List<BudgetResult>();
        public BudgetResultRepository()
        {
            PopulateTestData();
        }
        private void PopulateTestData()
        {
            var faker = new Faker();
            _budgetResults = new List<BudgetResult>();
            for (int i = 0; i < 10; i++)
            {
                var budgetResult = new BudgetResult();
                budgetResult.Id = i + 1;
                budgetResult.BudgetId = i + 1;
                var budget = new Budget();
                budgetResult.Budget = budget;
                budgetResult.TotalProfit = Convert.ToDouble(faker.Commerce.Price(1, 1000));
                _budgetResults.Add(budgetResult);
            }
        }
        public Task<int> Create(BudgetResult budgetResult)
        {
            _budgetResults.Add(budgetResult);
            return Task.FromResult(budgetResult.Id);
        }

        public Task<bool> Delete(int id)
        {
            if (_budgetResults.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            _budgetResults.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<BudgetResult>> ReadAll()
        {
            return Task.FromResult(_budgetResults);
        }

        public Task<BudgetResult?> ReadById(int id)
        {
            var budgetResult = _budgetResults.Find(x => x.Id == id);
            return Task.FromResult(budgetResult);
        }

        public Task<bool> Update(BudgetResult budgetResult)
        {
            var budgetResultToUpdate = _budgetResults.Find(x => x.Id == budgetResult.Id);
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
