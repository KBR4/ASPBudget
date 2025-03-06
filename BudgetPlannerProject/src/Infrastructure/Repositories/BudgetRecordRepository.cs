using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BudgetRecordRepository : IBudgetRecordRepository
    {
        private List<BudgetRecord> budgetRecords = new List<BudgetRecord>();
        public BudgetRecordRepository()
        {

        }
        public Task Create(BudgetRecord budgetRecord)
        {
            budgetRecords.Add(budgetRecord);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (budgetRecords.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            budgetRecords.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<BudgetRecord>> ReadAll()
        {
            return Task.FromResult(budgetRecords);
        }

        public Task<BudgetRecord?> ReadById(int id)
        {
            var budgetRecord = budgetRecords.Find(x => x.Id == id);
            return Task.FromResult(budgetRecord);
        }

        public Task<bool> Update(BudgetRecord budgetRecord)
        {
            var budgetRecordToUpdate = budgetRecords.Find(x => x.Id == budgetRecord.Id);
            if (budgetRecordToUpdate == null)
            {
                return Task.FromResult(false);
            }
            budgetRecordToUpdate.Name = budgetRecord.Name;
            budgetRecordToUpdate.CreationDate = budgetRecord.CreationDate;
            budgetRecordToUpdate.SpendingDate = budgetRecord.SpendingDate;
            budgetRecordToUpdate.Total = budgetRecord.Total;
            budgetRecordToUpdate.Comment = budgetRecord.Comment;

            return Task.FromResult(true);
        }
    }
}
