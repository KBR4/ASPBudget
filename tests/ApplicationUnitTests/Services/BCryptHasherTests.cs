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
