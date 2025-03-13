using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IBudgetResultService
    {
        public Task<BudgetResultDto?> GetById(int id);
        public Task<List<BudgetResultDto>> GetAll();
        public Task<int> Add(BudgetResultDto budgetResult);
        public Task<bool> Update(BudgetResultDto budgetResult);
        public Task<bool> Delete(int id);
    }
}
