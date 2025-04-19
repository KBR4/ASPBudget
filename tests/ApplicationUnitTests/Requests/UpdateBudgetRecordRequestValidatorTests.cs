using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;
using Application;

namespace ApplicationUnitTests.Requests
{
    public class UpdateBudgetRecordRequestValidatorTests
    {
        private readonly UpdateBudgetRecordRequestValidator _validator;
        private readonly Faker _faker;

        public UpdateBudgetRecordRequestValidatorTests()
        {
            _validator = new UpdateBudgetRecordRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(),
                SpendingDate = _faker.Date.Future(),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void Validate_WrongId_ShouldHaveValidationError(int id)
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = id,
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(4),
                SpendingDate = _faker.Date.Future(3),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Validate_LongName_ShouldHaveValidationError()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(ValidationConstants.MaxBudgetNameLength + 3, ValidationConstants.MaxBudgetNameLength + 10),
                CreationDate = _faker.Date.Past(3),
                SpendingDate = _faker.Date.Future(4),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_PastCreationDate_ShouldHaveValidationError()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = new DateTime(1968, 1, 1),
                SpendingDate = _faker.Date.Future(4),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.CreationDate);
        }

        [Fact]
        public void Validate_PastSpendingDate_ShouldHaveValidationError()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(4),
                SpendingDate = new DateTime(1969, 1, 2),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SpendingDate);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void Validate_WrongBudgetId_ShouldHaveValidationError(int budgetId)
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(4),
                SpendingDate = _faker.Date.Future(3),
                BudgetId = budgetId,
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BudgetId);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public void Validate_WrongTotal_ShouldHaveValidationError(double total)
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(4),
                SpendingDate = _faker.Date.Future(3),
                BudgetId = _faker.Random.Int(1, 100),
                Total = total,
                Comment = _faker.Random.String(1, ValidationConstants.MaxCommentLength)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Total);
        }

        [Fact]
        public void Validate_LongComment_ShouldHaveValidationError()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Random.String(1, ValidationConstants.MaxBudgetNameLength),
                CreationDate = _faker.Date.Past(3),
                SpendingDate = _faker.Date.Future(4),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Random.String(ValidationConstants.MaxCommentLength + 1, ValidationConstants.MaxCommentLength + 10)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Comment);
        }
    }
}
