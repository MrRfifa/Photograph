using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class ChangeNamesTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        public ChangeNamesTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task ChangeNames_UserNonExists_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeNamesRequest = fixture.Create<ChangeNamesRequest>();
            var userId = fixture.Create<int>();

            A.CallTo(() => _userRepository.ChangeNames(userId, A<string>._, A<string>._, A<string>._))
                .Throws(new UserNotFoundException($"User with ID {userId} not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangeNames(userId, validChangeNamesRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404); // Change the status code to indicate "Not Found"
            result.Value.Should().Be($"User with ID {userId} not found");
        }

        [Fact]
        public async Task ChangeNames_WrongPassword_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeNamesRequest = fixture.Create<ChangeNamesRequest>();
            var userId = fixture.Create<int>();

            A.CallTo(() => _userRepository.ChangeNames(userId, A<string>._, A<string>._, A<string>._))
                .Throws(new Exception("Incorrect password. Names change request denied."));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangeNames(userId, validChangeNamesRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500); // Change the status code to indicate "Not Found"
            result.Value.Should().BeEquivalentTo("Incorrect password. Names change request denied.");
        }

        [Fact]
        public async Task ChangeNames_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validChangeNamesRequest = fixture.Create<ChangeNamesRequest>();
            var userId = fixture.Create<int>();

            A.CallTo(() => _userRepository.ChangeNames(userId, A<string>._, A<string>._, A<string>._))
                .Returns(true);

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangeNames(userId, validChangeNamesRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200); // Change the status code to indicate "Not Found"
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Names changed successfully." });
        }


    }
}