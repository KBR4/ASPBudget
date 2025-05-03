using Api.ExceptionHandlers;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Data.Common;
using FluentAssertions;

namespace ApiUnitTests.ExceptionHandlers
{
    public class ExceptionHandlersTests
    {

        [Fact]
        public async Task ApplicationExceptionHandler_HandlesApplicationException_ReturnsProblemDetails()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new ApplicationExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new NotFoundApplicationException("");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            context.Response.StatusCode.Should().Be(404);
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
        }

        [Fact]
        public async Task ApplicationExceptionHandler_IgnoresNonApplicationException_ReturnsFalse()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new ApplicationExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Exception("");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
        }

        [Fact]
        public async Task DbExceptionHandler_HandlesDbException_ReturnsProblemDetails()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new DbExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Mock<DbException>().Object;

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
        }

        [Fact]
        public async Task DbExceptionHandler_IgnoresNonDbException_ReturnsFalse()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new DbExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Exception("");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
        }

        [Fact]
        public async Task GlobalExceptionHandler_HandlesAnyException_ReturnsProblemDetails()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new GlobalExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Exception("");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            context.Response.StatusCode.Should().Be(500);
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
        }

        [Fact]
        public async Task InvalidCredentialsExceptionHandler_HandlesInvalidCredentialsException_ReturnsProblemDetails()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new InvalidCredentialsExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new InvalidCredentialsException("Invalid credentials");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            problemDetailsServiceMock.Verify(
                x => x.WriteAsync(It.Is<ProblemDetailsContext>(ctx =>
                    ctx.ProblemDetails.Status == StatusCodes.Status403Forbidden &&
                    ctx.ProblemDetails.Title == "Invalid credentials" &&
                    ctx.ProblemDetails.Detail == "Invalid credentials")),
                Times.Once);
        }

        [Fact]
        public async Task InvalidCredentialsExceptionHandler_IgnoresNonInvalidCredentialsException_ReturnsFalse()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new InvalidCredentialsExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Exception("Some other exception");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            problemDetailsServiceMock.Verify(x => x.WriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
        }

        [Fact]
        public async Task InvalidCredentialsExceptionHandler_SetsCorrectProblemDetailsType()
        {
            // Arrange
            var problemDetailsServiceMock = new Mock<IProblemDetailsService>();
            var handler = new InvalidCredentialsExceptionHandler(problemDetailsServiceMock.Object);

            var context = new DefaultHttpContext();
            var exception = new InvalidCredentialsException("Test");

            // Act
            await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            problemDetailsServiceMock.Verify(
                x => x.WriteAsync(It.Is<ProblemDetailsContext>(ctx =>
                    ctx.ProblemDetails.Type == nameof(InvalidCredentialsException))),
                Times.Once);
        }
    }
}
