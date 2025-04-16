using Application.Requests;
using Application.Services;
using FluentAssertions;
using Bogus;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.DependencyInjection;
using Application.Exceptions;
using Azure.Core;

namespace ApplicationIntegrationTests.Services
{
    [Collection("IntegrationTests")]
    public class UserServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly Faker _faker;
        private readonly TestingFixture _fixture;
        private readonly IUserService _userService;

        public UserServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            _faker = new Faker();
            var scope = fixture.ServiceProvider.CreateScope();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        }

        [Fact]
        public async Task Add_ShouldCreateUserInDatabase()
        {
            // Arrange
            var firstName = _faker.Person.FirstName;
            var lastName = _faker.Person.LastName;
            var email = _faker.Person.Email;         
            var request = new CreateUserRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            //Act
            await _userService.Add(request);

            //Assert
            var users = await _userService.GetAll();
            users.Should().Contain(q => q.FirstName == firstName && q.LastName == lastName && q.Email == email);
        }

        [Fact]
        public async Task GetById_ShouldReturnUserFromDatabase()
        {
            // Arrange
            var createdUser = await _fixture.CreateUser();

            // Act
            var result = await _userService.GetById(createdUser.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdUser.Id);
            result.FirstName.Should().Be(createdUser.FirstName);
            result.LastName.Should().Be(createdUser.LastName);
            result.Email.Should().Be(createdUser.Email);
        }

        [Fact]
        public async Task GetById_ForNonExistingUserInDatabase_ShouldThrowNotFoundApplicationException()
        {
            // Arrange
            var createdUser = await _fixture.CreateUser();

            // Act + Assert
            await _userService.Invoking(s => s.GetById(createdUser.Id + 35))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("User not found");
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsersFromDatabase()
        {
            // Arrange
            await _fixture.CreateUser();
            await _fixture.CreateUser();

            // Act
            var users = await _userService.GetAll();

            // Assert
            users.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Update_ShouldModifyUserInDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();
            var newFirstName = "NewUserFirstName";
            var newLastName = "NewUserLastName";
            var newEmail = "NewUserEmail@gmail.com";
            var request = new UpdateUserRequest
            {
                Id = user.Id,
                FirstName = newFirstName,
                LastName = newLastName,
                Email = newEmail
            };

            // Act
            await _userService.Update(request);
            var updatedUser = await _userService.GetById(user.Id);

            // Assert
            updatedUser.Should().NotBeNull();
            updatedUser.FirstName.Should().Be(request.FirstName);
            updatedUser.LastName.Should().Be(request.LastName);
            updatedUser.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Updating_NonExistingUser_ShouldThrowEntityUpdateExceptionException()
        {
            // Arrange
            var user = await _fixture.CreateUser();
            var newFirstName = "NewUserFirstName";
            var newLastName = "NewUserLastName";
            var newEmail = "NewUserEmail@gmail.com";
            var request = new UpdateUserRequest
            {
                Id = user.Id + 35,
                FirstName = newFirstName,
                LastName = newLastName,
                Email = newEmail
            };

            // Act + Assert
            await _userService.Invoking(s => s.Update(request))
            .Should()
            .ThrowAsync<EntityUpdateException>()
            .WithMessage("User wasn't updated.");
        }

        [Fact]
        public async Task Delete_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();

            // Act
            await _userService.Delete(user.Id);
            
            // Assert
            await _userService.Invoking(s => s.GetById(user.Id))
            .Should()
            .ThrowAsync<NotFoundApplicationException>()
            .WithMessage("User not found");
        }

        [Fact]
        public async Task Deleting_NonExistingUserFromDatabase_ShouldThrowEntityDeleteException()
        {
            // Arrange
            var user = await _fixture.CreateUser();

            // Act + Assert
            await _userService.Invoking(s => s.Delete(user.Id + 35))
            .Should()
            .ThrowAsync<EntityDeleteException>()
            .WithMessage("Error when deleting User.");
        }
    }
}
