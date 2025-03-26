using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.UserRepository;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(UserDto user)
        {
            var mappedUser = _mapper.Map<User>(user);
            if (mappedUser != null)
            {
                await _userRepository.Create(mappedUser);
            }
            return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _userRepository.ReadAll();
            var mappedUsers = users.Select(q => _mapper.Map<UserDto>(q)).ToList();
            return mappedUsers;
        }

        public async Task<UserDto?> GetById(int id)
        {
            var user = await _userRepository.ReadById(id);
            var mappedUser = _mapper.Map<UserDto>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UserDto user)
        {
            if (user == null)
            {
                return false;
            }
            var mappedUser = _mapper.Map<User>(user);
            return await _userRepository.Update(mappedUser);
        }
    }
}
