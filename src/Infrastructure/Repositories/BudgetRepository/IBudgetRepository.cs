using Domain.Entities;

namespace Infrastructure.Repositories.BudgetRepository
{
    public interface IBudgetRepository
    {
        public Task<Budget?> ReadById(int id);
        public Task<IEnumerable<Budget>> ReadAll();
        public Task<int> Create(Budget budget);
        public Task<bool> Update(Budget budget);
        public Task<bool> Delete(int id);
        Task<IEnumerable<Budget>> ReadByCreatorId(int creatorId);
    }
}
