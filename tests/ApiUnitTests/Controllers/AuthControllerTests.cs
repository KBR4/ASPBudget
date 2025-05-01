using Api.Controllers;
using Application.Requests;
using Application.Responses;
using Application.Services;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Moq;

namespace ApiUnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsCreatedWithLocationHeader()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                Password = "ValidPass1!"
            };
            const int userId = 1;
            _authServiceMock.Setup(x => x.Register(request)).ReturnsAsync(userId);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UserController.GetById), createdAtActionResult.ActionName);
            Assert.Equal("User", createdAtActionResult.ControllerName);
            Assert.Equal(userId, createdAtActionResult.RouteValues["id"]);

            var valueType = createdAtActionResult.Value.GetType();
            var idProperty = valueType.GetProperty("Id");
            Assert.NotNull(idProperty);
            var idValue = (int)idProperty.GetValue(createdAtActionResult.Value);
            Assert.Equal(userId, idValue);

            _authServiceMock.Verify(x => x.Register(request), Times.Once);
        }

        [Fact]
        public async Task Register_ServiceThrowsException_PropagatesException()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@example.com",
                Password = "ValidPass1!"
            };
            _authServiceMock.Setup(x => x.Register(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Register(request));
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "ValidPass1!"
            };
            var expectedResponse = new LoginResponse { Token = "test-token" };
            _authServiceMock.Setup(x => x.Login(request)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
            _authServiceMock.Verify(x => x.Login(request), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_PropagatesUnauthorizedException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "wrong@example.com",
                Password = "WrongPass1!"
            };
            _authServiceMock.Setup(x => x.Login(request))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.Login(request));
        }
    }
}
