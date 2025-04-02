using Application.Dtos;
using Application.Requests;

namespace Application.Services
{
    public interface IBudgetRecordService
    {
        public Task<BudgetRecordDto?> GetById(int id);
        public Task<IEnumerable<BudgetRecordDto>> GetAll();
        public Task<int> Add(CreateBudgetRecordRequest request);
        public Task<bool> Update(UpdateBudgetRecordRequest request);
        public Task<bool> Delete(int id);
    }
}
