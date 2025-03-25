using Dapper;
using Npgsql;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await _connection.OpenAsync();
            var budgetResultId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgetResults (budgetId, TotalProfit)
                  VALUES(@BudgetId, @TotalProfit)
                  RETURNING id", new { budgetResult.BudgetId, budgetResult.TotalProfit });

            await _connection.CloseAsync();
            return budgetResultId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgetResults WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<BudgetResult>> ReadAll()
        {
            await _connection.OpenAsync();
            var budgetResults = await _connection.QueryAsync<BudgetResult>(
                @"SELECT Id, BudgetId, TotalProfit FROM budgetResults");

            await _connection.CloseAsync();
            return budgetResults.ToList();
        }

        public async Task<BudgetResult?> ReadById(int id)
        {
            await _connection.OpenAsync();
            var budgetResult = await _connection.QueryFirstOrDefaultAsync<BudgetResult>(
                @"SELECT Id, BudgetId, TotalProfit FROM budgetResults WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return budgetResult;
        }

        public async Task<bool> Update(BudgetResult budgetResult)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgetResults
                  SET BudgetId = @BudgetId, TotalProfit = @TotalProfit 
                  WHERE @Id = id", new { budgetResult.Id, budgetResult.BudgetId, budgetResult.TotalProfit });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }
    }
}
