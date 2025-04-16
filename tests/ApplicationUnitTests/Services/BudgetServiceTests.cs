using Application.Exceptions;
using Application.Requests;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.BudgetRepository;
using Moq;
using Bogus;
using Application.Mappings;

namespace ApplicationUnitTests.Services
{
    public class BudgetServiceTests
    {
        private readonly Mock<IBudgetRepository> _budgetRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IBudgetService _budgetService;
        private readonly Faker _faker;

        public BudgetServiceTests()
        {
            _budgetRepositoryMock = new Mock<IBudgetRepository>();

            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            
            _budgetService = new BudgetService(
                _budgetRepositoryMock.Object,
                _mapper);

            _faker = new Faker();
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsBudgetId()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            var creatorId = _faker.Random.Int(1, 100);
            var request = new CreateBudgetRequest
            {
                Name = _faker.Commerce.ProductName(),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Lorem.Sentence(),
                CreatorId = creatorId
            };

            _budgetRepositoryMock.Setup(x => x.Create(It.IsAny<Budget>()))
                .ReturnsAsync(budgetId);

            // Act
            var result = await _budgetService.Add(request);

            // Assert
            result.Should().Be(budgetId);
            _budgetRepositoryMock.Verify(x => x.Create(It.Is<Budget>(b =>
                b.Name == request.Name &&
                b.StartDate == request.StartDate &&
                b.FinishDate == request.FinishDate &&
                b.Description == request.Description &&
                b.CreatorId == request.CreatorId)), Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingBudget_ReturnsBudgetDto()
        {
            // Arrange
            var budget = new Budget
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Lorem.Sentence(),
                CreatorId = _faker.Random.Int(1, 100)
            };

            _budgetRepositoryMock.Setup(x => x.ReadById(budget.Id))
                .ReturnsAsync(budget);

            // Act
            var result = await _budgetService.GetById(budget.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(budget.Id);
            result.Name.Should().Be(budget.Name);
            result.StartDate.Should().Be(budget.StartDate);
            result.FinishDate.Should().Be(budget.FinishDate);
            result.Description.Should().Be(budget.Description);
            result.CreatorId.Should().Be(budget.CreatorId);
        }

        [Fact]
        public async Task GetById_NonExistingBudget_ThrowsNotFoundException()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            _budgetRepositoryMock.Setup(x => x.ReadById(budgetId))
                .ReturnsAsync((Budget)null);

            // Act & Assert
            await _budgetService.Invoking(x => x.GetById(budgetId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage("Budget not found.");
        }

        [Fact]
        public async Task GetAll_WithBudgets_ReturnsBudgetDtos()
        {
            // Arrange
            var budgets = new List<Budget>
            {
                new Budget { Id = 1, Name = "Budget 1" },
                new Budget { Id = 2, Name = "Budget 2" }
            };

            _budgetRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(budgets);

            // Act
            var result = await _budgetService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task Update_ValidRequest_UpdatesBudget()
        {
            // Arrange
            var request = new UpdateBudgetRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Lorem.Sentence(),
                CreatorId = _faker.Random.Int(1, 100)
            };

            _budgetRepositoryMock.Setup(x => x.Update(It.IsAny<Budget>()))
                .ReturnsAsync(true);

            // Act
            await _budgetService.Update(request);

            // Assert
            _budgetRepositoryMock.Verify(x => x.Update(It.Is<Budget>(b =>
                b.Id == request.Id &&
                b.Name == request.Name &&
                b.StartDate == request.StartDate &&
                b.FinishDate == request.FinishDate &&
                b.Description == request.Description &&
                b.CreatorId == request.CreatorId)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingBudget_ThrowsUpdateException()
        {
            // Arrange
            var request = new UpdateBudgetRequest
            {
                Id = _faker.Random.Int(1, 100),
                Name = _faker.Commerce.ProductName(),
                StartDate = _faker.Date.Future(),
                FinishDate = _faker.Date.Future(2),
                Description = _faker.Lorem.Sentence(),
                CreatorId = _faker.Random.Int(1, 100)
            };

            _budgetRepositoryMock.Setup(x => x.Update(It.IsAny<Budget>()))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetService.Invoking(x => x.Update(request))
                .Should().ThrowAsync<EntityUpdateException>()
                .WithMessage("Budget wasn't updated.");
        }

        [Fact]
        public async Task Delete_ExistingBudget_DeletesBudget()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            _budgetRepositoryMock.Setup(x => x.Delete(budgetId))
                .ReturnsAsync(true);

            // Act
            await _budgetService.Delete(budgetId);

            // Assert
            _budgetRepositoryMock.Verify(x => x.Delete(budgetId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingBudget_ThrowsDeleteException()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            _budgetRepositoryMock.Setup(x => x.Delete(budgetId))
                .ReturnsAsync(false);

            // Act & Assert
            await _budgetService.Invoking(x => x.Delete(budgetId))
                .Should().ThrowAsync<EntityDeleteException>()
                .WithMessage("Error when deleting Budget.");
        }
    }
}
