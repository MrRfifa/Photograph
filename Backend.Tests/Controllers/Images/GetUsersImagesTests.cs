
namespace Backend.Tests.Controllers.Images
{
    public class GetUsersImagesTests
    {

        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public GetUsersImagesTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }

        [Fact]
        public async Task GetUsersImages_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            var validUserId = fixture.Create<int>();

            var images = new List<Image>
        {
            new Image { Id = 1, Title = "Image 1", Description = "Description 1", UserId = validUserId },
            new Image { Id = 2, Title = "Image 2", Description = "Description 2", UserId = validUserId }
        };

            A.CallTo(() => _imageRepository.GetImagesByUserId(validUserId))
                .Returns(images);

            var controller = new ImageController(_userRepository,_imageRepository);

            // Act
            var result = await controller.GetUsersImages(validUserId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull().And.BeOfType<List<GetImageWithDetails>>();
        }

        [Fact]
        public async Task GetUsersImages_NoImagesFound_ReturnsNotFoundResult()
        {
            // Arrange
            var fixture = new Fixture();
            var invalidUserId = fixture.Create<int>();

            A.CallTo(() => _imageRepository.GetImagesByUserId(invalidUserId))
                .Returns(new List<Image>());

            var controller = new ImageController(_userRepository,_imageRepository);

            // Act
            var result = await controller.GetUsersImages(invalidUserId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo("Images not found");
        }


    }
}