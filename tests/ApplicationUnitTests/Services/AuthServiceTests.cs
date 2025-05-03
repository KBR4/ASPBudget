using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;

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
        public async Task Register_ValidRequest_ReturnsUserId()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                Email = "test@example.com",
                Password = "Password123!",
                FirstName = "abc",
                LastName = "xyz"
            };

            var user = new User { Id = 1, FirstName = "xyz", LastName = "abc", Email = "abc@mail.com" };
            _mapperMock.Setup(x => x.Map<User>(request)).Returns(user);
            _hasherMock.Setup(x => x.HashPassword(request.Password)).Returns("hashed-pass");
            _userRepoMock.Setup(x => x.Create(user)).ReturnsAsync(1);

            // Act
            var result = await _authService.Register(request);

            // Assert
            result.Should().Be(1);
            _mapperMock.Verify(x => x.Map<User>(request), Times.Once);
            _hasherMock.Verify(x => x.HashPassword(request.Password), Times.Once);
            _userRepoMock.Verify(x => x.Create(It.Is<User>(u =>
                u.PasswordHash == "hashed-pass" &&
                u.Role == UserRoles.User)), Times.Once);
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
            result.Token.Should().NotBeNullOrEmpty();
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

            var user = new User { FirstName = "xyz", LastName = "abc", Email = "abc@mail.com", PasswordHash = "hashed-pass" };
            _userRepoMock.Setup(x => x.ReadByEmail(request.Email)).ReturnsAsync(user);
            _hasherMock.Setup(x => x.VerifyPassword(request.Password, user.PasswordHash)).Returns(false);

            // Act & Assert
            await _authService.Invoking(x => x.Login(request))
                .Should().ThrowAsync<UnauthorizedAccessException>();
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
