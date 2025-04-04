using Dapper;
using Domain.Entities;
using Npgsql;

namespace Infrastructure.Repositories.UserRepository
{
    public class UserPostgresRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection;
        public UserPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(User user)
        {
            var userId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO users (last_name, first_name, email)
                  VALUES(@LastName, @FirstName, @Email)
                  RETURNING id", new { user.LastName, user.FirstName, user.Email });

            return userId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM users WHERE id = @Id", new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> ReadAll()
        {
            var users = await _connection.QueryAsync<User>(
                @"SELECT id, last_name, first_name, email FROM users");

            return users.ToList();
        }

        public async Task<User?> ReadById(int id)
        {
            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, last_name, first_name, email FROM users WHERE id = @Id", new { Id = id });

            return user;
        }

        public async Task<bool> Update(User user)
        {
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE users
                  SET last_name = @LastName, first_name = @FirstName, email = @Email 
                  WHERE id = @Id", new { user.Id, user.LastName, user.FirstName, user.Email });

            return affectedRows > 0;
        }
    }
}