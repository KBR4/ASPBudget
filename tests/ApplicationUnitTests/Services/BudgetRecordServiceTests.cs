using Application.Exceptions;
using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRecordRepository;
using Moq;
using Bogus;
using Application.Mappings;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class BudgetRecordServiceTests
    {
        private readonly Mock<IBudgetRecordRepository> _budgetRecordRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IBudgetRecordService _budgetRecordService;
        private readonly Faker _faker;

        public BudgetRecordServiceTests()
        {
            _budgetRecordRepositoryMock = new Mock<IBudgetRecordRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            var loggerMock = new Mock<ILogger<BudgetRecordService>>();
            _budgetRecordService = new BudgetRecordService(
                _budgetRecordRepositoryMock.Object,
                _mapper, loggerMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsBudgetRecordId()
        {
            // Arrange
            var budgetRecordId = _faker.Random.Int(1, 100);
            var budgetId = _faker.Random.Int(1, 100);
            var request = new CreateBudgetRecordRequest
            {
                Name = _faker.Commerce.ProductName(),
                SpendingDate = _faker.Date.Future(),
                BudgetId = budgetId,
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Lorem.Sentence()
            };

            _budgetRecordRepositoryMock.Setup(x => x.Create(It.IsAny<BudgetRecord>()))
                .ReturnsAsync(budgetRecordId);

            // Act
            var result = await _budgetRecordService.Add(request);

            // Assert
            result.Should().Be(budgetRecordId);
            _budgetRecordRepositoryMock.Verify(x => x.Create(It.Is<BudgetRecord>(br =>
                br.Name == request.Name &&
                br.SpendingDate == request.SpendingDate &&
                br.BudgetId == request.BudgetId &&
                br.Total == request.Total &&
                br.Comment == request.Comment)), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingBudgetRecord_ReturnsBudgetRecordDto()
        {
            // Arrange
            var budgetRecord = new BudgetRecord
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                SpendingDate = _faker.Date.Future(),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Lorem.Sentence()
            };

            _budgetRecordRepositoryMock.Setup(x => x.ReadById(budgetRecord.Id))
                .ReturnsAsync(budgetRecord);

            // Act
            var result = await _budgetRecordService.GetById(budgetRecord.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(budgetRecord.Id);
            result.Name.Should().Be(budgetRecord.Name);
            result.SpendingDate.Should().Be(budgetRecord.SpendingDate);
            result.BudgetId.Should().Be(budgetRecord.BudgetId);
            result.Total.Should().Be(budgetRecord.Total);
            result.Comment.Should().Be(budgetRecord.Comment);
        }

        [Fact]
        public async Task GetById_NonExistingBudgetRecord_ThrowsNotFoundException()
        {
            // Arrange
            var budgetRecordId = _faker.Random.Int(1, 100);
            _budgetRecordRepositoryMock.Setup(x => x.ReadById(budgetRecordId))
                .ReturnsAsync((BudgetRecord)null);

            // Act & Assert
            await _budgetRecordService.Invoking(x => x.GetById(budgetRecordId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage("BudgetRecord not found.");
        }

        [Fact]
        public async Task GetAll_WithBudgetRecords_ReturnsBudgetRecordDtos()
        {
            // Arrange
            var budgetRecords = new List<BudgetRecord>
            {
                new BudgetRecord { Id = 1, Name = "Record 1" },
                new BudgetRecord { Id = 2, Name = "Record 2" }
            };

            _budgetRecordRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(budgetRecords);

            // Act
            var result = await _budgetRecordService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task Update_ValidRequest_UpdatesBudgetRecord()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                SpendingDate = _faker.Date.Future(),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Lorem.Sentence()
            };

            _budgetRecordRepositoryMock.Setup(x => x.Update(It.IsAny<BudgetRecord>()))
                .ReturnsAsync(true);

            // Act
            await _budgetRecordService.Update(request);

            // Assert
            _budgetRecordRepositoryMock.Verify(x => x.Update(It.Is<BudgetRecord>(br =>
                br.Id == request.Id &&
                br.Name == request.Name &&
                br.SpendingDate == request.SpendingDate &&
                br.BudgetId == request.BudgetId &&
                br.Total == request.Total &&
                br.Comment == request.Comment)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingBudgetRecord_ThrowsUpdateException()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                SpendingDate = _faker.Date.Future(),
                BudgetId = _faker.Random.Int(1, 100),
                Total = _faker.Random.Double(1, 1000),
                Comment = _faker.Lorem.Sentence()
            };

            _budgetRecordRepositoryMock.Setup(x => x.Update(It.IsAny<BudgetRecord>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetRecordService.Invoking(x => x.Update(request))
                .Should().ThrowAsync<EntityUpdateException>()
                .WithMessage("BudgetRecord wasn't updated.");
        }

        [Fact]
        public async Task Delete_ExistingBudgetRecord_DeletesBudgetRecord()
        {
            // Arrange
            var budgetRecordId = _faker.Random.Int(1, 100);
            _budgetRecordRepositoryMock.Setup(x => x.Delete(budgetRecordId))
                .ReturnsAsync(true);

            // Act
            await _budgetRecordService.Delete(budgetRecordId);

            // Assert
            _budgetRecordRepositoryMock.Verify(x => x.Delete(budgetRecordId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingBudgetRecord_ThrowsDeleteException()
        {
            // Arrange
            var budgetRecordId = _faker.Random.Int(1, 100);
            _budgetRecordRepositoryMock.Setup(x => x.Delete(budgetRecordId))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetRecordService.Invoking(x => x.Delete(budgetRecordId))
                .Should().ThrowAsync<EntityDeleteException>()
                .WithMessage("BudgetRecord for deletion not found");
        }
    }
}
