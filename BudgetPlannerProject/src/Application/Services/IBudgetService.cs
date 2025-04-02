using Application.Dtos;
using Application.Requests;

namespace Application.Services
{
    public interface IBudgetService
    {
        public Task<BudgetDto?> GetById(int id);
        public Task<IEnumerable<BudgetDto>> GetAll();
        public Task<int> Add(CreateBudgetRequest request);
        public Task<bool> Update(UpdateBudgetRequest request);
        public Task<bool> Delete(int id);
    }
}
