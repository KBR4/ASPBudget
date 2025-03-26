using Application.Dtos;

namespace Application.Services
{
    public interface IBudgetResultService
    {
        public Task<BudgetResultDto?> GetById(int id);
        public Task<IEnumerable<BudgetResultDto>> GetAll();
        public Task<int> Add(BudgetResultDto budgetResult);
        public Task<bool> Update(BudgetResultDto budgetResult);
        public Task<bool> Delete(int id);
    }
}
