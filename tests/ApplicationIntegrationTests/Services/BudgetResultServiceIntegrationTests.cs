using Application.Exceptions;
using Application.Requests;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetResultRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    [Collection("IntegrationTests")]
    public class BudgetResultServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IBudgetResultService _budgetResultService;
        private readonly Faker _faker;

        public BudgetResultServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetResultService = scope.ServiceProvider.GetRequiredService<IBudgetResultService>();
            _faker = new Faker();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetResultInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var request = new CreateBudgetResultRequest
            {
                BudgetId = budget.Id,
                TotalProfit = Math.Round(_faker.Random.Double(1000, 10000), 2)
            };

            // Act
            var resultId = await _budgetResultService.Add(request);

            // Assert
            var budgetResults = await _budgetResultService.GetAll();
            budgetResults.Should().Contain(q => q.BudgetId == request.BudgetId && q.TotalProfit == request.TotalProfit);
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetResultFromDatabase()
        {
            // Arrange
            var createdBudgetResult = await _fixture.CreateBudgetResult();

            // Act
            var result = await _budgetResultService.GetById(createdBudgetResult.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdBudgetResult.Id);
            result.BudgetId.Should().Be(createdBudgetResult.BudgetId);
            result.TotalProfit.Should().Be(createdBudgetResult.TotalProfit);
        }

        [Fact]
        public async Task GetById_ForNonExistingBudgetResultInDatabase_ShouldThrowNotFoundApplicationException()
        {
            // Arrange
            var createdBudgetResult = await _fixture.CreateBudgetResult();

            // Act + Assert
            await _budgetResultService.Invoking(s => s.GetById(createdBudgetResult.Id + 35))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("BudgetResult not found");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBudgetResultsFromDatabase()
        {
            // Arrange
            await _fixture.CreateBudgetResult();
            await _fixture.CreateBudgetResult();

            // Act
            var budgetResults = await _budgetResultService.GetAll();

            // Assert
            budgetResults.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyBudgetResultInDatabase()
        {
            // Arrange          
            var budgetResult = await _fixture.CreateBudgetResult();
            var budget = await _fixture.CreateBudget();
            var newTotalProfit = Math.Round(_faker.Random.Double(1000, 10000), 2);
            var request = new UpdateBudgetResultRequest
            {
                Id = budgetResult.Id,
                BudgetId = budget.Id,
                TotalProfit = newTotalProfit
            };

            // Act
            await _budgetResultService.Update(request);
            var updatedBudgetResult = await _budgetResultService.GetById(budgetResult.Id);

            // Assert
            updatedBudgetResult.Should().NotBeNull();
            updatedBudgetResult.Id.Should().Be(budgetResult.Id);
            updatedBudgetResult.BudgetId.Should().Be(budget.Id);
            updatedBudgetResult.TotalProfit.Should().Be(newTotalProfit);
        }

        [Fact]
        public async Task Updating_NonExistingBudgetResult_ShouldThrowEntityUpdateExceptionException()
        {
            // Arrange          
            var budgetResult = await _fixture.CreateBudgetResult();
            var budget = await _fixture.CreateBudget();
            var newTotalProfit = Math.Round(_faker.Random.Double(1000, 10000), 2);
            var request = new UpdateBudgetResultRequest
            {
                Id = budgetResult.Id + 35,
                BudgetId = budget.Id,
                TotalProfit = newTotalProfit
            };

            // Act + Assert
            await _budgetResultService.Invoking(s => s.Update(request))
            .Should()
            .ThrowAsync<EntityUpdateException>()
            .WithMessage("BudgetResult wasn't updated.");
        }

        [Fact]
        public async Task Delete_ShouldRemoveBudgetResultFromDatabase()
        {
            // Arrange
            var budgetResult = await _fixture.CreateBudgetResult();

            // Act
            await _budgetResultService.Delete(budgetResult.Id);

            // Assert
            await _budgetResultService.Invoking(s => s.GetById(budgetResult.Id))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("BudgetResult not found");
        }

        [Fact]
        public async Task Deleting_NonExistingBudgetResultFromDatabase_ShouldThrowEntityDeleteException()
        {
            // Arrange
            var budgetResult = await _fixture.CreateBudgetResult();

            // Act + Assert
            await _budgetResultService.Invoking(s => s.Delete(budgetResult.Id + 35))
            .Should()
            .ThrowAsync<EntityDeleteException>()
            .WithMessage("Error when deleting BudgetResult.");
        }
    }
}
