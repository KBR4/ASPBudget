using Domain.Entities;

namespace Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<User?> ReadById(int id);
        public Task<User?> ReadByEmail(string email);
        public Task<IEnumerable<User>> ReadAll();
        public Task<int> Create(User user);
        public Task<bool> Update(User user);
        public Task<bool> Delete(int id);
    }
}
