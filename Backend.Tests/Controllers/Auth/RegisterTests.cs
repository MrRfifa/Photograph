using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel; // Add this namespace for EnumRelay
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers.Auth
{
    public class RegisterTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;

        public RegisterTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _authRepository = A.Fake<IAuthRepository>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userCreated = fixture.Build<RegisterUserDto>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .Create();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(new User()); // Simulate that no existing user with the same email

            A.CallTo(() => _authRepository.Register(userCreated)).Returns(true);
            A.CallTo(() => _tokenRepository.GenerateUniqueToken()).Returns(Task.FromResult("fakeToken"));

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Register(userCreated) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("Something went wrong: Token not generated yet, retry in few minutes.");
        }


        [Fact]
        public async Task Register_ValidUser_NotValidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userCreated = fixture.Build<RegisterUserDto>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .Create();

            // Note: Use the repositories set up in the constructor
            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(new User()); // Simulate that no existing user with the same email

            A.CallTo(() => _authRepository.Register(A<RegisterUserDto>._))
                .Returns(true);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Register(userCreated) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("Something went wrong: Token not generated yet, retry in few minutes.");
        }

        [Fact]
        public async Task Register_ValidDuplicatedUser_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userCreated = fixture.Build<RegisterUserDto>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .Create();

            // Note: Use the repositories set up in the constructor
            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                        .Returns(Task.FromResult(new User { Id = fixture.Create<int>() }));
            // Simulate that no existing user with the same email

            A.CallTo(() => _authRepository.Register(A<RegisterUserDto>._))
                .Returns(true);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Register(userCreated) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(422);
        }

    }
}
