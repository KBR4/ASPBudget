using Application.Requests;
using FluentValidation.TestHelper;
using Bogus;

namespace ApplicationUnitTests.Requests
{
    public class CreateBudgetResultRequestValidatorTests
    {
        private readonly CreateBudgetResultRequestValidator _validator;
        private readonly Faker _faker;

        public CreateBudgetResultRequestValidatorTests()
        {
            _validator = new CreateBudgetResultRequestValidator();
            _faker = new Faker();
        }

        [Fact]
        public void Validate_ValidRequest_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var request = new CreateBudgetResultRequest
            {
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
        public void Validate_WrongBudgetId_ShouldHaveValidationError(int budgetId)
        {
            // Arrange
            var request = new CreateBudgetResultRequest
            {
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
            var request = new CreateBudgetResultRequest
            {
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
