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
            var userDto = new UserDto { Id = userId };
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
            var users = new List<UserDto> { new UserDto { Id = 1 }, new UserDto { Id = 2 } };
            _userServiceMock.Setup(x => x.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateUserRequest();
            var userId = _faker.Random.Int(1, 100);
            _userServiceMock.Setup(x => x.Add(request)).ReturnsAsync(userId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues["id"].Should().Be(userId);
            createdResult.Value.Should().BeEquivalentTo(new { Id = userId });
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateUserRequest();

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _userServiceMock.Verify(x => x.Update(request), Times.Once);
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
    }
}
