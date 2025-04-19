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
    public class BudgetRecordControllerTests
    {
        private readonly Mock<IBudgetRecordService> _budgetRecordServiceMock;
        private readonly BudgetRecordController _controller;
        private readonly Faker _faker;

        public BudgetRecordControllerTests()
        {
            _budgetRecordServiceMock = new Mock<IBudgetRecordService>();
            _controller = new BudgetRecordController(_budgetRecordServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task GetById_ExistingRecord_ReturnsOkWithRecord()
        {
            // Arrange
            var recordId = _faker.Random.Int(1, 100);
            var recordDto = new BudgetRecordDto { Id = recordId };
            _budgetRecordServiceMock.Setup(x => x.GetById(recordId)).ReturnsAsync(recordDto);

            // Act
            var result = await _controller.GetById(recordId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(recordDto);
        }

        [Fact]
        public async Task GetById_NonExistingRecord_ReturnsNotFound()
        {
            // Arrange
            var recordId = _faker.Random.Int(1, 100);
            _budgetRecordServiceMock.Setup(x => x.GetById(recordId))
                .ThrowsAsync(new NotFoundApplicationException("BudgetRecord not found"));

            // Act & Assert
            await FluentActions
               .Invoking(() => _controller.GetById(recordId))
               .Should()
               .ThrowAsync<NotFoundApplicationException>()
               .WithMessage("BudgetRecord not found");
        }

        [Fact]
        public async Task GetAll_WithRecords_ReturnsOkWithRecords()
        {
            // Arrange
            var records = new List<BudgetRecordDto> { new BudgetRecordDto { Id = 1 }, new BudgetRecordDto { Id = 2 } };
            _budgetRecordServiceMock.Setup(x => x.GetAll()).ReturnsAsync(records);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(records);
        }

        [Fact]
        public async Task Add_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var request = new CreateBudgetRecordRequest();
            var recordId = _faker.Random.Int(1, 100);
            _budgetRecordServiceMock.Setup(x => x.Add(request)).ReturnsAsync(recordId);

            // Act
            var result = await _controller.Add(request);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues["id"].Should().Be(recordId);
            createdResult.Value.Should().BeEquivalentTo(new { Id = recordId });
        }

        [Fact]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new UpdateBudgetRecordRequest();

            // Act
            var result = await _controller.Update(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _budgetRecordServiceMock.Verify(x => x.Update(request), Times.Once);
        }

        [Fact]
        public async Task Delete_ExistingRecord_ReturnsNoContent()
        {
            // Arrange
            var recordId = _faker.Random.Int(1, 100);

            // Act
            var result = await _controller.Delete(recordId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _budgetRecordServiceMock.Verify(x => x.Delete(recordId), Times.Once);
        }
    }
}
