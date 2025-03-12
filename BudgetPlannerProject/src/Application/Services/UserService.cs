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
        private IUserRepository userRepository;
        private IMapper mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task Add(UserDto user)
        {
            var mappedUser = mapper.Map<User>(user);
            if (mappedUser != null)
            {
                await userRepository.Create(mappedUser);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await userRepository.Delete(id);
        }

        public async Task<List<UserDto>> GetAll()
        {
            var users = await userRepository.ReadAll();
            var mappedUsers = users.Select(q => mapper.Map<UserDto>(q)).ToList();
            return mappedUsers;
        }

        public async Task<UserDto?> GetById(int id)
        {
            var user = await userRepository.ReadById(id);
            var mappedUser = mapper.Map<UserDto>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UserDto user)
        {
            var mappedUser = mapper.Map<User>(user);
            return await userRepository.Update(mappedUser);
        }
    }
}
