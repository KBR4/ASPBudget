using Application.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<int> Add(UserDto user)
        {
            var mappedUser = mapper.Map<User>(user);
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

        public async Task<List<UserDto>> GetAll()
        {
            var users = await _userRepository.ReadAll();
            var mappedUsers = users.Select(q => mapper.Map<UserDto>(q)).ToList();
            return mappedUsers;
        }

        public async Task<UserDto?> GetById(int id)
        {
            var user = await _userRepository.ReadById(id);
            var mappedUser = mapper.Map<UserDto>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UserDto user)
        {
            var mappedUser = mapper.Map<User>(user);
            return await _userRepository.Update(mappedUser);
        }
    }
}
