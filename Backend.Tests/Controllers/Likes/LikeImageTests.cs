using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Likes
{
    public class LikeImageTests
    {
        private readonly ILikeRepository _likeRepository;

        public LikeImageTests()
        {
            _likeRepository = A.Fake<ILikeRepository>();
        }


        [Fact]
        public async Task LikeImage_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.LikeImage(validUserId, validImageId))
                .Returns(true);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikeImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image liked successfully." });
        }

        [Fact]
        public async Task LikeImage_UserAlreadyLikedImage_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();

            var validUserId = fixture.Create<int>();
            var validImageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.LikeImage(validUserId, validImageId))
                .Returns(false);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikeImage(validUserId, validImageId) as ObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("User has already liked the image");
        }

    }
}