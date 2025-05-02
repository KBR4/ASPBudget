using Dapper;
using Domain.Entities;
using Infrastructure.Database.TypeMappings;
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
            const string query = @"INSERT INTO users (last_name, first_name, email, password_hash, role)
                  VALUES(@LastName, @FirstName, @Email, @PasswordHash, @Role::user_role)
                  RETURNING id";

            return await _connection.ExecuteScalarAsync<int>(query, user.AsDapperParams());          
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
                @"SELECT id, last_name, first_name, email, password_hash, role FROM users");

            return users.ToList();
        }

        public async Task<User?> ReadById(int id)
        {
            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, last_name, first_name, email, password_hash, role FROM users WHERE id = @Id", new { Id = id });

            return user;
        }

        public async Task<User?> ReadByEmail(string email)
        {
            const string query = "SELECT * FROM users WHERE email = @Email";
            return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<bool> Update(User user)
        {
            const string query = @"UPDATE users
                  SET last_name = @LastName, first_name = @FirstName, email = @Email
                  WHERE id = @Id";
            var affectedRows = await _connection.ExecuteAsync(query, user.AsDapperParams());
            return affectedRows > 0;
        }
    }
}