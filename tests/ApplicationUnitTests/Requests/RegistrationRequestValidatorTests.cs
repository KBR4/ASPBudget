using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;
using Application;
using static Application.Requests.RegistrationRequest;

namespace ApplicationUnitTests.Requests
{
    public class RegistrationRequestValidatorTests
    {
        private readonly RegistrationRequestValidator _validator;
        private readonly Faker _faker;

        public RegistrationRequestValidatorTests()
        {
            _validator = new RegistrationRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRegistrationRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
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
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email,
                Password = "Password With Spaces1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_EmptyFirstName_ShouldHaveValidationError(string firstName)
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = firstName,
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_EmptyLastName_ShouldHaveValidationError(string lastName)
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = lastName,
                Email = _faker.Person.Email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Validate_LongFirstName_ShouldHaveValidationError()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(ValidationConstants.MaxUserNameLength + 1, ValidationConstants.MaxUserNameLength + 10),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validate_LongLastName_ShouldHaveValidationError()
        {
            // Arrange
            var request = new RegistrationRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(ValidationConstants.MaxUserNameLength + 1, ValidationConstants.MaxUserNameLength + 10),
                Email = _faker.Person.Email,
                Password = "ValidPass1"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
    }
}

