using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class GetUserSpecificInfoTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public GetUserSpecificInfoTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task GetUserSpecificInfo_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidUserId = fixture.Create<int>();

            A.CallTo(() => _userRepository.GetUserById(invalidUserId))
                .Throws(new UserNotFoundException("User not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.GetUserSpecificInfo(invalidUserId) as ObjectResult;

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);

            result.Value.Should().BeEquivalentTo("User not found");
        }

        [Fact]
        public async Task GetUserSpecificInfo_ValidUser_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();
            var validUserId = fixture.Create<int>();

            A.CallTo(() => _userRepository.GetUserById(validUserId))
                .Returns(user);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.GetUserSpecificInfo(validUserId) as ObjectResult;

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }


    }
}