using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class ChangePasswordTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public ChangePasswordTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public async Task ChangePassword_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidChangePasswordRequest = fixture.Build<ChangePasswordRequest>()
                .Without(cpr => cpr.ConfirmPassword)
                .Without(cpr => cpr.CurrentPassword)
                .Without(cpr => cpr.Password)
                .Create();
            var userId = fixture.Create<int>();

            var controller = new UserController(_userRepository, _tokenRepository);

            // Manually add model state error for demonstration purposes
            controller.ModelState.AddModelError(nameof(ChangePasswordRequest.Password), "Password is required");

            // Act
            var result = await controller.ChangePassword(userId, invalidChangePasswordRequest) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>();
            result.StatusCode.Should().Be(400);

            // Assuming the error message is included in the ModelState errors dictionary
            result.Value.Should().BeOfType<SerializableError>().Which.Should().ContainKey(nameof(ChangePasswordRequest.Password));
            result.Value.Should().BeOfType<SerializableError>().Which[nameof(ChangePasswordRequest.Password)].Should().BeOfType<string[]>().Which.Should().Contain("Password is required");
        }


        [Fact]
        public async Task ChangePassword_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidUserId = fixture.Create<int>();
            var validChangePasswordRequest = fixture.Create<ChangePasswordRequest>();

            A.CallTo(() => _userRepository.ChangePassword(invalidUserId, A<string>._, A<string>._, A<string>._))
                .Throws(new UserNotFoundException($"User with id {invalidUserId} not found"));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangePassword(invalidUserId, validChangePasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<ObjectResult>();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo($"User with id {invalidUserId} not found");
        }

        [Fact]
        public async Task ChangePassword_PasswordsDoNotMatch_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();

            // Creating ChangePasswordRequest with mismatched passwords
            var invalidChangePasswordRequest = fixture.Build<ChangePasswordRequest>()
                .With(cpr => cpr.ConfirmPassword, "DifferentPassword")
                .Create();

            A.CallTo(() => _userRepository.ChangePassword(userId, A<string>._, A<string>._, A<string>._))
                .Throws(new Exception("Incorrect passwords. Passwords do not match."));

            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangePassword(userId, invalidChangePasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be("Incorrect passwords. Passwords do not match.");
        }

        [Fact]
        public async Task ChangePassword_PasswordVerificationNotDone_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();

            // Creating ChangePasswordRequest with mismatched passwords
            var validChangePasswordRequest = fixture.Create<ChangePasswordRequest>();

            A.CallTo(() => _userRepository.ChangePassword(userId, A<string>._, A<string>._, A<string>._))
                .Throws(new Exception("Incorrect password. Password change request denied."));
            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangePassword(userId, validChangePasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be("Incorrect password. Password change request denied.");
        }

        [Fact]
        public async Task ChangePassword_PasswordChangeSuccess_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();

            // Creating ChangePasswordRequest with mismatched passwords
            var validChangePasswordRequest = fixture.Create<ChangePasswordRequest>();

            A.CallTo(() => _userRepository.ChangePassword(userId, A<string>._, A<string>._, A<string>._))
                .Returns(true);
            var controller = new UserController(_userRepository, _tokenRepository);

            // Act
            var result = await controller.ChangePassword(userId, validChangePasswordRequest) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Password changed successfully." });
        }

    }
}