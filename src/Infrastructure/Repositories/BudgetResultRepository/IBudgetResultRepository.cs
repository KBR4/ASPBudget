using Domain.Entities;

namespace Infrastructure.Repositories.BudgetResultRepository
{
    public interface IBudgetResultRepository
    {
        public Task<BudgetResult?> ReadById(int id);
        public Task<IEnumerable<BudgetResult>> ReadAll();
        public Task<int> Create(BudgetResult budgetResult);
        public Task<bool> Update(BudgetResult budgetResult);
        public Task<bool> Delete(int id);
    }
}
