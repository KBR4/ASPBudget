using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using Application.Exceptions;
using System.Security.Claims;

namespace ApplicationUnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _configMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IPasswordHasher> _hasherMock = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            SetupJwtConfig();
            _authService = new AuthService(_configMock.Object, _mapperMock.Object, _userRepoMock.Object, _hasherMock.Object);
        }

        private void SetupJwtConfig()
        {
            _configMock.Setup(x => x["JwtSettings:Secret"]).Returns("super-secret-key-at-least-32-chars");
            _configMock.Setup(x => x["JwtSettings:Issuer"]).Returns("test-issuer");
            _configMock.Setup(x => x["JwtSettings:Audience"]).Returns("test-audience");
            _configMock.Setup(x => x["JwtSettings:ExpirationInMinutes"]).Returns("30");
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "valid@example.com",
                Password = "ValidPass123!"
            };

            var user = new User
            {
                Id = 1,
                Email = request.Email,
                PasswordHash = "hashed-pass",
                FirstName = "abc",
                LastName = "xyz",
                Role = UserRoles.Admin
            };

            _userRepoMock.Setup(x => x.ReadByEmail(request.Email)).ReturnsAsync(user);
            _hasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);

            // Act
            var result = await _authService.Login(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ClaimsPrincipal>();
            result.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
            result.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == UserRoles.Admin.ToString());

            _userRepoMock.Verify(x => x.ReadByEmail(request.Email), Times.Once);
            _hasherMock.Verify(x => x.VerifyPassword(request.Password, user.PasswordHash), Times.Once);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsPrincipal()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "valid@example.com",
                Password = "ValidPass123!"
            };

            var user = new User
            {
                Id = 1,
                Email = request.Email,
                PasswordHash = "hashed-pass",
                FirstName = "abc",
                LastName = "xyz",
                Role = UserRoles.Admin
            };

            _userRepoMock.Setup(x => x.ReadByEmail(request.Email)).ReturnsAsync(user);
            _hasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(true);

            // Act
            var result = await _authService.Login(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ClaimsPrincipal>();
            result.Identities.Should().ContainSingle();
            result.Identities.First().Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
            result.Identities.First().Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString());
            result.Identities.First().Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());

            _userRepoMock.Verify(x => x.ReadByEmail(request.Email), Times.Once);
            _hasherMock.Verify(x => x.VerifyPassword(request.Password, user.PasswordHash), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "valid@example.com",
                Password = "WrongPass"
            };

            var user = new User
            {
                FirstName = "xyz",
                LastName = "abc",
                Email = "valid@example.com",
                PasswordHash = "hashed-pass"
            };

            _userRepoMock.Setup(x => x.ReadByEmail(request.Email)).ReturnsAsync(user);
            _hasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);

            // Act & Assert
            await _authService.Invoking(x => x.Login(request))
                .Should().ThrowAsync<UnauthorizedAccessException>(); // Changed from InvalidCredentialsException
        }

        [Fact]
        public async Task Login_NonexistentUser_ThrowsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "AnyPass111"
            };

            _userRepoMock.Setup(x => x.ReadByEmail(request.Email)).ReturnsAsync((User)null);

            // Act & Assert
            await _authService.Invoking(x => x.Login(request))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
