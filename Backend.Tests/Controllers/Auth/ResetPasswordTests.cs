
namespace Backend.Tests.Controllers.Auth
{
    public class ResetPasswordTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;

        public ResetPasswordTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _authRepository = A.Fake<IAuthRepository>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }

        [Fact]
        public async Task ResetPassword_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var resetPasswordRequest = fixture.Create<ResetPasswordRequest>();

            var expectedErrorMessage = "User not found.";

            A.CallTo(() => _authRepository.GetUserByResetToken(A<string>._))
                .Returns(null as User);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ResetPassword(resetPasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo(expectedErrorMessage);
        }

        [Fact]
        public async Task ResetPassword_ExpiredToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var resetPasswordRequest = fixture.Create<ResetPasswordRequest>();

            var expiredUser = fixture.Build<User>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .With(uc => uc.ResetTokenExpires, DateTime.Now.AddHours(-1))
                            .Create();

            A.CallTo(() => _authRepository.GetUserByResetToken(resetPasswordRequest.Token))
                    .Returns(expiredUser);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ResetPassword(resetPasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid Token.");
        }

        [Fact]
        public async Task ResetPassword_ValidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var resetPasswordRequest = fixture.Create<ResetPasswordRequest>();

            var user = fixture.Build<User>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .With(u => u.ResetTokenExpires, DateTime.Now.AddDays(1))
                            .Create();

            A.CallTo(() => _authRepository.GetUserByResetToken(resetPasswordRequest.Token))
                    .Returns(user);
            A.CallTo(() => _authRepository.Save()).Returns(true);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ResetPassword(resetPasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Password successfully reset." });
        }
    }
}