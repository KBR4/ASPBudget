using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> ReadById(int id);
        public Task<List<User>> ReadAll();
        public Task Create(User user);
        public Task<bool> Update(User user);
        public Task<bool> Delete(int id);
    }
}
