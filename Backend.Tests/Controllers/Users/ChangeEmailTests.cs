using Backend.Dtos.requests;

namespace Backend.Tests.Controllers.Users
{
    public class ChangeEmailTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        public ChangeEmailTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }


        [Fact]
        public async Task ChangeEmail_UserNonExists_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeEmailRequest = fixture.Create<ChangeEmailRequest>();
            var userId = fixture.Create<int>();

            A.CallTo(() => _userRepository.ChangeEmail(userId, A<string>._, A<string>._))
                .Throws(new UserNotFoundException($"User with ID {userId} not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangeEmail(userId, validChangeEmailRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"User with ID {userId} not found");
        }

        [Fact]
        public async Task ChangeEmail_WrongPassword_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeEmailRequest = fixture.Create<ChangeEmailRequest>();
            var userId = fixture.Create<int>();
            A.CallTo(() => _userRepository.ChangeEmail(userId, A<string>._, A<string>._))
                .Throws(new Exception("Incorrect password. Email change request denied."));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangeEmail(userId, validChangeEmailRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be("Incorrect password. Email change request denied.");
        }

        [Fact]
        public async Task ChangeEmail_NonUniqueToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeEmailRequest = fixture.Create<ChangeEmailRequest>();
            var userId = fixture.Create<int>();
            A.CallTo(() => _userRepository.ChangeEmail(userId, A<string>._, A<string>._))
                .Throws(new Exception("Unable to generate a unique token after multiple attempts."));

            var controller = new UserController(_userRepository, _tokenRepository);
            // Act
            var result = await controller.ChangeEmail(userId, validChangeEmailRequest) as ObjectResult;
            // Assert
            result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be("Unable to generate a unique token after multiple attempts.");
        }


        [Fact]
        public async Task ChangeEmail_NoTokenInDatabase_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeEmailRequest = fixture.Create<ChangeEmailRequest>();
            var userId = fixture.Create<int>();
            A.CallTo(() => _userRepository.ChangeEmail(userId, A<string>._, A<string>._))
                .Returns(false);

            var controller = new UserController(_userRepository, _tokenRepository);
            // Act
            var result = await controller.ChangeEmail(userId, validChangeEmailRequest) as BadRequestObjectResult;
            // Assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Error when handling token");
        }

        [Fact]
        public async Task ChangeEmail_ValidTokenInDatabase_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var validChangeEmailRequest = fixture.Create<ChangeEmailRequest>();
            var userId = fixture.Create<int>();
            var userVerified = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            A.CallTo(() => _userRepository.GetUserById(userId)).Returns(userVerified);

            A.CallTo(() => _userRepository.ChangeEmail(userId, A<string>._, A<string>._))
                .Returns(true);

            A.CallTo(() => _tokenRepository.SendEmail(A<SendEmailRequest>._))
                        .DoesNothing();

            var controller = new UserController(_userRepository, _tokenRepository);
            // Act
            var result = await controller.ChangeEmail(userId, validChangeEmailRequest) as ObjectResult;
            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new
            {
                message = "A verification mail is sent to the new address, the token will expire in 15 minutes",
                status = "success"
            });
        }
    }
}