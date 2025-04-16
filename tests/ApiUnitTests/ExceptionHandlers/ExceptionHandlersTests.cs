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
    }
}
