using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class VerifyEmailChangeTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        public VerifyEmailChangeTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task VerifyEmailChange_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var token = fixture.Create<string>();
            A.CallTo(() => _userRepository.GetUserByEmailChangeToken(token))
                .Returns(null as User);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyEmailChange(token) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("User not found");
        }

        [Fact]
        public async Task VerifyEmailChange_NoValidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var userWithInvalidToken = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .With(uc => uc.EmailChangeTokenExpires, DateTime.Now.AddDays(-1))
                .Create();

            var token = fixture.Create<string>();

            A.CallTo(() => _userRepository.GetUserByEmailChangeToken(token))
                .Returns(userWithInvalidToken);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyEmailChange(token) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid token.");
        }

        [Fact]
        public async Task VerifyEmailChange_ValidToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var userWithValidToken = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .With(uc => uc.EmailChangeTokenExpires, DateTime.Now.AddDays(1))
                .Create();

            var token = fixture.Create<string>();

            A.CallTo(() => _userRepository.GetUserByEmailChangeToken(token))
                .Returns(userWithValidToken);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyEmailChange(token) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Email change successfully verified." });
        }



    }
}