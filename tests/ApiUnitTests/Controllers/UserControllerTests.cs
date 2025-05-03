using Api.Controllers;
using Application.Dtos;
using Application.Requests;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Bogus;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApiUnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;
        private readonly Faker _faker;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task GetById_ExistingUser_ReturnsOkWithUser()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            var userDto = new UserDto { 
                Id = userId, 
                LastName = _faker.Person.LastName, 
                FirstName = _faker.Person.FirstName, 
                Email = _faker.Person.Email 
            };
            _userServiceMock.Setup(x => x.GetById(userId)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetById(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task GetById_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            _userServiceMock.Setup(x => x.GetById(userId))
                .ThrowsAsync(new NotFoundApplicationException("User not found"));

            // Act & Assert
            await FluentActions
               .Invoking(() => _controller.GetById(userId))
               .Should()
               .ThrowAsync<NotFoundApplicationException>()
               .WithMessage("User not found");
        }

        [Fact]
        public async Task GetAll_WithUsers_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<UserDto> { new UserDto { Id = 1, FirstName = "xyz", LastName = "abc", Email = "abc@mail.com" }, new UserDto { Id = 2, FirstName = "xyz", LastName = "abc", Email = "abc2@mail.com" } };
            _userServiceMock.Setup(x => x.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task Delete_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);

            // Act
            var result = await _controller.Delete(userId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _userServiceMock.Verify(x => x.Delete(userId), Times.Once);
        }

        [Fact]
        public async Task Update_WhenAdminUpdatesAnyUser_ShouldSucceed()
        {
            // Arrange
            SetCurrentUser(1, UserRoles.Admin);
            var request = CreateTestRequest(2);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _userServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Update_WhenUserUpdatesSelf_ShouldSucceed()
        {
            // Arrange
            var userId = 1;
            SetCurrentUser(userId, UserRoles.User);
            var request = CreateTestRequest(userId);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _userServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Update_WhenUserUpdatesOther_ShouldForbid()
        {
            // Arrange
            SetCurrentUser(1, UserRoles.User);
            var request = CreateTestRequest(2);

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<ForbidResult>();
            _userServiceMock.Verify(x => x.Update(It.IsAny<UpdateUserRequest>()), Times.Never);
        }

        [Fact]
        public async Task Update_WhenUnauthenticated_ShouldThrow()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext();
            var request = CreateTestRequest(1);

            // Act & Assert
            await FluentActions.Awaiting(() => _controller.Update(request))
                .Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task Update_WhenServiceFails_ShouldPropagateException()
        {
            // Arrange
            SetCurrentUser(1, UserRoles.Admin);
            var request = CreateTestRequest(1);
            _userServiceMock.Setup(x => x.Update(request))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await FluentActions.Awaiting(() => _controller.Update(request))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Database error");
        }

        [Theory]
        [InlineData(1, 1, UserRoles.User, true)]  // User updates self
        [InlineData(1, 2, UserRoles.Admin, true)] // Admin updates other
        [InlineData(1, 2, UserRoles.User, false)] // User tries update other
        public async Task Update_AuthorizationScenarios(
            int currentUserId, int requestId,
            UserRoles role, bool shouldSucceed)
        {
            // Arrange
            SetCurrentUser(currentUserId, role);
            var request = CreateTestRequest(requestId);

            // Act
            var result = await _controller.Update(request);

            // Assert
            if (shouldSucceed)
            {
                result.Should().BeOfType<OkResult>();
                _userServiceMock.Verify(x => x.Update(request), Times.Once);
            }
            else
            {
                result.Should().BeOfType<ForbidResult>();
                _userServiceMock.Verify(x => x.Update(It.IsAny<UpdateUserRequest>()), Times.Never);
            }
        }

        private UpdateUserRequest CreateTestRequest(int id) => new()
        {
            Id = id,
            LastName = "abc",
            FirstName = "xyz",
            Email = "abcxyz123@example.com"
        };

        private void SetCurrentUser(int userId, UserRoles role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
        }
    }
}
