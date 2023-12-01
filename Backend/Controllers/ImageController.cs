using Backend.Data;
using Backend.Dtos.requests;
using Backend.Exceptions;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;

        public ImageController(IUserRepository userRepository, IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
            _userRepository = userRepository;
        }

        [HttpPost("upload/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadImage(int userId, [FromForm] UploadImageRequest uploadImageRequest)
        {
            try
            {
                if (uploadImageRequest.file == null || uploadImageRequest.file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }

                var user = await _userRepository.GetUserById(userId);

                if (user is null)
                {
                    throw new UserNotFoundException($"User with ID {userId} not found");
                }

                var imageToSave = await _imageRepository.UploadImage(uploadImageRequest.file, userId, uploadImageRequest.ImageDescription, uploadImageRequest.ImageTitle);

                if (!imageToSave)
                {
                    throw new Exception("Image not saved successfully");
                }

                return Ok(new { status = "success", message = "Image uploaded successfully." });
            }
            catch (UserNotFoundException ex)
            {
                // Handle the user not found exception
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions, log them, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }


        [HttpGet("get/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetImage(int imageId)
        {
            try
            {
                // Retrieve the Image entity from the database based on 'imageId'.
                var image = await _imageRepository.GetImageById(imageId);
                if (image == null)
                {
                    return NotFound("Image not found");
                }

                if (image.ImageFile == null || image.ImageFile.FileContentBase64 == null)
                {
                    return NotFound("Image not found or content is missing.");
                }
                var imageDetailsToReturn = new GetImageWithDetails
                {
                    Id = image.Id,
                    Description = image.Description,
                    Title = image.Title,
                    FileName = image.ImageFile.FileName,
                    FileContentBase64 = image.ImageFile.FileContentBase64,
                    UploadDate = DateOnly.FromDateTime(image.UploadDate),
                    Username = image.User?.FirstName + " " + image.User?.LastName
                };
                // Retrieve the binary image data directly from the FileContentBase64 property.
                // byte[] imageData = image.ImageFile.FileContentBase64;

                // Set the appropriate content type based on the image file format (e.g., image/jpeg).
                // string contentType = "image/jpeg"; // Modify based on your image type.

                // return File(imageData, contentType);
                return Ok(imageDetailsToReturn);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpPost("upload-profile-image/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        {
            try
            {

                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }
                var user = await _userRepository.GetUserById(userId);

                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }
                var profileImageUploaded = await _imageRepository.UploadProfileImage(file, userId);
                if (!profileImageUploaded)
                {
                    throw new Exception("Image not saved successfully");
                }

                return Ok(new { status = "success", message = "Profile image uploaded successfully." });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }


        [HttpGet("get-profile-image/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfileImage(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                // Retrieve the Image entity from the database based on 'imageId'.

                if (user.FileContentBase64 == null)
                {
                    return NotFound("Image not found or content is missing.");
                }

                // Retrieve the binary image data directly from the FileContentBase64 property.
                byte[] imageData = user.FileContentBase64;
                // Set the appropriate content type based on the image file format (e.g., image/jpeg).
                string contentType = "image/jpg"; // Modify based on your image type.

                return File(imageData, contentType);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("get-images/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsersImages(int userId)
        {
            try
            {
                var images = await _imageRepository.GetImagesByUserId(userId);

                if (images.Count == 0)
                {
                    return NotFound("Images not found");
                }

                var getImageWithDetails = images.Select(image => new GetImageWithDetails
                {
                    Id = image.Id,
                    Description = image.Description,
                    Title = image.Title,
                    FileName = image.ImageFile?.FileName ?? string.Empty,
                    UploadDate = DateOnly.FromDateTime(image.UploadDate),
                    FileContentBase64 = image.ImageFile?.FileContentBase64 ?? new byte[0]
                }).ToList();

                return Ok(getImageWithDetails);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }


        [HttpGet("get-images")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllImagesWithDateSort()
        {
            try
            {
                var images = await _imageRepository.GetImagesWithDateSort();

                if (images.Count == 0)
                {
                    return NotFound("Images not found");
                }
                var getImageWithDetails = images
                                            .Select(image => new GetImageWithDetails
                                            {
                                                Id = image.Id,
                                                Description = image.Description,
                                                Title = image.Title,
                                                FileName = image.ImageFile?.FileName ?? string.Empty,
                                                UploadDate = DateOnly.FromDateTime(image.UploadDate),
                                                FileContentBase64 = image.ImageFile?.FileContentBase64 ?? new byte[0]
                                            })
                                            .ToList();
                return Ok(getImageWithDetails);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpDelete("delete/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                var image = await _imageRepository.DeleteImage(imageId);
                if (!image)
                {
                    return NotFound($"Image with ID {imageId} not found or not deleted.");
                }

                return Ok(new { status = "success", message = "Image deleted successfully." });
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }


    }
}