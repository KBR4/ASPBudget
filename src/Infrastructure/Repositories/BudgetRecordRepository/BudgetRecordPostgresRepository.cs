using Dapper;
using Domain.Entities;
using Npgsql;

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
            budgetRecord.CreationDate = DateTime.Now;
            var budgetRecordId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgetRecords (name, creation_date, spending_date, budget_id, total, comment)
                  VALUES (@Name, @CreationDate, @SpendingDate, @BudgetId, @Total, @Comment)
                  RETURNING id", budgetRecord);

            return budgetRecordId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgetRecords WHERE id = @Id", new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<BudgetRecord>> ReadAll()
        {
            var budgetRecords = await _connection.QueryAsync<BudgetRecord>(
                @"SELECT id, name, creation_date, spending_date, budget_id, total, comment FROM budgetRecords");

            return budgetRecords.ToList();
        }

        public async Task<BudgetRecord?> ReadById(int id)
        {
            var budgetRecord = await _connection.QueryFirstOrDefaultAsync<BudgetRecord>(
                @"SELECT id, name, creation_date, spending_date, budget_id, total, comment FROM budgetRecords WHERE id = @Id", new { Id = id });

            return budgetRecord;
        }

        public async Task<bool> Update(BudgetRecord budgetRecord)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgetRecords
                SET name = @Name, spending_date = @SpendingDate, 
                      budget_id = @BudgetId, total = @Total, comment = @Comment
                WHERE id = @Id", new { budgetRecord.Id, budgetRecord.Name, budgetRecord.SpendingDate, budgetRecord.BudgetId, budgetRecord.Total, budgetRecord.Comment});

            return affectedRows > 0;
        }

        public async Task<IEnumerable<BudgetRecord>> ReadByBudgetId(int budgetId)
        {
            var budgetRecords = await _connection.QueryAsync<BudgetRecord>(
                @"SELECT id, name, creation_date, spending_date, budget_id, total, comment 
          FROM budgetRecords 
          WHERE budget_id = @BudgetId",
                new { BudgetId = budgetId });
            return budgetRecords.ToList();
        }
    }
}
