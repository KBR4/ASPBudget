using Bogus;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories.BudgetRepository
{
    [ExcludeFromCodeCoverage]
    public class BudgetInMemoryRepository : IBudgetRepository
    {
        private List<Budget> _budgets = new List<Budget>();

        public BudgetInMemoryRepository()
        {
            PopulateTestData();
        }

        private void PopulateTestData()
        {
            var faker = new Faker();
            for (int i = 0; i < 10; i++)
            {
                var budget = new Budget { Name = faker.Commerce.Product() };
                budget.Id = i + 1;
                budget.StartDate = faker.Date.Future(2, DateTime.Now);
                budget.FinishDate = faker.Date.Future(5, budget.StartDate);
                budget.Description = faker.Rant.Review();
                var creator = new User()
                {
                    Id = i + 1,
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Email = faker.Person.Email
                };
                _budgets.Add(budget);
            }
        }

        public Task<int> Create(Budget budget)
        {
            _budgets.Add(budget);
            return Task.FromResult(budget.Id);
        }

        public Task<bool> Delete(int id)
        {
            if (!_budgets.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            _budgets.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Budget>> ReadAll()
        {
            return Task.FromResult<IEnumerable<Budget>>(_budgets);
        }

        public Task<Budget?> ReadById(int id)
        {
            var budget = _budgets.Find(x => x.Id == id);
            return Task.FromResult(budget);
        }

        public Task<bool> Update(Budget budget)
        {
            var budgetToUpdate = _budgets.Find(x => x.Id == budget.Id);
            if (budgetToUpdate == null)
            {
                return Task.FromResult(false);
            }
            budgetToUpdate.Name = budget.Name;
            budgetToUpdate.StartDate = budget.StartDate;
            budgetToUpdate.FinishDate = budget.FinishDate;
            budgetToUpdate.Description = budget.Description;
            budgetToUpdate.CreatorId = budget.CreatorId;
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Budget>> ReadByCreatorId(int creatorId)
        {
            throw new NotImplementedException();
        }
    }
}
