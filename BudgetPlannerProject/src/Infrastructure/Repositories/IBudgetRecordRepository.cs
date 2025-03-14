using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IBudgetRecordRepository
    {
        public Task<BudgetRecord?> ReadById(int id);
        public Task<IEnumerable<BudgetRecord>> ReadAll();
        public Task<int> Create(BudgetRecord budgetRecord);
        public Task<bool> Update(BudgetRecord budgetRecord);
        public Task<bool> Delete(int id);
    }
}
