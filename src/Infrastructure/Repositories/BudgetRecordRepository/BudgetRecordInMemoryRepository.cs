﻿using Bogus;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories.BudgetRecordRepository
{
    [ExcludeFromCodeCoverage]
    public class BudgetRecordInMemoryRepository : IBudgetRecordRepository
    {
        private List<BudgetRecord> _budgetRecords = new List<BudgetRecord>();

        public BudgetRecordInMemoryRepository()
        {
            PopulateTestData();
        }

        private void PopulateTestData()
        {
            var faker = new Faker();
            for (int i = 0; i < 10; i++)
            {
                var budgetRecord = new BudgetRecord();
                budgetRecord.Id = i + 1;
                budgetRecord.Name = faker.Commerce.Product();
                budgetRecord.CreationDate = faker.Date.Future(2, DateTime.Now);
                budgetRecord.SpendingDate = faker.Date.Future(3, budgetRecord.CreationDate);
                budgetRecord.Total = Convert.ToDouble(faker.Commerce.Price(1, 1000));
                budgetRecord.Comment = faker.Commerce.ProductDescription();
                _budgetRecords.Add(budgetRecord);
            }
        }

        public Task<int> Create(BudgetRecord budgetRecord)
        {
            _budgetRecords.Add(budgetRecord);
            return Task.FromResult(budgetRecord.Id);
        }

        public Task<bool> Delete(int id)
        {
            if (!_budgetRecords.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            _budgetRecords.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<BudgetRecord>> ReadAll()
        {
            return Task.FromResult<IEnumerable<BudgetRecord>>(_budgetRecords);
        }

        public Task<BudgetRecord?> ReadById(int id)
        {
            var budgetRecord = _budgetRecords.Find(x => x.Id == id);
            return Task.FromResult(budgetRecord);
        }

        public Task<bool> Update(BudgetRecord budgetRecord)
        {
            var budgetRecordToUpdate = _budgetRecords.Find(x => x.Id == budgetRecord.Id);
            if (budgetRecordToUpdate == null)
            {
                return Task.FromResult(false);
            }
            budgetRecordToUpdate.Name = budgetRecord.Name;
            budgetRecordToUpdate.CreationDate = budgetRecord.CreationDate;
            budgetRecordToUpdate.SpendingDate = budgetRecord.SpendingDate;
            budgetRecordToUpdate.BudgetId = budgetRecord.BudgetId;
            budgetRecordToUpdate.Total = budgetRecord.Total;
            budgetRecordToUpdate.Comment = budgetRecord.Comment;
            return Task.FromResult(true);
        }

        public Task<IEnumerable<BudgetRecord>> ReadByBudgetId(int budgetId)
        {
            throw new NotImplementedException();
        }
    }
}
