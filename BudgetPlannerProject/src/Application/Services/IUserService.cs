using Application.Dtos;
using Application.Requests;

namespace Application.Services
{
    public interface IUserService
    {
        public Task<UserDto?> GetById(int id);
        public Task<IEnumerable<UserDto>> GetAll();
        public Task<int> Add(CreateUserRequest request);
        public Task<bool> Update(UpdateUserRequest request);
        public Task<bool> Delete(int id);
    }
}
