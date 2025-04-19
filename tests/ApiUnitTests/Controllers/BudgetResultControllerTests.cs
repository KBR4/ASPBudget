using Api.Controllers;
using Application.Dtos;
using Application.Requests;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Bogus;
using Application.Exceptions;
using Domain.Entities;

namespace ApiUnitTests.Controllers
{
    public class BudgetResultControllerTests
    {
        private readonly Mock<IBudgetResultService> _budgetResultServiceMock;
        private readonly BudgetResultController _controller;
        private readonly Faker _faker;

        public BudgetResultControllerTests()
        {
            _budgetResultServiceMock = new Mock<IBudgetResultService>();
            _controller = new BudgetResultController(_budgetResultServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task GetById_ExistingResult_ReturnsOkWithResult()
        {
            // Arrange
            var resultId = _faker.Random.Int(1, 100);
            var resultDto = new BudgetResultDto { Id = resultId };
            _budgetResultServiceMock.Setup(x => x.GetById(resultId)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.GetById(resultId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(resultDto);
        }

        [Fact]
        public async Task GetById_NonExistingResult_ReturnsNotFound()
        {
            // Arrange
            var resultId = _faker.Random.Int(1, 100);
            _budgetResultServiceMock.Setup(x => x.GetById(resultId))
                .ThrowsAsync(new NotFoundApplicationException("BudgetResult not found"));

            // Act & Assert
            await FluentActions
               .Invoking(() => _controller.GetById(resultId))
               .Should()
               .ThrowAsync<NotFoundApplicationException>()
               .WithMessage("BudgetResult not found");
        }

        [Fact]
        public async Task GetAll_WithResults_ReturnsOkWithResults()
        {
            // Arrange
            var results = new List<BudgetResultDto> { new BudgetResultDto { Id = 1 }, new BudgetResultDto { Id = 2 } };
            _budgetResultServiceMock.Setup(x => x.GetAll()).ReturnsAsync(results);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(results);
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateBudgetResultRequest();
            var resultId = _faker.Random.Int(1, 100);
            _budgetResultServiceMock.Setup(x => x.Add(request)).ReturnsAsync(resultId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues["id"].Should().Be(resultId);
            createdResult.Value.Should().BeEquivalentTo(new { Id = resultId });
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateBudgetResultRequest();

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _budgetResultServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingResult_ReturnsNoContent()
        {
            // Arrange
            var resultId = _faker.Random.Int(1, 100);

            // Act
            var result = await _controller.Delete(resultId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _budgetResultServiceMock.Verify(x => x.Delete(resultId), Times.Once);
        }
    }
}
