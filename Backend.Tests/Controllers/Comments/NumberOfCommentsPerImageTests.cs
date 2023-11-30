using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Comments
{
    public class NumberOfCommentsPerImageTests
    {
        private readonly ICommentRepository _commentRepository;
        public NumberOfCommentsPerImageTests()
        {
            _commentRepository = A.Fake<ICommentRepository>();
        }

        [Fact]
        public async Task NumberOfCommentsPerImage_ExistingImage_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();
            var expectedResult = fixture.Create<int>();

            A.CallTo(() => _commentRepository.NumberOfCommentsPerImage(imageId))
                .Returns(expectedResult);

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.NumberOfCommentsPerImage(imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = expectedResult });
        }

        [Fact]
        public async Task NumberOfCommentsPerImage_NonExistingImage_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var imageId = fixture.Create<int>();

            A.CallTo(() => _commentRepository.NumberOfCommentsPerImage(imageId))
                .Throws(new Exception($"Image with ID {imageId} not found"));

            var controller = new CommentController(_commentRepository);

            // Act
            var result = await controller.NumberOfCommentsPerImage(imageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().Be($"An error occurred while processing the request: Image with ID {imageId} not found");
        }

    }
}