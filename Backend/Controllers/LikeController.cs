using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        [HttpPost("like/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LikeImage(int userId, int imageId)
        {
            try
            {
                var result = await _likeRepository.LikeImage(userId, imageId);

                if (result)
                {
                    return Ok(new { status = "success", message = "Image liked successfully." });
                }

                return BadRequest("User has already liked the image");

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpDelete("unlike/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UnlikeImage(int userId, int imageId)
        {
            try
            {

                var result = await _likeRepository.UnlikeImage(userId, imageId);

                if (result)
                {
                    return Ok(new { status = "success", message = "Image unliked successfully." });
                }

                return BadRequest("User hasn't liked the image");

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("liked/{userId}/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LikedImage(int userId, int imageId)
        {
            try
            {
                var result = await _likeRepository.LikedImage(userId, imageId);

                if (result)
                {
                    return Ok(new { status = "success", message = "Image already liked." });
                }

                return Ok(new { status = "failed", message = "Image not liked." });

            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }

        [HttpGet("likes-per-image/{imageId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LikesPerImage(int imageId)
        {
            try
            {
                var result = await _likeRepository.NumberOfLikesPerImage(imageId);
                return Ok(new { status = "success", message = result });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request: " + ex.Message);
            }
        }



    }
}