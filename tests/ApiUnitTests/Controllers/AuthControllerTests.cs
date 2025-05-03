using Api.Controllers;
using Application.Requests;
using Application.Responses;
using Application.Services;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiUnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;
        private readonly Faker _faker = new();

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsCreatedWithLocationHeader()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Password = "ValidPass1!"
            };
            const int userId = 1;
            _authServiceMock.Setup(x => x.Register(request)).ReturnsAsync(userId);

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>()
                .Which.Should().Satisfy<CreatedAtActionResult>(res =>
                {
                    res.ActionName.Should().Be(nameof(UserController.GetById));
                    res.ControllerName.Should().Be("User");
                    res.RouteValues["id"].Should().Be(userId);

                    var value = res.Value;
                    value.GetType().GetProperty("Id").Should().NotBeNull();
                    value.GetType().GetProperty("Id").GetValue(value).Should().Be(userId);
                });

            _authServiceMock.Verify(x => x.Register(request), Times.Once);
        }

        [Fact]
        public async Task Register_ServiceThrowsException_PropagatesException()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Password = "ValidPass1!"
            };
            _authServiceMock.Setup(x => x.Register(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await _controller.Invoking(async x => await x.Register(request))
                .Should().ThrowAsync<Exception>()
                .WithMessage("Database error");
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Internet.Email(),
                Password = "ValidPass1!"
            };
            var expectedResponse = new LoginResponse { Token = "test-token" };
            _authServiceMock.Setup(x => x.Login(request)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResponse);

            _authServiceMock.Verify(x => x.Login(request), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_PropagatesUnauthorizedException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Internet.Email(),
                Password = "WrongPass1!"
            };
            _authServiceMock.Setup(x => x.Login(request))
                .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act & Assert
            await _controller.Invoking(async x => await x.Login(request))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
