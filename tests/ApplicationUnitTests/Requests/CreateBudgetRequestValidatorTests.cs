using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;
using Application;

namespace ApplicationUnitTests.Requests
{
    public class CreateBudgetRequestValidatorTests
    {
        private readonly CreateBudgetRequestValidator _validator;
        private readonly Faker _faker;

        public CreateBudgetRequestValidatorTests()
        {
            _validator = new CreateBudgetRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Random.String(1, ValidationConstants.MaxDescriptionLength),
                CreatorId = _faker.Random.Int(1, 100)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_LongName_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(ValidationConstants.MaxBudgetNameLength + 1, ValidationConstants.MaxBudgetNameLength + 10),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Random.String(1, ValidationConstants.MaxDescriptionLength),
                CreatorId = _faker.Random.Int(1, 100)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_PastStartDate_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength - 10),
                StartDate = new DateTime(1969, 1, 1),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Random.String(1, ValidationConstants.MaxDescriptionLength),
                CreatorId = _faker.Random.Int(1, 100)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartDate);
        }

        [Fact]
        public void Validate_PastFinishDate_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength - 10),
                StartDate = _faker.Date.Future(2),
                FinishDate = new DateTime(1969, 1, 1),
                Description = _faker.Random.String(1, ValidationConstants.MaxDescriptionLength),
                CreatorId = _faker.Random.Int(1, 100)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FinishDate);
        }

        [Fact]
        public void Validate_LongDescription_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Random.String(ValidationConstants.MaxDescriptionLength + 1, ValidationConstants.MaxDescriptionLength + 10),
                CreatorId = _faker.Random.Int(1, 100)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage($"The length of 'Description' must be {ValidationConstants.MaxDescriptionLength} characters or fewer. You entered {request.Description.Length} characters.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void Validate_WrongCreatorId_ShouldHaveValidationError(int creatorId)
        {
            // Arrange
            var request = new CreateBudgetRequest
            {
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Random.String(1, ValidationConstants.MaxDescriptionLength - 10),
                CreatorId = creatorId
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CreatorId);
        }
    }
}
