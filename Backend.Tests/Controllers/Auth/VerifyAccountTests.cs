
namespace Backend.Tests.Controllers.Auth
{
    public class VerifyAccountTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        public VerifyAccountTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _authRepository = A.Fake<IAuthRepository>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }

        [Fact]
        public async Task Verify_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            A.CallTo(() => _authRepository.GetUserByVerificationToken(A<string>._))
                .Returns(null as User);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Verify("fakeToken") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid token.");
        }

        [Fact]
        public async Task Verify_UserExists_ReturnsOkResult()
        {
            // Arrange
            //var fixture = new Fixture();
            A.CallTo(() => _authRepository.GetUserByVerificationToken(A<string>._))
                .Returns(A.Fake<User>());

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);
            // Act
            var result = await controller.Verify("fakeToken") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "User verified! :)" });
        }

    }
}