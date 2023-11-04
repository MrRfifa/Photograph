using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Interfaces;
using Backend.Models.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;

        public ImageController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] int userId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }
                var user = await _userRepository.GetUserById(userId);

                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    // Convert the image to a base64-encoded string.
                    byte[] bytes = ms.ToArray();
                    string base64Image = Convert.ToBase64String(bytes);

                    // Create an ImageFile instance with the base64-encoded content.
                    var imageFile = new ImageFile
                    {
                        FileName = file.FileName,
                        FileContentBase64 = bytes // Store the binary data directly
                    };

                    var imageToSave = new Image
                    {
                        Description = "test description",
                        Title = "test",
                        ImageFile = imageFile,
                        UploadDate = DateTime.Now,
                        User = user,
                        UserId = userId
                    };

                    _context.Images.Add(imageToSave);
                    await _context.SaveChangesAsync();

                    return Ok("Image uploaded successfully.");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("get/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetImage(int imageId)
        {
            try
            {
                // Retrieve the Image entity from the database based on 'imageId'.
                var image = _context.Images
                    .Include(i => i.ImageFile)   // Include the related ImageFile entity
                    .FirstOrDefault(i => i.Id == imageId);
                if (image == null)
                {
                    return NotFound("Image not found");
                }

                if (image.ImageFile == null || image.ImageFile.FileContentBase64 == null)
                {
                    return NotFound("Image not found or content is missing.");
                }

                // Retrieve the binary image data directly from the FileContentBase64 property.
                byte[] imageData = image.ImageFile.FileContentBase64;

                // Set the appropriate content type based on the image file format (e.g., image/jpeg).
                string contentType = "image/jpeg"; // Modify based on your image type.

                return File(imageData, contentType);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

    }
}