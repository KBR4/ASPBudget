using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;

namespace ApplicationUnitTests.Requests
{
    public class UpdateBudgetResultRequestValidatorTests
    {
        private readonly UpdateBudgetResultRequestValidator _validator;
        private readonly Faker _faker;

        public UpdateBudgetResultRequestValidatorTests()
        {
            _validator = new UpdateBudgetResultRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new UpdateBudgetResultRequest
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = _faker.Random.Double(1, 1000)
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
            var request = new UpdateBudgetResultRequest
            {
                Id = id,
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void Validate_WrongBudgetId_ShouldHaveValidationError(int budgetId)
        {
            // Arrange
            var request = new UpdateBudgetResultRequest
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = budgetId,
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BudgetId);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public void Validate_WrongTotal_ShouldHaveValidationError(double totalProfit)
        {
            // Arrange
            var request = new UpdateBudgetResultRequest
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = totalProfit
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TotalProfit);
        }
    }
}
