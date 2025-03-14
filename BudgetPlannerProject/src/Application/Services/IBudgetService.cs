using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IBudgetService
    {
        public Task<BudgetDto?> GetById(int id);
        public Task<List<BudgetDto>> GetAll();
        public Task<int> Add(BudgetDto budget);
        public Task<bool> Update(BudgetDto budget);
        public Task<bool> Delete(int id);
    }
}
