using Dapper;
using Npgsql;
using Domain.Entities;

namespace Infrastructure.Repositories.BudgetResultRepository
{
    public class BudgetResultPostgresRepository : IBudgetResultRepository
    {
        private readonly NpgsqlConnection _connection;
        public BudgetResultPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(BudgetResult budgetResult)
        {
            var budgetResultId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgetResults (budget_id, total_profit)
                  VALUES(@BudgetId, @TotalProfit)
                  RETURNING id", new { budgetResult.BudgetId, budgetResult.TotalProfit });

            return budgetResultId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgetResults WHERE id = @Id", new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<BudgetResult>> ReadAll()
        {
            var budgetResults = await _connection.QueryAsync<BudgetResult>(
                @"SELECT id, budget_id, total_profit FROM budgetResults");

            return budgetResults.ToList();
        }

        public async Task<BudgetResult?> ReadById(int id)
        {
            var budgetResult = await _connection.QueryFirstOrDefaultAsync<BudgetResult>(
                @"SELECT id, budget_id, total_profit FROM budgetResults WHERE id = @Id", new { Id = id });

            return budgetResult;
        }

        public async Task<bool> Update(BudgetResult budgetResult)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgetResults
                  SET budget_id = @budgetId, total_profit = @TotalProfit 
                  WHERE id = @Id", budgetResult);

            return affectedRows > 0;
        }
    }
}
