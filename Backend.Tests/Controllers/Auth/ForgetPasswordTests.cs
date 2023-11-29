using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel; // Add this namespace for EnumRelay
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests.Controllers.Auth
{
    public class ForgetPasswordTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly ITokenRepository _tokenRepository;
        public ForgetPasswordTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _authRepository = A.Fake<IAuthRepository>();
            _tokenRepository = A.Fake<ITokenRepository>();
        }

        [Fact]
        public async Task ForgetPassword_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(null as User);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ForgotPassword("userNotFound@email.found") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("User not found.");
        }

        [Fact]
        public async Task ForgetPassword_UserWithNullPasswordToken_ReturnsBadRequestResult()
        {
            // Arrange
            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(A.Fake<User>());
            A.CallTo(() => _tokenRepository.GenerateUniqueToken())
                .Returns((string)null);

            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ForgotPassword("userNotFound@email.found") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Something went wrong");
        }


        [Fact]
        public async Task ForgetPassword_ValidUserEmailAndToken_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var validUser = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();
            A.CallTo(() => _userRepository.GetUserByEmail(A<string>._))
                .Returns(validUser);
            A.CallTo(() => _tokenRepository.GenerateUniqueToken())
                .Returns("fakeToken");
            A.CallTo(() => _authRepository.ForgetPassword("validUser@valid.user")).Returns(true);


            var controller = new AuthController(_userRepository, _authRepository, _tokenRepository);

            // Act
            var result = await controller.ForgotPassword("validUser@valid.user") as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "A confirmation mail was sent to the provided mail" });
        }



    }
}