using Application.Dtos;

namespace Application.Services
{
    public interface IUserService
    {
        public Task<UserDto?> GetById(int id);
        public Task<IEnumerable<UserDto>> GetAll();
        public Task<int> Add(UserDto user);
        public Task<bool> Update(UserDto user);
        public Task<bool> Delete(int id);
    }
}
