using Application.Requests;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRecordRepository;
using Infrastructure.Repositories.BudgetRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    public class BudgetRecordServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly TestingFixture _fixture;
        private readonly IBudgetRecordService _budgetRecordService;
        private readonly IBudgetRecordRepository _budgetRecordRepository;
        private readonly IBudgetRepository _budgetRepository;

        public BudgetRecordServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            var scope = fixture.ServiceProvider.CreateScope();
            _budgetRecordService = scope.ServiceProvider.GetRequiredService<IBudgetRecordService>();
            _budgetRecordRepository = scope.ServiceProvider.GetRequiredService<IBudgetRecordRepository>();
            _budgetRepository = scope.ServiceProvider.GetRequiredService<IBudgetRepository>();
        }

        [Fact]
        public async Task Add_ShouldCreateBudgetRecordInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var request = new CreateBudgetRecordRequest
            {
                Name = "Test123",
                SpendingDate = DateTime.Now,
                BudgetId = budget.Id,
                Total = 100.50,
                Comment = "Comment12345c"
            };

            // Act
            var recordId = await _budgetRecordService.Add(request);
            var record = await _budgetRecordRepository.ReadById(recordId);

            // Assert
            record.Should().NotBeNull();
            record!.Name.Should().Be(request.Name);
            record.BudgetId.Should().Be(budget.Id);
            record.Total.Should().Be(request.Total);
        }

        [Fact]
        public async Task GetById_ShouldReturnBudgetRecordFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var recordId = await _budgetRecordRepository.Create(new BudgetRecord
            {
                Name = "Test123",
                BudgetId = budget.Id,
                Total = 200.75
            });

            // Act
            var result = await _budgetRecordService.GetById(recordId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(recordId);
            result.Name.Should().Be("Test123");
            result.BudgetId.Should().Be(budget.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBudgetRecordsFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            await _budgetRecordRepository.Create(new BudgetRecord { BudgetId = budget.Id, Name = "Record 1" });
            await _budgetRecordRepository.Create(new BudgetRecord { BudgetId = budget.Id, Name = "Record 2" });

            // Act
            var records = await _budgetRecordService.GetAll();

            // Assert
            records.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyBudgetRecordInDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var recordId = await _budgetRecordRepository.Create(new BudgetRecord
            {
                Name = "Original",
                BudgetId = budget.Id,
                Total = 100
            });

            var request = new UpdateBudgetRecordRequest
            {
                Id = recordId,
                Name = "Updated",
                BudgetId = budget.Id,
                Total = 200,
                Comment = "Updated comment"
            };

            // Act
            await _budgetRecordService.Update(request);
            var updatedRecord = await _budgetRecordRepository.ReadById(recordId);

            // Assert
            updatedRecord.Should().NotBeNull();
            updatedRecord!.Name.Should().Be("Updated");
            updatedRecord.Total.Should().Be(200);
            updatedRecord.Comment.Should().Be("Updated comment");
        }

        [Fact]
        public async Task Delete_ShouldRemoveBudgetRecordFromDatabase()
        {
            // Arrange
            var budget = await _fixture.CreateBudget();
            var recordId = await _budgetRecordRepository.Create(new BudgetRecord
            {
                Name = "To Delete",
                BudgetId = budget.Id
            });

            // Act
            await _budgetRecordService.Delete(recordId);
            var deletedRecord = await _budgetRecordRepository.ReadById(recordId);

            // Assert
            deletedRecord.Should().BeNull();
        }
    }
}
