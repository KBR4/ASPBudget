using Application.Exceptions;
using Application.Mappings;
using Application.Requests;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class UserServiceTests
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private Faker _faker;
        public UserServiceTests()
        {
            _faker = new Faker();

            _userRepositoryMock = new Mock<IUserRepository>();

            _userRepository = _userRepositoryMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();
            var loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepository, _mapper, loggerMock.Object);
        }

        [Fact]
        public async Task GetById_ExistingUser_ReturnsUserDto()
        {
            // Arrange
            var user = new User
            {
                Id = _faker.Random.Int(1, 100),
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email
            };

            _userRepositoryMock.Setup(x => x.ReadById(user.Id))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetById(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetById_NonExistingUser_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            _userRepositoryMock.Setup(x => x.ReadById(userId))
                .ReturnsAsync((User)null);

            // Act + Assert
            await _userService.Invoking(x => x.GetById(userId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage("User not found");
        }

        [Fact]
        public async Task GetAll_WithUsers_ReturnsUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, FirstName = _faker.Person.FirstName, LastName =_faker.Person.LastName, Email = _faker.Person.Email },
                new User { Id = 2, FirstName = _faker.Person.FirstName, LastName =_faker.Person.LastName, Email = _faker.Person.Email }
            };

            _userRepositoryMock.Setup(x => x.ReadAll())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task Update_ValidRequest_UpdatesUser()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Id = _faker.Random.Int(1, 100),
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await _userService.Update(request);

            // Assert
            _userRepositoryMock.Verify(x => x.Update(It.Is<User>(u =>
                u.Id == request.Id &&
                u.FirstName == request.FirstName &&
                u.LastName == request.LastName &&
                u.Email == request.Email)), Times.Once);
        }

        [Fact]
        public async Task Update_NonExistingUser_ThrowsUpdateException()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Id = _faker.Random.Int(1, 100),
                FirstName = _faker.Person.FirstName,
                LastName = _faker.Person.LastName,
                Email = _faker.Person.Email
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act + Assert
            await _userService.Invoking(x => x.Update(request))
                .Should().ThrowAsync<EntityUpdateException>()
                .WithMessage("User wasn't updated.");
        }

        [Fact]
        public async Task Delete_ExistingUser_DeletesUser()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            _userRepositoryMock.Setup(x => x.Delete(userId))
                .ReturnsAsync(true);

            // Act
            await _userService.Delete(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.Delete(userId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingUser_ThrowsDeleteException()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            _userRepositoryMock.Setup(x => x.Delete(userId))
                .ReturnsAsync(false);

            // Act + Assert
            await _userService.Invoking(x => x.Delete(userId))
                .Should().ThrowAsync<EntityDeleteException>()
                .WithMessage("Error when deleting User.");
        }
    }
}
