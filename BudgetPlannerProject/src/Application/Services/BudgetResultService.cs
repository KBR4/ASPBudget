using Application.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetResultService : IBudgetResultService
    {
        private IBudgetResultRepository budgetResultRepository;
        public BudgetResultService(IBudgetResultRepository budgetResultRepository)
        {
            this.budgetResultRepository = budgetResultRepository;
        }
        public Task Add(BudgetResultDto budgetResult)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BudgetResultDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetResultDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BudgetResultDto budgetResult)
        {
            throw new NotImplementedException();
        }
    }
}
