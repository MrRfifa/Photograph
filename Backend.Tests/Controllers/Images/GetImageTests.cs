using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controllers.Images
{
    public class GetImageTests
    {

        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public GetImageTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }


        [Fact]
        public async Task GetImage_ImageNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            int invalidImageId = fixture.Create<int>();

            A.CallTo(() => _imageRepository.GetImageById(invalidImageId))
                    .Throws(new Exception($"image with id {invalidImageId} not found"));

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetImage(invalidImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo($"An error occurred while processing the request: image with id {invalidImageId} not found");
        }

        [Fact]
        public async Task GetImage_NullImageFile_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();

            int invalidImageId = fixture.Create<int>();
            var invalidImage = new Image
            {
                ImageFile = null
            };

            A.CallTo(() => _imageRepository.GetImageById(invalidImageId))
                .Returns(invalidImage);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetImage(invalidImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo("Image not found or content is missing.");
        }

        [Fact]
        public async Task GetImage_NullImageFileContent_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();

            int invalidImageId = fixture.Create<int>();

            var invalidImageFile = new ImageFile { FileContentBase64 = null };

            var invalidImage = new Image
            {
                ImageFile = invalidImageFile
            };

            A.CallTo(() => _imageRepository.GetImageById(invalidImageId))
                .Returns(invalidImage);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetImage(invalidImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo("Image not found or content is missing.");
        }

        [Fact]
        public async Task GetImage_ValidImageId_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            int validImageId = fixture.Create<int>();

            var validUser = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male) // Replace with a valid enum value
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();

            var validImage = new Image{

            };

            A.CallTo(() => _imageRepository.GetImageById(validImageId))
                .Returns(validImage);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetImage(validImageId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            var imageDetails = result.Value as GetImageWithDetails;
            imageDetails.Should().NotBeNull();
        }

    }
}