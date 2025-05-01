using Bogus;
using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories.UserRepository
{
    [ExcludeFromCodeCoverage]
    public class UserInMemoryRepository : IUserRepository
    {
        private List<User> _users = new List<User>();

        public UserInMemoryRepository()
        {
            PopulateTestData();
        }

        private void PopulateTestData()
        {
            var faker = new Faker();
            for (int i = 0; i < 10; i++)
            {
                var user = new User();
                user.Id = i + 1;
                user.LastName = faker.Person.LastName;
                user.FirstName = faker.Person.FirstName;
                user.Email = faker.Person.Email;
                _users.Add(user);
            }
        }

        public Task<int> Create(User user)
        {
            _users.Add(user);
            return Task.FromResult(user.Id);
        }

        public Task<bool> Delete(int id)
        {
            if (!_users.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            _users.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<User>> ReadAll()
        {
            return Task.FromResult<IEnumerable<User>>(_users);
        }

        public Task<User?> ReadById(int id)
        {
            var user = _users.Find(x => x.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> ReadByEmail(string email)
        {
            var user = _users.Find(x => x.Email == email);
            return Task.FromResult(user);
        }

        public Task<bool> Update(User user)
        {
            var userToUpdate = _users.Find(x => x.Id == user.Id);
            if (userToUpdate == null)
            {
                return Task.FromResult(false);
            }
            userToUpdate.LastName = user.LastName;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.Email = user.Email;
            return Task.FromResult(true);
        }
    }
}
