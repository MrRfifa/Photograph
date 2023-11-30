using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Comments
{
    public class UpdateCommentImageTests
    {
        private readonly ICommentRepository _commentRepository;
        public UpdateCommentImageTests()
        {
            _commentRepository = A.Fake<ICommentRepository>();
        }

        [Fact]
        public async Task UpdateCommentImage_Success_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var changeCommentRequest = fixture.Create<ChangeCommentRequest>();

            A.CallTo(() => _commentRepository.UpdateCommentImage(userId, imageId, changeCommentRequest.UserCommentId, changeCommentRequest.NewComment))
                .Returns(true);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UpdateCommentImage(changeCommentRequest, userId, imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Comment updated successfully." });
        }
        [Fact]
        public async Task UpdateCommentImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var changeCommentRequest = fixture.Create<ChangeCommentRequest>();

            A.CallTo(() => _commentRepository.UpdateCommentImage(userId, imageId, changeCommentRequest.UserCommentId, changeCommentRequest.NewComment))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UpdateCommentImage(changeCommentRequest, userId, imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }

        [Fact]
        public async Task UpdateCommentImage_UserHasNotCommented_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var changeCommentRequest = fixture.Create<ChangeCommentRequest>();

            A.CallTo(() => _commentRepository.UpdateCommentImage(userId, imageId, changeCommentRequest.UserCommentId, changeCommentRequest.NewComment))
                .Returns(false);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.UpdateCommentImage(changeCommentRequest, userId, imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("User hasn't commented the image");
        }

    }
}