using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IBudgetRepository
    {
        public Task<Budget?> ReadById(int id);
        public Task<IEnumerable<Budget>> ReadAll();
        public Task<int> Create(Budget budget);
        public Task<bool> Update(Budget budget);
        public Task<bool> Delete(int id);
    }
}
