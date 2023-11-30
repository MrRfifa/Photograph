using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Likes
{
    public class UnlikeImageTests
    {
        private readonly ILikeRepository _likeRepository;

        public UnlikeImageTests()
        {
            _likeRepository = A.Fake<ILikeRepository>();
        }

        [Fact]
        public async Task UnlikeImage_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.UnlikeImage(validUserId, validImageId))
                .Returns(true);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.UnlikeImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image unliked successfully." });
        }

        [Fact]
        public async Task UnlikeImage_UserAlreadyUnlikedImage_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.UnlikeImage(validUserId, validImageId))
                .Returns(false);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.UnlikeImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("User hasn't liked the image");
        }
    }
}