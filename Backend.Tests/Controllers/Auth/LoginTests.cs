
namespace Backend.Tests.Controllers.Auth
{
    public class LoginTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        public LoginTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _authRepository = A.Fake<IAuthRepository>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }
        [Fact]
        public async Task Login_EmptyUser_ReturnsBadRequestResult()
        {
            //arrange

            // var fixture = new Fixture();
            var invalidUserDto = (LoginUserDto)null;
            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);
            //act
            var result = await controller.Login(invalidUserDto) as ObjectResult;

            //assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid request. Please provide valid login credentials.");
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidUserDto = fixture.Create<LoginUserDto>();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Throws(new UserNotFoundException("User with email: notfound@example.com not found"));
            A.CallTo(() => _authRepository.Login(invalidUserDto))
                .Throws(new UserNotFoundException("User with email: notfound@example.com not found"));


            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Login(invalidUserDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("User not found. Please check your credentials.");
        }


        [Fact]
        public async Task Login_UserNotVerified_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invalidUserDto = fixture.Create<LoginUserDto>();
            var userWithNoVerification = fixture.Build<User>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .Without(uc => uc.VerifiedAt)
                            .Create();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(userWithNoVerification);
            A.CallTo(() => _authRepository.Login(invalidUserDto))
                .Throws(new Exception("Not verified."));
            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Login(invalidUserDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Not verified.");
        }


        [Fact]
        public async Task Login_User_WrongPassword_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                        .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invalidUserDto = fixture.Create<LoginUserDto>();
            var userVerified = fixture.Build<User>()
                            .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                            .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                            .Create();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(userVerified);
            A.CallTo(() => _authRepository.Login(invalidUserDto))
                .Throws(new Exception("Wrong password."));
            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Login(invalidUserDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Wrong password.");
        }

        [Fact]
        public async Task Login_UserWithInvalidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var invalidUserDto = fixture.Create<LoginUserDto>();
            var userVerified = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(userVerified);

            A.CallTo(() => _authRepository.Login(invalidUserDto))
                .Returns((string)null);

            A.CallTo(() => _tokenRepository.CreateToken(userVerified))
               .Returns((string)null);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Login(invalidUserDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Authentication failed. Please check your credentials.");
        }

        [Fact]
        public async Task Login_User_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var validUserDto = fixture.Create<LoginUserDto>();
            var userVerified = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(userVerified);

            A.CallTo(() => _authRepository.Login(validUserDto))
                .Returns("fakeToken");

            A.CallTo(() => _tokenRepository.CreateToken(userVerified))
               .Returns("fakeToken");

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.Login(validUserDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { message = "success", Token = "fakeToken" });
        }

    }
}