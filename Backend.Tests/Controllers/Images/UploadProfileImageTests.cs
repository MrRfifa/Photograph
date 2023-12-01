using System.Text;
using Microsoft.AspNetCore.Http;

namespace Backend.Tests.Controllers.Images
{
    public class UploadProfileImageTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public UploadProfileImageTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }

        [Fact]
        public async Task UploadProfileImage_NullImageFile_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            IFormFile invalidProfileImage = null;
            var userId = fixture.Create<int>();
            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadProfileImage(userId, invalidProfileImage) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid file.");
        }

        [Fact]
        public async Task UploadProfileImage_UserNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            // Create a non-empty stream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

            var validProfileImage = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");

            var userId = fixture.Create<int>();

            A.CallTo(() => _userRepository.GetUserById(userId))
                    .Throws(new UserNotFoundException($"user with id {userId} not found"));

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadProfileImage(userId, validProfileImage) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo($"An error occurred while processing the request: user with id {userId} not found");
        }

        [Fact]
        public async Task UploadProfileImage_NotSavedSuccessfully_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create a non-empty stream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

            var validProfileImage = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");

            var userId = fixture.Create<int>();

            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();


            A.CallTo(() => _userRepository.GetUserById(userId))
                    .Returns(user);

            A.CallTo(() => _imageRepository.UploadProfileImage(validProfileImage, userId))
                    .Returns(false);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadProfileImage(userId, validProfileImage) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo("An error occurred while processing the request: Image not saved successfully");
        }

        [Fact]
        public async Task UploadProfileImage_SavedSuccessfully_ReturnsOkResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            // Create a non-empty stream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

            var validProfileImage = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");

            var userId = fixture.Create<int>();

            var user = fixture.Build<User>()
                .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
                .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
                .Create();


            A.CallTo(() => _userRepository.GetUserById(userId))
                    .Returns(user);

            A.CallTo(() => _imageRepository.UploadProfileImage(validProfileImage, userId))
                    .Returns(true);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadProfileImage(userId, validProfileImage) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(new { status = "success", message = "Profile image uploaded successfully." });
        }


    }
}