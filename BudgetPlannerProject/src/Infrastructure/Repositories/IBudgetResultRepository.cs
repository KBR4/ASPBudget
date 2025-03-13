using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IBudgetResultRepository
    {
        public Task<BudgetResult?> ReadById(int id);
        public Task<List<BudgetResult>> ReadAll();
        public Task<int> Create(BudgetResult budgetResult);
        public Task<bool> Update(BudgetResult budgetResult);
        public Task<bool> Delete(int id);
    }
}
