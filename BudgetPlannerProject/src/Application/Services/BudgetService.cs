using Application.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetService : IBudgetService
    {
        private IBudgetRepository budgetRepository;
        public BudgetService(IBudgetRepository budgetRepository)
        {
            this.budgetRepository = budgetRepository;
        }
        public Task Add(BudgetDto budget)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BudgetDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BudgetDto budget)
        {
            throw new NotImplementedException();
        }
    }
}
