using Application.Dtos;
using Application.Requests;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetResultRepository;
using Infrastructure.Repositories.BudgetRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    public class BudgetResultServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IBudgetResultService _budgetResultService;
        private readonly IBudgetResultRepository _budgetResultRepository;

        public BudgetResultServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetResultService = scope.ServiceProvider.GetRequiredService<IBudgetResultService>();
            _budgetResultRepository = scope.ServiceProvider.GetRequiredService<IBudgetResultRepository>();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetResultInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var request = new CreateBudgetResultRequest
            {
                BudgetId = budget.Id,
                TotalProfit = 1500.75
            };

            // Act
            var resultId = await _budgetResultService.Add(request);
            var result = await _budgetResultRepository.ReadById(resultId);

            // Assert
            result.Should().NotBeNull();
            result!.BudgetId.Should().Be(budget.Id);
            result.TotalProfit.Should().Be(request.TotalProfit);
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetResultFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var resultId = await _budgetResultRepository.Create(new BudgetResult
            {
                BudgetId = budget.Id,
                TotalProfit = 2000.50
            });

            // Act
            var result = await _budgetResultService.GetById(resultId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(resultId);
            result.BudgetId.Should().Be(budget.Id);
            result.TotalProfit.Should().Be(2000.50);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBudgetResultsFromDatabase()
        {
            // Arrange
            var budget1 = await _fixture.CreateBudget();
            var budget2 = await _fixture.CreateBudget();
            await _budgetResultRepository.Create(new BudgetResult { BudgetId = budget1.Id, TotalProfit = 1000 });
            await _budgetResultRepository.Create(new BudgetResult { BudgetId = budget2.Id, TotalProfit = 2000 });

            // Act
            var results = await _budgetResultService.GetAll();

            // Assert
            results.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyBudgetResultInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var resultId = await _budgetResultRepository.Create(new BudgetResult
            {
                BudgetId = budget.Id,
                TotalProfit = 1000
            });

            var request = new UpdateBudgetResultRequest
            {
                Id = resultId,
                BudgetId = budget.Id,
                TotalProfit = 1500
            };

            // Act
            await _budgetResultService.Update(request);
            var updatedResult = await _budgetResultRepository.ReadById(resultId);

            // Assert
            updatedResult.Should().NotBeNull();
            updatedResult!.TotalProfit.Should().Be(1500);
        }

        [Fact]
        public async Task Delete_ShouldRemoveBudgetResultFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var resultId = await _budgetResultRepository.Create(new BudgetResult
            {
                BudgetId = budget.Id,
                TotalProfit = 1000
            });

            // Act
            await _budgetResultService.Delete(resultId);
            var deletedResult = await _budgetResultRepository.ReadById(resultId);

            // Assert
            deletedResult.Should().BeNull();
        }
    }
}
