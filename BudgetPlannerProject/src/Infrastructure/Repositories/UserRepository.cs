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
        private List<User> users = new List<User>();
        public UserRepository()
        {

        }
        public Task Create(User user)
        {
            users.Add(user);
            return Task.CompletedTask;
        }

        public Task<bool> Delete(int id)
        {
            if (users.Any(x => x.Id == id))
            {
                return Task.FromResult(false);
            }
            users.RemoveAll(x => x.Id == id);
            return Task.FromResult(true);
        }

        public Task<List<User>> ReadAll()
        {
            return Task.FromResult(users);
        }

        public Task<User?> ReadById(int id)
        {
            var user = users.Find(x => x.Id == id);
            return Task.FromResult(user);
        }

        public Task<bool> Update(User user)
        {
            var userToUpdate = users.Find(x => x.Id == user.Id);
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
