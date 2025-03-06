using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        public Task<UserDto?> GetById(int id);
        public Task<List<UserDto>> GetAll();
        public Task Add(UserDto user);
        public Task<bool> Update(UserDto user);
        public Task<bool> Delete(int id);
    }
}
