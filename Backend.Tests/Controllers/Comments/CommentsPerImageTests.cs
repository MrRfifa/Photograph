using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Comments
{
    public class CommentsPerImageTests
    {
        private readonly ICommentRepository _commentRepository;
        public CommentsPerImageTests()
        {
            _commentRepository = A.Fake<ICommentRepository>();
        }

        [Fact]
        public async Task CommentsPerImage_ExistingImage_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();
            var expectedComments = new List<Comment> { /* create some comments here */ };

            A.CallTo(() => _commentRepository.GetCommentPerImage(imageId))
                .Returns(expectedComments);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.CommentsPerImage(imageId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = expectedComments });
        }


        [Fact]
        public async Task CommentsPerImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();

            A.CallTo(() => _commentRepository.GetCommentPerImage(imageId))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.CommentsPerImage(imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }

    }
}