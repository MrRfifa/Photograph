

namespace Backend.Tests.Controllers.Images
{
    public class GetAllImagesWithDateSortTests
    {

        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public GetAllImagesWithDateSortTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }

        [Fact]
        public async Task GetAllImagesWithDateSort_ValidData_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();

            var images = new List<Image>
        {
            new Image { Id = 1, Title = "Image 1", Description = "Description 1" },
            new Image { Id = 2, Title = "Image 2", Description = "Description 2" }
        };

            A.CallTo(() => _imageRepository.GetImagesWithDateSort())
                .Returns(images);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetAllImagesWithDateSort() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull().And.BeOfType<List<GetImageWithDetails>>();
        }

        [Fact]
        public async Task GetAllImagesWithDateSort_NoImagesFound_ReturnsNotFoundResult()
        {
            // Arrange
            A.CallTo(() => _imageRepository.GetImagesWithDateSort())
                .Returns(new List<Image>());

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.GetAllImagesWithDateSort() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo("Images not found");
        }


    }
}