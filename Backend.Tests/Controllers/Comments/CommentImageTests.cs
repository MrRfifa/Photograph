using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Comments
{
    public class CommentImageTests
    {
        private readonly ICommentRepository _commentRepository;
        public CommentImageTests()
        {
            _commentRepository = A.Fake<ICommentRepository>();
        }

        [Fact]
        public async Task CommentImage_Success_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var commentText = fixture.Create<string>();

            A.CallTo(() => _commentRepository.CommentImage(userId, imageId, commentText))
                .Returns(true);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.CommentImage(userId, imageId, commentText) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image commented successfully." });
        }

        [Fact]
        public async Task CommentImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var userId = fixture.Create<int>();
            var imageId = fixture.Create<int>();
            var commentText = fixture.Create<string>();

            A.CallTo(() => _commentRepository.CommentImage(userId, imageId, commentText))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.CommentImage(userId, imageId, commentText) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }


    }
}