using Moq;
using BCrypt.Net;
using Application.Services;

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
        public void HashPassword_ValidPassword_ReturnsNonEmptyHash()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var result = _hasher.HashPassword(password);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.NotEqual(password, result);
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
            Assert.True(result);
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
            Assert.False(result);
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
            Assert.False(resultNull);
        }

        [Fact]
        public void HashPassword_AlwaysGeneratesUniqueSalt()
        {
            // Arrange
            var password = "SecurePassword123!";

            // Act
            var hash1 = _hasher.HashPassword(password);
            var hash2 = _hasher.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1, hash2); // Different salts should produce different hashes
            Assert.True(_hasher.VerifyPassword(password, hash1)); // Both should verify correctly
            Assert.True(_hasher.VerifyPassword(password, hash2));
        }
    }
}
