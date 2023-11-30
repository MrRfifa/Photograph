using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Likes
{
    public class LikesPerImageTests
    {

        private readonly ILikeRepository _likeRepository;

        public LikesPerImageTests()
        {
            _likeRepository = A.Fake<ILikeRepository>();
        }

        [Fact]
        public async Task LikesPerImage_ExistingImage_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();
            var expectedResult = fixture.Create<int>();

            A.CallTo(() => _likeRepository.NumberOfLikesPerImage(imageId))
                .Returns(expectedResult);

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikesPerImage(imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = expectedResult });
        }

        [Fact]
        public async Task LikesPerImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();

            A.CallTo(() => _likeRepository.NumberOfLikesPerImage(imageId))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new LikeController(_likeRepository);

            // Act
            var result = await controller.LikesPerImage(imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }

    }
}