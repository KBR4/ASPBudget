using Moq;
using BCrypt.Net;
using Application.Services;
using FluentAssertions;

namespace ApplicationUnitTests.Services
{
    public class BCryptHasherTests
    {
        private readonly BCryptHasher _hasher;

        public BCryptHasherTests()
        {
            _hasher = new BCryptHasher();
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "SecurePassword123!";
            var hash = _hasher.HashPassword(password);

            // Act
            var result = _hasher.VerifyPassword(password, hash);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var originalPassword = "SecurePassword123!";
            var wrongPassword = "WrongPassword456!";
            var hash = _hasher.HashPassword(originalPassword);

            // Act
            var result = _hasher.VerifyPassword(wrongPassword, hash);

            // Assert
            result.Should().Be(false);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void VerifyPassword_NullOrEmptyHash_ReturnsFalse(string hash)
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act & Assert for null hash
            var resultNull = _hasher.VerifyPassword(password, hash);
            resultNull.Should().Be(false);
        }
    }
}
