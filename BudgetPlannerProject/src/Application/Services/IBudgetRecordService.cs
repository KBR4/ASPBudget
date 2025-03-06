using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IBudgetRecordService
    {
        public Task<BudgetRecordDto?> GetById(int id);
        public Task<List<BudgetRecordDto>> GetAll();
        public Task Add(BudgetRecordDto budgetRecord);
        public Task<bool> Update(BudgetRecordDto budgetRecord);
        public Task<bool> Delete(int id);
    }
}
