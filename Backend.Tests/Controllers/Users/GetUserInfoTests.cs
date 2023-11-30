using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Backend.Tests.Controllers.Users
{
    public class GetUserInfoTests
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        public GetUserInfoTests()
        {
            _tokenRepository = A.Fake<ITokenRepository>();
            _userRepository = A.Fake<IUserRepository>();
        }

        [Fact]
        public void GetUserInfo_AuthenticatedUser_ReturnsOkResultWithClaims()
        {
            // Arrange
            var controller = new UserController(_userRepository, _tokenRepository);
            var claimsIdentity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, "John"),
        new Claim(ClaimTypes.Email, "john@example.com"),
        // Add more claims as needed
    }, "test");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = controller.GetUserInfo() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public void GetUserInfo_UnauthenticatedUser_ReturnsOkResultWithNull()
        {
            // Arrange
            var controller = new UserController(_userRepository, _tokenRepository);
            var claimsPrincipal = new ClaimsPrincipal(); // Unauthenticated user
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = controller.GetUserInfo() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be("null");
        }

        // Add more test cases as needed

    }
}