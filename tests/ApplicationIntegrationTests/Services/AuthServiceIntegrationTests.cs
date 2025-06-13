using Application.Exceptions;
using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApplicationIntegrationTests.Services
{
    [Collection("IntegrationTests")]
    public class AuthServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();

            // Получаем реальные зависимости
            _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            _passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            // Конфигурация JWT
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["JwtSettings:Secret"] = "super-secret-key-at-least-32-chars",
                    ["JwtSettings:Issuer"] = "test-issuer",
                    ["JwtSettings:Audience"] = "test-audience",
                    ["JwtSettings:ExpirationInMinutes"] = "30"
                })
                .Build();

            _authService = new AuthService(
                config,
                scope.ServiceProvider.GetRequiredService<IMapper>(),
                _userRepository,
                _passwordHasher);
        }
        
        [Fact]
        public async Task Login_WithNonexistentEmail_ShouldThrowInvalidCredentials()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "any-pass"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.Login(request));
        }
    }
}
