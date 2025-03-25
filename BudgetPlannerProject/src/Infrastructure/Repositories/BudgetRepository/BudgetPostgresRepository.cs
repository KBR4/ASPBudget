using Dapper;
using Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.BudgetRepository
{
    public class BudgetPostgresRepository : IBudgetRepository
    {
        private readonly NpgsqlConnection _connection;
        public BudgetPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Budget budget)
        {
            await _connection.OpenAsync();
            var budgetId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgets (name, startdate, finishdate, description, creatorId)
                  VALUES(@Name, @StartDate, @FinishDate, @Description, @CreatorId)
                  RETURNING id", new { budget.Name, budget.StartDate, budget.FinishDate, budget.Description, budget.CreatorId });

            await _connection.CloseAsync();
            return budgetId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgets WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Budget>> ReadAll()
        {
            await _connection.OpenAsync();
            var budgets = await _connection.QueryAsync<Budget>(
                @"SELECT id, name, startdate, finishdate, description, creatorId FROM budgets");

            await _connection.CloseAsync();
            return budgets.ToList();
        }

        public async Task<Budget?> ReadById(int id)
        {
            await _connection.OpenAsync();
            var budget = await _connection.QueryFirstOrDefaultAsync<Budget>(
                @"SELECT id, name, startdate, finishdate, description, creatorId FROM budgets WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return budget;
        }

        public async Task<bool> Update(Budget budget)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgets
                  SET name = @Name, startdate = @StartDate, finishdate = @FinishDate, description = @Description, creatorId = @CreatorId
                  WHERE @Id = id", new { budget.Id, budget.Name, budget.StartDate, budget.FinishDate, budget.Description, budget.CreatorId });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }
    }
}
