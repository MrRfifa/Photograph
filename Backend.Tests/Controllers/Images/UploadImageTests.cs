using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Backend.Tests.Controllers.Images
{
    public class UploadImageTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public UploadImageTests()
        {
            _userRepository = A.Fake<IUserRepository>();
            _imageRepository = A.Fake<IImageRepository>();
        }


        [Fact]
        public async Task UploadImage_NullImageFile_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            var image = fixture.Build<UploadImageRequest>()
                    .Without(uir => uir.file)
                    .Create();
            var userId = fixture.Create<int>();
            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadImage(userId, image) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            result.Value.Should().BeEquivalentTo("Invalid file.");
        }

        [Fact]
        public async Task UploadImage_NonExistingUser_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Microsoft.AspNetCore.Http.IFormFile),
                    typeof(IFormFile)));
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Create a non-empty stream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");
            var image = new UploadImageRequest
            {
                file = formFile,
                ImageTitle = fixture.Create<string>(),
                ImageDescription = fixture.Create<string>()
            };

            var invalidUserId = fixture.Create<int>();

            A.CallTo(() => _userRepository.GetUserById(invalidUserId))
                    .Throws(new UserNotFoundException($"user with id {invalidUserId} not found"));

            A.CallTo(() =>
                _imageRepository.UploadImage(formFile, invalidUserId, image.ImageTitle, image.ImageDescription))
                .Throws(new UserNotFoundException($"user with id {invalidUserId} not found"));

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadImage(invalidUserId, image) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            result.Value.Should().BeEquivalentTo($"user with id {invalidUserId} not found");
        }

        [Fact]
        public async Task UploadImage_ImageNotSavedSuccessfully_ReturnsBadRequestResult()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Microsoft.AspNetCore.Http.IFormFile),
                    typeof(IFormFile)));
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Create a non-empty stream
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

            var formFile = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");
            var image = new UploadImageRequest
            {
                file = formFile,
                ImageTitle = fixture.Create<string>(),
                ImageDescription = fixture.Create<string>()
            };

            var invalidUserId = fixture.Create<int>();

            A.CallTo(() =>
                _imageRepository.UploadImage(formFile, invalidUserId, image.ImageTitle, image.ImageDescription))
                .Returns(false);

            var controller = new ImageController(_userRepository, _imageRepository);

            // Act
            var result = await controller.UploadImage(invalidUserId, image) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeEquivalentTo($"An error occurred while processing the request: Image not saved successfully");
        }

        // [Fact]
        // public async Task UploadImage_ImageSavedSuccessfully_ReturnsOkResult()
        // {
        //     // Arrange
        //     var fixture = new Fixture();
        //     fixture.Customizations.Add(
        //         new TypeRelay(
        //             typeof(Microsoft.AspNetCore.Http.IFormFile),
        //             typeof(IFormFile)));
        //     fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        //         .ForEach(b => fixture.Behaviors.Remove(b));
        //     fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        //     // Create a non-empty stream
        //     using var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a non-empty stream."));

        //     var formFile = new FormFile(stream, 0, stream.Length, "streamFile", "image.jpg");
        //     var image = new UploadImageRequest
        //     {
        //         file = formFile,
        //         ImageTitle = fixture.Create<string>(),
        //         ImageDescription = fixture.Create<string>()
        //     };

        //     var validUserId = fixture.Create<int>();

        //     var user = fixture.Build<User>()
        //         .With(uc => uc.Gender, UsersGenders.male | UsersGenders.female)
        //         .With(uc => uc.Role, UsersRoles.owner | UsersRoles.admin)
        //         .Create();

        //     A.CallTo(() => _userRepository.GetUserById(validUserId))
        //         .Returns(user);

        //     A.CallTo(() => _imageRepository.Save())
        //         .Returns(true);

        //     A.CallTo(() =>
        //         _imageRepository.UploadImage(formFile, validUserId, image.ImageTitle, image.ImageDescription))
        //         .Returns(true);

        //     var controller = new ImageController(_userRepository, _imageRepository);

        //     // Act
        //     var result = await controller.UploadImage(validUserId, image) as ObjectResult;

        //     // Assert
        //     result.Should().NotBeNull();
        //     result.StatusCode.Should().Be(200);
        //     result.Value.Should().BeEquivalentTo(new { status = "success", message = "Image uploaded successfully." });
        // }



    }
}