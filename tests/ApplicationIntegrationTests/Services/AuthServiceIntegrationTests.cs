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
        public async Task Register_ShouldCreateUserWithHashedPassword()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = $"test-{Guid.NewGuid()}@example.com",
                Password = "ValidPass1!"
            };

            // Act
            var userId = await _authService.Register(request);
            var createdUser = await _userRepository.ReadById(userId);

            // Assert
            createdUser.Should().NotBeNull();
            createdUser.Email.Should().Be(request.Email);
            createdUser.FirstName.Should().Be(request.FirstName);
            createdUser.LastName.Should().Be(request.LastName);
            createdUser.Role.Should().Be(UserRoles.User);
            _passwordHasher.VerifyPassword(request.Password, createdUser.PasswordHash)
                .Should().BeTrue();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnValidToken()
        {
            // Arrange
            var user = await _fixture.CreateUser(password: "ValidPass1!");
            var request = new LoginRequest
            {
                Email = user.Email,
                Password = "ValidPass1!"
            };

            // Act
            var result = await _authService.Login(request);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.Token);

            token.Claims.Should().Contain(c =>
                (c.Type == ClaimTypes.Email || c.Type == "email") &&
                c.Value == user.Email);

            token.Claims.Should().Contain(c =>
                (c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid") &&
                c.Value == user.Id.ToString());
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldThrowUnauthorized()
        {
            // Arrange
            var user = await _fixture.CreateUser(password: "correct-pass");
            var request = new LoginRequest
            {
                Email = user.Email,
                Password = "wrong-pass"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.Login(request));
        }

        [Fact]
        public async Task Login_WithNonexistentEmail_ShouldThrowUnauthorized()
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
