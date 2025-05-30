﻿using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;
using Application;

namespace ApplicationUnitTests.Requests
{
    public class CreateUserRequestValidatorTests
    {
        private readonly CreateUserRequestValidator _validator;
        private readonly Faker _faker;

        public CreateUserRequestValidatorTests()
        {
            _validator = new CreateUserRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email
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
        public void Validate_EmptyFirstName_ShouldHaveValidationError(string firstName)
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = firstName,
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email
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
            var request = new CreateUserRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = lastName,
                Email = _faker.Person.Email
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
            var request = new CreateUserRequest
            {
                FirstName = _faker.Random.String(ValidationConstants.MaxUserNameLength + 1, ValidationConstants.MaxUserNameLength + 10),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Person.Email
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
            var request = new CreateUserRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(ValidationConstants.MaxUserNameLength + 1, ValidationConstants.MaxUserNameLength + 10),
                Email = _faker.Person.Email
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
        [Fact]
        public void Validate_LongEmail_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                LastName = _faker.Random.String(1, ValidationConstants.MaxUserNameLength),
                Email = _faker.Random.String(ValidationConstants.MaxEmailLength + 1, ValidationConstants.MaxEmailLength + 10) + "@mail.ru"
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}
