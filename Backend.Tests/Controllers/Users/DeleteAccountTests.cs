using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class DeleteAccountTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public DeleteAccountTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task DeleteAccount_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidUserId = fixture.Create<int>();
            var password = fixture.Create<string>();

            A.CallTo(() => _userRepository.DeleteAccount(invalidUserId, password))
                .Throws(new UserNotFoundException($"User with id {invalidUserId} not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.DeleteAccount(invalidUserId, password) as ObjectResult;

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);

            result.Value.Should().BeEquivalentTo($"User with id {invalidUserId} not found");
        }

        [Fact]
        public async Task DeleteAccount_WrongPassword_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validUserId = fixture.Create<int>();
            var password = fixture.Create<string>();

            A.CallTo(() => _userRepository.DeleteAccount(validUserId, password))
                .Throws(new Exception("Incorrect password. Account deletion denied."));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.DeleteAccount(validUserId, password) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Incorrect password. Account deletion denied.");
        }

        [Fact]
        public async Task DeleteAccount_InvalidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validUserId = fixture.Create<int>();
            var password = fixture.Create<string>();

            A.CallTo(() => _userRepository.DeleteAccount(validUserId, password))
                .Throws(new Exception("Unable to generate a unique token after multiple attempts."));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.DeleteAccount(validUserId, password) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Unable to generate a unique token after multiple attempts.");
        }

        [Fact]
        public async Task DeleteAccount_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var validUserId = fixture.Create<int>();
            var password = fixture.Create<string>();
            var token = fixture.Create<string>();
            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            A.CallTo(() => _userRepository.GetUserById(validUserId))
                .Returns(user);
            A.CallTo(() => _userRepository.DeleteAccount(validUserId, password))
                .Returns(true);
            A.CallTo(() => _tokenRepository.SendEmail(A<SendEmailRequest>._))
                .DoesNothing();
            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.DeleteAccount(validUserId, password) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "A verification mail is sent to the email address, the token will expire in 15 minutes" });
        }


    }
}