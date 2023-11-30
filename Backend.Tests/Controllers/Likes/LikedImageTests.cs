using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Likes
{
    public class LikedImageTests
    {

        private readonly ILikeRepository _likeRepository;

        public LikedImageTests()
        {
            _likeRepository = A.Fake<ILikeRepository>();
        }

        [Fact]
        public async Task LikedImage_UserHasNotLikedImage_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.LikedImage(validUserId, validImageId))
                .Returns(true);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikedImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image already liked." });
        }

        [Fact]
        public async Task LikedImage_UserHasLikedImage_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.LikedImage(validUserId, validImageId))
                .Returns(false);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikedImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "failed", message = "Image not liked." });
        }

    }
}