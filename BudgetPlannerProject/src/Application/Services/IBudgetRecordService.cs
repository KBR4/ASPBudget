using Application.Dtos;

namespace Application.Services
{
    public interface IBudgetRecordService
    {
        public Task<BudgetRecordDto?> GetById(int id);
        public Task<IEnumerable<BudgetRecordDto>> GetAll();
        public Task<int> Add(BudgetRecordDto budgetRecord);
        public Task<bool> Update(BudgetRecordDto budgetRecord);
        public Task<bool> Delete(int id);
    }
}
