using Application;
using Application.Exceptions;
using Application.Requests;
using Application.Services;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRecordRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    [Collection("IntegrationTests")]
    public class BudgetRecordServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IBudgetRecordService _budgetRecordService;
        private readonly Faker _faker;

        public BudgetRecordServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetRecordService = scope.ServiceProvider.GetRequiredService<IBudgetRecordService>();
            _faker = new Faker();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetRecordInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var name = _faker.Commerce.ProductName();
            var spendingDate = _faker.Date.Future(3);
            var budgetId = budget.Id;
            var total = Math.Round(_faker.Random.Double(1, 1000), 2);
            var comment = _faker.Rant.Review().ClampLength(10, ValidationConstants.MaxCommentLength);
            var request = new CreateBudgetRecordRequest
            {
                Name = name,
                SpendingDate = spendingDate,
                BudgetId = budgetId,
                Total = total,
                Comment = comment
            };

            // Act
            var budgetRecordId = await _budgetRecordService.Add(request);

            // Assert
            var budgetRecords = await _budgetRecordService.GetAll();
            budgetRecords.Should().Contain(q => q.Name == name 
            && q.SpendingDate.Year == spendingDate.Year
            && q.SpendingDate.Month == spendingDate.Month
            && q.SpendingDate.Day == spendingDate.Day
            && q.BudgetId == budgetId
            && q.Total == total
            && q.Comment == comment
            );
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetRecordFromDatabase()
        {
            // Arrange
            var createdBudgetRecord = await _fixture.CreateBudgetRecord();

            // Act
            var result = await _budgetRecordService.GetById(createdBudgetRecord.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdBudgetRecord.Id);
            result.Name.Should().Be(createdBudgetRecord.Name);
            result.SpendingDate.Should().Be(createdBudgetRecord.SpendingDate);
            result.BudgetId.Should().Be(createdBudgetRecord.BudgetId);
            result.Total.Should().Be(createdBudgetRecord.Total);
            result.Comment.Should().Be(createdBudgetRecord.Comment);
        }

        [Fact]
        public async Task GetById_ForNonExistingBudgetRecordInDatabase_ShouldThrowNotFoundApplicationException()
        {
            // Arrange
            var createdBudgetRecord = await _fixture.CreateBudgetRecord();

            // Act + Assert
            await _budgetRecordService.Invoking(s => s.GetById(createdBudgetRecord.Id + 35))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("BudgetRecord not found.");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBudgetRecordsFromDatabase()
        {
            // Arrange
            await _fixture.CreateBudgetRecord();
            await _fixture.CreateBudgetRecord();

            // Act
            var budgetRecords = await _budgetRecordService.GetAll();

            // Assert
            budgetRecords.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyBudgetRecordInDatabase()
        {
            // Arrange
            var budgetRecord = await _fixture.CreateBudgetRecord();
            var budget = await _fixture.CreateBudget();

            var newName = "NewBudgetRecordName";
            var newSpendingDate = new DateTime(2030, 1, 1);
            var newTotal = 6666.66;
            var newComment = "NewComment";
            var request = new UpdateBudgetRecordRequest
            {
                Id = budgetRecord.Id,
                Name = newName,
                SpendingDate = newSpendingDate,
                Total = newTotal,
                BudgetId = budget.Id,
                Comment = newComment
            };

            // Act
            await _budgetRecordService.Update(request);
            var updatedBudgetRecord = await _budgetRecordService.GetById(budgetRecord.Id);

            // Assert
            updatedBudgetRecord.Should().NotBeNull();
            updatedBudgetRecord.Name.Should().Be(request.Name);
            updatedBudgetRecord.SpendingDate.Should().Be(request.SpendingDate);
            updatedBudgetRecord.Total.Should().Be(request.Total);
            updatedBudgetRecord.BudgetId.Should().Be(request.BudgetId);
            updatedBudgetRecord.Comment.Should().Be(request.Comment);
        }

        [Fact]
        public async Task Updating_NonExistingBudgetRecord_ShouldThrowEntityUpdateExceptionException()
        {
            // Arrange
            var budgetRecord = await _fixture.CreateBudgetRecord();
            var budget = await _fixture.CreateBudget();

            var newName = "NewBudgetRecordName";
            var newSpendingDate = new DateTime(2030, 1, 1);
            var newTotal = 6666.66;
            var newComment = "NewComment";
            var request = new UpdateBudgetRecordRequest
            {
                Id = budgetRecord.Id + 35,
                Name = newName,
                SpendingDate = newSpendingDate,
                Total = newTotal,
                BudgetId = budget.Id,
                Comment = newComment
            };

            // Act + Assert
            await _budgetRecordService.Invoking(s => s.Update(request))
            .Should()
            .ThrowAsync<EntityUpdateException>()
            .WithMessage("BudgetRecord wasn't updated.");
        }

        [Fact]
        public async Task Delete_ShouldRemoveBudgetRecordFromDatabase()
        {
            // Arrange
            var budgetRecord = await _fixture.CreateBudgetRecord();

            // Act
            await _budgetRecordService.Delete(budgetRecord.Id);

            // Assert
            await _budgetRecordService.Invoking(s => s.GetById(budgetRecord.Id))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("BudgetRecord not found.");
        }

        [Fact]
        public async Task Deleting_NonExistingBudgetRecordFromDatabase_ShouldThrowEntityDeleteException()
        {
            // Arrange
            var budgetRecord = await _fixture.CreateBudgetRecord();

            // Act + Assert
            await _budgetRecordService.Invoking(s => s.Delete(budgetRecord.Id + 35))
            .Should()
            .ThrowAsync<EntityDeleteException>()
            .WithMessage("BudgetRecord for deletion not found");
        }
    }
}
