using Application.Exceptions;
using Application.Requests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService(IConfiguration configuration, IMapper mapper, IUserRepository userRepository, IPasswordHasher hasher) : IAuthService
    {
        public async Task<ClaimsPrincipal> Register(RegistrationRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);
            user.Role = UserRoles.User;

            var userId = await userRepository.Create(user);
            var createdUser = await userRepository.ReadById(userId);
            var principal = GenerateClaimsPrincipal(createdUser);
            return principal;
        }

        public async Task<ClaimsPrincipal> Login(LoginRequest request)
        {
            var user = await userRepository.ReadByEmail(request.Email);
            var passwordVerified =
                hasher.VerifyPassword(request.Password, user?.PasswordHash);
            if (user == null || user?.PasswordHash == null || !passwordVerified)
            {
                throw new UnauthorizedAccessException();
            }

            var principal = GenerateClaimsPrincipal(user);

            return principal;
        }

        private ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {
            var identity = new ClaimsIdentity(new[]
                 {
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            }, "HttponlyAuth");

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
