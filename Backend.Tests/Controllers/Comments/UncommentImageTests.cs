using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Comments
{
    public class UncommentImageTests
    {

        private readonly ICommentRepository _commentRepository;
        public UncommentImageTests()
        {
            _commentRepository = A.Fake<ICommentRepository>();
        }

        [Fact]
        public async Task UncommentImage_Success_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var userCommentId = fixture.Create<int>();

            A.CallTo(() => _commentRepository.DeleteCommentImage(userId, imageId, userCommentId))
                .Returns(true);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UncommentImage(userCommentId, userId, imageId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image uncommented successfully." });
        }

        [Fact]
        public async Task UncommentImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var userCommentId = fixture.Create<int>();

            A.CallTo(() => _commentRepository.DeleteCommentImage(userId, imageId, userCommentId))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UncommentImage(userCommentId, userId, imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }

        [Fact]
        public async Task UncommentImage_UserHasNotCommented_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var userCommentId = fixture.Create<int>();

            A.CallTo(() => _commentRepository.DeleteCommentImage(userId, imageId, userCommentId))
                .Returns(false);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UncommentImage(userCommentId, userId, imageId) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("User hasn't commented the image");
        }


    }
}