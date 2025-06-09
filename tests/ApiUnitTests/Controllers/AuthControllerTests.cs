using Api.Controllers;
using Application.Requests;
using Application.Responses;
using Application.Services;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

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
        public async Task Register_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Password = "ValidPass1!"
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            _authServiceMock.Setup(x => x.Register(request)).ReturnsAsync(principal);

            // Настраиваем мок для HttpContext и сервисов аутентификации
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    RequestServices = serviceProviderMock.Object
                }
            };

            // Act
            var result = await _controller.Register(request);

            // Assert
            result.Should().BeOfType<CreatedResult>();
            _authServiceMock.Verify(x => x.Register(request), Times.Once);
            authServiceMock.Verify(x => x.SignInAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                principal,
                It.IsAny<AuthenticationProperties>()),
                Times.Once);
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
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Internet.Email(),
                Password = "ValidPass1!"
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            _authServiceMock.Setup(x => x.Login(request)).ReturnsAsync(principal);

            // Настраиваем мок для HttpContext и сервисов аутентификации
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    RequestServices = serviceProviderMock.Object
                }
            };

            // Act
            var result = await _controller.Login(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _authServiceMock.Verify(x => x.Login(request), Times.Once);
            authServiceMock.Verify(x => x.SignInAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                principal,
                It.IsAny<AuthenticationProperties>()),
                Times.Once);
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
