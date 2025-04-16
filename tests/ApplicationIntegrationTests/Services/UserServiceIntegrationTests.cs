using Application.Requests;
using Application.Services;
using FluentAssertions;
using Bogus;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationIntegrationTests.Services
{
    public class UserServiceIntegrationTests : IClassFixture<TestingFixture>
    {
        private readonly Faker _faker;
        private readonly TestingFixture _fixture;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public UserServiceIntegrationTests(TestingFixture fixture)
        {
            _fixture = fixture;
            _faker = new Faker();
            var scope = fixture.ServiceProvider.CreateScope();
            _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        }

        [Fact]
        public async Task Add_ShouldCreateUserInDatabase()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email
            };

            // Act
            var userId = await _userService.Add(request);
            var user = await _userRepository.ReadById(userId);

            // Assert
            user.Should().NotBeNull();
            user!.FirstName.Should().Be(request.FirstName);
            user.LastName.Should().Be(request.LastName);
            user.Email.Should().Be(request.Email);
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
            var request = new UpdateUserRequest
            {
                Id = user.Id,
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email
            };

            // Act
            await _userService.Update(request);
            var updatedUser = await _userRepository.ReadById(user.Id);

            // Assert
            updatedUser.Should().NotBeNull();
            updatedUser!.FirstName.Should().Be(request.FirstName);
            updatedUser.LastName.Should().Be(request.LastName);
            updatedUser.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Delete_ShouldRemoveUserFromDatabase()
        {
            // Arrange
            var user = await _fixture.CreateUser();

            // Act
            await _userService.Delete(user.Id);
            var deletedUser = await _userRepository.ReadById(user.Id);

            // Assert
            deletedUser.Should().BeNull();
        }
    }
}
