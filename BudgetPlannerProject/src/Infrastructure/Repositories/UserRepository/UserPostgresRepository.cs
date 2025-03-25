using Dapper;
using Domain.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await _connection.OpenAsync();
            var userId = await _connection.QuerySingleAsync<int>(
                @"INSERT INTO users (lastname, firstname, email)
                  VALUES(@LastName, @FirstName, @Email)
                  RETURNING id", new { user.LastName, user.FirstName, user.Email });

            await _connection.CloseAsync();
            return userId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"DELETE FROM users WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> ReadAll()
        {
            await _connection.OpenAsync();
            var users = await _connection.QueryAsync<User>(
                @"SELECT id, lastname, firstname, email FROM users");

            await _connection.CloseAsync();
            return users.ToList();
        }

        public async Task<User?> ReadById(int id)
        {
            await _connection.OpenAsync();
            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, lastname, firstname, email FROM users WHERE @Id = id", new { Id = id });

            await _connection.CloseAsync();
            return user;
        }

        public async Task<bool> Update(User user)
        {
            await _connection.OpenAsync();
            var affectedRows = await _connection.ExecuteAsync(
                @"UPDATE users
                  SET lastname = @LastName, firstname = @FirstName, email = @Email 
                  WHERE @Id = id", new { user.Id, user.LastName, user.FirstName, user.Email });

            await _connection.CloseAsync();
            return affectedRows > 0;
        }
    }
}