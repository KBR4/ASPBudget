using Domain.Entities;

namespace Infrastructure.Repositories.BudgetRecordRepository
{
    public interface IBudgetRecordRepository
    {
        public Task<BudgetRecord?> ReadById(int id);
        public Task<IEnumerable<BudgetRecord>> ReadAll();
        public Task<int> Create(BudgetRecord budgetRecord);
        public Task<bool> Update(BudgetRecord budgetRecord);
        public Task<bool> Delete(int id);
    }
}
