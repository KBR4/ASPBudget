using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();
        public UserRepository()
        {
            PopulateTestData();
        }
        private void PopulateTestData()
        {
            var faker = new Faker();
            _users = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                var user = new User();
                user.Id = i + 1;
                user.LastName = faker.Person.LastName;
                user.FirstName = faker.Person.FirstName;
                user.Email = faker.Person.Email;
                List<Budget> budgets = new List<Budget>();
                user.BudgetPlans = budgets;
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
            if (_users.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            _users.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<User>> ReadAll()
        {
            return Task.FromResult(_users);
        }

        public Task<User?> ReadById(int id)
        {
            var user = _users.Find(x => x.Id == id);
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
            userToUpdate.BudgetPlans = user.BudgetPlans;

            return Task.FromResult(true);
        }
    }
}
