using Api.Controllers;
using Application.Dtos;
using Application.Requests;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Bogus;
using Application.Exceptions;

namespace ApiUnitTests.Controllers
{
    public class BudgetControllerTests
    {
        private readonly Mock<IBudgetService> _budgetServiceMock;
        private readonly BudgetController _controller;
        private readonly Faker _faker;

        public BudgetControllerTests()
        {
            _budgetServiceMock = new Mock<IBudgetService>();
            _controller = new BudgetController(_budgetServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task GetById_ExistingBudget_ReturnsOkWithBudget()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            var budgetDto = new BudgetDto { Id = budgetId };
            _budgetServiceMock.Setup(x => x.GetById(budgetId)).ReturnsAsync(budgetDto);

            // Act
            var result = await _controller.GetById(budgetId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(budgetDto);
        }

        [Fact]
        public async Task GetById_NonExistingBudget_ReturnsNotFound()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);
            _budgetServiceMock.Setup(x => x.GetById(budgetId))
                .ThrowsAsync(new NotFoundApplicationException("Budget not found"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundApplicationException>(() =>
                _controller.GetById(budgetId));

            exception.Message.Should().Be("Budget not found");
            _budgetServiceMock.Verify(x => x.GetById(budgetId), Times.Once);
        }

        [Fact]
        public async Task GetAll_WithBudgets_ReturnsOkWithBudgets()
        {
            // Arrange
            var budgets = new List<BudgetDto> { new BudgetDto { Id = 1 }, new BudgetDto { Id = 2 } };
            _budgetServiceMock.Setup(x => x.GetAll()).ReturnsAsync(budgets);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(budgets);
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateBudgetRequest();
            var budgetId = _faker.Random.Int(1, 100);
            _budgetServiceMock.Setup(x => x.Add(request)).ReturnsAsync(budgetId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues["id"].Should().Be(budgetId);
            createdResult.Value.Should().BeEquivalentTo(new { Id = budgetId });
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateBudgetRequest();

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _budgetServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingBudget_ReturnsNoContent()
        {
            // Arrange
            var budgetId = _faker.Random.Int(1, 100);

            // Act
            var result = await _controller.Delete(budgetId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _budgetServiceMock.Verify(x => x.Delete(budgetId), Times.Once);
        }
    }
}
