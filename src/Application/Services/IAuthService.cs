using Application.Requests;
using Application.Responses;
using Domain.Entities;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<int> Register(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        string GenerateJwtToken(User user);
    }
}
