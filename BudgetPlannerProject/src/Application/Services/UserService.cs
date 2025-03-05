using Application.Dtos;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public Task Add(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
