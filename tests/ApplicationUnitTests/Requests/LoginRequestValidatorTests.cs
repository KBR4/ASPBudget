using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;
using Application;
using static Application.Requests.LoginRequest;

namespace ApplicationUnitTests.Requests
{
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator;
        private readonly Faker _faker;

        public LoginRequestValidatorTests()
        {
            _validator = new LoginRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidLoginRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_EmptyEmail_ShouldHaveValidationError(string email)
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("test@")]
        [InlineData("test.com")]
        public void Validate_InvalidEmailFormat_ShouldHaveValidationError(string email)
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_EmptyPassword_ShouldHaveValidationError(string password)
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = password
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_ShortPassword_ShouldHaveValidationError()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = "short"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_LongPassword_ShouldHaveValidationError()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = new string('a', ValidationConstants.MaxPasswordLength + 1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_PasswordWithoutDigit_ShouldHaveValidationError()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = "NoDigitsHere"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_PasswordWithoutUpperAndLower_ShouldHaveValidationError()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = "alllowercase1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validate_PasswordWithSpaces_ShouldHaveValidationError()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = _faker.Person.Email,
                Password = "Password With Spaces1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
