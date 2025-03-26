using Application.Dtos;

namespace Application.Services
{
    public interface IBudgetService
    {
        public Task<BudgetDto?> GetById(int id);
        public Task<IEnumerable<BudgetDto>> GetAll();
        public Task<int> Add(BudgetDto budget);
        public Task<bool> Update(BudgetDto budget);
        public Task<bool> Delete(int id);
    }
}
