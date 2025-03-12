using Application.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetRecordService : IBudgetRecordService
    {
        private IBudgetRecordRepository budgetRecordRepository;
        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository)
        {
            this.budgetRecordRepository = budgetRecordRepository;
        }
        public Task Add(BudgetRecordDto budgetRecord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BudgetRecordDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BudgetRecordDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BudgetRecordDto budgetRecord)
        {
            throw new NotImplementedException();
        }
    }
}
