using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class VerifyAccountDeletionTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public VerifyAccountDeletionTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }


        [Fact]
        public async Task DeleteAccount_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var token = fixture.Create<string>();

            A.CallTo(() => _userRepository.DeleteAccountVerification(token))
                .Throws(new UserNotFoundException("User with id 9 not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyAccountDeletion(token) as ObjectResult;

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);

            result.Value.Should().BeEquivalentTo("User with id 9 not found");
        }

        [Fact]
        public async Task DeleteAccount_ExpiredToken_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var token = fixture.Create<string>();
            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .With(uc => uc.DeleteAccountTokenExpires, DateTime.Now.AddDays(-1))
                .Create();

            A.CallTo(() => _userRepository.DeleteAccountVerification(token))
                .Throws(new Exception("Token has expired"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyAccountDeletion(token) as ObjectResult;

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);

            result.Value.Should().BeEquivalentTo("Token has expired");
        }

        [Fact]
        public async Task VerifyAccountDeletion_ValidRequest_ReturnsNoContentResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var token = fixture.Create<string>();
            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            A.CallTo(() => _userRepository.GetUserByDeleteAccountToken(token))
                .Returns(user);

            A.CallTo(() => _userRepository.DeleteAccountVerification(token))
                .Returns(true);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.VerifyAccountDeletion(token) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
        }


    }
}