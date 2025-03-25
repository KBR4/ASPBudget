using Dapper;
using Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.BudgetRecordRepository
{
    public class BudgetRecordPostgresRepository : IBudgetRecordRepository
    {
        private readonly NpgsqlConnection _connection;
        public BudgetRecordPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(BudgetRecord budgetRecord)
        {
            await _connection.OpenAsync();
            var budgetRecordId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgetRecords (Name, CreationDate, SpendingDate, BudgetId, Total, Comment)
                  VALUES(@Name, @CreationDate, @SpendingDate, @BudgetId, @Total, @Comment)
                  RETURNING id", new { budgetRecord.Name, budgetRecord.CreationDate, budgetRecord.SpendingDate, 
                    budgetRecord.BudgetId, budgetRecord.Total, budgetRecord.Comment });

            await _connection.CloseAsync();
            return budgetRecordId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgetRecords WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<BudgetRecord>> ReadAll()
        {
            await _connection.OpenAsync();
            var budgetRecords = await _connection.QueryAsync<BudgetRecord>(
                @"SELECT Id, Name, CreationDate, SpendingDate, BudgetId, Total, Comment FROM budgetRecords");

            await _connection.CloseAsync();
            return budgetRecords.ToList();
        }

        public async Task<BudgetRecord?> ReadById(int id)
        {
            await _connection.OpenAsync();
            var budgetRecord = await _connection.QueryFirstOrDefaultAsync<BudgetRecord>(
                @"SELECT Name, CreationDate, SpendingDate, BudgetId, Total, Comment FROM budgetRecords WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return budgetRecord;
        }

        public async Task<bool> Update(BudgetRecord budgetRecord)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgetRecords
                SET Name = @Name, CreationDate = @CreationDate, SpendingDate = @SpendingDate, 
                      BudgetId = @BudgetId, Total = @Total, Comment = @Comment
                WHERE @Id = id", new {
                      budgetRecord.Id,
                      budgetRecord.Name,
                      budgetRecord.CreationDate,
                      budgetRecord.SpendingDate,
                      budgetRecord.BudgetId,
                      budgetRecord.Total,
                      budgetRecord.Comment
                });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }
    }
}
