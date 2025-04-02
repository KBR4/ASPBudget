using Application.Dtos;
using Application.Exceptions;
using Application.Requests;
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

        public async Task<int> Add(CreateUserRequest request)
        {
            var user = new User()
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email,
                BudgetPlans = new List<Budget>()
            };
            return await _userRepository.Create(user);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _userRepository.ReadAll();
            if (users is null || users.Count() == 0)
            {
                throw new NotFoundApplicationException("Users not found");
            }
            var mappedUsers = users.Select(q => _mapper.Map<UserDto>(q)).ToList();
            return mappedUsers;
        }

        public async Task<UserDto?> GetById(int id)
        {
            var user = await _userRepository.ReadById(id);
            var mappedUser = _mapper.Map<UserDto>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UpdateUserRequest request)
        {
            var user = new User()
            {
                Id = request.Id,
                LastName = request.LastName,
                FirstName = request.FirstName,
                Email = request.Email
            };
            return await _userRepository.Update(user);
        }
    }
}
