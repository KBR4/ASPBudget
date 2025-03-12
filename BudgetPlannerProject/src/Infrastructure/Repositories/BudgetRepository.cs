using Bogus;
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
            PopulateTestData();
        }
        private void PopulateTestData()
        {
            var faker = new Faker();
            budgets = new List<Budget>();
            for (int i = 0; i < 10; i++)
            {
                var budget = new Budget();
                budget.Id = i + 1;
                budget.Name = faker.Commerce.Product();
                budget.StartDate = faker.Date.Future(2, DateTime.Now);
                budget.FinishDate = faker.Date.Future(5, budget.StartDate);
                budget.Description = faker.Rant.Review();
                budget.BudgetRecords = new List<BudgetRecord>();
                var creator = new User()
                {
                    Id = i*i + 1,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email,
                    BudgetPlans = new List<Budget>() { budget }
                };

            }
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
