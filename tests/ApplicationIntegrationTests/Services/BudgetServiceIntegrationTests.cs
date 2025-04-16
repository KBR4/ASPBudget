using Application.Requests;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    public class BudgetServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IBudgetService _budgetService;
        private readonly IBudgetRepository _budgetRepository;

        public BudgetServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetService = scope.ServiceProvider.GetRequiredService<IBudgetService>();
            _budgetRepository = scope.ServiceProvider.GetRequiredService<IBudgetRepository>();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetInDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();
            var request = new CreateBudgetRequest
            {
                Name = "Test Budget",
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now.AddDays(30),
                Description = "Test Description",
                CreatorId = user.Id
            };

            // Act
            var budgetId = await _budgetService.Add(request);
            var budget = await _budgetRepository.ReadById(budgetId);

            // Assert
            budget.Should().NotBeNull();
            budget!.Name.Should().Be(request.Name);
            budget.CreatorId.Should().Be(user.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetFromDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();
            var budgetId = await _budgetRepository.Create(new Budget
            {
                Name = "Test Budget",
                CreatorId = user.Id
            });

            // Act
            var result = await _budgetService.GetById(budgetId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(budgetId);
            result.Name.Should().Be("Test Budget");
        }
    }
}
