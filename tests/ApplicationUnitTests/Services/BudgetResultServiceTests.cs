using Application.Exceptions;
using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetResultRepository;
using Moq;
using Bogus;
using Application.Mappings;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class BudgetResultServiceTests
    {
        private readonly Mock<IBudgetResultRepository> _budgetResultRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IBudgetResultService _budgetResultService;
        private readonly Faker _faker;

        public BudgetResultServiceTests()
        {
            _budgetResultRepositoryMock = new Mock<IBudgetResultRepository>();
            var loggerMock = new Mock<ILogger<BudgetResultService>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            _budgetResultService = new BudgetResultService(
                _budgetResultRepositoryMock.Object,
                _mapper, loggerMock.Object);

            _faker = new Faker();
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsBudgetResultId()
        {
            // Arrange
            var budgetResultId = _faker.Random.Int(1, 100);
            var budgetId = _faker.Random.Int(1, 100);
            var request = new CreateBudgetResultRequest
            {
                BudgetId = budgetId,
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            _budgetResultRepositoryMock.Setup(x => x.Create(It.IsAny<BudgetResult>()))
                .ReturnsAsync(budgetResultId);

            // Act
            var result = await _budgetResultService.Add(request);

            // Assert
            result.Should().Be(budgetResultId);
            _budgetResultRepositoryMock.Verify(x => x.Create(It.Is<BudgetResult>(br =>
                br.BudgetId == request.BudgetId &&
                br.TotalProfit == request.TotalProfit)), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingBudgetResult_ReturnsBudgetResultDto()
        {
            // Arrange
            var budgetResult = new BudgetResult
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            _budgetResultRepositoryMock.Setup(x => x.ReadById(budgetResult.Id))
                .ReturnsAsync(budgetResult);

            // Act
            var result = await _budgetResultService.GetById(budgetResult.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(budgetResult.Id);
            result.BudgetId.Should().Be(budgetResult.BudgetId);
            result.TotalProfit.Should().Be(budgetResult.TotalProfit);
        }

        [Fact]
        public async Task GetById_NonExistingBudgetResult_ThrowsNotFoundException()
        {
            // Arrange
            var budgetResultId = _faker.Random.Int(1, 100);
            _budgetResultRepositoryMock.Setup(x => x.ReadById(budgetResultId))
                .ReturnsAsync((BudgetResult)null);

            // Act & Assert
            await _budgetResultService.Invoking(x => x.GetById(budgetResultId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage("BudgetResult not found");
        }

        [Fact]
        public async Task GetAll_WithBudgetResults_ReturnsBudgetResultDtos()
        {
            // Arrange
            var budgetResults = new List<BudgetResult>
            {
                new BudgetResult { Id = 1, BudgetId = 1, TotalProfit = 100 },
                new BudgetResult { Id = 2, BudgetId = 2, TotalProfit = 200 }
            };

            _budgetResultRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(budgetResults);

            // Act
            var result = await _budgetResultService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task Update_ValidRequest_UpdatesBudgetResult()
        {
            // Arrange
            var request = new UpdateBudgetResultRequest
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            _budgetResultRepositoryMock.Setup(x => x.Update(It.IsAny<BudgetResult>()))
                .ReturnsAsync(true);

            // Act
            await _budgetResultService.Update(request);

            // Assert
            _budgetResultRepositoryMock.Verify(x => x.Update(It.Is<BudgetResult>(br =>
                br.Id == request.Id &&
                br.BudgetId == request.BudgetId &&
                br.TotalProfit == request.TotalProfit)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingBudgetResult_ThrowsUpdateException()
        {
            // Arrange
            var request = new UpdateBudgetResultRequest
            {
                Id = _faker.Random.Int(1, 100),
                BudgetId = _faker.Random.Int(1, 100),
                TotalProfit = _faker.Random.Double(1, 1000)
            };

            _budgetResultRepositoryMock.Setup(x => x.Update(It.IsAny<BudgetResult>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetResultService.Invoking(x => x.Update(request))
                .Should().ThrowAsync<EntityUpdateException>()
                .WithMessage("BudgetResult wasn't updated.");
        }

        [Fact]
        public async Task Delete_ExistingBudgetResult_DeletesBudgetResult()
        {
            // Arrange
            var budgetResultId = _faker.Random.Int(1, 100);
            _budgetResultRepositoryMock.Setup(x => x.Delete(budgetResultId))
                .ReturnsAsync(true);

            // Act
            await _budgetResultService.Delete(budgetResultId);

            // Assert
            _budgetResultRepositoryMock.Verify(x => x.Delete(budgetResultId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingBudgetResult_ThrowsDeleteException()
        {
            // Arrange
            var budgetResultId = _faker.Random.Int(1, 100);
            _budgetResultRepositoryMock.Setup(x => x.Delete(budgetResultId))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetResultService.Invoking(x => x.Delete(budgetResultId))
                .Should().ThrowAsync<EntityDeleteException>()
                .WithMessage("Error when deleting BudgetResult.");
        }
    }
}