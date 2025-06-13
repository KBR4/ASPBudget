using Dapper;
using Domain.Entities;
using Npgsql;

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
            var budgetId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO budgets (name, start_date, finish_date, description, creator_id)
                  VALUES(@Name, @StartDate, @FinishDate, @Description, @CreatorId)
                  RETURNING id", budget);

            return budgetId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM budgets WHERE id = @Id", new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Budget>> ReadAll()
        {
            var budgets = await _connection.QueryAsync<Budget>(
                @"SELECT id, name, start_date, finish_date, description, creator_id FROM budgets");

            return budgets.ToList();
        }

        public async Task<Budget?> ReadById(int id)
        {
            var budget = await _connection.QueryFirstOrDefaultAsync<Budget>(
                @"SELECT id, name, start_date, finish_date, description, creator_id FROM budgets WHERE id = @Id", new { Id = id });

            return budget;
        }

        public async Task<bool> Update(Budget budget)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE budgets
                  SET name = @Name, start_date = @StartDate, finish_date = @FinishDate, description = @Description, creator_Id = @CreatorId
                  WHERE id = @Id", new { budget.Id, budget.Name, budget.StartDate, budget.FinishDate, budget.Description, budget.CreatorId });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Budget>> ReadByCreatorId(int creatorId)
        {
            var budgets = await _connection.QueryAsync<Budget>(
                @"SELECT id, name, start_date, finish_date, description, creator_id 
                  FROM budgets 
                  WHERE creator_id = @CreatorId", new { CreatorId = creatorId });
            return budgets.ToList();
        }
    }
}
