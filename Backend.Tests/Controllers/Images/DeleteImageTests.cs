using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Images
{
    public class DeleteImageTests
    {

        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        public DeleteImageTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }

        [Fact]
        public async Task DeleteImage_ImageNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidImageId = fixture.Create<int>();

            A.CallTo(() => _imageRepository.DeleteImage(invalidImageId))
                    .Throws(new UserNotFoundException($"Image with ID {invalidImageId} not found or not deleted."));

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.DeleteImage(invalidImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo($"An error occurred while processing the request: Image with ID {invalidImageId} not found or not deleted.");
        }

        [Fact]
        public async Task DeleteImage_ImageDeletionNotSaved_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validImageId = fixture.Create<int>();

            var imageToDelete = new Image { };

            A.CallTo(() => _imageRepository.GetImageById(validImageId))
                    .Returns(imageToDelete);
            A.CallTo(() => _imageRepository.DeleteImage(validImageId))
                    .Returns(false);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.DeleteImage(validImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo($"Image with ID {validImageId} not found or not deleted.");
        }

        [Fact]
        public async Task DeleteImage_ImageDeletionDone_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validImageId = fixture.Create<int>();

            var imageToDelete = new Image { };

            A.CallTo(() => _imageRepository.GetImageById(validImageId))
                    .Returns(imageToDelete);
            A.CallTo(() => _imageRepository.DeleteImage(validImageId))
                    .Returns(true);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.DeleteImage(validImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image deleted successfully." });
        }



    }
}