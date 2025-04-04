using Application.Dtos;
using Application.Requests;

namespace Application.Services
{
    public interface IBudgetResultService
    {
        public Task<BudgetResultDto?> GetById(int id);
        public Task<IEnumerable<BudgetResultDto>> GetAll();
        public Task<int> Add(CreateBudgetResultRequest request);
        public Task Update(UpdateBudgetResultRequest request);
        public Task Delete(int id);
    }
}
