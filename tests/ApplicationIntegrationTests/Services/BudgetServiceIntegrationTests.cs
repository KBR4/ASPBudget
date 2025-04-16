using Application;
using Application.Exceptions;
using Application.Requests;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    [Collection("IntegrationTests")]
    public class BudgetServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly Faker _faker;
        private readonly TestingFixture _fixture;
        private readonly IBudgetService _budgetService;

        public BudgetServiceIntegrationTests(TestingFixture fixture)
        {
            _faker = new Faker();
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetService = scope.ServiceProvider.GetRequiredService<IBudgetService>();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetInDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();
            var name = "Budget1";
            var startDate = _faker.Date.Past(3);
            var finishDate = _faker.Date.Future(4);
            var description = "Desc";
            var request = new CreateBudgetRequest
            {
                Name = name,
                StartDate = startDate,
                FinishDate = finishDate,
                Description = description,
                CreatorId = user.Id
            };

            // Act
            await _budgetService.Add(request);

            // Assert
            var budgets = await _budgetService.GetAll();
            budgets.Should().Contain(q => q.Name == name
                //q.StartDate == startDate && q.FinishDate == finishDate &&
                && q.StartDate!.Year == startDate.Year
                && q.StartDate.Month == startDate.Month
                && q.StartDate.Day == startDate.Day
                && q.FinishDate!.Value.Year == finishDate.Year
                && q.FinishDate.Value.Month == finishDate.Month
                && q.FinishDate.Value.Day == finishDate.Day
                && q.Description == description 
                && q.CreatorId == user.Id);
            //из-за особенностей формата даты в постгресе приходится сравнивать вот так, просто равенство не работает (найти нормальное приведение?)
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetFromDatabase()
        {
            // Arrange
            var createdBudget = await _fixture.CreateBudget();

            // Act
            var result = await _budgetService.GetById(createdBudget.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdBudget.Id);
            result.Name.Should().Be(createdBudget.Name);
            result.StartDate.Should().Be(createdBudget.StartDate);
            result.FinishDate.Should().Be(createdBudget.FinishDate);
            result.Description.Should().Be(createdBudget.Description);
            result.CreatorId.Should().Be(createdBudget.CreatorId);
        }

        [Fact]
        public async Task GetById_ForNonExistingBudgetInDatabase_ShouldThrowNotFoundApplicationException()
        {
            // Arrange
            var createdBudget = await _fixture.CreateBudget();

            // Act + Assert
            await _budgetService.Invoking(s => s.GetById(createdBudget.Id + 35))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("Budget not found.");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBudgetsFromDatabase()
        {
            // Arrange
            await _fixture.CreateBudget();
            await _fixture.CreateBudget();

            // Act
            var budgets = await _budgetService.GetAll();

            // Assert
            budgets.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyBudgetInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var user = await _fixture.CreateUser();
            var newCreatorId = user.Id;

            var newName = "NewBudgetName";
            var newStartDate = new DateTime(2000, 1, 1);
            var newFinishDate = new DateTime(2030, 1, 1);
            var newDescription = "NewDescription";
            var request = new UpdateBudgetRequest
            {
                Id = budget.Id,
                Name = newName,
                StartDate = newStartDate,
                FinishDate = newFinishDate,
                Description = newDescription,
                CreatorId = newCreatorId
            };

            // Act
            await _budgetService.Update(request);
            var updatedBudget = await _budgetService.GetById(budget.Id);

            // Assert
            updatedBudget.Should().NotBeNull();
            updatedBudget.Name.Should().Be(request.Name);
            updatedBudget.StartDate.Should().Be(request.StartDate);
            updatedBudget.FinishDate.Should().Be(request.FinishDate);
            updatedBudget.Description.Should().Be(request.Description);
            updatedBudget.CreatorId.Should().Be(request.CreatorId);
        }

        [Fact]
        public async Task Updating_NonExistingBudget_ShouldThrowEntityUpdateExceptionException()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var user = await _fixture.CreateUser();
            var newCreatorId = user.Id;

            var newName = "NewBudgetName";
            var newStartDate = new DateTime(2000, 1, 1);
            var newFinishDate = new DateTime(2030, 1, 1);
            var newDescription = "NewDescription";
            var request = new UpdateBudgetRequest
            {
                Id = budget.Id + 35,
                Name = newName,
                StartDate = newStartDate,
                FinishDate = newFinishDate,
                Description = newDescription,
                CreatorId = newCreatorId
            };

            // Act + Assert
            await _budgetService.Invoking(s => s.Update(request))
            .Should()
            .ThrowAsync<EntityUpdateException>()
            .WithMessage("Budget wasn't updated.");
        }

        [Fact]
        public async Task Delete_ShouldRemoveBudgetFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();

            // Act
            await _budgetService.Delete(budget.Id);

            // Assert
            await _budgetService.Invoking(s => s.GetById(budget.Id))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("Budget not found.");
        }

        [Fact]
        public async Task Deleting_NonExistingUserFromDatabase_ShouldThrowEntityDeleteException()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();

            // Act + Assert
            await _budgetService.Invoking(s => s.Delete(budget.Id + 35))
            .Should()
            .ThrowAsync<EntityDeleteException>()
            .WithMessage("Error when deleting Budget.");
        }
    }
}
